using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float minDistance = 1;
    [SerializeField]
    private float maxDistance = 20;
    [SerializeField]
    private float wheelSpeed = 500;
    [SerializeField]
    private float xMoveSpeed = 500;
    [SerializeField]
    private float yMoveSpeed = 250;
    private float yminLimit = 0;
    private float ymaxLimit = 80;
    private float x, y;
    private float distance;

    private void Awake()
    {
        distance = Vector3.Distance(transform.position, target.position);
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void Update()
    {
        if (target == null) return;

        x += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;
        y = ClampAngle(y, yminLimit, ymaxLimit);
        transform.rotation = Quaternion.Euler(y, x, 0);

        distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    //private void LateUpdate()
    //{
    //    if (target == null) return;
    //    transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    //}

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void shakecamera()
    {
        transform.DOShakePosition(0.5f);
    }
}
