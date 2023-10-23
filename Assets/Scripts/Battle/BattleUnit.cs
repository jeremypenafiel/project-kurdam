using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] AswangBase _base;
    [SerializeField] BattleHud hud;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Aswang aswang { get; set; }

    public BattleHud Hud { get { return hud; }}

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public void Setup()
    {
        aswang = new Aswang(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = aswang.Base.Backsprite;
        else
            GetComponent<Image>().sprite = aswang.Base.Frontsprite;
        hud.SetData(aswang);

    }

    public string GetSubject()
    {
        if (isPlayerUnit)
            return "You";
        return "Enemy";
    }

    public BattleState GetState()
    {
        if(isPlayerUnit) return BattleState.Busy;
        return BattleState.EnemyMove;
    }

    public Moves GetMove(int currentMove)
    {
        if (isPlayerUnit) return aswang.moves[currentMove] ;
        return aswang.GetRandomMove();
    }

    public string GetDefeatText()
    {
        if (isPlayerUnit) return "The enemy has defeated you!";
        return "You have defeated the enemy!";
    }

}
