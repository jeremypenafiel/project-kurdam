using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Aswang aswang)
    {
        nameText.text = aswang.Base.aname;
        levelText.text = "Lvl" + aswang.Level;
        hpBar.SetHP(0.5f);/*((float) aswang.HP / aswang.Maxhp)*/

    }
}
