using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovesChangeSystemState {  ChangeSelection, Busy, MoveSelection,}

public class MovesChangeSystem : MonoBehaviour
{

    [SerializeField] MovesChangeDialogue dialogBox;


    public event Action Exit;
    public event Action Pause;
    MovesChangeSystemState state;
    int currentAction;
    int currentMove;

    Aswang player;

    public static MovesChangeSystem i { get; private set; }
    Coroutine currentCoroutine;

    private void Awake()
    {
        i = this;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }



    public void StartBattle(Aswang Player)
    {
        i = this;
        player = Player;
        MoveSelection();
        dialogBox.SetMoveNames(player.moves);
        dialogBox.SetMoveChangeNames(player.movesLearned);
    }



    // State Setups
    void ChangeSelection()
    {
        state = MovesChangeSystemState.ChangeSelection;
        currentCoroutine = StartCoroutine(dialogBox.TypeDialog("Choose move to replace"));

       
    }

    void MoveSelection()
    {
        state = MovesChangeSystemState.MoveSelection;
        currentCoroutine = StartCoroutine(dialogBox.TypeDialog("Choose the move to be replaced"));


    }







    public void HandleUpdate()
    {
        if (state == MovesChangeSystemState.ChangeSelection)
        {
            HandleChangeSelection();
        }

        else if (state == MovesChangeSystemState.MoveSelection)
        {
            HandleMoveSelection();
        }
      
    }


    // Enter states / Handle states
    void HandleChangeSelection()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < player.movesLearned.Count - 1)
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < player.movesLearned.Count - 2)
            {
                currentAction += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 1)
            {
                currentAction -= 2;
            }
        }
        dialogBox.UpdateChangeSelection(currentAction, player.movesLearned[currentAction]);
        Debug.Log(currentAction.ToString());

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StopCoroutine(currentCoroutine);
            AudioManager.i.PlaySFX(AudioId.UISelect);
            dialogBox.UpdateChangeSelection();
            ChangeMvoe(currentMove, currentAction);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StopCoroutine(currentCoroutine);
            AudioManager.i.PlaySFX(AudioId.UISelect);
            dialogBox.UpdateChangeSelection();
            MoveSelection();
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < 4 - 1)
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
            if (currentMove < 4- 2)
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
        dialogBox.UpdateMoveSelection(currentMove, player.moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StopCoroutine(currentCoroutine);
            AudioManager.i.PlaySFX(AudioId.UISelect);
            ChangeSelection();


        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StopCoroutine(currentCoroutine);
            AudioManager.i.PlaySFX(AudioId.UISelect);

            Exit?.Invoke();
        }
    }
   
    public void ChangeMvoe(int oldMove, int newMove) 
    {
        player.moves[oldMove] = player.movesLearned[newMove];
        dialogBox.SetMoveNames(player.moves);
        MoveSelection();
    }
}
