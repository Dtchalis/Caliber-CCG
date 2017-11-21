using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// variables commented out means the functionality hasn't been implemented yet
/// </summary>
[System.Serializable]
public class Cards {

    public string name;
    public int caliber;
    public enum defaultType { STR, WIS, PSY };
    public defaultType defType;
    public int defaultAmount;
    public enum cardTypes { None, Attack, Move, Support };
    public cardTypes cardType;

    //Attack Card Variables
    public int damage;
    public Vector2?[] AttackScope = new Vector2?[6];
    public enum ScopePoints { P1, P2, P3, P4, P5, P6};

    //Move Card Variables
    public Vector3 moveDistance; //one Archon can't move more than 6 spaces so throw error if sum of vector is > 6
    public enum directions { F, FR, BR, B, BL, FL } //clockwise
    public bool[] isDirectionViable = new bool[6];

    //Support Card Variables
    public enum actionTypes { None, Auto, Ignition, Conditional, Reactive };
    public actionTypes actType;
    public enum costTypes { None, STR, WIS, PSY};
    public costTypes costType;
    public int costAmount;

    //cfx --> card effect
    public enum cfxDurationTypes { None, Turns, Rounds };
    public cfxDurationTypes cfxDurationType;
    public int cfxDurationAmount;
    public string cfxDescription;

    public static int numCards = 0;

    //based on type of card hide/show certain attributes in inspector

    /// <summary>
    /// Card --> Constructor
    /// </summary>
    public Cards()
    {
        name = null;
        caliber = 0;
        defType = 0;
        defaultAmount = 0;

        cardType = cardTypes.None;

        damage = 0;
        for (int i = 0; i < AttackScope.Length; i++)
        {
            AttackScope[i] = null;
        }

        moveDistance = new Vector3(0,0,0);
        for (int i = 0; i < isDirectionViable.Length; i++)
        {
            isDirectionViable[i] = true;
        }

        actType = 0;
        costType = 0;
        costAmount = 0;

        cfxDurationType = 0;
        cfxDurationAmount = 0;
}

    public Cards(params Vector2?[] points)
    {
        
    }


    /// <summary>
    /// Attack Card --> Constructor
    /// </summary>
    /// <param name="_name"> The name of the card </param>
    /// <param name="_caliber"> The amount of points required to set the card </param>
    /// <param name="_defType"> The type of points derived from defaulting the card </param>
    /// <param name="_defAmount"> The amount of points derived from defaulting the card </param>
    /// <param name="_damage"> The amount of damage dealt by an attack card </param>
    /// <param name="_P1"> The first area in which a target can/will recieve damage from an attack card </param>
    /// <param name="_P2"> The second area in which a target can/will recieve damage from an attack card </param>
    /// <param name="_P3"> The third area in which a target can/will recieve damage from an attack card </param>
    /// <param name="_P4"> The fourth area in which a target can/will recieve damage from an attack card </param>
    /// <param name="_P5"> The fifth area in which a target can/will recieve damage from an attack card </param>
    /// <param name="_P6"> The final area in which a target can/will recieve damage from an attack card </param>
    public Cards(string _name, int _caliber, defaultType _defType, int _defAmount, int _damage, params Vector2?[] _point)
    {
        name = _name;
        caliber = _caliber;
        defType = _defType;
        defaultAmount = _defAmount;

        cardType = cardTypes.Attack;

        damage = _damage;
        AttackScope[(int)ScopePoints.P1] = _point[(int)ScopePoints.P1];
        AttackScope[(int)ScopePoints.P2] = _point[(int)ScopePoints.P2];
        AttackScope[(int)ScopePoints.P3] = _point[(int)ScopePoints.P3];
        AttackScope[(int)ScopePoints.P4] = _point[(int)ScopePoints.P4];
        AttackScope[(int)ScopePoints.P5] = _point[(int)ScopePoints.P5];
        AttackScope[(int)ScopePoints.P6] = _point[(int)ScopePoints.P6];

        numCards++;
    }

    /// <summary>
    /// Move Card --> Constructor
    /// </summary>
    /// <param name="_name"> The name of the card </param>
    /// <param name="_caliber"> The amount of points required to set the card </param>
    /// <param name="_defType"> The type of points derived from defaulting the card </param>
    /// <param name="_defAmount"> The amount of points derived from defaulting the card </param>
    /// <param name="_moveDistance"> The travel distance enabled by a movement card - the sum of the x, y and z components can not exceed a value of 6 </param>
    /// <param name="_F"> Determines whether or not the card enables movement going forward </param>
    /// <param name="_FR"> Determines whether or not the card enables movement going forward and to the right </param>
    /// <param name="_BR"> Determines whether or not the card enables movement going backward and to the right </param>
    /// <param name="_B"> Determines whether or not the card enables movement going backward </param>
    /// <param name="_BL"> Determines whether or not the card enables movement going backward and to the left </param>
    /// <param name="_FL"> Determines whether or not the card enables movement going forward and to the left </param>
    public Cards(string _name, int _caliber, defaultType _defType, int _defAmount, Vector3 _moveDistance, params bool[] _direction)
    {
        name = _name;
        caliber = _caliber;
        defType = _defType;
        defaultAmount = _defAmount;

        cardType = cardTypes.Move;

        moveDistance = _moveDistance; //this can't be larger than 6.
        isDirectionViable[(int)directions.F] = _direction[(int)directions.F];
        isDirectionViable[(int)directions.FR] = _direction[(int)directions.FR];
        isDirectionViable[(int)directions.BR] = _direction[(int)directions.BR];
        isDirectionViable[(int)directions.B] = _direction[(int)directions.B];
        isDirectionViable[(int)directions.BL] = _direction[(int)directions.BL];
        isDirectionViable[(int)directions.FL] = _direction[(int)directions.FL];

        numCards++;
    }

    /// <summary>
    /// Support Card --> Constructor
    /// </summary>
    public Cards(string _name, int _caliber, defaultType _defType, int _defAmount, actionTypes _actType, costTypes _costType, int _costAmount, cfxDurationTypes _cfxDurationType, int _cfxDurationAmount) 
    {//add functionality for at least 2 costTypes/amounts to be used
        name = _name;
        caliber = _caliber;
        defType = _defType;
        defaultAmount = _defAmount;

        cardType = cardTypes.Support;

        actType = _actType;
        costType = _costType;
        costAmount = _costAmount;

        cfxDurationType = _cfxDurationType;
        cfxDurationAmount = _cfxDurationAmount;

        numCards++;
    }

}






