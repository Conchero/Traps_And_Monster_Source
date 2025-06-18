using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    GameObject arm;
    [SerializeField] GameObject SpawnBullet;
    [SerializeField] GameObject bulletPrefab;


    bool shoot = false;
    int nbBulletInMagazine;

    float fireRate = 1.0f;
    float reloadCoolDown = 1.5f;
    public float forceImpulse = 1.0f;

    float timerAttack = 0;
    bool charging = false;
    bool chargedBullet = false;

    float rotationUpDownArm;

    bool comboBullet = false;
    int comboStreak = 0;
    float comboTimer = 0; 

    public int[] stats = new int[1];
    public bool ChargedBullet { get => chargedBullet; set => chargedBullet = value; }
    public int[] Stats { get => stats; set => stats = value; }
    public bool ComboBullet { get => comboBullet; set => comboBullet = value; }
    public int ComboStreak { get => comboStreak; set => comboStreak = value; }
    public float ComboTimer { get => comboTimer; set => comboTimer = value; }

    TpsController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        arm = gameObject.GetComponentInParent<WeaponBehaviour>().gameObject;
        stats[0] = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot == true)
        {
          
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                shoot = false;
                fireRate = 1.0f;
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            arm.transform.localEulerAngles = arm.transform.localEulerAngles = new Vector3(arm.GetComponentInParent<TpsController>().playerCameraParent.rotation.eulerAngles.x - 5f, 0, 0);

        }
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        { 
            arm.transform.localEulerAngles = new Vector3(arm.GetComponentInParent<TpsController>().playerCameraParent.rotation.eulerAngles.x -5f ,0,0);
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "LeftTrigger", false))
            {
                timerAttack = 0;
                charging = false;
            }
        }
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetMouseButton(1) == false && charging == true)
            {
                timerAttack = 0;
                charging = false;
            }
        }

       if (comboStreak > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > 2.5f)
            {
                comboTimer = 0;
                comboStreak = 0;
            }
        }

    }

    public void Attack()
    {

    }


    public void SpecialCaCAttack()
    {
   
    }

    public void DistanceAttack()
    {
        if (shoot == false)
        {
            chargedBullet = false;
            if (comboStreak >= 5)
            {
                comboBullet = true;
            }
            GameObject bulletGameObject = Instantiate(bulletPrefab, SpawnBullet.transform.position, arm.transform.rotation * Quaternion.Euler(90,0,0));
            bulletGameObject.name = gameObject.transform.parent.gameObject.transform.parent.gameObject.name + "Bullet";
            bulletGameObject.GetComponent<Rigidbody>().AddForce(arm.transform.TransformVector(Vector3.forward) * forceImpulse, ForceMode.Impulse);
            shoot = true;
        }
    }

    public void DistanceSpecialAttack()
    {
        charging = true;
        if (charging == true)
        {
            timerAttack += Time.deltaTime;

            if (timerAttack >= 1.5f)
            {
                if (comboStreak >= 5)
                {
                    comboBullet = true;
                }
                for (int i = 0; i < 3; i++)
                {
                    GameObject bulletGameObject = Instantiate(bulletPrefab, SpawnBullet.transform.position, arm.transform.rotation * Quaternion.Euler(90, 0, 0));
                    bulletGameObject.name = gameObject.transform.parent.gameObject.transform.parent.gameObject.name + "Bullet";
                    bulletGameObject.GetComponent<Rigidbody>().AddForce(arm.transform.TransformVector(Vector3.forward) * forceImpulse, ForceMode.Impulse);
                }
                charging = false;
                chargedBullet = true;
                timerAttack = 0;
            }

        }
    }
}
