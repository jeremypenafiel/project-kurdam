using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] AswangBase _base;
    [SerializeField] int level;

    public Aswang aswang { get; set; }
    public void Setup()
    {
        new Aswang(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = pokemon.Base.BackSprite;
        else
            GetComponent<Image>().sprite = pokemon.Base.FrontSprite;

    }
}
