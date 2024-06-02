using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
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
        // this.player = player;
        // player.Character.Animator.IsMoving = false;
        // Teleport();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER");
        this.player = other.gameObject.GetComponent<PlayerController>();
        player.Character.Animator.IsMoving = false;
        Teleport();
    }

    private void Start()
    {

        fader = GameController.Instance.fader;
        // fader = FindObjectOfType<Fader>();
    }
    void Teleport()
    {
        Debug.Log("teleported");
        GameController.Instance.PauseGame(true);
        StartCoroutine(fader.FadeIn(0.5f));
        
        
        // fader.sequence.OnComplete(() => player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position););
        // fader.sequence.onComplete += () => player.Character.SetPositionAndSnapToTile(spawnPoint.position);
        // var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        

   
        // if(fader.tweener.IsComplete())
        // {
        fader.tweener.onComplete += () =>
        {
            var destPortal = FindObjectsOfType<LocationPortal>().First(x =>
                x != this && x.destinationPortal == this.destinationPortal && x.gameObject.name == sceneToLoad);
            player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);
            StartCoroutine(fader.FadeOut(0.5f));
        };
            
        // }

        

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
        StartCoroutine(fader.FadeIn(0.5f));
        fader.tweener.onComplete += () =>
        {
            var destPortal = FindObjectsOfType<LocationPortal>().First(x =>
                x != this && x.destinationPortal == this.destinationPortal && x.gameObject.name == sceneToLoad);
            player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);
            StartCoroutine(fader.FadeOut(0.5f));
        };
        // var destPortal = FindObjectsOfType<LocationPortal>().First(x=>x.gameObject.name == "Spawn");
        // Debug.Log(destPortal.gameObject.name);
        // Debug.Log(destPortal.Spawnpoint.position);
        //
        // player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);
        // StartCoroutine(fader.FadeOut(0.5f));
        GameController.Instance.PauseGame(false);
    }

    public bool TriggerRepeatedly => false;
    public Transform Spawnpoint => spawnPoint;

    public enum DestinationIdentifier { A, B, C, D, E }
}
