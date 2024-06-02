using Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;





public class Quest
{
    public QuestBase Base {get;private set;}

    public QuestStatus Status { get; set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }


    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Base.Name,
            status = Status     
        };
        return saveData;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;
        
        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue); 

        var questlist = QuestList.GetQuestList();
        questlist.AddQuest(this);

    }

    public IEnumerator CompletedQuest(Inventory inventory)
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompletedDialogue);
        
        // if quest has required item, remove it from inventory
        // if (Base.RequiredItem != null && itemsModel.Contains(Base.RequiredItem))
        // {
        //     itemsModel.RemoveItem(Base.RequiredItem);
        // }
        
        // if quest has reward item, add it to inventory
        if (Base.RewardItem != null)
        {
            inventory.AddItem(Base.RewardItem);
        }
        // var questlist = QuestList.GetQuestList();
        // questlist.AddQuest(this);

    }

    public bool CanBeCompleted(Inventory inventory)
    {
        // if quest does have required item
        if (Base.RequiredItem != null)
        {
            //check  if required item is inventory
            // return inventory.Contains(Base.RequiredItem);
        }
        if (Base.RequiredItems != null)
        {
            ;
            foreach (var item in Base.RequiredItems)
            {
                var check = inventory.ContainsItem(item);
                if (!check) { return check; }
                return check;
            }
            
        }
        return true;
    }
    
    
}

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}
public enum QuestStatus { None, Started, Completed }