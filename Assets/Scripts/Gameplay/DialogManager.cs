using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] int lettersPerSecond;
    int currentLine = 0;
    Dialog dialog;
    bool isTyping;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;



    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);

        StartCoroutine(TypeDialog(dialog.Lines[0]));


    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;

        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }


    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }
}
