using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour, IWeapon
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
    Animator animator = null;
    bool playAnim;
    float timeAnim;
    [SerializeField] GameObject DamageFeeback;
    // Start is called before the first frame update

    Vector3 projectileDir;

    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        arm = gameObject.GetComponentInParent<WeaponBehaviour>().gameObject;
        stats[0] = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null && animator == null)
        {
            animator = arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>();
        }

        if (playAnim == true)
        {
            projectileDir = gameObject.transform.parent.GetComponent<WeaponBehaviour>().player.transform.parent.Find("UI").gameObject.GetComponentInChildren<UIAim>().m_target - gameObject.transform.position;
            timeAnim += Time.deltaTime;
            if (chargedBullet == false)
            {
                if (timeAnim >= 0.65 && shoot == false)
                {
                    if (comboStreak >= 3)
                    {
                        comboBullet = true;
                        SoundManager.Instance.ComboActivatedPlay(gameObject);
                    }
                    SoundManager.Instance.BowShootPlay(gameObject);
                    GameObject bulletGameObject = Instantiate(bulletPrefab, SpawnBullet.transform.position, arm.transform.rotation * Quaternion.Euler(90, 0, 0));
                    bulletGameObject.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "Arrow";
                    bulletGameObject.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * forceImpulse, ForceMode.Impulse);
                    shoot = true;
                }

                if (timeAnim >= 0.85)
                {
                    if (animator != null)
                    {
                        animator.SetBool("BowShoot", false);
                    }
                   // animator.SetBool(gameObject.name + "Idle", false);


                    timeAnim = 0;
                    playAnim = false;
                }
            }

            if (chargedBullet == true)
            {
                if (timeAnim >= 0.65 && shoot == false)
                {
                    if (comboStreak >= 3)
                    {
                        comboBullet = true;
                        SoundManager.Instance.ComboActivatedPlay(gameObject);
                    }
                    SoundManager.Instance.BowShootPlay(gameObject);
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject bulletGameObject = Instantiate(bulletPrefab, SpawnBullet.transform.position, arm.transform.rotation * Quaternion.Euler(90, 0, 0));
                        bulletGameObject.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "Arrow";
                        bulletGameObject.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * forceImpulse, ForceMode.Impulse);
                    }
                    shoot = true;
                }
                if (timeAnim >= 1)
                {
                    animator.SetBool("ChargedBow", false);
                   // animator.SetBool(gameObject.name + "Idle", false);

                    charging = false;
                    timerAttack = 0;
                    timeAnim = 0;
                    playAnim = false;
                }
            }

        }



        if (shoot == true && playAnim == false)
        {

            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                shoot = false;
                fireRate = 1.0f;
            }
        }




        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "RightBumper", false) && timeAnim < 0.65)
            {
                timerAttack = 0;
                animator.SetBool("ChargedBow", false);
                charging = false;
                timeAnim = 0;
                shoot = false;
                playAnim = false;
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                  //  animator.SetBool(gameObject.name + "Idle", false);
            }
        }
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetMouseButton(2) == false && charging == true && timeAnim < 0.65)
            {
                timerAttack = 0;
                animator.SetBool("ChargedBow", false);
                charging = false;
                timeAnim = 0;
                shoot = false;
                playAnim = false;
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                  //  animator.SetBool(gameObject.name + "Idle", false);
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
        if (shoot == false && playAnim == false)
        {
            chargedBullet = false;
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            playAnim = true;
            if (animator != null)
            {
                animator.SetBool("BowShoot", true);
            }
        }
    }

    public void DistanceSpecialAttack()
    {
        charging = true;
        if (charging == true && shoot == false)
        {
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            timerAttack += Time.deltaTime;
            if (timerAttack >= 0.5)
            {
                playAnim = true;
                chargedBullet = true;
                if (animator != null)
                {
                    animator.SetBool("ChargedBow", true);
                }

            }
        }
    }

    public void InstantiateDamageFeeback(Vector3 _positionCollider, Quaternion _rotationCollider, int _damageInflicted, int _armor)
    {
        GameObject damageFeedback = Instantiate(DamageFeeback, _positionCollider + new Vector3(0, 2, 0), _rotationCollider);
        //Calcul des degats infligés
        int damageDone = _damageInflicted - _armor;
        if (damageDone < 0)
        {
            damageDone = 0;
        }


        damageFeedback.GetComponentInChildren<TextMeshProUGUI>().text = damageDone.ToString();
        damageFeedback.layer = arm.GetComponent<WeaponBehaviour>().player.layer;
        Vector3 lookAtDirection = arm.GetComponent<WeaponBehaviour>().player.gameObject.transform.position - damageFeedback.transform.position;
        lookAtDirection.y = 0;
        Quaternion rotationFeedback = Quaternion.LookRotation(lookAtDirection);
        damageFeedback.transform.rotation = rotationFeedback;
    }
}
