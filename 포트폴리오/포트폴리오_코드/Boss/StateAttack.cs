using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateAttack : Istate<BossCtrl>
{
    public void OnEnter(BossCtrl boss)
    {
        boss.Attack_patron = Random.Range(0, 3);
        boss.delay = 0f;
    }

    public void OnFixedUpdate(BossCtrl boss)
    {
        
    }

    public void OnUpdate(BossCtrl boss)
    {
        boss.transform.LookAt(boss.player_position);
        float dist = (boss.player_position.position - boss.boss_position.position).sqrMagnitude;
        boss.delay += Time.deltaTime;
        if(boss.delay >= 5f)
        {
            boss.Attack_Parton();
            boss.delay = 0f;
        }
        else if(boss.delay < 5f)
        {
            if (dist > boss.attackDist * boss.attackDist)
            {
                boss.delay = 0f;
                boss.ChangeState(BossCtrl.BossState.Trace);
            }
        }
    }

    public void OnExit(BossCtrl boss)
    {

    }
}
