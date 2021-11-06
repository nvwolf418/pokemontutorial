using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//for get component
using UnityEngine.UI;
//for image animations, monsters images coming in to fight
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    //removed these to hardcode
    //[SerializeField] PokemonBase baseStats;
    //[SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit;  }
    }

    public BattleHud Hud
    {
        get { return hud; }
    }

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        //we are using the local position for its frame of reference form canvas, not the world
        originalPos = image.transform.localPosition;
        originalColor = image.color;

    }
    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Pokemon.BaseStats.BackSprite;
        }
        else
        {
            GetComponent<Image>().sprite = Pokemon.BaseStats.FrontSprite;
        }


        hud.SetData(pokemon);

        //so when new battle starts up the fainted alpha to zero is reset
        image.color = originalColor;

       PlayerEnterAnimation();
    }

    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveY(originalPos.y + 20f, 0.25f));
            sequence.Join(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        } 
        else
        {
            sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 20f, 0.25f));
            sequence.Join(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
        sequence.Join(image.transform.DOLocalMoveY(originalPos.y, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        //join runs the previous sequence together!!
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
