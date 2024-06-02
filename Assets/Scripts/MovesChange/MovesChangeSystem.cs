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
        dialogBox.SetMoveChangeNames(player.moves);
    }



    // State Setups
    void ChangeSelection()
    {
        state = MovesChangeSystemState.ChangeSelection;
        currentCoroutine = StartCoroutine(dialogBox.TypeDialog("Choose An Action"));
        dialogBox.EnableActionSelector(true);
    }

    void MoveSelection()
    {
        state = MovesChangeSystemState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
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
            if (currentMove < player.moves.Count - 1)
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
            if (currentMove < player.moves.Count - 2)
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
        dialogBox.UpdateChangeSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {

            AudioManager.i.PlaySFX(AudioId.UISelect);

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {

            AudioManager.i.PlaySFX(AudioId.UISelect);
            MoveSelection();
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < player.moves.Count - 1)
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
            if (currentMove < player.moves.Count - 2)
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
            ChangeSelection();


        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            
            AudioManager.i.PlaySFX(AudioId.UISelect);

            Exit?.Invoke();
        }
    }
   
}
