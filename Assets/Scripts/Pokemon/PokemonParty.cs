using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get { return pokemons; }  
    }

    private void Start()
    {
        foreach(var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }


    public Pokemon GetHealthyPokemon()
    {
        Debug.Log($"Health pokemon + {pokemons.Count}");
        //where will loop through and satisfies this condition, will return null if doesn't exist
        foreach(var pokemon in pokemons)
        {
            Debug.Log($"{pokemon.BaseStats.PokeName} has this much hp : {pokemon.HP}");
        }
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}
