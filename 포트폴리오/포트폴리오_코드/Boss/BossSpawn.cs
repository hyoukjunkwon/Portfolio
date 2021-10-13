using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private Playerctrl playerCtrl;
    private float zone = 10f;
    [SerializeField] private GameObject boss;
    [SerializeField] private Transform spawnpos;

    void Update()
    {
        float dist = (playerCtrl.transform.position - transform.position).sqrMagnitude;
        
        if (dist <= zone * zone)
        {
            boss.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zone);
    }
}
