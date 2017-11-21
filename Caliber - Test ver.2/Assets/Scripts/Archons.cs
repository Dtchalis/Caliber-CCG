using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Archons {
    public static int numArchons;
    public static int numActive;

    public string name;
    public int maxHP, currentHP, rank;
    public int currentXP, nextXP, maxXP;
    public Vector2? weakSpot;
    public Vector2? currentSpot;
    public enum Classes { Theif, Knight, Sorcerer, Cleric, Noble};
    public Classes Class;

    /// <summary>
    /// Constructor that handles the data for each archon in the game
    /// </summary>
    /// <param name="_name"> The name of the Archon </param>
    /// <param name="_HP"> The total HP of the Archon</param>
    /// <param name="_Rank"> The rank of the Archon </param>
    /// <param name="_Class">The class of the Archon </param>
    public Archons(string _name, int _HP, int _Rank, Classes _Class)
    {
        name = _name;
        maxHP = _HP;
        rank = _Rank;
        Class = _Class;

        //Status.currentXP = 0;
        //Status.nextXP = 0;
        //Status.totalXP = 0;
        //Status.weakSpot = returnPosition();
        //Status.currentSpot = Status.weakSpot;

        //States.isArchonVulnerable = null;
        //States.isArchonInPlay = null;
        

        numArchons++;
    }
}
