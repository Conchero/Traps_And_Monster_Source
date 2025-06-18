using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class Sword : MonoBehaviour, IWeapon
{
    GameObject arm;
    GameObject player;
    int attack = 0;

    bool charging = false;
    bool chargedAttack = false;
    float timerChargedAttack = 0;

    public int[] stats = new int[2];

    int comboStreak;
    float timerCombo;
    bool comboEngaged;

    float timerAnim = 0;
    int nbAnim;
    Vector3 ennemiWeapon;

    TpsController controller;

    Animator animator = null;


    [SerializeField] GameObject DamageFeeback;

    public int ComboStreak { get => comboStreak; set => comboStreak = value; }
    public float TimerCombo { get => timerCombo; set => timerCombo = value; }

    public ParticleSystem particleTrail;
    public ParticleSystem particle;
    public ParticleSystem glow;

    CinemachineVirtualCamera virtualCam;
    float shakeTimer;
    Gamepad gamepad;

    bool hitSomething;

    void ShakeCamera(float _intensity, float _time)
    {
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _intensity;
        shakeTimer = _time;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(1f, 1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        arm = gameObject.GetComponentInParent<WeaponBehaviour>().gameObject;
        player = arm.GetComponentInParent<TpsController>().gameObject;


        //dps vivant
        stats[0] = 4;
        //dps wall
        stats[1] = 3;

        comboStreak = 0;
        timerCombo = 0;
        comboEngaged = false;

        virtualCam = controller.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            gamepad = (Gamepad)InputSystem.GetDevice(controller.device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null && arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
        {
            animator = arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>();
        }

        if (!virtualCam.enabled)
        {
            foreach (CinemachineVirtualCamera virtualCamera in controller.gameObject.GetComponentsInChildren<CinemachineVirtualCamera>())
            {
                if (virtualCamera.enabled)
                {
                    virtualCam = virtualCamera;
                }
            }
        }

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                if (gamepad != null)
                {
                    gamepad.SetMotorSpeeds(0f, 0f);
                }
            }
        }

        //Attack tree
        switch (attack)
        {
            case 0:
                particleTrail.Stop();
                particle.Stop();
                glow.Stop();
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    if (nbAnim > 1)
                    {
                        animator.SetBool("SwordAttack" + (nbAnim).ToString(), false);
                    }
                    else if (nbAnim == 1)
                    {
                        animator.SetBool("SwordAttack1", false);
                    }

                }
                gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
            case 1:
                if (chargedAttack == false)
                {
                    if (nbAnim == 1)
                    {
                        if (timerAnim > 0.16)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.53)
                        {
                            attack = 2;
                        }
                    }
                    if (nbAnim == 2)
                    {
                        if (timerAnim > 0)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.25)
                        {
                            attack = 2;
                        }
                    }
                    if (nbAnim == 3)
                    {
                        if (timerAnim >= 0.267)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.400)
                        {
                            attack = 2;
                        }
                    }
                    if (nbAnim == 4)
                    {
                        if (timerAnim >= 0.133)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.600)
                        {
                            attack = 2;
                        }
                    }
                }
                if (chargedAttack == true)
                {
                    // Debug.Log(timerAnim);
                    gameObject.GetComponent<BoxCollider>().enabled = true;

                    if (timerAnim >= 1)
                    {
                        attack = 2;

                    }
                }

                break;
            case 2:
                gameObject.GetComponent<BoxCollider>().enabled = false;

                if (chargedAttack == false)
                {
                    if (nbAnim == 1)
                    {
                        if (timerAnim >= 0.867)
                        {
                            attack = 0;
                            //animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                            hitSomething = false;
                            animator.SetBool("SwordAttack1", false);
                        }
                    }
                    if (nbAnim == 2)
                    {
                        if (timerAnim >= 0.4)
                        {
                            attack = 0;

                           // animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                            hitSomething = false;
                            animator.SetBool("SwordAttack2", false);
                        }
                    }
                    if (nbAnim == 3)
                    {
                        if (timerAnim >= 0.533)
                        {
                            attack = 0;

                            //animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                            hitSomething = false;
                            animator.SetBool("SwordAttack3", false);
                        }
                    }
                    if (nbAnim == 4)
                    {
                        if (timerAnim >= 0.733)
                        {
                            attack = 0;

                           // animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                            hitSomething = false;
                            animator.SetBool("SwordAttack4", false);
                        }
                    }
                }
                if (chargedAttack == true)
                {
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                    {
                        animator.SetBool("ChargedSwordAttack", false);
                    }
                    attack = 0;
                    chargedAttack = false;
                   // animator.SetBool(gameObject.name + "Idle", false);
                    timerAnim = 0;
                    arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                    hitSomething = false;
                }
                break;

            case 3:
                if (nbAnim > 1)
                {
                    animator.SetBool("SwordAttack" + (nbAnim).ToString(), false);
                }
                else if (nbAnim == 1)
                {
                    animator.SetBool("SwordAttack1", false);
                }

                Combo();
                arm.GetComponent<WeaponBehaviour>().comboEngaged = true;

                break;

            case 4:
                timerAnim += Time.deltaTime;
                //Combo End
                if (timerAnim >= 1.500)
                {
                    animator.SetBool("SwordCombo", false);
                    animator.SetBool("SwordAttack1", false);
                    animator.SetBool("SwordAttack2", false);
                    animator.SetBool("SwordAttack3", false);
                    animator.SetBool("SwordAttack4", false);
                    animator.SetBool("ChargedSwordAttack", false);
                    chargedAttack = false;

                    timerAnim = 0;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    attack = 0;
                 //   animator.SetBool(gameObject.name + "Idle", false);
                    comboEngaged = false;
                    arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
                   // Debug.Log("Done");
            arm.GetComponent<WeaponBehaviour>().comboEngaged = false;
                }
                break;
        }

        //Charging management 
        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "RightBumper", false))
            {
                timerChargedAttack = 0;
                charging = false;
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    animator.SetBool("ChargedSwordAttack", false);
                }
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
              //  animator.SetBool(gameObject.name + "Idle", false);
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (charging == true && Input.GetMouseButton(2) == false)
            {
                timerChargedAttack = 0;
                charging = false;
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    animator.SetBool("ChargedSwordAttack", false);
                }
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
               // animator.SetBool(gameObject.name + "Idle", false);

            }
        }

        if (attack == 0 && charging == false)
        {
            arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //Anim management
        if (attack >= 1 && attack < 3)
        {
            timerAnim += Time.deltaTime;
        }

        //Combo Management 
        if (comboStreak > 0)
        {
            timerCombo += Time.deltaTime;
            if (timerCombo > 3.5f)
            {
                comboStreak = 0;
            }
        }


        if (comboStreak >= 4)
        {
            if (animator.GetBool("ChargedSwordAttack") == true)
            {
                animator.SetBool("ChargedSwordAttack", false);
                comboStreak = 4;
                attack = 3;
                timerCombo = 0;
                timerAnim = 0;
            }

            if (nbAnim > 1)
            {
                if (animator.GetBool("SwordAttack" + (nbAnim).ToString()) == false)
                {
                    comboStreak = 4;
                    attack = 3;
                    timerCombo = 0;
                }
            }
            else if (nbAnim == 1)
            {
                if (animator.GetBool("SwordAttack1") == false)
                {
                    comboStreak = 4;
                    attack = 3;
                    timerCombo = 0;
                }
            }

        }


    }

    public void Attack()
    {

        //Debug.Log("Sword");
        if (attack == 0)
        {
            particleTrail.Play();
            particle.Play();
            glow.Play();

            SoundManager.Instance.WeaponAttackPlay(gameObject);
            chargedAttack = false;
            attack = 1;
            if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
            {
                animator.SetBool("SwordAttack" + (comboStreak + 1).ToString(), true);
                animator.SetBool("Jump", false);
                nbAnim = comboStreak + 1;
            }
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
        }
        // Debug.Log(gameObject.transform.parent.gameObject.transform.parent.gameObject.name);

    }

    public void SpecialCaCAttack()
    {
        if (timerChargedAttack < 0.533 && chargedAttack == false)
        {
            charging = true;
            timerChargedAttack += Time.deltaTime;
            if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
            {
                animator.SetBool("ChargedSwordAttack", true);
                animator.SetBool("Jump", false);
            }
            //arm.transform.Rotate(Vector3.left * Time.deltaTime * 10);
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
        }
        else if (timerChargedAttack >= 0.533)
        {
            particleTrail.Play();
            particle.Play();
            glow.Play();

            timerAnim = timerChargedAttack;
            chargedAttack = true;
            charging = false;
            timerChargedAttack = 0;
            if (attack == 0)
            {
                SoundManager.Instance.WeaponAttackPlay(gameObject);
                attack = 1;
            }
        }

    }

    public void DistanceAttack()
    {

    }

    public void DistanceSpecialAttack()
    {

    }

    void Combo()
    {
        timerAnim += Time.deltaTime;
        if (animator.GetBool("SwordCombo") == false)
        {
            //animator.Play("SwordCombo");
            animator.SetBool("SwordCombo", true);
            animator.SetBool("Jump", false);
            SoundManager.Instance.ComboActivatedPlay(gameObject);
        }
        if (comboEngaged == false)
        {
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            if (timerAnim >= 1.167)
            {
                comboEngaged = true;
            }
            // Debug.Log("Charging");

        }
        if (comboEngaged == true && attack == 3)
        {
            if (gameObject.GetComponent<SphereCollider>().enabled == false)
            {
                gameObject.GetComponent<SphereCollider>().enabled = true;
                attack = 4;
                comboStreak = 0;
                //  Debug.Log("Combo");

            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        NormalAttackCollider(collider);
        ComboAttackCollider(collider);

    }

    void OnTriggerExit(Collider collider)
    {
        if (hitSomething == true)
        {
        }
    }

    void InstantiateDamageFeeback(Vector3 _positionCollider, Quaternion _rotationCollider, int _damageInflicted, int _armor)
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

        ShakeCamera(3, 0.3f);
    }

    void NormalAttackCollider(Collider collider)
    {
        if (attack == 1 && hitSomething == false)
        {
            if (collider.CompareTag("Ennemi"))
            {
                if (chargedAttack == false)
                {
                 //   Debug.Log(collider.gameObject.tag);
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                    //Debug.Log("Ouch sword");
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(0.5f);

                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Ennemi>().m_armor);
                }
                else if (chargedAttack == true)
                {
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0] * 2, player.GetComponentInChildren<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 10, ForceMode.Impulse);
                    //Debug.Log("Ouch sword charged !");
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Ennemi>().m_armor);
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(1);

                }



                SoundManager.Instance.WeaponEnnemiHitPlay(gameObject);
                arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                comboStreak++;
                timerCombo = 0;
                hitSomething = true;
            }

            if (collider.CompareTag("Trap"))
            {
                if (collider.gameObject.GetComponentInParent<Traps>().gameObject.GetComponentInChildren<TrapAttack>().player.name != gameObject.GetComponentInParent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().gameObject.name)
                {
                    if (chargedAttack == false)
                    {
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1], player.GetComponentInChildren<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1], collider.gameObject.GetComponentInParent<Traps>().m_armor);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1] * 2, player.GetComponentInChildren<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword charged !");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1] * 2, collider.gameObject.GetComponentInParent<Traps>().m_armor);
                    }
                    SoundManager.Instance.WeaponWallHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    hitSomething = true;
                }
      
            }

            if (collider.CompareTag("Nexus")) 
            {
                if (GameplaySettings.Instance.m_customSettings.PlayersCanHitNexusCAC)
                {
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_linkedCamp != collider.GetComponent<Nexus>().m_linkedCamp)
                    {


                        if (chargedAttack == false)
                        {
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>());

                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Nexus>().m_armor);


                        }
                        else if (chargedAttack == true)
                        {
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0] * 2, player.GetComponentInChildren<EntityPlayer>());

                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Nexus>().m_armor);



                        }

                        SoundManager.Instance.NexusHitPlay(gameObject);
                        arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                        hitSomething = true;
                    }
                }
            }

            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject.name != player.GetComponentInChildren<EntityPlayer>().gameObject.name)
                {
                    if (chargedAttack == false)
                    {
                        // Debug.Log("PlayerHit");
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>().gameObject, null);
                        // collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                        //Debug.Log("Ouch sword");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(0.5f);
                    }
                    else if (chargedAttack == true)
                    {
                        // Debug.Log("PlayerHit");
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0] * 2, player.GetComponentInChildren<EntityPlayer>().gameObject, null);
                        //collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 10, ForceMode.Impulse);
                        //Debug.Log("Ouch sword charged !");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(1f);
                    }
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    comboStreak++;
                    timerCombo = 0;
                    hitSomething = true;
                }
            }

            if (collider.CompareTag("Invocation"))
            {
                if (collider.gameObject.GetComponent<comportementGeneralIA>().fromPlayercolor != GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
                {
                    if (chargedAttack == false)
                    {
                        Debug.Log(collider.gameObject.tag);
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                        //Debug.Log("Ouch sword");
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(0.5f);

                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0] * 2, player.GetComponentInChildren<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 10, ForceMode.Impulse);
                        //Debug.Log("Ouch sword charged !");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(1f);

                    }
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    comboStreak++;
                    timerCombo = 0;
                    hitSomething = true;
                }
            }

        }
    }


    void ComboAttackCollider(Collider collider)
    {
        if (attack == 4)
        {
            if (collider.CompareTag("Ennemi"))
            {
                ennemiWeapon = collider.gameObject.transform.position - gameObject.transform.position;


                collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>().gameObject);
                if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                {
                    collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                }
                collider.gameObject.GetComponent<Rigidbody>().AddForce(ennemiWeapon.normalized * 75, ForceMode.Impulse);
                InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Ennemi>().m_armor);
                collider.gameObject.GetComponent<Ennemi>().StopEnemy(1f);

            }

            if (collider.CompareTag("Wall"))
            {
                //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[1]);
            }


            if (collider.CompareTag("Trap"))
            {
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_linkedCamp != collider.GetComponent<Nexus>().m_linkedCamp)
                {
                    collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1], player.GetComponentInChildren<EntityPlayer>().gameObject);
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1], collider.gameObject.GetComponent<Traps>().m_armor);
                    //Debug.Log("Ouch sword");
                }
            }

            if (collider.CompareTag("Player"))
            {
                //verifier qu'on ne se tape pas nous même
                if (collider.gameObject.name != player.GetComponentInChildren<EntityPlayer>().gameObject.name)
                {
                    //Debug.Log("PlayerHit");
                    EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                    tempPlayer.RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>().gameObject, null);
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<EntityPlayer>().m_armor);
                }
            }

            if (collider.CompareTag("Invocation"))
            {
                ennemiWeapon = collider.gameObject.transform.position - gameObject.transform.position;

                collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(1f);


                collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0], player.GetComponentInChildren<EntityPlayer>().gameObject);
                if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                {
                    collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                }
                collider.gameObject.GetComponent<Rigidbody>().AddForce(ennemiWeapon.normalized * 75, ForceMode.Impulse);
                InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<comportementGeneralIA>().Armor);

            }

        }
    }
}
