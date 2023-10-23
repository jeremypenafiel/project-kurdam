using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public DiceBase Base;


    // Use this for initialization
    private void Start()
    {
   
        
        // Assign Renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
       // diceSides = Resources.LoadAll<Sprite>($"d{this.sides}/");
    }

    public IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        Base.ReturnedSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range( 0, Base.Sides) +1;

            // Set sprite to upper face of dice from array according to random value

                spriteRenderer.sprite = Base.DiceSides[randomDiceSide-1];
            


            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        Base.ReturnedSide = randomDiceSide;

        // Show final dice value in Console

    }


}
