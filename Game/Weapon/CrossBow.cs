using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrossBow : MonoBehaviour, IWeapon
{
    GameObject arm;
    [SerializeField] GameObject SpawnBolt;
    [SerializeField] GameObject SpawnBolt2;
    [SerializeField] GameObject SpawnBolt3;
    [SerializeField] GameObject boltPrefab;


    bool shoot = true;
    int nbBulletInMagazine;

    float fireRate = 2.5f;
    float reloadCoolDown = 1.5f;

    float timerAttack = 0;
    bool charging = false;
    bool chargedBullet = false;
    public float forceImpulse = 1f;


    float rotationUpDownArm;

    bool comboBullet = false;
    int comboStreak = 0;
    float comboTimer = 0;

    public int[] stats = new int[2];
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

    float xQ;
    float yQ;
    float zQ;

    Vector3 projectileDir;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        arm = gameObject.GetComponentInParent<WeaponBehaviour>().gameObject;
        stats[0] = 10;
        stats[1] = stats[0] * 3;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                xQ += 0.1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                xQ -= 0.1f;
            }
            Debug.Log("x : " + xQ);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                yQ += 0.1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                yQ -= 0.1f;
            }
            Debug.Log("y : " + yQ);
        }

        if (Input.GetKey(KeyCode.P))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                zQ += 0.1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                zQ -= 0.1f;
            }
            Debug.Log("z : " + zQ);
        }

        //gameObject.transform.localRotation = new Quaternion(-0.2f, 0.3f, -0.8f,1);




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
                if (timeAnim >= 0.150 && shoot == false)
                {
                    if (comboStreak >= 9)
                    {
                        comboBullet = true;
                        SoundManager.Instance.ComboActivatedPlay(gameObject);
                    }
                    SoundManager.Instance.CrossBowShootPlay(gameObject);
                    GameObject bolt1 = Instantiate(boltPrefab, SpawnBolt.transform.position, SpawnBolt.transform.rotation);
                    GameObject bolt2 = Instantiate(boltPrefab, SpawnBolt2.transform.position, SpawnBolt2.transform.rotation);
                    GameObject bolt3 = Instantiate(boltPrefab, SpawnBolt3.transform.position, SpawnBolt3.transform.rotation);

                    bolt2.transform.localRotation = bolt2.transform.localRotation * Quaternion.Euler(10, 0, 0);
                    bolt3.transform.localRotation = bolt3.transform.localRotation * Quaternion.Euler(-10, 0, 0);

                    bolt1.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";
                    bolt2.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";
                    bolt3.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";

                    if (comboStreak >= 9)
                    {
                        bolt1.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * 4 * forceImpulse, ForceMode.Impulse);
                        bolt2.GetComponent<Rigidbody>().AddForce(bolt2.transform.TransformVector(Vector3.up) * 4 * forceImpulse, ForceMode.Impulse);
                        bolt3.GetComponent<Rigidbody>().AddForce(bolt3.transform.TransformVector(Vector3.up) * 4 * forceImpulse, ForceMode.Impulse);
                    }
                    else
                    {
                        bolt1.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * forceImpulse, ForceMode.Impulse);
                        bolt2.GetComponent<Rigidbody>().AddForce(bolt2.transform.TransformVector(Vector3.up) * forceImpulse, ForceMode.Impulse);
                        bolt3.GetComponent<Rigidbody>().AddForce(bolt3.transform.TransformVector(Vector3.up) * forceImpulse, ForceMode.Impulse);
                    }
                    shoot = true;
                }

                if (timeAnim >= 0.667)
                {
                    if (animator != null)
                    {
                        animator.SetBool("CrossbowShoot", false);
                    }
                   // animator.SetBool("CrossBowIdle", false);
                    timeAnim = 0;
                    playAnim = false;
                }
            }
            else if (chargedBullet == true)
            {
                if (timeAnim >= 0.0 && shoot == false)
                {
                    if (comboStreak >= 9)
                    {
                        comboBullet = true;
                        SoundManager.Instance.ComboActivatedPlay(gameObject);
                    }
                    SoundManager.Instance.CrossBowShootPlay(gameObject);
                    GameObject bolt1 = Instantiate(boltPrefab, SpawnBolt.transform.position, SpawnBolt.transform.rotation);
                    GameObject bolt2 = Instantiate(boltPrefab, SpawnBolt2.transform.position, SpawnBolt2.transform.rotation);
                    GameObject bolt3 = Instantiate(boltPrefab, SpawnBolt3.transform.position, SpawnBolt3.transform.rotation);

                    bolt2.transform.localRotation = bolt2.transform.localRotation * Quaternion.Euler(10, 0, 0);
                    bolt3.transform.localRotation = bolt3.transform.localRotation * Quaternion.Euler(-10, 0, 0);

                    bolt1.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";
                    bolt2.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";
                    bolt3.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "bolt";


                    if (comboStreak >= 9)
                    {
                        bolt1.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * 4 * forceImpulse, ForceMode.Impulse);
                        bolt2.GetComponent<Rigidbody>().AddForce(bolt2.transform.TransformVector(Vector3.up) * 4 * forceImpulse, ForceMode.Impulse);
                        bolt3.GetComponent<Rigidbody>().AddForce(bolt3.transform.TransformVector(Vector3.up) * 4 * forceImpulse, ForceMode.Impulse);
                    }
                    else
                    {
                        bolt1.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * forceImpulse, ForceMode.Impulse);
                        bolt2.GetComponent<Rigidbody>().AddForce(bolt2.transform.TransformVector(Vector3.up) * forceImpulse, ForceMode.Impulse);
                        bolt3.GetComponent<Rigidbody>().AddForce(bolt3.transform.TransformVector(Vector3.up) * forceImpulse, ForceMode.Impulse);
                    }
                    shoot = true;

                }

                if (timeAnim >= 1)
                {
                    if (animator != null)
                    {
                        animator.SetBool("ChargedCrossBow", false);
                    }
                  //  animator.SetBool("CrossBowIdle", false);
                    timeAnim = 0;
                    playAnim = false;
                    charging = false;
                    chargedBullet = true;
                    timerAttack = 0;
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
                fireRate = 2.5f;
            }
        }







        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "RightBumper", false) && timeAnim < 0.0)
            {
                timerAttack = 0;
                animator.SetBool("ChargedCrossBow", false);
                charging = false;
                timeAnim = 0;
                playAnim = false;
                shoot = false;
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
               // animator.SetBool(gameObject.name + "Idle", false);
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetMouseButton(2) == false && charging == true && timeAnim < 0.0)
            {
                timerAttack = 0;
                animator.SetBool("ChargedCrossBow", false);
                charging = false;
                timeAnim = 0;
                playAnim = false;
                shoot = false;
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
              //  animator.SetBool(gameObject.name + "Idle", false);
            }
        }
        if (comboStreak > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > 5f)
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
            if (animator != null)
            {
                animator.SetBool("CrossbowShoot", true);
            }
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            playAnim = true;



        }
    }

    public void DistanceSpecialAttack()
    {
        charging = true;
        if (charging == true)
        {
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            timerAttack += Time.deltaTime;

            if (timerAttack >= 4f)
            {
                playAnim = true;
                chargedBullet = true;
                if (animator != null)
                {
                    animator.SetBool("ChargedCrossBow", true);
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
