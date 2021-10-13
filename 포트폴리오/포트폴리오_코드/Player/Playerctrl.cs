using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;
using UnityEngine.UI;
using DG.Tweening;

public class Playerctrl : MonoBehaviour
{
    public enum Weapon
    {
        G_SWORD,
        KATANA
    }
    public Weapon weapon = Weapon.G_SWORD;
    [SerializeField]
    private Transform cameraTransform;
    private Movement3D movement3D;
    private Animationctrl animatorCtrl;
    private bool Dash = false;

    [SerializeField]
    private GameObject G_Sword;
    [SerializeField]
    private GameObject Katana;
    private bool move = true;
    private bool Die = false;

    [SerializeField]
    private float maxplayerHP;
    public float currentplayerHP;

    [SerializeField]
    private Slider Hpbar;
    [SerializeField]
    private GameObject Dron;
    private CapsuleCollider Cap;
    public CameraCtrl Camera;
    public GameObject hpbarpanel;


    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentplayerHP = maxplayerHP;

        movement3D = GetComponent<Movement3D>();
        animatorCtrl = GetComponentInChildren<Animationctrl>();
        Cap = GetComponent<CapsuleCollider>();

    }

    private void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        animatorCtrl.OnMovement(x, z);
        if (Dash == false)
        {
            movement3D.MoveSpeed = z > 0 ? 6.0f : 3.5f;
            movement3D.MoveTo(cameraTransform.rotation * new Vector3(x, 0, z));
            transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        }
        else if (Dash == true)
        {
            movement3D.MoveSpeed = 10.0f;
            movement3D.MoveTo(cameraTransform.rotation * new Vector3(0, 0, z));
            transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        }

        if (move == false)
        {
            movement3D.MoveSpeed = 0f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            animatorCtrl.OnDash();
            Dash = true;
        }
        else
        {
            animatorCtrl.OffDash();
            Dash = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            animatorCtrl.OnBladeAttack_L();
            move = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            animatorCtrl.OnBladeAttack_R();
            move = false;
        }

        if (weapon == Weapon.G_SWORD)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weapon = Weapon.KATANA;
                animatorCtrl.SetKatana();
            }
        }

        if(weapon == Weapon.KATANA)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weapon = Weapon.G_SWORD;
                animatorCtrl.SetG_Sword();
            }
        }

        Hpbar.value = Mathf.Lerp(Hpbar.value, currentplayerHP / maxplayerHP, Time.deltaTime * 5);
        if (currentplayerHP <= 0)
        {
            Die = true;
            move = false;
            Destroy(Dron, 1f);
            Cap.enabled = false;
        }

    }

    public void ChangeG_Sword()
    {
        Katana.SetActive(false);
        G_Sword.SetActive(true);
        move = false;
    }

    public void ChangeKatana()
    {
        G_Sword.SetActive(false);
        Katana.SetActive(true);
        move = false;
    }

    public void MoveStart()
    {
        move = true;
    }

    public void MoveStop()
    {
        move = false;
    }

    public void TakePlayerDamage(int damage)
    {
        if (Die == true)
            return;
        currentplayerHP -= damage;
        hpbarpanel.transform.DOShakePosition(0.5f,4f);
    }

    public void TakePlayerDamage_animator()
    {
        if(Die == true)
           return;
        animatorCtrl.Ondamage();
        hpbarpanel.transform.DOShakePosition(0.5f, 4f);
        Camera.shakecamera();
    }

    public void OnDie()
    {
        animatorCtrl.OnDie();
    }
}
