using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Aswang", menuName ="Aswang/Create new Aswang")]
public class AswangBase : ScriptableObject
{
    [SerializeField] string Aname;

    [TextArea]
    [SerializeField] string Description;

    [SerializeField] Sprite backSprite;
    [SerializeField] Sprite frontSprite;

    [SerializeField] AswangType AswangType1;

    //Base Stats
    [SerializeField] int MaxHP;
    [SerializeField] int ArmorClass;
    [SerializeField] int Attack;
    [SerializeField] int Magical;
    [SerializeField] int Defense;
    [SerializeField] int Faith;
    [SerializeField] int Speed;
    [SerializeField] List<LearnableMove> learnableMoves;

    public string aname
    {
        get { return Aname; }
    }
    public string description
    {
        get { return Description; }
    }

    public Sprite Frontsprite
    {
        get { return frontSprite; }
    }

    public Sprite Backsprite
    {
        get { return backSprite; }
    }

    public AswangType aswangType1
    {
        get { return AswangType1; }
    }

    public int maxhp
    {
        get { return MaxHP; }
    }

    public int armorClass
    {
        get { return ArmorClass; }
    }   

    public int attack
    {
        get { return Attack; }
    }

    public int magical
    {
        get { return Magical; }
    }

    public int defense
    {
        get { return Defense; }
    }

    public int faith
    {
        get { return Faith; }
    }

    public int speed
    {
        get { return Speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] public MovesBase movesBase;
    [SerializeField] public int level;

    public int Level
    {
        get { return level; }
    }

    public MovesBase movesbase
    {
        get { return movesBase; }
    }
}

public enum AswangType
{
    Tawo,
    Espiritu,
    Halimaw


}
