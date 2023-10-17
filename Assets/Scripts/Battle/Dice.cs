using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    // number of sides of the dice
    [SerializeField] private int sides;
    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;
    // Reference to sprite renderer to change sprites
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isEnabled;

    // Use this for initialization
    private void Start()
    {
   
        
        // Assign Renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = this.isEnabled;

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>($"d{this.sides}/");
        Debug.Log(diceSides.Length);
    }

    // If you left click over the dice then RollTheDice coroutine is started
    private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, this.sides-1);

            // Set sprite to upper face of dice from array according to random value
            spriteRenderer.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log(finalSide);
    }



}
