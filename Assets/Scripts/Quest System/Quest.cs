using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum QuestStatus { None, Started, Completed}
public class Quest
{
    public QuestBase Base {get;private set;}

    public QuestStatus Status { get; set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);

        var questlist =QuestList.GetQuestList();
        questlist.AddQuest(this);

    }

    public IEnumerator CompletedQuest()
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompletedDialogue);

/*        var inventory = ItemController._itemsModel.inventoryItems;
*/        if (Base.RequiredItem!=null)
        {

        }

        if (Base.RewardItem!=null)
        {
            //add item
        }
        var questlist = QuestList.GetQuestList();
        questlist.AddQuest(this);

    }

    public bool CanBeCompleted()
    {
        if (Base.RequiredItem != null)
        {
            //check  if required item is inventory
            return true;
        }
        return false;
    }
}

