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
        inventorySystem.view.ActivateCamera();
        inventorySystem.view.RegisterExitListener(CloseInventory);
        gc.WorldCamera.gameObject.SetActive(false);
        gc.VisionLimiter.SetActive(false);
        var player = gc.PlayerController.GetComponent<Player>().GetPlayer();
        //inventorySystem.StartInventorySystem(player);
    }
    public override void Execute()
    {
        //InventorySystem.i.HandleUpdate();

    }

    private void CloseInventory()
    {
        gc.StateMachine.Pop();

    }

    public override void Exit()
    {
        inventorySystem.gameObject.SetActive(false);
        inventorySystem.view.RemoveExitListener(CloseInventory);
        gc.WorldCamera.gameObject.SetActive(true);
        gc.VisionLimiter.SetActive(true);
    }
}
