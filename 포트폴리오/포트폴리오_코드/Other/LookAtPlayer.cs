using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform playerpos;
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("playeR");
        playerpos = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerpos);
    }
}
