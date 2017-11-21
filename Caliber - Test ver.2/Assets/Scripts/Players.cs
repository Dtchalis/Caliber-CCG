using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Players : MonoBehaviour {
    
    public List<Cards> Hand, Deck, Set, Discard, Caliber;
    public List<Archons> All, Active, Standby, Defeated;
    public Library PlayerLibrary;
    public PlayerStatus Status;
    public PlayerStates States;

    public GameObject ArchonObject;
    public List<GameObject> ArchonInstance;

    public GameObject[] CardObject;
    public List<GameObject> CardInstance;

    /// <summary>
    /// A collection of statistics that define the player
    /// </summary>
    public struct PlayerStatus
    {
        public string Username { get; set; }
        public int HP { get; set; }
        public int DeckSize { get; set; }
        public int caliberCapacity { get; set; }
        public int currentCaliber { get; set; }
        //NumOfType keeps track of how many of each caliber type we have
        public int[] NumOfType { get; set; }
        public enum DefaultTypes { STR, WIS, PSY };
        public int NumCardsInHand, NumCardsSet, NumCardsDiscarded, NumCardsDefaulted;
        
        //don't forget weakspots and archons
        public int NumActiveArchons, NumStandbyArchons, NumDefeatedArchons;
    }

    /// <summary>
    /// A collection of player states, that record the player's condition
    /// </summary>
    public struct PlayerStates
    {
        public bool isPlayerTurn;
        public bool isCardDrawn;
        public bool isCardDefaulted;
        public bool isCardSet;
        public bool isCaliberFull;
        
        public bool isMasterOn; //isMasterOn = debug variable
    }

    // Use this for initialization
    void Start() {

        PlayerLibrary = new Library();

        PopulateArchons_Random();
        InstantiateArchons(Active);

        //Declare contents of deck          
        PopulateDeck_Random(Status.DeckSize);

        //Shuffle contents of deck
        ShuffleTarget(Deck);

        //Populate Hand with cards from Deck
        States.isMasterOn = true;
        DrawCardsFrom(Deck, Status.NumCardsInHand);
        States.isCardDrawn = false;
        States.isMasterOn = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            switch (States.isMasterOn)
            {
                case false:
                    States.isMasterOn = true;
                    Debug.Log("Master is On");
                    break;
                case true:
                    States.isMasterOn = false;
                    Debug.Log("Master is Off");
                    break;
            }
        }

        
    }

    /// <summary>
    /// Initializes the player's status and states.
    /// </summary>
    public Players()
    {
        //Initialize Player's status and states
        Status.Username = "Player";
        Status.HP = 50;
        Status.DeckSize = 40;
        Status.caliberCapacity = 0;
        Status.currentCaliber = 0;
        Status.NumOfType = new int[3];
        Status.NumOfType[(int)PlayerStatus.DefaultTypes.STR] = 0;
        Status.NumOfType[(int)PlayerStatus.DefaultTypes.WIS] = 0;
        Status.NumOfType[(int)PlayerStatus.DefaultTypes.PSY] = 0;
        Status.NumCardsInHand = 5;
        Status.NumCardsSet = 0;
        Status.NumCardsDiscarded = 0;

        Status.NumActiveArchons = 0;
        Status.NumStandbyArchons = 0;
        Status.NumDefeatedArchons = 0;

        States.isPlayerTurn = false;
        States.isCardDrawn = false;
        States.isCardDefaulted = false;
        States.isCardSet = false;
        States.isCaliberFull = false;
    }

    //game manager will know that cards can only be accessed from certain locations/states via enums
    //ex. if cards are in discard, they are not in hand, deck or on field and cannot be accessed via normal draw, normal set or normal discard.
    //i.e there are "normal" actions and "effect" actions --> index identifier or seperate function?
    //they can only be accessed via "Card Effect" or "Reload"
    
    void PopulateArchons_Random()
    {
        UnityEngine.Random.state.Equals(Time.deltaTime);
        for (int i = 0; i < 5; i++)
        {
            var rng = UnityEngine.Random.Range(0, PlayerLibrary.archonLib.Count);
            All.Insert(i, PlayerLibrary.archonLib[rng]);
            Active.Insert(i, PlayerLibrary.archonLib[rng]);
        }
        
        for (int i = 0; i < 2; i++)
        {
            Standby.Insert(i, Active[i]);
            Active.RemoveRange(i, 1);
        }

    }

    void InstantiateArchons(List<Archons> _Target)
    {
        UnityEngine.Random.state.Equals(Time.deltaTime);
        switch (_Target == null)
        {
            case true:
                break;
            case false:
                if (_Target == Active)
                {
                    for (int i = 0; i < GameManager.GameSettings.numActiveArchons; i++)
                    {
                        var tX = UnityEngine.Random.Range(0, 6);
                        var tY = UnityEngine.Random.Range(0, 6);

                        ArchonInstance.Add(Instantiate(ArchonObject));
                        ArchonInstance[i].GetComponent<ArchonManager>().ArchonOwner = this.gameObject;

                        for (int c = 0; c < 1;)
                        {
                            if (Field.Map[tX, tY].tile.GetComponent<TileManager>().isOccupied == true || Field.Map[tX, tY].tile.GetComponent<TileManager>().isActive == false)
                            {
                                    tX = UnityEngine.Random.Range(0, 6);
                                    tY = UnityEngine.Random.Range(0, 6);
                            }
                            else
                            {
                             ArchonInstance[i].GetComponent<Transform>().position = Field.Map[tX, tY].tile.transform.position + new Vector3(0, 10, 0);
                             ArchonInstance[i].GetComponent<ArchonManager>().setArchonParams(_Target[i], this.gameObject, _Target, Field.Map[tX, tY].tile);
                             Field.Map[tX, tY].tile.GetComponent<TileManager>().isOccupied = true;
                             c++;
                            }
                        }


                    }
                }
                else
                {
                    return;
                }
                break;
        }
    }

    /// <summary>
    /// Populates the deck with 40 random cards from player's library
    /// </summary>
    void PopulateDeck_Random(int _deckSize)
    {
        UnityEngine.Random.state.Equals(Time.deltaTime);
        for (int i = 0; i < _deckSize; i++)
        {
            Deck.Add(PlayerLibrary.cardLib[UnityEngine.Random.Range(0, new Library().cardLib.Count)]);
            //this could use some work --> end up with a lot of duplicates right next to each other 
            //it's partially because there are only 11 cards in the library currently
        }
    }

    /// <summary>
    /// Instantiate Cards --> Instantiates card objects from the target collection
    /// </summary>
    /// <param name="_Target"> The collection of cards to instantiate </param>
    public void InstantiateCards(List<Cards> _Target)
    {
        if(_Target == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            for (int i = 0; i != _Target.Count; i++)
            {
                CardInstance.Add(GameObject.Instantiate(CardObject[(int)_Target[i].cardType]));
                CardInstance[i].GetComponent<CardManager>().SetCardParams(gameObject, _Target, i, _Target[i].cardType);

                CardInstance[i].transform.SetParent(GameObject.Find("Canvas").transform, false);
                Vector3 InstanceOrigin = CardInstance[0].transform.position;
                CardInstance[i].GetComponent<RectTransform>().position = new Vector3(InstanceOrigin.x - 10 * i, InstanceOrigin.y, InstanceOrigin.z);
                CardInstance[i].transform.SetParent(CardInstance[i].GetComponent<CardManager>().CardOwner.transform, true);
                CardInstance[i].transform.SetSiblingIndex(i - i);
            }
        }
    }

    /// <summary>
    /// Destroy Cards 1 --> Destroys card objects within the target collection
    /// </summary>
    /// <param name="_Target"> The collection of GameObjects to destroy </param>
    public void DestroyCards(List<GameObject> _Target)
    {
        if(_Target == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            // Debug.Log(_Target.Count);
            for (int i = 0; i < _Target.Count; i++)
            {
                GameObject.Destroy(_Target[i]);
            }
            _Target.RemoveRange(0, _Target.Count);
        }
        
    }

    /// <summary>
    /// Destroy Cards 2 --> Destroys a single card object at a specific index within the target collection
    /// </summary>
    /// <param name="_Target"> The collection of GameObjects to target </param>
    /// <param name="_index"> The index of the card to destroy </param>
    public void DestroyCards(List<GameObject> _Target, int _index)
    {
        if(_Target == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            for (int i = 0; i < _Target.Count; i++)
            {
                GameObject.Destroy(_Target[i]);
            }
            //CardInstance.RemoveRange(_index, 1);
        }
    }

    /// <summary>
    /// Shuffle Function --> Shuffles a collection of cards
    /// </summary>
    /// <param name="_Target"> The collection of cards to target </param>
    public void ShuffleTarget(List<Cards> _Target)
    {
        List<Cards> TempList = new List<Cards>();
        UnityEngine.Random.state.Equals(Time.deltaTime);

        //take an element from target and store in a random index in templist
        for (int i = 0; i < _Target.Count; i++)
        {
            int rng = UnityEngine.Random.Range(0, _Target.Count);
            TempList.Insert(i, _Target.ElementAt(rng));
            _Target.RemoveRange(rng, 1);

            //re-populate target with cards from templist
            _Target.Insert(i, TempList.ElementAt(i));
        }

    }

    /// <summary>
    /// Draw function --> draw a specified number of cards from target and add to Hand
    /// </summary>
    /// <param name="_Target"> The collection of cards to target </param>
    /// <param name="_amount"> The number of the cards to draw </param>
    public void DrawCardsFrom(List<Cards> _Target, int _amount)
    {
        if (_Target == null)
        {
            Debug.Log("ERROR");
        }
        else if (Status.NumCardsInHand == 7)
        {
            Debug.Log("You can't have more than 7 cards in your hand");
            return;
        }
        else if (States.isMasterOn == true)
        {
            for (int i = 0; i < _amount; i++)
            {
                Hand.Add(_Target[i]);
                Debug.Log(_Target[i].name);
                Status.NumCardsInHand++;
            }
            _Target.RemoveRange(0, _amount);
        }
        else if (States.isCardDrawn == true)
        {
            Debug.Log("You can't draw more than 1 card per turn");
            return;
        }
        
        else if (States.isCardDrawn == false)
        {
            Hand.Add(_Target.First());
            _Target.RemoveRange(0, 1);
            States.isCardDrawn = true;
        }

        Debug.Log(Status.NumCardsInHand);
    }

    /// <summary>
    /// Set Function --> Take a specified card, add it to the field/set and remove it from target collection.
    /// </summary>
    /// /// <param name="_Target"> The collection of cards to target </param>
    /// <param name="_index"> The index of the card to be set </param>
    public bool SetCardFrom(List<Cards> _Target, int _index)
    {

        if (_Target == null)
        {
            Debug.Log("ERROR");
            return false;
        }
        else if(_Target == Set)
        {
            Debug.Log("You can't set cards that are already in this pile");
        }
        else if(_Target == Caliber)
        {
            //you're not allowed to set this card while it's in this pile
            Debug.Log("you're not allowed to set this card while it's in this pile");
            return false;
        }
        else if(_Target[_index].caliber + Status.currentCaliber <= Status.caliberCapacity)
        {
            Set.Add(_Target.ElementAt(_index));

            Status.NumCardsSet++;
            Status.currentCaliber += _Target[_index].caliber;

            GameObject.Find("Caliber Amount").GetComponent<Text>().text = string.Format(("{0}/{1}"), Status.currentCaliber, Status.caliberCapacity);

            _Target.Remove(_Target[_index]);
        }
        else if (_Target[_index].caliber + Status.currentCaliber > Status.caliberCapacity)
        {
            var difference = _Target[_index].caliber - (Status.caliberCapacity - Status.currentCaliber);

            Debug.Log("You can't set cards that will exceed your caliber capacity");
            Debug.Log("This card exceeds your caliber capacity by:" + difference);
            //the player should decide when their turn ends sans a timer that forces a turn end.
            //States.isCaliberFull = true;
            return false;
        }

        GameObject.Find("Set Amount").GetComponent<Text>().text = string.Format(("{0}"), Status.NumCardsSet);

        Debug.Log(Status.NumCardsSet);
        return true;

    }

    /// <summary>
    /// Discard Function --> Take a specified card, add it to the discard pile and remove it from target collection.
    /// </summary>
    /// <param name="_Target"> The collection of cards to target </param>
    /// <param name="_index"> The index of the card to discard </param>
    void DiscardCardsFrom(List<Cards> _Target, int _index)
    {
        if (_Target == null)
        {
            Debug.Log("ERROR");
        }
        else if(_Target == Discard)
        {
            Debug.Log("You can't discard a card that's already in this pile");
        }
        else
        {
            Discard.Add(_Target.ElementAt(_index));

            Status.NumCardsDiscarded++;

            _Target.RemoveRange(_index, 1);
        }
        Debug.Log(Status.NumCardsDiscarded);
    }

    /// <summary>
    /// Default Function --> Take a specified card, add it to the default pile and remove it from target collection.
    /// </summary>
    /// <param name="_Target"> The collection of cards to target </param>
    /// <param name="_index"> The index of the card to default </param>
    public bool DefaultCardsFrom(List<Cards> _Target, int _index)
    {
        if (_Target == null)
        {
            Debug.Log("ERROR");
            return false;
        }
        else if (_Target == Caliber)
        {
            Debug.Log("You can't default cards that are already in this pile.");
            return false;
        }
        else if (_Target == Set)
        {
            Debug.Log("You can't default cards that are in the set pile");
            return false;
        }
        else if (States.isCardDefaulted == true)
        {
            Debug.Log("You can't default more than 1 card per turn");
            return false;
        }
        else if (Status.caliberCapacity + _Target[_index].defaultAmount >= 15 || Status.caliberCapacity >= 15)
        {
            //let the player choose a card to discard so as to make the currentCaliber <= 15
            //i.e highlight cards that are acceptible as candidates to make the choice easier
            Debug.Log("FUNCTIONALITY NOT IMPLIMENTED YET");
        }
        else if(_Target != Caliber)
        {
            Caliber.Add(_Target.ElementAt(_index));

            Status.caliberCapacity += _Target[_index].defaultAmount;
            Status.NumCardsInHand--;
            Status.NumCardsDefaulted++;
            Status.NumOfType[(int)_Target[_index].defType]++;
            States.isCardDefaulted = true;

            GameObject.Find("Caliber Amount").GetComponent<Text>().text = string.Format(("{0}/{1}"), Status.currentCaliber, Status.caliberCapacity);

            _Target.Remove(_Target[_index]);

            //_Target.RemoveRange(_index, 1);
            //_Target.Remove(_Target[_index]);
        }
        return true;

        //condition for default --> if caliber >= 15, replace a card in caliber with new card
        //condition for default --> if caliberCapacity + carddefault >= 15 ||

        //this debug call doesn't work;
        //Debug.Log(Status.NumOfType[(int)_Target[_index].defType]);
        //Debug.Log(Status.caliberCapacity);
    }

    public void SetAction(GameObject _Target)
    {
        var targetCard = _Target.GetComponent<CardManager>();
        var targetLocation = _Target.GetComponent<CardManager>().Location;
        var targetIndex = _Target.GetComponent<CardManager>().indexOfCard;

        switch (targetCard.typeOfCard)
        {
            case Cards.cardTypes.Attack:
                 States.isCardSet = SetCardFrom(targetLocation, targetIndex);

                if(States.isCardSet == true)
                {
                    DestroyCards(targetCard.CardOwner.GetComponent<Players>().CardInstance);
                    InstantiateCards(Set);

                    //use a bool to determine the state of the archon

                    //on click --> if owner of archon == current player turn
                }
                else
                {

                }

                States.isCardSet = false;
                //InstantiateCards(Set);
                //you also want to consider/possibly allow/force the card's effect based on it's description
                //you want to then allow the player to select an archon to use the card
                //then you want to inflict damage based on the scope of the card.
                //you want to consider the effects of any/all archons affected by the card.
                //is it the card's job to determine what it can't do once activated? or the gamemanager's job? or the job of the archon attacker/defender?
                //you also want to allow/force the player to activate the cards ability based on it's activation type.
                break;
            case Cards.cardTypes.Move:
                SetCardFrom(targetLocation, targetIndex);
                //
                break;
            case Cards.cardTypes.Support:
                SetCardFrom(targetLocation, targetIndex);
                //
                break;
            default:
                break;
        }

        //DestroyCards(targetCard.CardOwner.GetComponent<Players>().CardInstance);
        //InstantiateCards(targetLocation);
        //InstantiateCards(Set);

    }

    public void DefaultAction(GameObject _Target)
    {
        var targetCard = _Target.GetComponent<CardManager>();
        var targetLocation = _Target.GetComponent<CardManager>().Location;
        var targetIndex = _Target.GetComponent<CardManager>().indexOfCard;

        var canDefault = DefaultCardsFrom(targetLocation, targetIndex);

        if(canDefault == true)
        {
            DestroyCards(targetCard.CardOwner.GetComponent<Players>().CardInstance);
            InstantiateCards(GameManager.CurrentPlayer.Hand);
        }
        else
        {
            //do nothing
        }
        
    }


    //empty
    public void DisplayEffect(GameObject _Target)
    {

    }

    public void BringToFront(GameObject _Target)
    {
        //next I want to be able to drag cards left and right to change index

        var targetCards = _Target.GetComponent<CardManager>();

        if(targetCards.indexOfCard == 0)
        {
            //do nothing
        }
        else if(States.isMasterOn == false)
        {
            DestroyCards(_Target.GetComponent<CardManager>().CardOwner.GetComponent<Players>().CardInstance);
            Debug.Log("Version 1");
            for (int i = _Target.GetComponent<CardManager>().indexOfCard; i > 0; i--)
            {
                var tempCard = _Target.GetComponent<CardManager>().Location[0];
                _Target.GetComponent<CardManager>().Location.Add(tempCard);
                _Target.GetComponent<CardManager>().Location.RemoveRange(0, 1);
            }
            InstantiateCards(_Target.GetComponent<CardManager>().Location);
        }
        /*else
        {
            Debug.Log("Version 2");
            var tempCard = targetCards.Location[targetCards.indexOfCard];
            targetCards.Location[targetCards.indexOfCard] = targetCards.Location[0];
            targetCards.Location[0] = tempCard;
            DestroyCards(targetCards.CardOwner.GetComponent<Players>().CardInstance);
            InstantiateCards(targetCards.Location);
        }*/
        
    }

    public void GiveArchonCard(params GameObject[] _Target)
    {
        //var _Archon = _Target[0];
        //var _Player = _Target[1];

        //Debug.Log(string.Format("{0},{1}", this.name, CardInstance.Count));
        //_Target[0].GetComponent<ArchonManager>().Card = CardInstance[0];
        EventManager.ArchonAttack(_Target[0], CardInstance[0]);

        //_Archon[0].GetComponent<ArchonManager>().Card = CardInstance[0];
        //return CardInstance[0];
    }
}


