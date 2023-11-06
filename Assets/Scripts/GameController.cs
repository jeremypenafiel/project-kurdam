using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {FreeRoam, Battle }
public class GameController : MonoBehaviour
{
    GameState state;
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.Run += EndBattle;

    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var player = playerController.GetComponent<Player>().GetPlayer();
        var wildAswang = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildAswang();
        battleSystem.StartBattle(player, wildAswang);

    }

    void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true );
    }
    private void Update()
    {
        

        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
