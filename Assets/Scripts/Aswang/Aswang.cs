using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aswang
{
    public AswangBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Moves> moves {  get; set; }
    public Aswang(AswangBase abase, int alevel)
    {
        Base = abase;
        this.Level = alevel;
        HP = MaxHP;

        //generate moves
        moves = new List<Moves>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                moves.Add(new Moves(move.MovesBase));

            if (moves.Count >= 4)
                break;

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
        get { return Mathf.FloorToInt((Base.Strength - 10) /2); }
    }

       public int Dexterity
    {
        get { return Mathf.FloorToInt((Base.Dexterity - 10) / 2); }
    }

    public int Constitution
    {
        get { return Mathf.FloorToInt((Base.Constitution - 10) / 2); }
    }

    public int Intelligence
    {
        get { return Mathf.FloorToInt((Base.Intelligence - 10) / 2); }
    }

    public int Charisma
    {
        get { return Mathf.FloorToInt((Base.Charisma - 10) / 2); }
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
