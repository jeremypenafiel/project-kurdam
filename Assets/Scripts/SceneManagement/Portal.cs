using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour, IPLayerTriggerable
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destinationPortal;

    PlayerController player;
    Fader fader;

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        SwitchScene();
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }
    void SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        GameController.Instance.PauseGame(true);
        fader.FadeIn(0.5f);
        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);

        player.Character.SetPositionAndSnapToTile(destPortal.Spawnpoint.position);

        fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

        Destroy(gameObject);
    }

    public Transform Spawnpoint => spawnPoint;

    public enum DestinationIdentifier { A, B, C, D, E }
}