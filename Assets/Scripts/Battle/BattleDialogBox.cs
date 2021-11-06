using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//need for text object
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //to make letters appear at once, we make a coroutine
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int itt = 0; itt < actionTexts.Count; ++itt)
        {
            if(itt == selectedAction)
            {
                actionTexts[itt].color = highlightedColor;
            } 
            else
            {
                actionTexts[itt].color = Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int itt = 0; itt < moveTexts.Count; ++itt)
        {
            if (itt == selectedMove)
            {
                moveTexts[itt].color = highlightedColor;
            }
            else
            {
                moveTexts[itt].color = Color.black;
            }
        }

        ppText.text = $"PP {move.PP}/ {move.Base.PP}";
        typeText.text = move.Base.Type.ToString();
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int itt = 0; itt < moveTexts.Count; ++itt)
        {
            if (itt < moves.Count)
            {
                moveTexts[itt].text = moves[itt].Base.MoveName;
            }
            else
            {
                moveTexts[itt].text = "--";
            }
        }
    }

}
