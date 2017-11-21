using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{

    private string currentPile;
    private bool AreCardsDisplayed;


    public delegate void GameSetup();
    public static event GameSetup OnInitField;
    public static event GameSetup OnCreateField;

    public delegate void PlayerAction(GameObject _Target);
    public static event PlayerAction OnSet_Start;
    public static event PlayerAction OnDefault;
    public static event PlayerAction OnSelect;

    public static event PlayerAction OnDiscard;
    public static event PlayerAction OnDraw;

    public static event PlayerAction OnSet_Confirm;

    public delegate void ArchonAction(params GameObject[] _Targets);
    public static event ArchonAction OnGetCard;
    public static event ArchonAction OnAttack;
    public static event ArchonAction OnMove;
    public static event ArchonAction OnSupport;

    // Use this for initialization
    void Start()
    {
        currentPile = null;
        AreCardsDisplayed = false;
    }

    /// <summary>
    /// Checks the name of the button being pressed and returns the collection of cards to display
    /// </summary>
    /// <param name="_buttonName"> The name of the button being pressed </param>
    /// <param name="_Target"> The collection of cards to display </param>
    List<Cards> SelectPile(string _buttonName, Players _Target)
    {
        switch (_buttonName)
        {
            case "Hand_Button":
                return _Target.Hand;
            case "CC_Button":
                return _Target.Caliber;
            case "Deck_Button":
                return _Target.Deck;
            case "Set_Button":
                return _Target.Set;
            default:
                return null;
        }
    }

    /// <summary>
    /// Display Event --> Displays cards based on the name of the button being pressed
    /// </summary>
    public void DisplayCards(string _buttonName)
    {
        //temporary variable to store CardInst --> this will change when we have 3 prefabs
        //use getcomponent if not working
        var PlayersCards = GameManager.CurrentPlayerOBJ.GetComponent<Players>().CardInstance;
        var CurrentPlayer = GameManager.CurrentPlayerOBJ.GetComponent<Players>();

        if (AreCardsDisplayed == false)
        {
            CurrentPlayer.InstantiateCards(SelectPile(_buttonName, CurrentPlayer));

            AreCardsDisplayed = true;
            currentPile = _buttonName;
            Debug.Log(AreCardsDisplayed);
        }
        else if (_buttonName != currentPile && AreCardsDisplayed == true)
        {
            CurrentPlayer.DestroyCards(PlayersCards);
            CurrentPlayer.InstantiateCards(SelectPile(_buttonName, CurrentPlayer));

            AreCardsDisplayed = true;
            currentPile = _buttonName;
            Debug.Log(AreCardsDisplayed);
        }
        else
        {
            CurrentPlayer.DestroyCards(PlayersCards);

            AreCardsDisplayed = false;
            currentPile = null;
            Debug.Log(AreCardsDisplayed);
        }

    }

    //let's other members of the game know when the field is drawn
    public static void CreateField()
    {
        if(OnInitField != null)
        {
            OnInitField();
        }
    }

    //doesn't need the gameobject being passed in, better way to impliment?
    public void PlayerDrawCard(GameObject _Player)
    {
        //_Target??

        if(OnDraw != null)
        {
            OnDraw(_Player);
        }
        
    }

    public void PlayerSetCard(GameObject _Card)
    {
        if (_Card.GetComponent<CardManager>().caliberReq + GameManager.CurrentPlayer.Status.currentCaliber <= GameManager.CurrentPlayer.Status.caliberCapacity)
        {
            if (OnSet_Start != null)
            {
                OnSet_Start(_Card);
            }
        }
    }

    public void PlayerDefaultCard(GameObject _Card)
    {
        if(OnDefault != null)
        {
            OnDefault(_Card);
        }
    }

    public void PlayerSelectCard(GameObject _Card)
    {
        if (OnSelect != null)
        {
            OnSelect(_Card);
        }
    }

    public static void ArchonGetCard(GameObject _Archon)
    {
        if(OnGetCard != null)
        {
            OnGetCard(_Archon);
        }
    }

    public static void ArchonAttack(GameObject _Archon, GameObject _Card)
    {
        if(OnAttack != null)
        {
            OnAttack(_Archon, _Card);
        }
    }


    private void OnEnable()
    {
        EventManager.OnInitField += CreateField;
        EventManager.OnDraw += DrawCards;
        EventManager.OnSet_Start += SetThisCard;
        EventManager.OnDefault += DefaultThisCard;
        EventManager.OnSelect += ShowThisCard;
        EventManager.OnGetCard += GiveCardToArchon;
    }

    private void OnDisable()
    {
        EventManager.OnInitField -= CreateField;
        EventManager.OnDraw -= DrawCards;
        EventManager.OnSet_Start -= SetThisCard;
        EventManager.OnDefault -= DefaultThisCard;
        EventManager.OnSelect -= ShowThisCard;
        EventManager.OnGetCard -= GiveCardToArchon;
    }

    public void SetThisCard(GameObject _Card)
    {
        GameManager.CurrentPlayer.GetComponent<Players>().SetAction(_Card);
    }

    public void ShowThisCard(GameObject _Card)
    {
        GameManager.CurrentPlayer.GetComponent<Players>().BringToFront(_Card);
    }

    public void DefaultThisCard(GameObject _Card)
    {
        GameManager.CurrentPlayer.GetComponent<Players>().DefaultAction(_Card);
    }

    public void DrawCards(GameObject _Player)
    {

        //need to be able to control the number of cards to draw
        GameManager.CurrentPlayer.DrawCardsFrom(GameManager.CurrentPlayer.Deck, 1);
        GameManager.CurrentPlayer.DestroyCards(GameManager.CurrentPlayer.CardInstance);
        GameManager.CurrentPlayer.InstantiateCards(GameManager.CurrentPlayer.Hand);
    }
    
    public void GiveCardToArchon(params GameObject[] _Archons)
    {
        GameManager.CurrentPlayer.GetComponent<Players>().GiveArchonCard(_Archons[0].gameObject);
    }

    public void AttackWithArchon(params GameObject[] _lol)
    {

    }
    
}
