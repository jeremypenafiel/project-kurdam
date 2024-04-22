using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    List<Quest> quests =new List<Quest>();

    public event Action OnUpdated;

    public void AddQuest(Quest quest)
    {
        if (!quests.Contains(quest))
            quests.Add(quest);

        OnUpdated?.Invoke();
    }

    public static QuestList GetQuestList()
    {
        return FindObjectOfType<PlayerController>().GetComponent<QuestList>();
    }

    public bool isStarted(string questName)
    {
       var questStatus =quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
       return questStatus == QuestStatus.Started || questStatus == QuestStatus.Completed;
    }

    public bool isComplete(string questName)
    {
        var questStatus = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return  questStatus == QuestStatus.Completed;
    }
}
