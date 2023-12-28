using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] int lettersPerSecond;


    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void ShowDialog(Dialog dialog)
    {
        dialogBox.SetActive(true);
        /*dialogText.text = dialog.Lines[0];*/

        StartCoroutine(TypeDialog(dialog.Lines[0]));


    }

    public IEnumerator TypeDialog(string line)
    {
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
