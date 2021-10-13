using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollosion : MonoBehaviour
{
    private int G_Sword_damage;
    private int Katana_damage;
    [SerializeField]
    private Playerctrl playerctrl;
    [SerializeField]
    private GameObject Effect;

    private void Start()
    {
        G_Sword_damage = Random.Range(35, 41);
        Katana_damage = Random.Range(25, 31);
    }
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") )
        {
            if (playerctrl.weapon == Playerctrl.Weapon.G_SWORD)
            {
                other.GetComponent<EnemyCtrl>().TakeDamage_Sword(G_Sword_damage);
                startfx();
            }
            else if (playerctrl.weapon == Playerctrl.Weapon.KATANA)
            {
                other.GetComponent<EnemyCtrl>().TakeDamage_Sword(Katana_damage);
                startfx();
            }
        }
        if( other.CompareTag("Boss"))
        {
            if (playerctrl.weapon == Playerctrl.Weapon.G_SWORD)
            {
                other.GetComponent<BossCtrl>().TakeBossDamage(G_Sword_damage);
                startfx();
            }
            else if (playerctrl.weapon == Playerctrl.Weapon.KATANA)
            {
                other.GetComponent<BossCtrl>().TakeBossDamage(Katana_damage);
                startfx();
            }
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    private void startfx()
    {
        GameObject ex = Pool.Instance.Spawn("hit-line-2", transform.position, Quaternion.identity);
        Pool.Instance.Despawn(ex, 1f);
    }
}
