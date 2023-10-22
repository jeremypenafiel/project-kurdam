using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Move", menuName ="Create new move")]
public class MovesBase : ScriptableObject
{
    [SerializeField] string Name;
    [TextArea]
    [SerializeField] string Description;

    [SerializeField] AswangType Type;
    [SerializeField] int baseDamage;
    [SerializeField] DiceBase diceBase;

    public string MoveName
    {
        get { return Name; }
    }

    public string description
    {
        get { return Description; }
    }

    public AswangType type
    {
        get { return Type; }
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
