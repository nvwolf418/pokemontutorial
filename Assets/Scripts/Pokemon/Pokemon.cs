using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classes only shown in inspector if we use this attribute
[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase BaseStats
    {
        get { return _base; }
    }
    public int Level
    {
        get { return level; }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public bool HpChanged { get; set; }

    //this definition changed as we dont need it as we wont use intsructor if calling from inspector
    //public Pokemon(PokemonBase baseStats, int level)
    public void Init()
    {
        //these are removed as we add pokemon to our party, the details will be added via the inspector
        //this.BaseStats = baseStats;
        //this.Level = level;



        Moves = new List<Move>();

        foreach (var move in BaseStats.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4)
            {
                break;
            }
        }

        CalculateStats();

        HP = MaxHp;


        ResetStatBoosts();
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((BaseStats.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((BaseStats.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((BaseStats.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((BaseStats.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((BaseStats.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((BaseStats.MaxHp * Level) / 100f) + 10;
    }


    void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
        };
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        //apply stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if(boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }
        return statVal;
    }



    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach(var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if(boost > 0)
            {
                StatusChanges.Enqueue($"{BaseStats.PokeName}'s {stat} increased!");
            }
            else
            {
                StatusChanges.Enqueue($"{BaseStats.PokeName}'s {stat} decreased!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");

        }
    }
/*
 * This calculates the attack stat per level in pokemon game
 */
public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    /*
     * This calculates the defense stat per level in pokemon game
     */
    public int Defense
    {
        get { return GetStat(Stat.Defense); ; }
    }

    /*
     * This calculates the special attack stat per level in pokemon game
     */
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }

    /*
     * This calculates the special defense stat per level in pokemon game
     */
    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); }
    }

    /*
     * This calculates the speed stat per level in pokemon game
     */
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    /*
     * This calculates the Max HP stat per level in pokemon game
     */
    public int MaxHp { get; private set; }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        //apply critical if possible
        float critical = Random.value <= .0625f ? 1f : 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.BaseStats.TypeOne) * TypeChart.GetEffectiveness(move.Base.Type, this.BaseStats.TypeTwo);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false,
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? attacker.SpDefense : attacker.Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float) attack / defense)  + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);
       

        return damageDetails;
    }

    public void UpdateHP(int dmg)
    {

        HP = Mathf.Clamp(HP - dmg, 0, MaxHp);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionID)
    {
        Status = ConditionsDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{BaseStats.PokeName} {Status.StartMessage}");
    }

    public void CureStatus()
    {
        Status = null;
    }


    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
    }

    public bool OnBeforeMove()
    {
        if(Status?.OnBeforeMove != null)
        {
            return Status.OnBeforeMove(this);
        }

        return true;
    }
    public void OnBattleOver()
    {
        ResetStatBoosts();
    }


}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
