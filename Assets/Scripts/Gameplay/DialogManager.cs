using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public bool IsShowing { get; private set; }



    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        IsShowing = true;
        dialogBox.SetActive(true);

        foreach(var line in dialog.Lines)
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
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


    public void HandleUpdate()
    {
      
    }
}
