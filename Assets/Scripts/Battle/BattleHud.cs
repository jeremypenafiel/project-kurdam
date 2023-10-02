using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Aswang aswang)
    {
        nameText.text = aswang.Base.aname;
        levelText.text = "Lvl" + aswang.Level;
        hpBar.SetHP((float) aswang.HP / aswang.Maxhp);

    }
}
