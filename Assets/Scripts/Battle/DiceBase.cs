using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice/Create new Dice")]
public class DiceBase : MonoBehaviour
{
    // number of sides of the dice
    [SerializeField] private int sides;
    // Array of dice sides sprites to load from Resources folder
    [SerializeField] private List<Sprite> diceSides;
    // Reference to sprite renderer to change sprites
    private SpriteRenderer spriteRenderer;
    [SerializeField] public bool isEnabled;
    public int ReturnedSide { get; set; }

    // Use this for initialization
    private void Start()
    {


        // Assign Renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        // diceSides = Resources.LoadAll<Sprite>($"d{this.sides}/");
    }

    // If you left click over the dice then RollTheDice coroutine is started
    /*private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }*/

    // Coroutine that rolls the dice
    public IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        ReturnedSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = UnityEngine.Random.Range(0, this.sides - 1);

            // Set sprite to upper face of dice from array according to random value
            spriteRenderer.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        ReturnedSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log(ReturnedSide);

    }


    public void setDice(bool value)
    {
        this.spriteRenderer.enabled = value;
    }
}
