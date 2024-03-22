using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    [SerializeField] List<Sprite> runDownSprites;
    [SerializeField] List<Sprite> runUpSprites;
    [SerializeField] List<Sprite> runRightSprites;
    [SerializeField] List<Sprite> runLeftSprites;

    [SerializeField] List<Sprite> sneakDownSprites;
    [SerializeField] List<Sprite> sneakUpSprites;
    [SerializeField] List<Sprite> sneakRightSprites;
    [SerializeField] List<Sprite> sneakLeftSprites;


    // Parameters

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    private float sneakFrameRate = 0.32f;
    private float runFrameRate = 0.08f;
    public bool IsSneaking;
    public bool IsRunning;


    // States

    SpriteAnimator walkDownAnimator;
    SpriteAnimator walkUpAnimator;
    SpriteAnimator walkRightAnimator;
    SpriteAnimator walkLeftAnimator;
    
    SpriteAnimator runDownAnimator;
    SpriteAnimator runUpAnimator;
    SpriteAnimator runRightAnimator;
    SpriteAnimator runLeftAnimator;

    SpriteAnimator sneakDownAnimator;
    SpriteAnimator sneakUpAnimator;
    SpriteAnimator sneakRightAnimator;
    SpriteAnimator sneakLeftAnimator;

    List<SpriteAnimator> walkAnimators;
    List<SpriteAnimator> runAnimators;
    List<SpriteAnimator> sneakAnimators;

    SpriteRenderer spriteRenderer;

    SpriteAnimator currentAnimator;

    bool wasPreviouslyMoving;

    private enum Directions
    {
        Up = 0, 
        Down = 1,
        Left = 2,
        Right = 3
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        walkDownAnimator = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimator = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnimator = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnimator = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        runDownAnimator = new SpriteAnimator(runDownSprites, spriteRenderer, runFrameRate);
        runUpAnimator = new SpriteAnimator(runUpSprites, spriteRenderer, runFrameRate);
        runRightAnimator = new SpriteAnimator(runRightSprites, spriteRenderer, runFrameRate);
        runLeftAnimator = new SpriteAnimator(runLeftSprites, spriteRenderer, runFrameRate);

        sneakDownAnimator = new SpriteAnimator(sneakDownSprites, spriteRenderer, sneakFrameRate);
        sneakUpAnimator = new SpriteAnimator(sneakUpSprites, spriteRenderer, sneakFrameRate);
        sneakRightAnimator = new SpriteAnimator(sneakRightSprites, spriteRenderer, sneakFrameRate);
        sneakLeftAnimator = new SpriteAnimator(sneakLeftSprites, spriteRenderer, sneakFrameRate);

        walkAnimators = new List<SpriteAnimator>
        {
            walkUpAnimator, walkDownAnimator, walkLeftAnimator, walkRightAnimator
        };

        runAnimators = new List<SpriteAnimator>
        {
            runUpAnimator, runDownAnimator, runLeftAnimator, runRightAnimator
        };

        sneakAnimators = new List<SpriteAnimator>
        {
            sneakUpAnimator, sneakDownAnimator, sneakLeftAnimator, sneakRightAnimator
        };

        currentAnimator = walkDownAnimator;

    }

    private void Update()
    {
        var previousAnimator = currentAnimator;
        List<SpriteAnimator> currentAnimators;

        if (IsRunning)
        {
            currentAnimators = runAnimators;
        }
        else if (IsSneaking)
        {
            currentAnimators = sneakAnimators;
        }
        else
        {
            currentAnimators = walkAnimators;
        }

        if (MoveX == 1)
        {
            currentAnimator = currentAnimators[(int)Directions.Right];
        }else if (MoveX == -1)
        {
            currentAnimator = currentAnimators[(int)Directions.Left];

        }else if (MoveY == 1)
        {
            currentAnimator = currentAnimators[(int)Directions.Up];
        }else if (MoveY == -1)
        {
            currentAnimator = currentAnimators[(int)Directions.Down];
        }

        if (currentAnimator != previousAnimator || IsMoving != wasPreviouslyMoving )
        {
            currentAnimator.Start();
        }
         
        if (IsMoving)
        {

            currentAnimator.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnimator.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }

}
