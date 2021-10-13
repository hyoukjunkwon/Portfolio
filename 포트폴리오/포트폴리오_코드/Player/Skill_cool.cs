using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_cool : MonoBehaviour
{
    [SerializeField]
    private autotargeting skill_cooltine;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Text cooltime;

    private void Update()
    {
        cooltime.text = skill_cooltine.current_cooltime.ToString("N1");
        hpSlider.value = Mathf.Lerp(hpSlider.value, skill_cooltine.current_cooltime / skill_cooltine.skillcooltime, Time.time);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(skill_cooltine.UI_position.transform.position);
        this.transform.position = screenPosition;
        
    }
}
