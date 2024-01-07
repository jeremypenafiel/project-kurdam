using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EncounterableArea : MonoBehaviour, IPLayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        Debug.Log(gameObject.name);
        GameController.Instance.StartBattle();
    }
}
