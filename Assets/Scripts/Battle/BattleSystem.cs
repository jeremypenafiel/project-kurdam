using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum BattleState { Start, PlayerAction, EnemyMove, Busy, PlayerMove, AttackRoll, DamageRoll }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] DiceHud diceHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogBox;
    [SerializeField] Dice d6;
    [SerializeField] Dice d20;

    BattleState state;
    int currentAction;
    int currentMove;


    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        diceHud.gameObject.SetActive(false);
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.aswang);

        dialogBox.SetMoveNames(playerUnit.aswang.moves);

        yield return dialogBox.TypeDialog($"A wild {playerUnit.aswang.Base.aname} appeared.");
        yield return new WaitForSeconds(1f);
        
        ActionSelection();

    }

    void ActionSelection()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose An Action"));
        dialogBox.EnableActionSelector(true);
    }

    void MoveSelection()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }



    void AttackRoll()
    {
        state = BattleState.AttackRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice to attack."));   
        diceHud.gameObject.SetActive(true);
    }

    void DamageRoll()
    {
        state = BattleState.DamageRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice for damage."));
    }

    IEnumerator RollDice(Dice dice)
    {
        yield return StartCoroutine(dice.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"You rolled  {dice.Base.ReturnedSide}."));
    }

    IEnumerator RollingAttackDice(Dice dice, BattleUnit target)
    {
        bool isHit;
        yield return RollDice(dice);
        yield return new WaitForSeconds(1f);
        isHit = CheckIfHit(dice, target);
      
        yield return RollDialog(isHit);
        yield return new WaitForSeconds(1f);
        if (isHit)
        {
            DamageRoll();
        }
        else
        {
            StartCoroutine(EnemyMove(dice));
        }

    }

    IEnumerator EnemyMove(Dice dice)
    {
        state = BattleState.EnemyMove;
        yield return (dialogBox.TypeDialog("Enemy is attacking."));
        /*var move = enemyUnit.aswang.RandomMove();
        state = BattleState.AttackRoll;
        yield return RollDice(dice);*/

    }
    private IEnumerator RollingDamageDice(Dice dice)
    {
        state = BattleState.Busy;
        Moves move = playerUnit.aswang.moves[currentMove];
        yield return RollDice(dice);
        yield return new WaitForSeconds(1f);
        bool isDead = enemyUnit.aswang.TakeDamage(move, playerUnit.aswang, dice.Base.ReturnedSide);

        if(isDead)
        {
            yield return StartCoroutine(dialogBox.TypeDialog($"{enemyUnit.aswang.Base.name} died"));
        }
        else
        {
            EnemyMove(d6);
        }   
    }


    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }

        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }else if(state == BattleState.AttackRoll)
        {
            HandleAttackRoll(d20);

        }else if(state==BattleState.DamageRoll)
        {
            HandleDamageRoll(d6);

        } else if (state == BattleState.EnemyMove)
        {
            HandleEnemyMove();
        }
    }

    private void HandleEnemyMove()
    {
        Debug.Log("enemy move");
    }

  

    IEnumerator RollDialog(bool isHit)
    {
        if (isHit)
        {
            yield return StartCoroutine(dialogBox.TypeDialog($"You hit the enemy!"));
        }
        else
        {
            yield return StartCoroutine(dialogBox.TypeDialog($"You missed the enemy!"));

        }
    }

   

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                //run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.aswang.moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.aswang.moves.Count - 2)
            {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            AttackRoll();
            /*if (currentMove == 0)
            {
                /
            }
            else if (currentMove == 1)
            {
                //run
            }*/
        }
    }
    private void HandleAttackRoll(Dice dice)
    {   
        bool isHit;
        diceHud.SetText(state);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(RollingAttackDice(dice, enemyUnit));
        }

    }

    private void HandleDamageRoll(Dice dice)
    {
        diceHud.SetText(state);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(RollingDamageDice(dice));
        }

    }


    private bool CheckIfHit(Dice dice, BattleUnit target )
    {
        return dice.Base.ReturnedSide >= target.aswang.armorClass;

    }
}
