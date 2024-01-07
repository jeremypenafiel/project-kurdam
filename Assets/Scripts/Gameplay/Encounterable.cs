using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EncounterableArea : MonoBehaviour, IPLayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        if(player.Character.distance > player.Character.distanceThreshold)
        {
            Debug.Log(gameObject.name);
            Debug.Log(player.Character.distance);
            Debug.Log(player.Character.distanceThreshold);
            GameController.Instance.StartBattle();
        }
    }
}
