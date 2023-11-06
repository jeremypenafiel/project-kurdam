using DG.Tweening;
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum BattleState { Start, ActionSelection, EnemyMove, Busy, MoveSelection, PlayerAttackRoll, PlayerDamageRoll, PerformMove }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] DiceHud diceHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogueBox dialogBox;
    [SerializeField] Dice d6;
    [SerializeField] Dice d20;

    public event Action<bool> OnBattleOver;
    public event Action Run;

    BattleState state;
    int currentAction;
    int currentMove;
    Dice currentDice;

    Aswang wildAswang;
    Aswang player;
    public void StartBattle(Aswang player, Aswang wildAswang)
    {
        this.player = player;
        this.wildAswang = wildAswang;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        diceHud.gameObject.SetActive(false);
        d6.gameObject.SetActive(false);
        d20.gameObject.SetActive(false);
        playerUnit.Setup(player);
        enemyUnit.Setup(wildAswang);
        currentDice = d20;
        dialogBox.SetMoveNames(playerUnit.Aswang.moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Aswang.Base.Aname} appeared.");
        yield return new WaitForSeconds(1f);
        
        ActionSelection();

    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose An Action"));
        diceHud.gameObject.SetActive(false);
        dialogBox.EnableActionSelector(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }


    void PlayerAttackRoll()
    {
        state = BattleState.PlayerAttackRoll;
        
        StartCoroutine(dialogBox.TypeDialog("Roll the dice to attack."));
        diceHud.SetText("Attack Roll");
        if (currentDice != d20)
        {
            currentDice.gameObject.SetActive(false);
        }
        d20.gameObject.SetActive(true);
        diceHud.gameObject.SetActive(true);
    }

    void PlayerDamageRoll(Dice dice)
    {
        state = BattleState.PlayerDamageRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice for damage."));
        d20.gameObject.SetActive(false);
        dice.gameObject.SetActive(true);
        diceHud.SetText("Damage Roll");
    }

    void EnemyMove()
    {
        state = BattleState.EnemyMove;
        StartCoroutine(dialogBox.TypeDialog("Enemy is attacking."));

        diceHud.SetText("Attack Roll");
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }

        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }else if(state == BattleState.PlayerAttackRoll)
        {
            HandlePlayerAttackRoll(d20);

        }else if(state==BattleState.PlayerDamageRoll)
        {
            HandlePlayerDamageRoll();

        } else if (state==BattleState.EnemyMove)
        {
            HandleEnemyMove();
        }
    }

    private void PerformEnemyMove()
    {
        StartCoroutine(PerformAttackRoll(enemyUnit, playerUnit, currentDice));
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

                StartCoroutine(RunBattle());
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Aswang.moves.Count - 1)
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
            if (currentMove < playerUnit.Aswang.moves.Count - 2)
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    private void HandlePlayerAttackRoll(Dice dice)
    {  
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformAttackRoll(playerUnit, enemyUnit, currentDice));
        }
    }

    private void HandlePlayerDamageRoll()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformDamageRoll(playerUnit,enemyUnit, currentDice));
        }

    }
    
    private void HandleEnemyMove()
    {
        PerformEnemyMove();
    }

    IEnumerator PerformAttackRoll(BattleUnit sourceUnit, BattleUnit targetUnit, Dice previousDice)
    {
        state = BattleState.PerformMove;
        Moves move = sourceUnit.GetMove(currentMove);

        if (previousDice != d20) previousDice.gameObject.SetActive(false);
        d20.gameObject.SetActive(true);
        currentDice = d20;

        bool isHit;
        string subject = sourceUnit.GetSubject();

        yield return StartCoroutine(d20.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} rolled {d20.Base.ReturnedSide}."));
        yield return new WaitForSeconds(1f);
        isHit = CheckIfHit(d20, targetUnit);
        yield return StartCoroutine(RollDialog(isHit, sourceUnit));
        if (isHit)
        {
            if (sourceUnit.IsPlayerUnit)
            {
                PlayerDamageRoll(GetDice(move));
            }
            else
            {
                StartCoroutine(PerformDamageRoll(sourceUnit, targetUnit, d20, move));
            }
        }
        else
        {
            if (sourceUnit.IsPlayerUnit)
            {
                EnemyMove();
            }
            else
            {
                ActionSelection();
            }
        }

    }


    IEnumerator PerformDamageRoll(BattleUnit sourceUnit, BattleUnit targetUnit, Dice previousDice, Moves move)
    {
        state = BattleState.PerformMove;
        
        Dice dice = GetDice(move);
        string subject = sourceUnit.GetSubject();
        previousDice.gameObject.SetActive(false);
        dice.gameObject.SetActive(true);
        currentDice = dice;
        string modifierText = move.Base.Type.getModifierText();
        int modifier = getModifier(move.Base.Type.Modifier, sourceUnit.Aswang);

        yield return StartCoroutine(dice.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} rolled {dice.Base.ReturnedSide} + {modifier} {modifierText} modifier."));
        yield return new WaitForSeconds(1f);
        int damageRoll = dice.Base.ReturnedSide;
        int damage = CalculateTotalDamage(move, sourceUnit.Aswang, targetUnit.Aswang, damageRoll);
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} did {damage} total damage."));

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(0.5f);

        playerUnit.PlayHitAnimation();

        bool isDead = targetUnit.Aswang.TakeDamage(move, sourceUnit.Aswang, damage);
        yield return targetUnit.Hud.UpdateHP();

        if (isDead)
        {
            yield return (StartCoroutine(dialogBox.TypeDialog(targetUnit.GetDefeatText())));
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(1f);
            OnBattleOver(true);
        }
        else
        {
            if(sourceUnit.IsPlayerUnit)
            {
                EnemyMove();
            }
            else
            {
                ActionSelection();
            }
        }
    }

    IEnumerator RunBattle()
    {
        yield return StartCoroutine(dialogBox.TypeDialog("You Fled from the Aswang"));
        yield return new WaitForSeconds(1f);
        Run();
    }

    IEnumerator PerformDamageRoll(BattleUnit sourceUnit, BattleUnit targetUnit, Dice previousDice)
    {
        state = BattleState.Busy;
        Moves move = sourceUnit.GetMove(currentMove);
        string modifierText = move.Base.Type.getModifierText();
        int modifier = getModifier(move.Base.Type.Modifier, sourceUnit.Aswang);

        Dice dice = GetDice(move);
        string subject = sourceUnit.GetSubject();
        previousDice.gameObject.SetActive(false);
        dice.gameObject.SetActive(true);
        currentDice = dice;

        yield return StartCoroutine(dice.RollTheDice());
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} rolled {dice.Base.ReturnedSide} + {modifier} {modifierText} modifier."));
        yield return new WaitForSeconds(1f);
        int damageRoll = dice.Base.ReturnedSide;
        int damage = CalculateTotalDamage(move, sourceUnit.Aswang, targetUnit.Aswang, damageRoll);
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} did {damage} total damage."));

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(0.5f);

        enemyUnit.PlayHitAnimation();

        bool isDead = targetUnit.Aswang.TakeDamage(move, sourceUnit.Aswang, damage);
        yield return targetUnit.Hud.UpdateHP();

        if (isDead)
        {
            yield return (StartCoroutine(dialogBox.TypeDialog(targetUnit.GetDefeatText())));
            enemyUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(1f);
            OnBattleOver(true);
        }
        else
        {
            if (sourceUnit.IsPlayerUnit)
            {
                EnemyMove();
            }
            else
            {
                ActionSelection();
            }
        }
    }

    private bool CheckIfHit(Dice dice, BattleUnit target )
    {
        return dice.Base.ReturnedSide >= target.Aswang.ArmorClass;

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

    private int CalculateTotalDamage(Moves move, Aswang sourceUnit, Aswang targetUnit, int damage)
    {
        Modifier playerStat = move.Base.Type.Modifier;
        int modifier;
        switch (playerStat)
        {
            case Modifier.Strength:
                modifier = sourceUnit.Strength;
                break;
            case Modifier.Dexterity:
                modifier = sourceUnit.Dexterity;
                break;
            default:
                modifier = 0;
                break;
        }

        damage += modifier;

        for (int i = 0; i < targetUnit.Base.Resistances.Count; i++)
        {
            if (targetUnit.Base.Resistances[i] == move.Base.Type)
            {
                damage = Mathf.FloorToInt(damage / 2);
                break;
            }
        }

        for (int i = 0; i < targetUnit.Base.Vulnerabilities.Count; i++)
        {
            if (targetUnit.Base.Vulnerabilities[i] == move.Base.Type)
            {
                damage = Mathf.FloorToInt(damage * 2);
                break;
            }
        }

        return damage;
    }
    
    private int getModifier(Modifier modifier, Aswang sourceUnit)
    {
        int playerModifier;

        switch (modifier)
        {
            case Modifier.Strength:
                playerModifier = sourceUnit.Strength;
                break;
            case Modifier.Dexterity:
                playerModifier = sourceUnit.Dexterity;
                break;
/*            case Modifier.Constitution:
                break;
            case Modifier.Intelligence:
                break;
            case Modifier.Wisdom:
                break;
            case Modifier.Charisma:
                break;*/
            default:
                playerModifier = 0;
                break;
        }
        return playerModifier;
    }
}
