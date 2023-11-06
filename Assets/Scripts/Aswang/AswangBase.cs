using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Aswang", menuName ="Aswang/Create new Aswang")]
public class AswangBase : ScriptableObject
{
    [SerializeField] string aname;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite backSprite;
    [SerializeField] Sprite frontSprite;

    [SerializeField] AswangType aswangType1;

    //Base Stats
    [SerializeField] int maxHP;
    [SerializeField] int armorClass;
    [SerializeField] int strength;
    [SerializeField] int dexterity;
    [SerializeField] int constitution;
    [SerializeField] int intelligence;
    [SerializeField] int wisdom;
    [SerializeField] int charisma;
    [SerializeField] List<LearnableMove> learnableMoves;
    [SerializeField] List<DamageType> resistances;
    [SerializeField] List<DamageType> vulnerabilities;

    public string Aname
    {
        get { return aname; }
    }
    public string Description
    {
        get { return description; }
    }

    public Sprite Frontsprite
    {
        get { return frontSprite; }
    }

    public Sprite Backsprite
    {
        get { return backSprite; }
    }

    public AswangType AswangType1
    {
        get { return aswangType1; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int ArmorClass
    {
        get { return armorClass; }
    }   

    public int Strength
    {
        get { return strength; }
    }

    public int Dexterity
    {
        get { return dexterity; }
    }

    public int Constitution
    {
        get { return constitution; }
    }

    public int Intelligence
    {
        get { return intelligence; }
    }

    public int Charisma
    {
        get { return charisma; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

    public List<DamageType> Resistances
    {
        get { return resistances; }
    }

    public List<DamageType> Vulnerabilities
    {
        get { return vulnerabilities; }
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

    public MovesBase MovesBase
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
