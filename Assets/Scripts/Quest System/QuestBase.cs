using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quests/Create a new quest")]
public class QuestBase : ScriptableObject   
{
    [SerializeField] string name;
    [SerializeField] string description;

    [SerializeField] Dialog startDialogue;
    [SerializeField] Dialog inprogressDialogue;
    [SerializeField] Dialog completedDialogue;

    [SerializeField] ItemsBase requiredItem;
    [SerializeField] ItemsBase rewardItem;


    public string Name=> name;
    public string Description => description;
    public Dialog StartDialogue => startDialogue;  
    public Dialog InprogressDialogue => inprogressDialogue?.Lines?.Count>0? inprogressDialogue:StartDialogue;
    public Dialog CompletedDialogue => completedDialogue;
    public ItemsBase RewardItem => rewardItem;
    public ItemsBase RequiredItem => requiredItem;

}
