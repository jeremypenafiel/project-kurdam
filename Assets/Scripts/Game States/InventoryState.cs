using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class InventoryState : State<GameController>
{
    [FormerlySerializedAs("inventorySystem")] [SerializeField] Inventory inventory;
    public static InventoryState i { get; private set; }
    GameController gc;


    public void Awake()
    {
        i = this;
    }



    public override void Enter(GameController owner)
    {
        gc = owner;
        inventory.ActivateView(true);
        // inventory.view.ActivateCamera();
        inventory.OnExitPressed += CloseInventory;
            
        // inventory.view.RegisterExitListener(CloseInventory);
        // gc.WorldCamera.gameObject.SetActive(false);
        // gc.VisionLimiter.SetActive(false);
        // var player = gc.PlayerController.GetComponent<Player>().GetPlayer();
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
        inventory.ActivateView(false);
        inventory.OnExitPressed -= CloseInventory;
        // gc.WorldCamera.gameObject.SetActive(true);
        // gc.VisionLimiter.SetActive(true);
    }
}
