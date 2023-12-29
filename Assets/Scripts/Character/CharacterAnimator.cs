using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;


    // Parameters

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }


    // States

    SpriteAnimator walkDownAnimator;
    SpriteAnimator walkUpAnimator;
    SpriteAnimator walkRightAnimator;
    SpriteAnimator walkLeftAnimator;

    SpriteRenderer spriteRenderer;

    SpriteAnimator currentAnimator;

    bool wasPreviouslyMoving;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnimator = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimator = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnimator = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnimator = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        currentAnimator = walkDownAnimator;

    }

    private void Update()
    {
        var previousAnimator = currentAnimator;

        if (MoveX == 1)
        {
            currentAnimator = walkRightAnimator;
        }else if (MoveX == -1)
        {
            currentAnimator = walkLeftAnimator;

        }else if (MoveY == 1)
        {
            currentAnimator = walkUpAnimator;
        }else if (MoveY == -1)
        {
            currentAnimator = walkDownAnimator;
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
