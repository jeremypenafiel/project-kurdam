using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceHud : MonoBehaviour
{
    [SerializeField] private Dice d6;
    [SerializeField] private Dice d8;
    [SerializeField] private Dice d20;
    [SerializeField] TextMeshPro textMesh;
    private CanvasRenderer canvas;

    


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<CanvasRenderer>();
        //canvas.enabled = false;
    }

    public void setDice()
    {
        d6.setDice(true);
    }

    void setText(string text)
    {
        this.textMesh.text = text;
    }
}
