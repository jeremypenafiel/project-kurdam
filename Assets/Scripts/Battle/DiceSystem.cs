using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiceSystem : MonoBehaviour
{
    [SerializeField] Dice d6;
    [SerializeField] Dice d20;
    [SerializeField] DiceHud diceHud;

    private Dice currentDice;

    public void Setup()
    {
        diceHud.gameObject.SetActive(false);
        d20.gameObject.SetActive(false);
        d6.gameObject.SetActive(false);
    }

    public void SetDiceHudText(string text)
    {
        diceHud.SetText(text);
    }

    public Dice CurrentDice
    {
        get => currentDice;

        set => currentDice = value;
    }

    public void SetupAttackRoll()
    {
        SetDiceHudText("Attack Roll");
        if (!diceHud.gameObject.activeSelf) diceHud.gameObject.SetActive(true);

        if (CurrentDice != d20)
        {
            CurrentDice?.gameObject.SetActive(false);
            d20.gameObject.SetActive(true);
            CurrentDice = d20;
        }
    }

    public IEnumerator AttackRoll()
    {
        return CurrentDice.RollTheDice();
    }


    public void SetupDamageRoll(Moves move)
    {
        CurrentDice?.gameObject.SetActive(false);
        SetDiceHudText("Damage Roll");
        CurrentDice = GetDice(move);
        CurrentDice.gameObject.SetActive(true);
    }

    public IEnumerator DamageRoll()
    {
        return CurrentDice.RollTheDice();
    }

    public int GetDiceRollValue()
    {
        return CurrentDice.Base.ReturnedSide;
    }

    private Dice GetDice(Moves move)
    {
        switch (move.Base.DiceBase.Sides)
        {
            case 6:
                return d6;
            case 20:
                return d20;
            default:
                return d6;
        }
    }

    public void DisableHud()
    {
        diceHud.gameObject.SetActive(false);
        CurrentDice?.gameObject.SetActive(false);
    }



}
