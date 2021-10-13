using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE,
        WIN
    }

    public State state = State.PATROL;

    private Transform playerTr; 
    private Transform enemyTr; 
    private Animator animator;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public bool isDie = false; 
    private WaitForSeconds ws;

    private MoveAgent moveAgent;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private EnemyFire enemyfire;
    [SerializeField] private Playerctrl playerctrl;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        playerTr = player.transform;
        enemyTr = this.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyfire = GetComponent<EnemyFire>();
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;
            float dist = (playerTr.position - enemyTr.position).sqrMagnitude;
            if (dist <= attackDist * attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist * traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            if (playerctrl.currentplayerHP <= 0)
            {
                state = State.WIN;
            }
            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    enemyfire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    animator.SetBool("Attack", false);
                    break;
                case State.TRACE:
                    enemyfire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    animator.SetBool("Attack", false);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    animator.SetBool("Attack",true);
                    enemyfire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyfire.isFire = false;
                    moveAgent.Stop();
                    animator.SetInteger("DieIdx", Random.Range(0, 3));
                    animator.SetTrigger("Die");
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
                case State.WIN:
                    isDie = false;
                    enemyfire.isFire = false;
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    animator.SetBool("Attack", false);
                    break;
            }
        }
    }

    private void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, traceDist);
    }

    
}
