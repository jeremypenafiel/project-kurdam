using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aswang
{
    AswangBase _base;
    int level;

    public int currentHP { get; set; }



    public List<Moves> moves {  get; set; }
    public Aswang(AswangBase abase, int alevel)
    {
        _base = abase;
        this.level = alevel;
        currentHP = _base.maxhp;

        //generate moves
        moves = new List<Moves>();
        foreach (var move in _base.LearnableMoves)
        {
            if (move.Level <= level)
                moves.Add(new Moves(move.movesbase));

            if (moves.Count >= 4)
                break;

        }
    }
        public int Attack
    {
        get { return Mathf.FloorToInt((_base.attack * level) / 100f) + 5; }
    }

       public int Maxhp
    {
        get { return Mathf.FloorToInt((_base.maxhp * level) / 100f) + 5; }
    }

       public int Magical
    {
        get { return Mathf.FloorToInt((_base.magical * level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((_base.defense * level) / 100f) + 5; }
    }

    public int Faith
    {
        get { return Mathf.FloorToInt((_base.faith * level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((_base.speed * level) / 100f) + 5; }
    }

}
