using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchonManager : MonoBehaviour
{
    public GameObject ArchonOwner;
    public List<Material> testType;
    public ArchonStates States;

    public GameObject Card;

    //public GameObject currentTile;

    public GameObject currentTile;

    public struct ArchonStates
    {
        public bool? isVulnerable;
        public bool? isInPlay;
        public bool? isSelected;
        public bool? isCardSet;
    }

    ArchonManager()
    {
        States.isCardSet = false;
    }

    private void OnEnable()
    {
        EventManager.OnSet_Start += SetArchonState;
        EventManager.OnAttack += lawl;
    }

    private void OnDisable()
    {
        EventManager.OnSet_Start -= SetArchonState;
        EventManager.OnAttack -= lawl;
    }

    //use to detect location
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TileManager>())
        {
            //currentTile = other.gameObject;
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnMouseDown()
    {

        if(States.isCardSet != null && ArchonOwner == GameManager.CurrentPlayerOBJ)
        {
            switch (States.isCardSet)
            {
                case null:
                    break;
                case false:
                    break;
                case true:
                    EventManager.ArchonGetCard(this.gameObject);
                    break;
            }
        }
    }

    public void lawl(params GameObject[] _Targets)
    {
        var _Archon = _Targets[0];
        var _Card = _Targets[1];

        if(_Archon == this.gameObject)
        {
            Debug.Log(_Archon);
            Debug.Log(_Card);
            
        }
    }

    public void SetArchonState(GameObject _Card)
    {
        States.isCardSet = true;
    }

    public void setArchonParams(Archons _Target, GameObject _Owner, List<Archons> _Collection, GameObject _CurrentTile)
    {
        gameObject.name = _Target.name;
        ArchonOwner = _Owner;
        //currentTile = _CurrentTile;
        
        gameObject.GetComponent<MeshRenderer>().material = (ArchonOwner.name == "Player_1") ? testType[0] : testType[1];
    }
}
