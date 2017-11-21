using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public Vector2 pos;
    public List<GameObject> neighbours = new List<GameObject>();
    enum neighbourDirections { F, FR, BR, B, BL, FL};

    public Tiles.terrainType? terrain;

    public bool isActive;
    public bool isOccupied;
    public bool isWeakspot;

    int tx = 0; 
    int ty = 0;

    // Use this for initialization
    void Start()
    {
        //if the tile we're about to add is null don't add it

        

        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    tx = 0;
                    ty = 1;
                    break;
                case 1:
                    tx = 1;
                    ty = 0;
                    break;
                case 2:
                    tx = 1;
                    ty = -1;
                    break;
                case 3:
                    tx = 0;
                    ty = -1;
                    break;
                case 4:
                    tx = -1;
                    ty = 0;
                    break;
                case 5:
                    tx = -1;
                    ty = -1;
                    break;
            }

            switch (Field.Map[(int)pos.x + tx, (int)pos.y + ty].tile == null)
            {
                case true:
                    break;
                case false:
                    neighbours.Add(Field.Map[(int)pos.x + tx, (int)pos.y + ty].tile);
                    break;
            }
        }

        

        //won't know what to reference because the tiles aren't associated with a script after being instantiated.
        /*neighbours[(int)neighbourDirections.F] = Field.Map[(int)pos.x, (int)pos.y + 1].tile;
        neighbours[(int)neighbourDirections.FR] = Field.Map[(int)pos.x + 1, (int)pos.y + 1].tile;
        neighbours[(int)neighbourDirections.BR] = Field.Map[(int)pos.x + 1, (int)pos.y].tile;
        neighbours[(int)neighbourDirections.B] = Field.Map[(int)pos.x, (int)pos.y - 1].tile;
        neighbours[(int)neighbourDirections.BL] = Field.Map[(int)pos.x - 1, (int)pos.y].tile;
        neighbours[(int)neighbourDirections.FL] = Field.Map[(int)pos.x - 1, (int)pos.y - 1].tile;*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //Debug.Log("LAWL");
        /*for (int i = 0; i < neighbours.Count; i++)
        {
            Debug.Log(neighbours[i].name);
        }*/
    }


    private void OnEnable()
    {
        //EventManager.OnAttack += setparams;
    }

    private void OnDisable()
    {
        //EventManager.OnAttack -= setparams;
    }

    private void setparams(GameObject _Archon, GameObject _Card)
    {
        //in here we'll run the sscript for highlighting tiles, respective
        //to the position of te archon pased in and the scope of the card passed in.

        //if this hex is equal to a point relative to the archon's scope, highlight it.
        //mark it as a damage zone.
        //archons in the damage zone recieve the damage amount specified by the card.

        Debug.Log("SETTING PARAMS");
        Debug.Log(_Card.name);
        Debug.Log(_Archon.name);
    }

    
}
