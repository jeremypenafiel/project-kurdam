using DG.Tweening;
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
    [SerializeField] GameObject expBar;
    [SerializeField] TextMeshProUGUI lvl;

    Aswang _aswang;
    public void SetData(Aswang aswang)
    {   
        _aswang = aswang;
        nameText.text = aswang.Base.Aname;
        armorClassText.text =  $"{aswang.ArmorClass}";
        hpBar.SetHP((float)aswang.HP / aswang.MaxHP);
        lvl.text = "Lvl " + _aswang.Level;
        SetExp();

    }

    public void SetExp()
    {
        if (expBar == null) return;
        
        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }

/*    public void SetLevel()
    {

    }*/

    public IEnumerator SetExpSmooth(bool reset=false)
    {
        if (expBar == null) yield break;
        
        if (reset == true)
        {
            expBar.transform.localScale = new Vector3(0, 1, 1);
        }
        float normalizedExp = GetNormalizedExp();
        lvl.text = "Lvl " + _aswang.Level;
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currLevelExp = _aswang.Base.GetExpForLevel(_aswang.Level);
        int nextLevelExp = _aswang.Base.GetExpForLevel(_aswang.Level+1);

        float normalizedExp = (float)(_aswang.Exp -currLevelExp) / (_aswang.Exp -nextLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }
    public IEnumerator UpdateHP()
    {
        yield return hpBar.setHPSmooth((float) _aswang.HP / _aswang.MaxHP);
    }
}
    