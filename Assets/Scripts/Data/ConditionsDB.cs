using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                   pokemon.UpdateHP(pokemon.MaxHp/8);
                   pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} hurt itself due to poison");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    Debug.Log(pokemon.HP + "before burn");
                   pokemon.UpdateHP(pokemon.MaxHp/16);
                    Debug.Log(pokemon.HP + "afeter burn");
                   pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} hurt itself due to burn");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been paralyzed.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} is paralyzed, it can't move!");
                        return false;
                    }
                    return  true;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "has been frozen.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} is not frozen anymore, it can move!");
                        return true;
                    }

                    return  false;
                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep.",
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for a random number of turns 1-3
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves.");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} has risen!");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.BaseStats.PokeName} is sleeping!");
                    return false;
                }
            }
        }
    };
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz
}