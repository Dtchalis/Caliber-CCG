using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class CardManager : MonoBehaviour
{
    //Sprites
    public List<Sprite> Spr_CardImage = new List<Sprite>();
    public Sprite[] Spr_Caliber = new Sprite[10];
    public Sprite[] Spr_UIType = new Sprite[3];
    public Sprite[] Spr_DefaultNum = new Sprite[5];
    public List<Sprite> Spr_DefaultType = new List<Sprite>();
    public List<Sprite> Spr_Back = new List<Sprite>();
    public List<Sprite> Spr_DamageNum = new List<Sprite>(5);
    public List<GameObject> scopePoints = new List<GameObject>();

    //Owner of Card
    public GameObject CardOwner;
    //Collection the card came from
    public List<Cards> Location;
    //Index of the card in the collection
    public int indexOfCard;
    //Type of Card
    public Cards.cardTypes typeOfCard;


    public CardSpecific CardSpec;
    public int caliberReq;

    public struct CardSpecific
    {
        public Vector2?[] Scope;
        //move, support etc.
    }

    //GameObjects 
    public GameObject Name_Obj, Caliber_Obj, CardType_Obj, DefaultNum_Obj, DefaultType_Obj, ScopeMarker_Obj, DamageNum_Obj;

    /// <summary>
    /// Global Card Parameters
    /// </summary>
    public void SetCardParams(GameObject _Player, List<Cards> _Target, int _index, Cards.cardTypes _typeOfCard)
    {
        gameObject.GetComponent<Image>().sprite = Spr_CardImage[(int)_typeOfCard - 1];
        //gonna have to find a better way of loading in images. //Icomparer?

        name = _Target[_index].name;
        Name_Obj.GetComponent<Text>().text = _Target[_index].name;
        Caliber_Obj.GetComponent<Image>().sprite = Spr_Caliber[_Target[_index].caliber - 1];
        DefaultType_Obj.GetComponent<Image>().sprite = Spr_DefaultType[(int)_Target[_index].defType];
        DefaultNum_Obj.GetComponent<Image>().sprite = Spr_DefaultNum[_Target[_index].defaultAmount - 1];
        CardType_Obj.GetComponent<Image>().sprite = Spr_UIType[(int)_Target[_index].cardType];

        CardOwner = _Player;
        Location = _Target;
        indexOfCard = _index;
        typeOfCard = _Target[_index].cardType;
        caliberReq = _Target[_index].caliber;

        switch (_typeOfCard)
        {
            case Cards.cardTypes.Attack:
                SetAttackParams(_Target, _index);
                break;
            case Cards.cardTypes.Move:
                SetMoveParams(_Target, _index);
                break;
            case Cards.cardTypes.Support:
                SetSuppportParams(_Target, _index);
                break;
        }
    }

    private void SetAttackParams(List<Cards> _Target, int _index)
    {
        DamageNum_Obj.GetComponent<Image>().sprite = Spr_DamageNum[(int)_Target[_index].damage - 1];
        CardSpec.Scope = _Target[_index].AttackScope;

        PositionScopePoints(_Target[_index].AttackScope);
    }

    private void PositionScopePoints(Vector2?[] _Target)
    {
        Vector2 InstOrigin = new Vector2(95, -170); //117, -195
        float[] Px = new float[_Target.Length];
        float[] Py = new float[_Target.Length];
        float xOffset = 22f;
        float yOffset = 25f;
        bool isXset = false;
        bool isYset = false;

        for (int i = 0; i < _Target.Length; i++)
        {
            switch (_Target[i] == null)
            {
                case true:
                    break;
                case false:
                    switch (Mathf.Abs(_Target[i].Value.x) >= 3)
                    {
                        case true:
                            isXset = true;
                            break;
                        case false:
                            break;
                    }
                    switch (Mathf.Abs(_Target[i].Value.y) >= 3)
                    {
                        case true:
                            isYset = true;
                            break;
                        case false:
                            break;
                    }
                    break;
            }
        }

        float ovXoffset = (isXset == true) ? 22f : 0f;
        float ovYoffset = (isYset == true) ? 25f : 0f;

        ovXoffset = 0f;
        ovYoffset = 0f;
        //now we need to be able to tell where most of the points are located so we can center it properly

        //InstOrigin = new Vector2(95 - ovXOffset, -195 + ovYOffset);

        for (int i = 0; i < _Target.Length; i++)
        {
            switch (_Target[i] == null)
            {
                case true:
                    break;
                case false:
                    Px[i] = _Target[i].Value.x;
                    Py[i] = _Target[i].Value.y;
                    float oddOffset = (Px[i] % 2 == 0) ? 0f : 12.5f;

                    scopePoints.Insert(i, GameObject.Instantiate(ScopeMarker_Obj));
                    scopePoints[i].transform.SetParent(this.transform, false);
                    scopePoints[i].name = string.Format("P{0}|{1},{2}", i, Px[i], Py[i]);
                    scopePoints[i].transform.localPosition = new Vector2((InstOrigin.x + (Px[i] * xOffset)) - ovXoffset, (InstOrigin.y + ((Py[i] * yOffset) - oddOffset)) - ovYoffset);
                    break;
            }
        }
    }

    private void SetMoveParams(List<Cards> _Target, int _index)
    {

    }

    private void SetSuppportParams(List<Cards> _Target, int _index)
    {

    }

    

    
    
}
