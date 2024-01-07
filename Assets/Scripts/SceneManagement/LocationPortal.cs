using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LocationPortal : MonoBehaviour, IPLayerTriggerable
{
    // Start is called before the first frame update
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] string sceneToLoad;

    PlayerController player;
    Fader fader;
    
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        player.Character.Animator.IsMoving = false;
        StartCoroutine(Teleport());
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }
    IEnumerator Teleport()
    {
        Debug.Log("teleported");
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);
        // var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        

   

        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal && x.gameObject.name == sceneToLoad);

        player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

    }
    public void SpawnPlayer(PlayerController player)
    {
        Debug.Log("SpawnPlayer");
        player.Character.Animator.IsMoving = false;
        StartCoroutine(Spawn(player));
    }
    IEnumerator Spawn(PlayerController player)
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);
        var destPortal = FindObjectsOfType<LocationPortal>().First(x=>x.gameObject.name == "Spawn");
        Debug.Log(destPortal.gameObject.name);
        Debug.Log(destPortal.Spawnpoint.position);

        player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);
        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }
    public Transform Spawnpoint => spawnPoint;

    public enum DestinationIdentifier { A, B, C, D, E }
}
