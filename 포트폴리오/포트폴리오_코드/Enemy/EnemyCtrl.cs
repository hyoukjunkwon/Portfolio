using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    private Animator animator;
    private EnemyAI enemy;

    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    [SerializeField]
    private GameObject HpBar;
    [SerializeField]
    private Slider HP;

    public Transform headupPosition;

    [SerializeField]
    private GameObject target;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyAI>();
        currentHP = maxHP;
    }

    private void Start()
    {
        HP.GetComponent<EnemyHPView>().Setup(this.GetComponent<EnemyCtrl>());
        HpBar.SetActive(false);
    }

    private void Update()
    {
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.state = EnemyAI.State.DIE;
            target.SetActive(false);
            Invoke("delete", 4f);
        }

        if (currentHP < maxHP && !isDie)
        {
            HpBar.SetActive(true);
        }
    }

    public void TakeDamage_Sword(int damage)
    {
        if (isDie == true)
            return;
        currentHP -= damage;
        animator.SetTrigger("OnHit");
    }

    public void TakeDamage_Bullet(int damage)
    {
        if (isDie == true)
            return;
        currentHP -= damage;
    }

    private void delete()
    {
        Destroy(HpBar);
        Destroy(gameObject);
    }
}
