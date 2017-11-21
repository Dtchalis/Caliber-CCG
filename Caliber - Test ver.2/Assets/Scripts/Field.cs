using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    //og field zPos     -60
    //new field zPos    -150

    static int height = 7;
    static int width = 7;
    static float cellHeight = 7.5f;
    int heightOffset;

    public GameObject TileObj;
    public GameObject displayArea;
    public List<Material> terrain;
    public static List<Material> terrainMaterials;
    //public static Tiles[,] Map = new Tiles[7, 7];

    public static Tiles[,] Map = new Tiles[7, 7];

    //public static List<GameObject> Mplol = new List<GameObject>();
    Vector3 originPos;

    private void OnEnable()
    {
        EventManager.OnSet_Start += lol;
    }

    private void OnDisable()
    {
        EventManager.OnSet_Start -= lol;
    }

    void lol(GameObject _Target)
    {
        //Debug.Log("ENABLED");
    }

    // Use this for initialization
    void Start()
    {
        originPos = new Vector3(0, 0);
        terrainMaterials = new List<Material>();

        for (int i = 0; i < terrain.Count; i++)
        {
            terrainMaterials[i] = terrain[i];
        }

        GenerateField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateField()
    {
        int xOffset = 38;
        int zOffset = 45;
        int index = 0;

        for (int x = 0; x < width; x++)
        {
            int oddOffset = (x % 2 == 0) ? 0 : 22;
            
            for (int z = 0; z < height; z++, index++)
            {
                Map[x, z] = new Tiles(Instantiate(TileObj, displayArea.transform), Tiles.terrainType.normal, true, false, false);
                Map[x, z].tile.transform.localPosition = new Vector3(x * xOffset, ((z * zOffset) + oddOffset) - heightOffset);
                Map[x, z].tile.name = Map[x, z].tile.GetComponent<TileManager>().terrain + " @ [" + x + "," + z + "]";
                Map[x, z].tile.GetComponent<TileManager>().pos = new Vector2(x, z);

                //Map[x, z].tile.GetComponent<MeshRenderer>().material = terrainMaterials[(int)Map[x,z].terrain];
            }
        }

        //these tiles are set to inactive to round out the shape of the field.
        Map[0, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[0, 1].tile.GetComponent<TileManager>().isActive = false;
        Map[1, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[2, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[4, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[5, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[6, 0].tile.GetComponent<TileManager>().isActive = false;
        Map[6, 1].tile.GetComponent<TileManager>().isActive = false;
        Map[0, 6].tile.GetComponent<TileManager>().isActive = false;
        Map[1, 6].tile.GetComponent<TileManager>().isActive = false;
        Map[5, 6].tile.GetComponent<TileManager>().isActive = false;
        Map[6, 6].tile.GetComponent<TileManager>().isActive = false;

        EraseTiles();

        
        //at some point --> write a function that takes the size of the field and rounds off the edges of the field. and fixes this^
    }

    private void EraseTiles(params Tiles[] _Target)
    {
        foreach (var e in Map)
        {
            if (e.tile.GetComponent<TileManager>().isActive != true)
            {
                Destroy(e.tile);
                //e.tile.SetActive(false);
            }
        }
    }

}
