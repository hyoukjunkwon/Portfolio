using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointGizmo : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(this.transform.position, _radius);
    }
}
