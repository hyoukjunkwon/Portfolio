using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack_shock : Istate<BossCtrl>
{
    public void OnEnter(BossCtrl boss)
    {
        boss.animator.SetBool("Attack", true);
        boss.animator.SetInteger("Attack_Pattern", 1);
        //boss.Invoke("StopAttack", 7);
    }

    public void OnFixedUpdate(BossCtrl boss)
    {

    }

    public void OnUpdate(BossCtrl boss)
    {
        boss.transform.LookAt(boss.player_position);
    }

    public void OnExit(BossCtrl boss)
    {

    }
}
