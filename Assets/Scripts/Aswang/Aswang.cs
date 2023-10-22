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
        HP = Maxhp;

        //generate moves
        moves = new List<Moves>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                moves.Add(new Moves(move.movesbase));

            if (moves.Count >= 4)
                break;

        }
    }
        public int Attack
    {
        get { return Mathf.FloorToInt((Base.attack * Level) / 100f) + 5; }
    }

       public int Maxhp
    {
        get { return Mathf.FloorToInt((Base.maxhp * Level) / 100f) + 5; }
    }

       public int Magical
    {
        get { return Mathf.FloorToInt((Base.magical * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.defense * Level) / 100f) + 5; }
    }

    public int Faith
    {
        get { return Mathf.FloorToInt((Base.faith * Level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.speed * Level) / 100f) + 5; }
    }

}
