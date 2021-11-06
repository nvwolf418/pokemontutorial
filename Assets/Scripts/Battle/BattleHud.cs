using System.Collections;
using System.Collections.Generic;
//this allows us to reference ui, see line 9
using UnityEngine.UI;
using UnityEngine;

public class BattleHud : MonoBehaviour
{
    //allows them to be accessed via the unity ui, inspector
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Pokemon _pokemon;

    //can't use get and set above, won't be able to access via unity ui
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.BaseStats.PokeName;
        levelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float) _pokemon.HP / _pokemon.MaxHp);

    }
}
