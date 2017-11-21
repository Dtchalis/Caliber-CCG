using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection that defines every available card in the game
/// </summary>
[System.Serializable]
public class Library {

    public List<Cards> cardLib = new List<Cards>();
    public List<Archons> archonLib = new List<Archons>();

    /// <summary>
    /// Card + Archon Library --> Constructor
    /// </summary>
    public Library()
    {
        ///Card Library --> 11 Cards, 5 ATK, 3 MOV,  3 SUP
        { 
        cardLib.Insert(0, new Cards("Rorshach's Complex", 3, Cards.defaultType.PSY, 5, Cards.actionTypes.Conditional, Cards.costTypes.PSY, 6, Cards.cfxDurationTypes.Rounds, 2));
        cardLib.Insert(1, new Cards("Akashik Lambaste", 4, Cards.defaultType.WIS, 4, Cards.actionTypes.Conditional, Cards.costTypes.PSY, 6, Cards.cfxDurationTypes.Turns, 3));
        cardLib.Insert(2, new Cards("The Hanuman Armature", 4, Cards.defaultType.WIS, 5, new Vector3(1, 4, 1), false, false, true, true, false, true));
        cardLib.Insert(3, new Cards("Throne of Artorius", 4, Cards.defaultType.STR, 4, 5, new Vector2(0,1), null, null, null, null, null));
        cardLib.Insert(4, new Cards("Susano'o, The Glass Glaive", 3, Cards.defaultType.PSY, 4, 4, new Vector2(-2, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(2,0), new Vector2(3,1), null));
        cardLib.Insert(5, new Cards("Beyond the Isthmus", 3, Cards.defaultType.STR, 5, new Vector3(2, 1, 3), true, true, false, true, false, false));
        cardLib.Insert(6, new Cards("Obsidian Rhapsody", 5, Cards.defaultType.WIS, 4, 3, new Vector2(-1, 1), new Vector2(2, 1), new Vector2(-1, 0), null, null, null));
        cardLib.Insert(7, new Cards("Fleeting Embers", 2, Cards.defaultType.PSY, 2, Cards.actionTypes.Reactive, Cards.costTypes.WIS, 3, Cards.cfxDurationTypes.Rounds, 2));
        cardLib.Insert(8, new Cards("Fafnir's Dance", 2, Cards.defaultType.STR, 3, new Vector3(1,1,3), true, false, false, true, false, true));
        cardLib.Insert(9, new Cards("Lance of Longinus", 3, Cards.defaultType.WIS, 5, 3, new Vector2(-3,0), new Vector2(3,0), new Vector2(0,-3), new Vector2(0, 3), new Vector2(-1, 0), new Vector2(1, 0)));
        cardLib.Insert(10, new Cards("Radiant Accel", 4, Cards.defaultType.PSY, 4, 3, new Vector2(-1, 2), new Vector2(-1, 1), new Vector2(1, 2), new Vector2(1, 1), new Vector2(0, -1), null));
        }

        ///Archon Library --> 5 Archons, 1 of each
        { 
        archonLib.Insert(0, new Archons("Artorius", 9, 1, Archons.Classes.Noble));
        archonLib.Insert(1, new Archons("Susano'o", 12, 2, Archons.Classes.Knight));
        archonLib.Insert(2, new Archons("Hermes", 7, 1, Archons.Classes.Theif));
        archonLib.Insert(3, new Archons("Kresnik", 15, 3, Archons.Classes.Sorcerer));
        archonLib.Insert(4, new Archons("Lakshimi", 10, 2, Archons.Classes.Cleric));
        }
    }
}
