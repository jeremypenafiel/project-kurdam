using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Move", menuName ="Create new move")]
public class MovesBase : ScriptableObject
{
    [SerializeField] string moveName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] AswangType type;
    [SerializeField] int baseDamage;
    [SerializeField] DiceBase diceBase;

    public string MoveName
    {
        get { return moveName; }
    }

    public string Description
    {
        get { return description; }
    }

    public AswangType Type
    {
        get { return type; }
    }

    public int BaseDamage
    {
        get { return baseDamage; }
    }

    public DiceBase DiceBase
    {
        get { return diceBase; }
    }
}
