using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovesChangeDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    

    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;
    [SerializeField] List<TextMeshProUGUI> moveChangeTexts;
    
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI typeText;




    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / 30);
        }
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

    public void UpdateChangeSelection(int selectedAction, Moves move)
    {
        for (int i = 0; i < moveChangeTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                moveChangeTexts[i].color = Color.blue;
            }
            else
            {
               moveChangeTexts[i].color = Color.white;
            }
            description.text= $"{move.Base.Description}";
            attackText.text = $"{move.Base.RollNumber}{move.Base.DiceBase.name}";
            typeText.text = $"{move.Base.Type.GetModifierText().ToUpper()}";
        }
    }
    public void UpdateChangeSelection()
    {
        for (int i = 0; i < moveChangeTexts.Count; i++)
        {

           moveChangeTexts[i].color = Color.white;
            
        }
    }

    public void UpdateMoveSelection(int selectedMove, Moves move)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (i == selectedMove)
            {
                moveTexts[i].color = Color.red;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
            description.text= $"{move.Base.Description}";
            attackText.text = $"{move.Base.RollNumber}{move.Base.DiceBase.name}";
            typeText.text = $"{move.Base.Type.GetModifierText().ToUpper()}";
        }
    }

    public void SetMoveNames(List<Moves> moves)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.MoveName;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

    public void SetMoveChangeNames(List<Moves> moves)
    {
        for (int i = 0; i < moveChangeTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveChangeTexts[i].text = moves[i].Base.MoveName;
            }
            else
            {
                moveChangeTexts[i].text = "-";
            }
        }
    }
}
