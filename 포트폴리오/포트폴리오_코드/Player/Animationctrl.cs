using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;

public class Animationctrl : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
    [SerializeField]
    private GameObject G_Sword_xtrail;
    [SerializeField]
    private GameObject Katana_xtrail;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMovement(float horizontal, float vertical)
    {
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        if (horizontal == 0 && vertical == 0) 
        {
            animator.SetBool("isMove", false);
        }
        else 
        {
            animator.SetBool("isMove", true);
        }
    }

    public void OnBladeAttack_L()
    {
        animator.SetTrigger("Mouse_Left_Click");
    }

    public void OnBladeAttack_R()
    {
        animator.SetTrigger("Mouse_Right_Click");
    }

    public void OnDash()
    {
        animator.SetBool("Dash",true);
    }

    public void OffDash()
    {
        animator.SetBool("Dash",false);
    }

    public void SetG_Sword()
    {
        animator.SetInteger("Weapon",0);
        G_Sword_xtrail.SetActive(true);
    }

    public void SetKatana()
    {
        animator.SetInteger("Weapon",1);
        Katana_xtrail.SetActive(true);
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void Ondamage()
    {
        animator.SetTrigger("Damage");
    }

    public void OnDie()
    {
        animator.SetTrigger("Die");
    }
}