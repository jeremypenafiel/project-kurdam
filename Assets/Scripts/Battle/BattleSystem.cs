using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
    int attackRoll;


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
        
        PlayerAction();

    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose An Action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
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
            StartCoroutine(HandleAttackRoll(d20));
        }else if(state==BattleState.DamageRoll)
        {
            HandleDamageRoll(d20);
        }
    }

    private void HandleDamageRoll(Dice d20)
    {
        throw new NotImplementedException();
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
                PlayerMove();
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
    private IEnumerator HandleAttackRoll(Dice dice)
    {
        bool isHit;
        diceHud.SetText(state);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            yield return StartCoroutine(dice.RollTheDice());
            yield return StartCoroutine(dialogBox.TypeDialog($"You rolled a {dice.ReturnedSide}."));
            isHit =  CheckIfHit(dice);

            if (isHit)
            {
                yield return StartCoroutine(dialogBox.TypeDialog($"You hit the enemy!"));
                state = BattleState.DamageRoll;
            }
            else
            {
                yield return StartCoroutine(dialogBox.TypeDialog($"You missed the enemy!"));
                state = BattleState.EnemyMove;
            }
        }
    }

    private bool CheckIfHit(Dice dice)
    {
        return dice.ReturnedSide >= enemyUnit.aswang.armorClass;
    }
}
