using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{

    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject expBar;

    Fighter _fighter;

    public void SetData(Fighter fighter)
    {

        _fighter = fighter;
        nameText.text = fighter.Base.Name;
        SetLevel();
        hpBar.SetHP((float) fighter.HP / fighter.MaxHp);       
    }

    float GetNormalizedExp()
    {
        int currLevelExp = _fighter.Base.GetExpForLevel(_fighter.Level);
        int nextLevelExp = _fighter.Base.GetExpForLevel(_fighter.Level + 1);

        float normalizedExp = (float)(_fighter.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public IEnumerator UpdateHP()
    {
        if (_fighter.HpChanged)
        {
            yield return hpBar.SetHPSmooth((float)_fighter.HP / _fighter.MaxHp);
            _fighter.HpChanged = false;
        }
    }

    public void SetLevel()
    {
        levelText.text = "Lvl " + _fighter.Level;
    }

    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalizedExp();

        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }


}