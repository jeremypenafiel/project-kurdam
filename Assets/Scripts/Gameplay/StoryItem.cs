using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItem : MonoBehaviour, IPLayerTriggerable
{
    [SerializeField] Dialog dialog;
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;


    Quest activeQuest;
   public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));


    }

    private IEnumerator Start()
    {
        if (questToComplete!=null)
        {
            var quest = new Quest(questToComplete);
            yield return quest.CompletedQuest();
            questToComplete = null;
        }
        if (questToStart != null)
        {
            activeQuest = new Quest(questToStart);
            yield return activeQuest.StartQuest();

            if (activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompletedQuest();
                activeQuest = null;
            }
        }

        else if (activeQuest != null)
        {
            if(activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompletedQuest();
                activeQuest = null;
            }
            else
            {
                yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InprogressDialogue);
            }
        }
    }
    public bool TriggerRepeatedly=> false;
}
