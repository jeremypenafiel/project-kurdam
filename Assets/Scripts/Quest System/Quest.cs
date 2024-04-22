using Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum QuestStatus { None, Started, Completed}

[System.Serializable]
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

        var questlist = QuestList.GetQuestList();
        questlist.AddQuest(this);

    }

    public IEnumerator CompletedQuest(ItemsModel itemsModel)
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompletedDialogue);
        
        // if quest has required item, remove it from inventory
        if (Base.RequiredItem != null && itemsModel.Contains(Base.RequiredItem))
        {
            itemsModel.RemoveItem(Base.RequiredItem);
        }
        
        // if quest has reward item, add it to inventory
        if (Base.RewardItem != null)
        {
            itemsModel.AddItem(Base.RewardItem);
        }
        // var questlist = QuestList.GetQuestList();
        // questlist.AddQuest(this);

    }

    public bool CanBeCompleted(ItemsModel inventory)
    {
        // if quest does have required item
        if (Base.RequiredItem != null)
        {
            //check  if required item is inventory
            return inventory.Contains(Base.RequiredItem);
        }

        return true;
    }
    
    
}

