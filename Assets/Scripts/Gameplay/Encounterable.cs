using UnityEngine;

public class EncounterableArea : MonoBehaviour, IPLayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        if(Random.Range(1, 101) <= 10)
        {
            player.Character.Animator.IsMoving = false;
            GameController.Instance.StartBattle();

        }
    }
    
    public bool TriggerRepeatedly=> true;

}
