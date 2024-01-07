using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EncounterableArea : MonoBehaviour, IPLayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {  
        if (UnityEngine.Random.Range(1, 101) <= 10)
        { 
            GameController.Instance.StartBattle();
        }
    }
}
