using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTrace : Istate<BossCtrl>
{
    public void OnEnter(BossCtrl boss)
    {
        boss.animator.SetBool("Move", true);
    }

    public void OnFixedUpdate(BossCtrl boss)
    {

    }

    public void OnUpdate(BossCtrl boss)
    {
        float dist = (boss.player_position.position - boss.boss_position.position).sqrMagnitude;
        if (dist > boss.attackDist* boss.attackDist)
        {
            boss.movement.traceTarget = boss.player_position.position;
        }
        else if ( dist <= boss.attackDist * boss.attackDist)
        {
            boss.movement.Stop();
            boss.animator.SetBool("Move", false);
            boss.ChangeState(BossCtrl.BossState.Attack);
        }
    }

    public void OnExit(BossCtrl boss)
    {
        
    }
}
