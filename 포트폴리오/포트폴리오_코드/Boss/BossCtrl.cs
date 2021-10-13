using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCtrl : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    public Movement movement;
    public float attackDist = 7.0f;
    [HideInInspector] public Transform player_position;
    [HideInInspector] public Transform boss_position;
    [HideInInspector] public bool isDie = false;
    [HideInInspector] public float currenthp;
    [HideInInspector] public float maxhp = 300;
    [HideInInspector] public int Attack_patron;
    [HideInInspector] public int Die_patron;
    public float delay;
    public float firedelay;
    public Transform bullet_pos_L;
    public Transform bullet_pos_R;
    public Transform Missile_pos_L;
    public Transform Missile_pos_R;
    public float Randompos;

    public Transform Missilepos = null;

    [SerializeField] public GameObject hpbarpanal;
    public GameObject shockattackCollision;
    [SerializeField] private Slider hpbar;
    [SerializeField] private GameObject bossTag;
    [SerializeField] private Playerctrl playerctrl;
    [SerializeField] private Transform slam_position;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        boss_position = this.GetComponent<Transform>();
        var player = GameObject.FindGameObjectWithTag("Player");
        player_position = player.transform;
    }

    public enum BossState
    {
        Trace,
        Die,
        Attack,
        Attack_missile,
        Attack_shock,
        Attack_bullet,
        Win
    }

    private StateMachine<BossCtrl> m_player;

    // Dictionary<키, 밸류>
    private Dictionary<BossState, Istate<BossCtrl>> m_states = new Dictionary<BossState, Istate<BossCtrl>>();

    private void Start()
    {
        m_states.Add(BossState.Trace, new StateTrace());
        m_states.Add(BossState.Die, new StateDie());
        m_states.Add(BossState.Attack, new StateAttack());
        m_states.Add(BossState.Attack_missile, new StateAttack_missile());
        m_states.Add(BossState.Attack_shock, new StateAttack_shock());
        m_states.Add(BossState.Attack_bullet, new StateAttack_bullet());
        m_states.Add(BossState.Win, new StateWin());

        m_player = new StateMachine<BossCtrl>(this, m_states[BossState.Trace]);
        currenthp = maxhp;
        hpbar.GetComponent<BossHPBar>().Setup(this.GetComponent<BossCtrl>());
        hpbarpanal.SetActive(false);
        StartCoroutine(CheckState());
    }

    public void ChangeState(BossState state)
    {
        Debug.LogWarning(state);
        m_player.SetState(m_states[state]);
    }

    private void FixedUpdate()
    {
        m_player.OnFixedUpdate();
    }

    void Update()
    {
        m_player.OnUpdate();
        Randompos = Random.Range(-0.5f,0.5f);
        if (currenthp <= 0)
        {
            ChangeState(BossCtrl.BossState.Die);
            isDie = true;
            bossTag.SetActive(false);
        }

        if (currenthp < maxhp && !isDie)
        {
            hpbarpanal.SetActive(true);
        }

        
    }

    public void TakeBossDamage(int damage)
    {
        if (isDie == true)
            return;
        currenthp -= damage;
    }

    public void Attack_Parton()
    {
        switch (Attack_patron)
        {
            case 0://미사일
                ChangeState(BossCtrl.BossState.Attack_missile);
                break;
            case 1://쇼크웨이브
                ChangeState(BossCtrl.BossState.Attack_shock);
                break;
            case 2://총알발사
                ChangeState(BossCtrl.BossState.Attack_bullet);
                break;
        }
    }

    public void StopAttack()
    {
        animator.SetBool("Attack",false);
        ChangeState(BossState.Attack);
        movement.Stop();
    }

    public void shockwave()
    {
        Vector3 aim = new Vector3(player_position.position.x, player_position.position.y + 50, player_position.position.z -2f);
        transform.position = aim;
    }

    public void StopMove()
    {
        movement.Stop();
    }

    public void FireBullet()
    {
        GameObject a,b;
        Vector3 dir_L = new Vector3(bullet_pos_L.position.x + Randompos, bullet_pos_L.position.y + Randompos, bullet_pos_L.position.z);
        Vector3 dir_R = new Vector3(bullet_pos_R.position.x + Randompos, bullet_pos_R.position.y + Randompos, bullet_pos_R.position.z);
        a = Pool.Instance.Spawn("Boss_Bullet", dir_L, bullet_pos_L.rotation);
        b = Pool.Instance.Spawn("Boss_Bullet", dir_R, bullet_pos_R.rotation);
        if (a != null)
            Pool.Instance.Despawn(a, 3f);
        if (b != null)
            Pool.Instance.Despawn(b, 3f);
    }

    public void FireMissile()
    {
        GameObject t_missile_L = Pool.Instance.Spawn("missile", Missile_pos_L.position, Quaternion.identity);
        GameObject t_missile_R = Pool.Instance.Spawn("missile", Missile_pos_R.position, Quaternion.identity);
        //if (t_missile_L != null)
        //    Pool.Instance.Despawn(t_missile_L, 3f);
        //if (t_missile_R != null)
        //    Pool.Instance.Despawn(t_missile_R, 3f);
        t_missile_L.GetComponent<Rigidbody>().velocity = Vector3.up * 7f;
        t_missile_R.GetComponent<Rigidbody>().velocity = Vector3.up * 7f;
    }

    public void OnshockAttackCollision()
    {
        shockattackCollision.SetActive(true);
        GameObject a = Pool.Instance.Spawn("GroundSlam (1)", slam_position.position, Quaternion.identity);
        if (a != null)
            Pool.Instance.Despawn(a, 1.5001f);
    }

    public void Destroy()
    {
        Destroy(hpbarpanal);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

    public void delete()
    {
        Destroy(hpbarpanal);
        Destroy(gameObject);
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if(playerctrl.currentplayerHP <= 0)
            {
                ChangeState(BossCtrl.BossState.Win);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
