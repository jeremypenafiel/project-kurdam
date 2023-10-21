using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum BattleState { Start, PlayerAction, EnemyMove, Busy, PlayerMove, PlayerAttackRoll, PlayerDamageRoll, EnemyAttackRoll, EnemyDamageRoll }

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
        enemyHud.SetData(enemyUnit.aswang);

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

    void PlayerAttackRoll()
    {
        state = BattleState.PlayerAttackRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice to attack."));
        diceHud.SetText("Attack Roll");
        diceHud.gameObject.SetActive(true);
    }

    void PlayerDamageRoll()
    {
        state = BattleState.PlayerDamageRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice for damage."));
        diceHud.SetText("Damage Roll");
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
        }else if(state == BattleState.PlayerAttackRoll)
        {
            HandleAttackRoll(d6);

        }else if(state==BattleState.PlayerDamageRoll)
        {
            HandleDamageRoll(d6);

        } 
    }



  

    IEnumerator RollDialog(bool isHit, BattleUnit unit)
    {
        string text;
        if (unit == playerUnit)
        {
            text = isHit ? "You hit the enemy!" : "You missed the enemy.";
        }
        else
        {
            text = isHit ? "The enemy hit you!" : "The enemy missed.";
        }
            
        yield return StartCoroutine(dialogBox.TypeDialog(text));
        yield return new WaitForSeconds(1f);    
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
            PlayerAttackRoll();
        }
    }
    private void HandleAttackRoll(Dice dice)
    {  
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformPlayerAttackRoll(dice, enemyUnit));
        }
    }

    private void HandleDamageRoll(Dice dice)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformPlayerDamageRoll(dice, enemyUnit));
        }

    }

    private void HandleEnemyMove()
    {
        
    }



    private IEnumerator PerformPlayerAttackRoll(Dice dice, BattleUnit targetUnit)
    {
        bool isHit;
        state = BattleState.Busy;
        yield return StartCoroutine(dice.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"You rolled {dice.Base.ReturnedSide}."));
        yield return new WaitForSeconds(1f);
        isHit = CheckIfHit(dice, targetUnit);
        yield return StartCoroutine(RollDialog(isHit, playerUnit));
        if (isHit)
        {
            PlayerDamageRoll();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator PerformPlayerDamageRoll(Dice dice, BattleUnit targetUnit)
    {
        state = BattleState.Busy;
        Moves move = playerUnit.aswang.moves[currentMove];
        yield return StartCoroutine(dice.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"You rolled {dice.Base.ReturnedSide}."));
        yield return new WaitForSeconds(1f);
        int damage = dice.Base.ReturnedSide;
        bool isDead = targetUnit.aswang.TakeDamage(move, playerUnit.aswang, damage);
        enemyHud.UpdateHP();
        if (isDead)
        {
            yield return StartCoroutine(dialogBox.TypeDialog($"You defeated the enemy {targetUnit.aswang.Base.aname}!"));
            yield return new WaitForSeconds(1f);
            //end battle
        }
        else
        {
            StartCoroutine(EnemyMove());
        }   
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        Moves move = enemyUnit.aswang.GetRandomMove();
        Dice moveDice = move.Base.Dice;
        bool isHit;

        yield return (dialogBox.TypeDialog("Enemy is attacking."));
        yield return new WaitForSeconds(1f);
        diceHud.SetText("Attack Roll");
        yield return StartCoroutine(d20.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"Enemy rolled {d20.Base.ReturnedSide}."));
        yield return new WaitForSeconds(1f);


        isHit = CheckIfHit(d20, playerUnit);
        yield return StartCoroutine(RollDialog(isHit, enemyUnit));
        if (isHit)
        {
            diceHud.SetText("Damage Roll");
            yield return StartCoroutine(moveDice.RollTheDice());
            yield return StartCoroutine(dialogBox.TypeDialog($"Enemy rolled {moveDice.Base.ReturnedSide}."));
            yield return new WaitForSeconds(1f);

            int damage = moveDice.Base.ReturnedSide;
            bool isDead = playerUnit.aswang.TakeDamage(move, enemyUnit.aswang, damage);
            playerHud.UpdateHP();

            if (isDead)
            {
                yield return StartCoroutine(dialogBox.TypeDialog($"You were defeated by the enemy {enemyUnit.aswang.Base.aname}!"));
                yield return new WaitForSeconds(1f);
                //end battle
            }
            else
            {
                ActionSelection();
            }
        }
        else
        {
            ActionSelection();
        }

    }



    private bool CheckIfHit(Dice dice, BattleUnit target )
    {
        return dice.Base.ReturnedSide >= target.aswang.armorClass;

    }
}
