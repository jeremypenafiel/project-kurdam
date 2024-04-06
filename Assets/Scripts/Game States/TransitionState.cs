using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class TransitionState: State<GameController>
{
    public static TransitionState I { get; private set; }
    [SerializeField] public AudioClip battleMusic;
    
    private GameController _gc;

    private void Awake()
    {
        I = this;
    }
    
    public override void Enter(GameController owner)
    {
        _gc = owner;
        
        AudioManager.i.PlayMusic(battleMusic);
        AudioManager.i.StopPlayAmbientSound();
        StartCoroutine(_gc.Transition());
    }

    public override void Exit()
    {
        _gc.StateMachine.Push(BattleState.i);
    }
    
}
