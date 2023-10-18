using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


public enum RollState { AttackRoll, DamageRoll}
public class DiceHud : MonoBehaviour
{
    [SerializeField] private Dice d6;
    /*[SerializeField] private Dice d8;
    [SerializeField] private Dice d20;*/
    [HideInInspector] public Dice currentDice;
    private RollState state;

    


    // Start is called before the first frame update
    void Start()
    {
        this.currentDice = d6;
    }


    private void Update()
    {
        switch (this.state)
        {
            case RollState.AttackRoll:
                
                this.currentDice = d6;
                break;
            case RollState.DamageRoll:
                break;

        }
        this.currentDice.gameObject.SetActive(true);
    }

    public void RollDice()
    {
        StartCoroutine(this.currentDice.RollTheDice());
    }
}
