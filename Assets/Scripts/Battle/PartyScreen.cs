using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;
    
    
    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;

    public void Init() 
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if(i < pokemons.Count)
            {
                memberSlots[i].SetData(pokemons[i]);
            } 
            else
            {
                //disable if pokemon list is say 3 instead of siix, disable last 3
                memberSlots[i].gameObject.SetActive(false);
            }

            messageText.text = "Choose a Pokemon";
        }
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int itt = 0; itt < pokemons.Count; itt++)
        {
            if(itt == selectedMember)
            {
                memberSlots[itt].SetSelected(true);
            }
            else
            {
                memberSlots[itt].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string msg)
    {
        messageText.text = msg;
    }
}

