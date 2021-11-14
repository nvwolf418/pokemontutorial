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
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    
    Pokemon _pokemon;
    Dictionary<ConditionID, Color> statusColors;

    //can't use get and set above, won't be able to access via unity ui
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.BaseStats.PokeName;
        levelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);

        statusColors = new Dictionary<ConditionID, Color>()
        {
            { ConditionID.psn, psnColor },
            { ConditionID.brn, brnColor },
            { ConditionID.slp, slpColor },
            { ConditionID.par, parColor },
            { ConditionID.frz, frzColor }
        };

        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }

    public void SetStatusText()
    {
        if(_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChanged)
        {
            yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp);
            _pokemon.HpChanged = false;
        }


    }
}
