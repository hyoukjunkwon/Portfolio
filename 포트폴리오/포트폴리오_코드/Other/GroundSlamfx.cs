using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamfx : MonoBehaviour
{
    public GameObject groundSlamFX;
    public GameObject groundShatterAnim;
    private Animation animQuake;

    void Start()
    {
        animQuake = groundShatterAnim.GetComponent<Animation>();
    }


    void Update()
    {
        StartCoroutine("StartGroundSlam");
        Invoke("Reset", 1.5f);
    }


    IEnumerator StartGroundSlam()
    {
        yield return new WaitForSeconds(0.01f);

        animQuake["Quake"].speed = 1;
        groundShatterAnim.GetComponent<Animation>().Play();

        groundSlamFX.SetActive(true);
    }

    private void Reset()
    {
        animQuake["Quake"].time = 0.0f;
        animQuake["Quake"].speed = 0;
        groundShatterAnim.GetComponent<Animation>().Play();

        groundSlamFX.SetActive(false);
    }
}
