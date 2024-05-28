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
    [SerializeField] GameObject objectToActivateOnComplete;
    [SerializeField] GameObject objectToActivateOnStart;
    [SerializeField] GameObject objectToDeactivateOnComplete;


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

        if (questToComplete != null)
        {
            var quest = new Quest(questToComplete);
            yield return (StartCoroutine(quest.CompletedQuest(playerItems)));
            if (objectToActivateOnComplete != null)
            { objectToActivateOnComplete.gameObject.SetActive(true); }
            if (objectToDeactivateOnComplete != null)
            { objectToDeactivateOnComplete.gameObject.SetActive(false); }
            //questToComplete = null;
        }

        if (questToStart != null)
        {
            var quest = new Quest(questToComplete);
            yield return (StartCoroutine(quest.StartQuest()));
            if (objectToActivateOnStart != null)
            { objectToActivateOnStart.gameObject.SetActive(true); }
            //questToStart = null;
        }

        if (itemAcquired!= null)
        {
            playerItems.AddItem(itemAcquired);
            gameObject.SetActive(false);
        }

    }

}
