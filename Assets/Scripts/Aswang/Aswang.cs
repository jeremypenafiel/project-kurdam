using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Aswang
{
    [FormerlySerializedAs("_base")] [SerializeField] AswangData data;
    [SerializeField] int level;
    


    public AswangData AswangData 
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
        foreach (var move in AswangData.LearnableMoves)
        {
            if (move.level <= Level)
            {
                moves.Add(new Moves(move.MovesBase));
            }
        }
        Exp = AswangData.GetExpForLevel(Level);
    }

    public Aswang(AswangData aswangData, int alevel)
    {
        AswangData = aswangData;
        Level = alevel;
        HP = MaxHP; 

        moves = new List<Moves>();
        foreach (var move in AswangData.LearnableMoves)
        {
            if (move.level <= Level)
            {
                moves.Add(new Moves(move.MovesBase));
            }
        }

    }

    public int MaxHP
    {
        get { return AswangData.MaxHP; }
    }

    public int ArmorClass
    {
        get { return AswangData.ArmorClass; }
    }

    public int Strength
    {
        get { return Mathf.FloorToInt((AswangData.Strength - 10) /2  + Mathf.FloorToInt(Growthrate * Level)); }
    }

       public int Dexterity
    {
        get { return Mathf.FloorToInt((AswangData.Dexterity - 10) / 2) + Mathf.FloorToInt(Growthrate * Level); }
    }

    public int Constitution
    {
        get { return Mathf.FloorToInt((AswangData.Constitution - 10) / 2) + Mathf.FloorToInt(Growthrate * Level); }
    }

    public int Intelligence
    {
        get { return Mathf.FloorToInt((AswangData.Intelligence - 10) / 2 + Mathf.FloorToInt(Growthrate * Level)); }
    }
    
    public int Charisma
    {
        get { return Mathf.FloorToInt((AswangData.Charisma - 10) / 2 + Mathf.FloorToInt(Growthrate * Level)); }
    }

    public float Growthrate
    {
        get { return AswangData.Growthrate; }
    }
    public bool CheckForLevelUp()
    {
        if (Exp> AswangData.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        else
        {
            return false;   
        }
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


}
