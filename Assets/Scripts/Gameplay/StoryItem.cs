using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class StoryItem : MonoBehaviour, IPLayerTriggerable
{
    [SerializeField] Dialog dialog;
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete; // interaction with this item will complete questToComplete
    [SerializeField] GameObject objectToActivateOnComplete;
    private InventoryModel _playerInventory;
    private bool isActive = false;
    [SerializeField] bool triggerOnce;
    public static event Action<Vector2> OnQuestIncomplete;

    Quest activeQuest;
   public void OnPlayerTriggered(PlayerController playerController)
   {
       Debug.Log("nag run ang triggered");
        playerController.Character.Animator.IsMoving = false;
        //playerItems = playerController.player.inventorySystem.Controller._itemsModel;

        StartCoroutine(QuestCheck());
   }

   private IEnumerator QuestCheck()
   {
       // if may quest to complete, complete it
       if (questToComplete != null)
       {
           var quest = new Quest(questToComplete);
           StartCoroutine(quest.CompletedQuest(_playerInventory));
           questToComplete = null;
       }
        
       // if may quest to start, start it
       if (questToStart != null)
       {
           // if wala sa questlist, run ni
           if (!isActive)
           {
               activeQuest = new Quest(questToStart);
               yield return activeQuest.StartQuest();
               isActive = true;
           }
           
           //else if cannot be completed, this runs
           else if (!activeQuest.CanBeCompleted(_playerInventory))
           {
               Debug.Log("nag run ni");
               yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InprogressDialogue);
               yield break;
           }
           // if completed, this runs. this will also run on first encounter sa story item if may ara na siya item 
           if (activeQuest.CanBeCompleted(_playerInventory))
           {
               Debug.Log("nag run pagd a");
               yield return StartCoroutine(activeQuest.CompletedQuest(_playerInventory));
               activeQuest = null;
               questToStart = null;
               gameObject.SetActive(false);
           }
               
       }

       if (triggerOnce)
        {
            gameObject.SetActive(false);
        }
   }

    public bool TriggerRepeatedly=> false;
}
