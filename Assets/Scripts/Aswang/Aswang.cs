using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aswang
{
    [SerializeField] AswangBase _base;
    [SerializeField] int level;
    


    public AswangBase Base 
    {
        get;

        set;
    }

    public int Level 
    {
        get;

        set;
    }
    public int Exp { get; set; }
    public int HP { get; set; }


    public List<Moves> moves {  get; set; }


    public void Init()
    {
        HP = MaxHP;

        moves = new List<Moves>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.level <= Level)
            {
                moves.Add(new Moves(move.MovesBase));
            }
        }
        Exp = Base.GetExpForLevel(Level);
    }

    public Aswang(AswangBase abase, int alevel)
    {
        Base = abase;
        Level = alevel;
        HP = MaxHP;
        Exp = abase.GetExpForLevel(Level);

        moves = new List<Moves>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.level <= Level)
            {
                moves.Add(new Moves(move.MovesBase));
            }
        }

    }

    public int MaxHP
    {
        get { return Base.MaxHP; }
    }

    public int ArmorClass
    {
        get { return Base.ArmorClass; }
    }

    public int Strength
    {
        get { return Mathf.FloorToInt((Base.Strength - 10) /2  + Mathf.FloorToInt(Growthrate * Level)); }
    }

       public int Dexterity
    {
        get { return Mathf.FloorToInt((Base.Dexterity - 10) / 2) + Mathf.FloorToInt(Growthrate * Level); }
    }

    public int Constitution
    {
        get { return Mathf.FloorToInt((Base.Constitution - 10) / 2) + Mathf.FloorToInt(Growthrate * Level); }
    }

    public int Intelligence
    {
        get { return Mathf.FloorToInt((Base.Intelligence - 10) / 2 + Mathf.FloorToInt(Growthrate * Level)); }
    }
    
    public int Charisma
    {
        get { return Mathf.FloorToInt((Base.Charisma - 10) / 2 + Mathf.FloorToInt(Growthrate * Level)); }
    }

    public float Growthrate
    {
        get { return Base.Growthrate; }
    }
    public bool CheckForLevelUp()
    {
        return Exp > Base.GetExpForLevel(Level + 1);
    }
    public bool TakeDamage(Moves move, Aswang attacker, int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public Moves GetRandomMove()
    {
        int r = Random.Range(0, moves.Count);
        return moves[r];

    }


    public void OnEquippedItemsChanged()
    {
        throw new System.NotImplementedException();
    }
}
