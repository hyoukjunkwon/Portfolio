using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    private BossCtrl bossHP;
    private Slider hpSlider;

    private void Update()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, bossHP.currenthp / bossHP.maxhp, Time.deltaTime * 5);
    }

    public void Setup(BossCtrl bosshp)
    {
        this.bossHP = bosshp;
        hpSlider = GetComponent<Slider>();
    }
}
