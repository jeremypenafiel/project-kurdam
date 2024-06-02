using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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

    public Moves(MoveSaveData saveData)
    {
        Base = MovesDB.GetObjectByName(saveData.name);
    }

    public MoveSaveData GetSaveData()
    {
        var saveData = new MoveSaveData()
        {
            name = Base.name
        };
        return saveData;
    }


}

[System.Serializable]
public class MoveSaveData
{
    public string name;
}
