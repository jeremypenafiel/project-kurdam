using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves 
{
    public MovesBase Base { get; set; }
    
    public Moves(MovesBase pBase)
    {
        Base = pBase;

    }

    public DamageType type
    {
        get {return Base.Type; }
    }
}
