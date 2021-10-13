using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWin : Istate<BossCtrl>
{
    public void OnEnter(BossCtrl boss)
    {

    }

    public void OnFixedUpdate(BossCtrl boss)
    {

    }

    public void OnUpdate(BossCtrl boss)
    {
        boss.movement.Stop();
    }

    public void OnExit(BossCtrl boss)
    {

    }
}
