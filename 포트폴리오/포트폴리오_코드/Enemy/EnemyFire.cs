using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private Transform playerTr;
    private Transform enemyTr;
    private Animator animator;

    private readonly int hashFire = Animator.StringToHash("Fire");

    private float nextFire = 0.0f;
    private readonly float fireRate = 0.1f;
    private readonly float damping = 10.0f;

    public bool isFire = false;

    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private Transform Firepos;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = this.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.2f, 0.5f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        GameObject go;
        go = Pool.Instance.Spawn("Enemy_Bullet", Firepos.position, Firepos.rotation);
        if (go != null)
        {
            Pool.Instance.Despawn(go, 2f);
        }
    }

    
}
