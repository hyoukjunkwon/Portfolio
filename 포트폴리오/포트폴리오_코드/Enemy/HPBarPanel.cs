using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarPanel : MonoBehaviour
{
    [SerializeField]
    private EnemyCtrl enemy;

    void Start()
    {
        enemy.GetComponent<EnemyCtrl>();
    }

    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemy.headupPosition.transform.position);
        this.transform.position = screenPosition;
    }
}
