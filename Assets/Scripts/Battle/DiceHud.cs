using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;




public class DiceHud : MonoBehaviour
{
    /* [SerializeField] private Dice d6;*/
    /*[SerializeField] private Dice d8;
    [SerializeField] private Dice d20;*/
    /*[HideInInspector] public Dice currentDice;*/
    [SerializeField] private TextMeshProUGUI text;





    // Start is called before the first frame update
    void Start()
    {
        /* this.currentDice = d6;*/
    }


    private void Update()
    {
        /*        switch (this.state)
                {
                    case RollState.AttackRoll:

                        this.currentDice = d6;
                        this.Text.text = "Attack Roll";
                        break;
                    case RollState.DamageRoll:
                        this.currentDice = d6;
                        this.Text.text = "Damage Roll";
                        break;

                }
                this.currentDice.gameObject.SetActive(true);*/
    }

    public void SetText(string text)
    {
        this.text.text = text;


    }
}
