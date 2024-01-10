using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { Start, ActionSelection, EnemyAttackRoll, EnemyDamageRoll, Busy, MoveSelection, PlayerAttackRoll, PlayerDamageRoll, PerformMove }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogueBox dialogBox;
    [SerializeField] DiceSystem diceSystem;

    [SerializeField] AudioClip battleMusic;
    [SerializeField] AudioClip victoryMusic;

    public event Action<bool> OnBattleOver;
    public event Action Run;
    public event Action PlayerFaint;
    public event Action Pause;
    BattleState state;
    int currentAction;
    int currentMove;

    Aswang wildAswang;
    Aswang player;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }



    public void StartBattle(Aswang player, Aswang wildAswang)
    {
        this.player = player;
        this.wildAswang = wildAswang;
        AudioManager.i.PlayMusic(battleMusic);
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        diceSystem.Setup();
        playerUnit.Setup(player);
        enemyUnit.Setup(wildAswang);
  
        dialogBox.SetMoveNames(playerUnit.Aswang.moves);


        
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Aswang.Base.Aname} appeared.");
        yield return new WaitForSeconds(1f);
        
        ActionSelection();

    }

    // State Setups
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose An Action"));
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

        diceSystem.SetupAttackRoll();
    }

    void PlayerDamageRoll(Moves move)
    {
        state = BattleState.PlayerDamageRoll;
        StartCoroutine(dialogBox.TypeDialog("Roll the dice for damage."));
        diceSystem.SetupDamageRoll(move);
    }

    void EnemyAttackRoll()
    {
        state = BattleState.EnemyAttackRoll;
        StartCoroutine(dialogBox.TypeDialog("Enemy is attacking."));

        diceSystem.SetupAttackRoll();
    }

    void EnemyDamageRoll(Moves move)
    {
        state = BattleState.EnemyDamageRoll;
        StartCoroutine(dialogBox.TypeDialog("Enemy is rolling for damage."));
        diceSystem.SetupDamageRoll(move);
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
            HandlePlayerAttackRoll();

        }else if(state==BattleState.PlayerDamageRoll)
        {
            HandlePlayerDamageRoll();

        } else if (state==BattleState.EnemyAttackRoll)
        {
            HandleEnemyAttackRoll();
        }else if(state==BattleState.EnemyDamageRoll)
        {
            HandleEnemyDamageRoll();
        }
    }


    // Enter states / Handle states
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
            AudioManager.i.PlaySFX(AudioId.UISelect);
            if (currentAction == 0)
            {
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                Flee();
                
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

            AudioManager.i.PlaySFX(AudioId.UISelect);
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAttackRoll();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
    
            AudioManager.i.PlaySFX(AudioId.UISelect);
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    private void HandlePlayerAttackRoll()
    {  
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformAttackRoll(playerUnit, enemyUnit, PlayerDamageRoll, EnemyAttackRoll));
        }
    }

    private void HandlePlayerDamageRoll()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PerformDamageRoll(playerUnit,enemyUnit, EnemyAttackRoll));
        }
    }
    private void HandleEnemyAttackRoll()
    {
        StartCoroutine(PerformAttackRoll(enemyUnit, playerUnit, EnemyDamageRoll, PlayerAttackRoll));
    }

    private void HandleEnemyDamageRoll()
    {
        StartCoroutine(PerformDamageRoll(enemyUnit, playerUnit, ActionSelection));
    }

    // Performing the Attack/Damage Rolls
     IEnumerator PerformAttackRoll(BattleUnit sourceUnit, BattleUnit targetUnit, Action<Moves> onHit, Action onMiss)
    {
        state = BattleState.PerformMove;
        AudioManager.i.PlaySFX(AudioId.UISelect);
        Moves move = sourceUnit.GetMove(currentMove);

        bool isHit;
        string subject = sourceUnit.GetSubject();

        yield return StartCoroutine(diceSystem.AttackRoll());
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} rolled {diceSystem.GetDiceRollValue()}."));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        AudioManager.i.PlaySFX(AudioId.UISelect);


        isHit = CheckIfHit(diceSystem.CurrentDice, targetUnit);
        yield return StartCoroutine(RollDialog(isHit, sourceUnit));

        if (isHit) 
            onHit?.Invoke(move);
        else 
            onMiss?.Invoke();

    }

    IEnumerator PerformDamageRoll(BattleUnit sourceUnit, BattleUnit targetUnit, Action onDamageRollOver)
    {
        state = BattleState.Busy;
        AudioManager.i.PlaySFX(AudioId.UISelect);

        Moves move = sourceUnit.GetMove(currentMove);
        var moveSfx = move.Base.Sound;
        string modifierText = move.Base.Type.GetModifierText();
        int modifier = GetModifier(move.Base.Type.Modifier, sourceUnit.Aswang);
        string subject = sourceUnit.GetSubject();

        yield return StartCoroutine(diceSystem.DamageRoll());
        int damageRoll = diceSystem.GetDiceRollValue();

        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} rolled {damageRoll} + {modifier} {modifierText} modifier."));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        AudioManager.i.PlaySFX(AudioId.UISelect);

        int damage = CalculateTotalDamage(move, sourceUnit.Aswang, targetUnit.Aswang, damageRoll);

        AudioManager.i.PlaySFX(moveSfx);
        

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(0.5f);


        targetUnit.PlayHitAnimation();
        AudioManager.i.PlaySFX(AudioId.Hit);


        bool isDead = targetUnit.Aswang.TakeDamage(move, sourceUnit.Aswang, damage);
        yield return targetUnit.Hud.UpdateHP();
        yield return StartCoroutine(dialogBox.TypeDialog($"{subject} did {damage} total damage."));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        AudioManager.i.PlaySFX(AudioId.UISelect);

        if (isDead)
        {
            yield return HandleAswangKill(targetUnit);
        }
        else
            onDamageRollOver?.Invoke();
    }
    void Flee()
    {
        state = BattleState.Busy;
        StartCoroutine(RunBattle());

    }
    IEnumerator RunBattle()
    {
        yield return StartCoroutine(dialogBox.TypeDialog("You fled from the aswang."));
        AudioManager.i.PlaySFX(AudioId.UISelect);
        Run();
    }

    // Helper Methods
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
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        AudioManager.i.PlaySFX(AudioId.UISelect);
    }

    IEnumerator HandleAswangKill(BattleUnit KilledUnit)
    {
        yield return (StartCoroutine(dialogBox.TypeDialog(KilledUnit.GetDefeatText())));
        diceSystem.DisableHud();
        KilledUnit.PlayFaintAnimation();
        AudioManager.i.PlaySFX(AudioId.Faint);
        yield return new WaitForSeconds(1f);
        
        if (!KilledUnit.IsPlayerUnit)
        {

            AudioManager.i.PlayMusic(victoryMusic);

            int expYield =KilledUnit.Aswang.Base.ExpYield;
            int enemyLevel = KilledUnit.Aswang.Level;
            int expGain = Mathf.FloorToInt(expYield * enemyLevel / 7);
            playerUnit.Aswang.Exp += expGain;
            yield return dialogBox.TypeDialog($"{playerUnit.Aswang.Base.Aname} gained {expGain} EXP. Points!");

            yield return playerUnit.Hud.SetExpSmooth();
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
            AudioManager.i.PlaySFX(AudioId.UISelect);

            while (playerUnit.Aswang.CheckForLevelUp())
            {
                player.Level++;
                yield return dialogBox.TypeDialog($"{playerUnit.Aswang.Base.Aname} leveled up to Level {player.Level}.");
                yield return playerUnit.Hud.SetExpSmooth(true);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
                AudioManager.i.PlaySFX(AudioId.UISelect);
            }

        }

        else
        {
            yield return dialogBox.TypeDialog($"You fainted.");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
           
            player.HP = player.MaxHP;
            PlayerFaint();
            
        }
        OnBattleOver(true);
    }


    private bool CheckIfHit(Dice dice, BattleUnit target )
    {
        return dice.Base.ReturnedSide >= target.Aswang.ArmorClass;

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
    
    private int GetModifier(Modifier modifier, Aswang sourceUnit)
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
            default:
                playerModifier = 0;
                break;
        }
        return playerModifier;
    }

    
}
