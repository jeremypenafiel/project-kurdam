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
        nameText.text = aswang.Base.Aname;
        armorClassText.text =  $"{aswang.ArmorClass}";
        hpBar.SetHP((float)aswang.HP / aswang.MaxHP);

    }
    public void UpdateHP()
    {
        hpBar.SetHP((float) _aswang.HP / _aswang.MaxHP);
    }
}
    