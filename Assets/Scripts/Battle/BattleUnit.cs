using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] AswangBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Aswang aswang { get; set; }
    public void Setup()
    {
        aswang = new Aswang(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = aswang.Base.Backsprite;
        else
            GetComponent<Image>().sprite = aswang.Base.Frontsprite;

    }
}
