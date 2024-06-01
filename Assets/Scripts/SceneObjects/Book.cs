using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] string line;
    [SerializeField] Sprite closed;
    [SerializeField] Sprite open;

    [SerializeField] ItemsBase itemAcquired;
    [SerializeField] QuestBase questToComplete; // interaction with this item will complete questToComplete
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questInProgress;
    [SerializeField] GameObject objectToActivateOnInteract;
    [SerializeField] GameObject objectToDeactivateOnInteract;


    private ItemsModel playerItems;


    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
    }
    public IEnumerator Interact(Transform initiator, PlayerController playerController)
    {
        GetComponent<SpriteRenderer>().sprite = open;
        Debug.Log("Interacting with book");
        yield return DialogManager.Instance.ShowDialog(dialog);
        GetComponent<SpriteRenderer>().sprite = closed;
        playerItems = playerController.player.inventorySystem.Controller._itemsModel;

        if (objectToActivateOnInteract != null)
        { objectToActivateOnInteract.gameObject.SetActive(true); }
        if (objectToDeactivateOnInteract != null)
        { objectToDeactivateOnInteract.gameObject.SetActive(false); }

        if (questToComplete != null)
        {
            var quest = new Quest(questToComplete);
            yield return (StartCoroutine(quest.CompletedQuest(playerItems)));

            //questToComplete = null;
        }
        if(questInProgress != null)
        {
            var quest = new Quest(questInProgress);
            yield return (StartCoroutine(DialogManager.Instance.ShowDialog(quest.Base.InprogressDialogue)));
        }

        if (questToStart != null)
        {
            var quest = new Quest(questToComplete);
            yield return (StartCoroutine(quest.StartQuest()));
            //questToStart = null;
        }

        if (itemAcquired!= null)
        {
            playerItems.AddItem(itemAcquired);
            gameObject.SetActive(false);
        }

    }

}
