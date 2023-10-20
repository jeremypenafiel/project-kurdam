using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice/Create new Dice")]
public class DiceBase : ScriptableObject
{
    
    [SerializeField] private int sides;
    // number of sides of the dice
    [SerializeField] private List<Sprite> diceSides;
    public int ReturnedSide { get; set; }
    public List<Sprite> DiceSides { get { return diceSides; } }

    public int Sides { get { return sides; } }


}
