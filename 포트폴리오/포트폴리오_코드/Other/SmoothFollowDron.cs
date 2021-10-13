using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowDron : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;
    public Transform player_target;
    [SerializeField]
    private float distance = 0.5f;
    [SerializeField]
    private float distance2 = 1.5f;

    private void Update()
    {
        var dir = player_target.position - transform.position;

        if (distance >= Vector3.Distance(player_target.position, transform.position))
        {
            transform.Translate(Vector3.zero);
        }
        else if (distance2 >= Vector3.Distance(player_target.position, transform.position))
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
        else
        {
            transform.position += dir.normalized * (speed+5.0f) * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, distance2);
    }

}