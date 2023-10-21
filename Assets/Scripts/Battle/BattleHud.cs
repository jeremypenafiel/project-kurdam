using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI armorClassText;
    [SerializeField] HPBar hpBar;

    Aswang _aswang;
    public void SetData(Aswang aswang)
    {   
        _aswang = aswang;
        nameText.text = aswang.Base.aname;
        armorClassText.text =  $"{aswang.armorClass}";
        hpBar.SetHP((float)aswang.HP / aswang.Maxhp);

    }
    public void UpdateHP()
    {
        hpBar.SetHP((float) _aswang.HP / _aswang.Maxhp);
    }
}
    