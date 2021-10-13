using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class autotargeting : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float attackRange = 15f;
    [SerializeField]
    private string enemyTag = "enemy";
    [SerializeField]
    private string bossTag = "boss";
    [SerializeField]
    private Transform partToRotate;
    [SerializeField]
    private float turnSpeed = 10f;
    [SerializeField]
    private float attackRate = 5f;

    [SerializeField]
    private Transform FireposL;
    [SerializeField]
    private Transform FireposR;
    private float firecountdown = 0f;
    [SerializeField]
    private Image crosshair1;
    private bool check = true;
    [SerializeField]
    private Transform Playerpos;
    [SerializeField]
    private Transform FireposC;
    public float current_cooltime;
    public float skillcooltime;
    public Transform UI_position;
    [SerializeField]
    private GameObject SKL;

    [SerializeField] private PartivleGroupEmitteR chargeEffectEmitter;
    [SerializeField] private PartivleGroupEmitteR chargeEndEffectEmitter;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] private float chargeDisableDelay = 0f;
    [SerializeField] private float spreadAcceleration = 1f;
    private float currentSpread;

    public void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] Boss = GameObject.FindGameObjectsWithTag(bossTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
        foreach (GameObject enemy in Boss)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
        if (nearestEnemy != null && shortestDistance <= attackRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.3f);
        current_cooltime = 0f;
    }

    void Update()
    {
        if (target == null)
        {
            crosshair1.gameObject.SetActive(false);
            transform.LookAt(Playerpos);
            current_cooltime -= Time.deltaTime;
            if (current_cooltime <= 0)
            {
                SKL.SetActive(false);
            }
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        crosshair1.transform.position = screenPosition;
        crosshair1.gameObject.SetActive(true);

        if (firecountdown <= 0f)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (check == true)
                {
                    SpawnBullet_L();
                }
                else if(check == false)
                {
                    SpawnBullet_R();
                }
            }
        }
        firecountdown -= Time.deltaTime;
        if (current_cooltime <= 0f)
        {
            SKL.SetActive(false);
            if (Input.GetKeyDown(KeyCode.R))
            {
                Shot();
                current_cooltime = skillcooltime;
                SKL.SetActive(true);
            }
        }
        current_cooltime -= Time.deltaTime;
    }

    protected virtual void Shot()
    {
        StartCoroutine(GunShot());
    }

    private IEnumerator GunShot()
    {
        if (chargeEffectEmitter)
        {
            chargeEffectEmitter.EnableEmission(true);
            chargeEffectEmitter.Emit(1);
            float elapsedTime = 0;
            while (elapsedTime <= chargeTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(DisableChargeEffect());

            if (chargeEndEffectEmitter)
            {
                chargeEndEffectEmitter.Emit(1);
            }
        }
        StartCoroutine(SpawnBullets());

        currentSpread += spreadAcceleration;
        if (currentSpread >= 1f)
            currentSpread = 1f;
    }

    private IEnumerator DisableChargeEffect()
    {
        yield return new WaitForSeconds(chargeDisableDelay);
        chargeEffectEmitter.EnableEmission(false);
        chargeEffectEmitter.ClearParticles();
    }

    private IEnumerator SpawnBullets()
    {
        Spawn_Skill();
        yield return null;
    }

    private void SpawnBullet_L()
    {
        GameObject go;
        go = Pool.Instance.Spawn("Player_Bullet(pt)", FireposL.position, FireposL.rotation);
        if (go != null)
        {
            Pool.Instance.Despawn(go, 2f);
        }
        firecountdown = 1f / attackRate;
        check = false;
    }

    private void SpawnBullet_R()
    {
        GameObject go;
        go = Pool.Instance.Spawn("Player_Bullet(pt)", FireposR.position, FireposR.rotation);
        if (go != null)
        {
            Pool.Instance.Despawn(go, 2f);
        }
        firecountdown = 1f / attackRate;
        check = true;
    }

    private void Spawn_Skill()
    {
        GameObject go;
        go = Pool.Instance.Spawn(("Projectile 6"),FireposC.position,FireposC.rotation);
        if(go != null)
        {
            Pool.Instance.Despawn(go, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

