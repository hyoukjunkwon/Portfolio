using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPView : MonoBehaviour
{
    private EnemyCtrl enemyHP;
    private Slider hpSlider;

    private void Update()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, enemyHP.CurrentHP / enemyHP.MaxHP, Time.deltaTime*5);
    }

    public void Setup(EnemyCtrl enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
    }
}