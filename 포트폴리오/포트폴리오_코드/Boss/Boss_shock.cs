using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_shock : MonoBehaviour
{
    private int damage = 30;

    private void Start()
    {

    }
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Playerctrl>().TakePlayerDamage(damage);
            if (other.GetComponent<Playerctrl>().currentplayerHP <= 0)
            {
                other.GetComponent<Playerctrl>().OnDie();
            }
            else
            {
                other.GetComponent<Playerctrl>().TakePlayerDamage_animator();
            }
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
