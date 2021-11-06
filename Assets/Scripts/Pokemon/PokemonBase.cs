using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This allows for the option to create new pokemon in the create window to fast create. The menu you would create a new C#script for example is where it is located.
[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase: ScriptableObject
{
    //serialize field needs to be used outside of this class, public is bad practice, we need to expose these by functions/properties
    [SerializeField] string pokeName;

    //text area so that you have some space to type in the description
    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType typeOne;
    [SerializeField] PokemonType typeTwo;

    //base stats of pokemon
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string PokeName
    {
        get { return pokeName; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PokemonType TypeOne
    {
        get { return typeOne; }
    }

    public PokemonType TypeTwo
    {
        get { return typeTwo; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpDefense
    {
        get { return defense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

//doesn't need to be directly serializble , but will need to be when referenced elsewhere like above, to show up in inspector in unity ui, need the system.serializable
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }
}


public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed
}

public class TypeChart
{
    static float[][] chart =
   {//            NOR FIR WAT ELE GRA ICE FIG POI GRO FLY PSY BUG ROC GHO
     /*NOR*/ new float[]{ 1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,0.5f,0f},
     /*FIR*/ new float[]{ 1f,0.5f,0.5f,1f,2f,2f,1f,1f,1f,1f,1f,2f,0.5f,1f},
     /*WAT*/ new float[]{ 1f,2f,0.5f,1f,0.5f,1f,1f,1f,2f,1f,1f,1f,2f,1f},
     /*ELE*/ new float[]{ 1f,2f,0.5f,0.5f,1f,1f,1f,0f,2f,1f,1f,1f,1f,1f},
     /*GRA*/ new float[]{ 1f,0.5f,2f,1f,0.5f,1f,1f,0.5f,2f,0.5f,1f,0.5f,2f,1f},
     /*ICE*/ new float[]{ 1f,0.5f,0.5f,1f,2f,0.5f,1f,1f,2f,2f,1f,1f,1f,1f},
     /*FIG*/ new float[]{ 2f,1f,1f,1f,1f,2f,1f,0.5f,1f,0.5f,0.5f,0.5f,2f,0f},
     /*POI*/ new float[]{ 1f,1f,1f,1f,2f,1f,1f,0.5f,0.5f,1f,1f,1f,0.5f,0.5f},
     /*GRO*/ new float[]{ 1f,2f,1f,2f,0.5f,1f,1f,2f,0f,1f,0.5f,2f,1f,1f},
     /*FLY*/ new float[]{ 1f,1f,1f,0.5f,2f,1f,2f,1f,1f,1f,1f,2f,0.5f,1f},
     /*PSY*/ new float[]{ 1f,1f,1f,1f,1f,1f,2f,2f,1f,1f,0.5f,1f,1f,1f},
     /*BUG*/ new float[]{ 1f,0.5f,1f,1f,2f,1f,0.5f,0.5f,1f,0.5f,2f,1f,1f,0.5f},
     /*ROC*/ new float[]{ 1f,2f,1f,1f,1f,2f,0.5f,1f,0.5f,2f,1f,2f,1f,1f},
     /*GHO*/ new float[]{ 0f,1f,1f,1f,1f,1f,1f,1f,1f,1f,2f,1f,1f,2f } 
     };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if(attackType == PokemonType.None || defenseType == PokemonType.None)
        {
            return 1;
        }

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }

}
