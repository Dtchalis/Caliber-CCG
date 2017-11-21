using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject DisplayArea;
    public static GameObject Manager;
    public GameObject PlayerObject;
    public static GameObject CurrentPlayerOBJ;
    public static Players CurrentPlayer;
    static GameSettings Settings;

    public List<GameObject> PlayerInstance;

    public int turns, rounds;

    string timer;
    int seconds, minutes;

    public struct GameSettings
    {
        public static int numActiveArchons;
        

        //put number of active archons, weakspots on/off?, random/select archons
        //random/select weakspots, swap on/off?, mulligan on/off?
    }

    public int numPlayers;

    // Use this for initialization
    void Start () {
        Manager = this.gameObject;
        numPlayers = 2;
        GameSettings.numActiveArchons = 3;
        //make a menu script on game start that presents all the mode options so you can set the game
        //mode prior to starting an actual match.

        GameStart(numPlayers);

        CurrentPlayerOBJ = SelectRandomPlayer(numPlayers);
        CurrentPlayer = CurrentPlayerOBJ.GetComponent<Players>();
        CurrentPlayerOBJ.GetComponent<Players>().States.isPlayerTurn = true;

        //not sure where to put this script --> sets player desk name equal to that of the current player
        GameObject.Find("Player Name").GetComponent<Text>().text = "\"" + CurrentPlayerOBJ.name + "\"";
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentPlayerOBJ.GetComponent<Players>().ShuffleTarget(CurrentPlayerOBJ.GetComponent<Players>().Hand);
            CurrentPlayerOBJ.GetComponent<Players>().DestroyCards(CurrentPlayerOBJ.GetComponent<Players>().CardInstance);
            CurrentPlayerOBJ.GetComponent<Players>().InstantiateCards(CurrentPlayerOBJ.GetComponent<Players>().Hand);
        }
    }

    void GameStart(int _numOfPlayers)
    {
        for (int i = 0; i < _numOfPlayers; i++)
        {
            PlayerInstance.Add(GameObject.Instantiate(PlayerObject));
            PlayerInstance[i].transform.SetParent(DisplayArea.transform, false);
            Players Init = PlayerInstance[i].GetComponent<Players>();

            PlayerInstance[i].name = Init.Status.Username + "_" + (i + 1);
            Debug.Log(Init.Status.Username + "_" + (i + 1));
        }

        //decide which player goes first
        //determine game modes, etc.
    }

    GameObject SelectRandomPlayer(int _numOfPlayers)
    {
        Random.state.Equals(Time.deltaTime);
        int index = Random.Range(0, _numOfPlayers);

        switch (index)
        {
            default:
                return PlayerInstance[0];
                
            case 1:
                return PlayerInstance[1];
                
            case 2:
                return PlayerInstance[2];

            case 3:
                return PlayerInstance[3];
        }
    }

 

}

