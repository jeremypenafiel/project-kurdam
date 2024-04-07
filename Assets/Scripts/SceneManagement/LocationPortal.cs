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
    SimpleBlit worldCamera;
    
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        player.Character.Animator.IsMoving = false;
        Teleport();
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
        worldCamera = GameObject.Find("Main Camera").GetComponent<SimpleBlit>();
        

    }
    void Teleport()
    {
        Debug.Log("teleported");
        GameController.Instance.PauseGame(true);
        fader.FadeIn(0.5f);
        StartCoroutine(worldCamera.FadeIn());
        // var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        

   

        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal && x.gameObject.name == sceneToLoad);

        player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);

        fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

    }
    public void SpawnPlayer(PlayerController player)
    {
        Debug.Log("SpawnPlayer");
        player.Character.Animator.IsMoving = false;
        Spawn(player);
    }
    void Spawn(PlayerController player)
    {
        GameController.Instance.PauseGame(true);
        fader.FadeIn(0.5f);
        var destPortal = FindObjectsOfType<LocationPortal>().First(x=>x.gameObject.name == "Spawn");
        Debug.Log(destPortal.gameObject.name);
        Debug.Log(destPortal.Spawnpoint.position);

        player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);
        fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }
    public Transform Spawnpoint => spawnPoint;

    public enum DestinationIdentifier { A, B, C, D, E }
}
