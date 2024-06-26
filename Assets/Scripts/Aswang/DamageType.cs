using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modifier { Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma };

[CreateAssetMenu(fileName = "Damage Type", menuName = "Damage Type")]
public class DamageType: ScriptableObject
{
    [SerializeField] private Modifier modifier;
    public Modifier Modifier { get { return modifier; } }

    public string GetModifierText()
    {
        return modifier.ToString();
    }

    public override string ToString()
    {
        return $"{base.ToString().Remove(base.ToString().Length-13)}";
    }
}   


