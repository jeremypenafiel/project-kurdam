using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] AswangBase _base;
    [SerializeField] BattleHud hud;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Aswang aswang { get; set; }

    public BattleHud Hud { get { return hud; }}

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public void Setup()
    {
        aswang = new Aswang(_base, level);
        if (isPlayerUnit)
            image.sprite = aswang.Base.Backsprite;
        else
            image.sprite = aswang.Base.Frontsprite;
        image.color = originalColor;
        PlayEnterAnimation();
        hud.SetData(aswang);
        

        
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-400f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(400f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
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
