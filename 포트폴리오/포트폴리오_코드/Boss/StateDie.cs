using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateDie : Istate<BossCtrl>
{
    public void OnEnter(BossCtrl boss)
    {
        boss.Die_patron = Random.Range(0, 2);
        boss.animator.SetTrigger("Die");
        boss.animator.SetInteger("Die_Pattern",boss.Die_patron);
        boss.Invoke("delete", 5.5f);
    }

    public void OnFixedUpdate(BossCtrl boss)
    {

    }

    public void OnUpdate(BossCtrl boss)
    {

    }

    public void OnExit(BossCtrl boss)
    {

    }
}
