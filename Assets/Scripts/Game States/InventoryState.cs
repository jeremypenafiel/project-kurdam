using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryState : State<GameController>
{
    [SerializeField] InventorySystem inventorySystem;
    public static InventoryState i { get; private set; }
    GameController gc;


    public void Awake()
    {
        i = this;
    }



    public override void Enter(GameController owner)
    {
        gc = owner;
        inventorySystem.gameObject.SetActive(true);
        gc.WorldCamera.gameObject.SetActive(false);
        gc.VisionLimiter.SetActive(false);
        inventorySystem.StartInventorySystem();
    }
    public override void Execute()
    {
        InventorySystem.i.HandleUpdate();
        if(Input.GetKeyDown(KeyCode.X))
        {
            gc.StateMachine.Pop();
        }
    }

    public override void Exit()
    {
        inventorySystem.gameObject.SetActive(false);
        gc.WorldCamera.gameObject.SetActive(true);
        gc.VisionLimiter.SetActive(true);
    }
}
