using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] Dialog dialog;
    [SerializeField] string line;
    [SerializeField] Sprite closed;
    [SerializeField] Sprite open;

    [SerializeField] ItemsBase itemAcquired;
    [SerializeField] QuestBase questToComplete; // interaction with this item will complete questToComplete
    [SerializeField] GameObject objectToActivateOnComplete;
    [SerializeField] Inventory _playerInventory;
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questInProgress;
    [SerializeField] GameObject objectToActivateOnInteract;
    [SerializeField] GameObject objectToDeactivateOnInteract;
    Quest activeQuest;





    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
        _playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    public IEnumerator Interact(Transform initiator, PlayerController playerController)
    {
        GetComponent<SpriteRenderer>().sprite = open;
        Debug.Log("Interacting with book");
        yield return DialogManager.Instance.ShowDialog(dialog);
        GetComponent<SpriteRenderer>().sprite = closed;
        // playerItems = playerController.player.inventorySystem.Controller._itemsModel;

        if (objectToActivateOnInteract != null)
        { objectToActivateOnInteract.gameObject.SetActive(true); }
        if (objectToDeactivateOnInteract != null)
        { objectToDeactivateOnInteract.gameObject.SetActive(false); }

        if (questToComplete != null)
        {
            var quest = new Quest(questToComplete);
            yield return (StartCoroutine(quest.CompletedQuest(_playerInventory)));
            objectToActivateOnComplete.gameObject.SetActive(true);
            questToComplete = null;
            yield return (StartCoroutine(quest.CompletedQuest(_playerInventory)));

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
            _playerInventory.AddItem(itemAcquired);
            gameObject.SetActive(false);
        }

    }

  


    public object CaptureState()
    {
        var saveData = new InteractQuestSaveData();
        saveData.activeQuest = activeQuest?.GetSaveData();

        if (questToStart != null)
        {
            saveData.questToStart = (new Quest(questToStart).GetSaveData());
        }
        if (questToComplete != null)
        {
            saveData.questToComplete = (new Quest(questToComplete).GetSaveData());
        }
        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InteractQuestSaveData;
        if (saveData != null)
        {
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;
            questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;

        }
    }
}
[System.Serializable]
public class InteractQuestSaveData
{
    public QuestSaveData activeQuest;
    public QuestSaveData questToStart;
    public QuestSaveData questToComplete;

}
