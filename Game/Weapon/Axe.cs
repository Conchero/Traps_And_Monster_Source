using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class Axe : MonoBehaviour, IWeapon
{
    GameObject player;
    GameObject arm;
    int attack = 0;
    bool chargedAttack = false;
    bool charging = false;
    float timerChargedAttack = 0;

    public int[] stats = new int[2];



    int comboStreak;
    float timerCombo;
    float timerComboEngaged;
    bool comboEngaged;

    TpsController controller;

    float timerAnim = 0;
    Animator animator = null;

    [SerializeField] GameObject DamageFeeback;


    public bool ComboEngaged { get => comboEngaged; set => comboEngaged = value; }
    public int ComboStreak { get => comboStreak; set => comboStreak = value; }
    public float TimerCombo { get => timerCombo; set => timerCombo = value; }

    public ParticleSystem particleTrail;
    public ParticleSystem particle;
    public ParticleSystem glow1;
    public ParticleSystem glow2;

    CinemachineVirtualCamera virtualCam;
    float shakeTimer;
    Gamepad gamepad;
	
    bool hitSomething = false;
    int nbAnim;

    float timerComboAttack;
    bool comboCanHit =false;
    void ShakeCamera(float _intensity, float _time)
    {
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _intensity;
        shakeTimer = _time;
		
        if(gamepad != null)
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

        timerComboEngaged = 5.0f;
        //Living Damage
        stats[0] = 5;
        //Wall Damage
        stats[1] = 1;
		
        virtualCam = controller.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();

        if(InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            gamepad = (Gamepad)InputSystem.GetDevice(controller.device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null && animator == null)
        {
            animator = arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>();
        }

        if(!virtualCam.enabled)
        {
            foreach(CinemachineVirtualCamera virtualCamera in controller.gameObject.GetComponentsInChildren<CinemachineVirtualCamera>())
            {
                if(virtualCamera.enabled)
                {
                    virtualCam = virtualCamera;
                }
            }
        }

        if(shakeTimer > 0)
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
		
        switch (attack)
        {
            case 0:
                particleTrail.Stop();
                particle.Stop();
                glow1.Stop();
                glow2.Stop();
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    if (nbAnim > 1)
                    {
                        animator.SetBool("AxeAttack" + (nbAnim).ToString(), false);
                    }
                    else if (nbAnim == 1)
                    {
                        animator.SetBool("AxeAttack1", false);
                    }

                }
                gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
            case 1:

                if (chargedAttack == false)
                {
                    if (nbAnim == 1)
                    {
                        if (timerAnim > 0.367)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.633)
                        {
                            attack = 2;
                        }
                    }
                    if (nbAnim == 2)
                    {
                        if (timerAnim > 0.367)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.633)
                        {
                            attack = 2;
                        }
                    }
                    if (nbAnim == 3)
                    {
                        if (timerAnim > 0.367)
                        {
                            gameObject.GetComponent<BoxCollider>().enabled = true;

                        }
                        if (timerAnim > 0.633)
                        {
                            attack = 2;
                        }
                    }
                }
                if (chargedAttack == true)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = true;
                    //Debug.Log(timerAnim);

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
                        if (timerAnim >= 0.833)
                        {
                            attack = 0;
                           // animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            hitSomething = false;
                            animator.SetBool("AxeAttack1", false);

                        }
                    }

                    if (nbAnim == 2)
                    {
                        if (timerAnim >= 0.833)
                        {
                            attack = 0;
                            //animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            hitSomething = false;
                            animator.SetBool("AxeAttack2", false);

                        }
                    }

                    if (nbAnim == 3)
                    {
                        if (timerAnim >= 0.833)
                        {
                            attack = 0;
                          //  animator.SetBool(gameObject.name + "Idle", false);
                            timerAnim = 0;
                            hitSomething = false;
                            animator.SetBool("AxeAttack3", false);

                        }
                    }

                }
                if (chargedAttack == true)
                {
                    if (timerAnim >= 1.333f)
                    {
                        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                        {
                            animator.SetBool("ChargedAxeAttack", false);
                           // animator.SetBool(gameObject.name + "Idle", false);
                            animator.applyRootMotion = false;
                        }
                        attack = 0;
                        chargedAttack = false;
                        timerAnim = 0;
                        hitSomething = false;
                    }
                }
                break;

            case 3:
                Combo();
                break;
        }


        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "RightBumper", false))
            {
                timerChargedAttack = 0;
                charging = false;
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    animator.SetBool("ChargedAxeAttack", false);
                    animator.applyRootMotion = false;
                }
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
               // animator.SetBool(gameObject.name + "Idle", false);
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
                    animator.SetBool("ChargedAxeAttack", false);
                    animator.applyRootMotion = false;
                }
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
               // animator.SetBool(gameObject.name + "Idle", false);
            }
        }

        if (attack == 0 && charging == false)
        {
            arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
        }

        if (comboStreak > 0)
        {
            timerCombo += Time.deltaTime;
            if (timerCombo > 3.5f)
            {
                comboStreak = 0;
            
            }
        }
        if (comboStreak >= 3)
        {
            comboStreak = 3;

            if (animator.GetBool("ChargedAxeAttack") == true)
            {
                animator.SetBool("ChargedAxeAttack", false);
                comboStreak = 3;
                attack = 3;
                timerCombo = 0;
                chargedAttack = false;
                timerChargedAttack = 0;
                charging = false;
                timerAnim = 0;      
            }

            if (nbAnim > 1)
            {
                if (animator.GetBool("AxeAttack" + (nbAnim).ToString()) == false)
                {
                    comboStreak = 3;
                    attack = 3;
                    timerCombo = 0;
                }
            }
            else if (nbAnim == 1)
            {
                if (animator.GetBool("AxeAttack1") == false)
                {
                    comboStreak = 3;
                    attack = 3;
                    timerCombo = 0;
                }
            }
        }

        //Anim management
        if (attack >= 1 && attack < 3)
        {
            timerAnim += Time.deltaTime;
        }

        if (comboEngaged == true)
        {
            comboCanHit = false;
            timerComboAttack += Time.deltaTime;
            if (timerComboAttack >= 0.5f)  
            {
                comboCanHit = true;  
                timerComboAttack = 0;
            }
        }


    }

    public void Attack()
    {
        if (attack == 0)
        {
            particleTrail.Play();
            particle.Play();
            glow1.Play();
            glow2.Play();

            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            SoundManager.Instance.WeaponAttackPlay(gameObject);
            chargedAttack = false;
            attack = 1;
            if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
            {
                animator.SetBool("AxeAttack" + (comboStreak + 1).ToString(), true);
                nbAnim = comboStreak + 1;
            }
        }
    }

    public void SpecialCaCAttack()
    {
        if (timerChargedAttack < 0.833 && chargedAttack == false)
        {
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            charging = true;
            timerChargedAttack += Time.deltaTime;
            if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
            {
                animator.SetBool("ChargedAxeAttack", true);
                animator.applyRootMotion = true;
                animator.applyRootMotion = false;
            }
            //arm.transform.Rotate(Vector3.left * Time.deltaTime * 10);
        }
        else if (timerChargedAttack >= 0.833)
        {
            particleTrail.Play();
            particle.Play();
            glow1.Play();
            glow2.Play();

            timerAnim = timerChargedAttack;
            charging = true;
            chargedAttack = true;
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
        if (comboEngaged == false)
        {
            arm.transform.localRotation = new Quaternion(0, 0, 0, 0);
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, -90));
            comboEngaged = true;
            arm.GetComponent<WeaponBehaviour>().comboEngaged = true;
        }
        if (comboEngaged == true)
        {
            if (animator.GetBool("AxeCombo") == false)
            {
                //animator.Play("AxeCombo");
                animator.SetBool("AxeCombo", true);
                gameObject.GetComponent<BoxCollider>().enabled = true;
            SoundManager.Instance.ComboActivatedPlay(gameObject);
            }
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
            comboStreak = 0;
            arm.GetComponent<WeaponBehaviour>().player.gameObject.transform.Rotate(Vector3.down * Time.deltaTime * 1000);
            //arm.gameObject.transform.Rotate(Vector3.down * Time.deltaTime * 1000);
            timerComboEngaged -= Time.deltaTime;
            hitSomething = false;
            if (timerComboEngaged <= 0)
            {
                timerComboEngaged = 5.0f;
                comboStreak = 0;
                attack = 0;
                arm.GetComponent<WeaponBehaviour>().player.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //arm.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                comboEngaged = false;
              //  animator.SetBool(gameObject.name + "Idle", false);
                gameObject.GetComponent<BoxCollider>().enabled = false;
                animator.SetBool("AxeAttack1", false);
                animator.SetBool("AxeAttack2", false);
                animator.SetBool("AxeAttack3", false);
                animator.SetBool("ChargedAxeAttack", false);
                animator.SetBool("AxeCombo", false);
            arm.GetComponent<WeaponBehaviour>().comboEngaged = false;
                timerComboAttack = 0;
            } 
        }
    }
      
    void OnTriggerEnter(Collider collider)
    {
        NormalAttackCollider(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        ComboAttackCollider(collider);
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
        if ((attack == 1 || attack == 3) && hitSomething == false)
        {
            // Debug.Log(collider.gameObject.tag);
            if (collider.CompareTag("Ennemi"))
            {

                if (chargedAttack == false)
                {
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        Debug.Log("hit du taunt");
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                    // Debug.Log("Ouch axe");
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(0.5f);

                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Ennemi>().m_armor);
                }
                else if (chargedAttack == true)
                {
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        Debug.Log("hit du taunt");
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                    //Debug.Log("Ouch axe charged");
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(1f);

                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Ennemi>().m_armor);
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
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1], collider.gameObject.GetComponent<Traps>().m_armor);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword charged !");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1] * 2, collider.gameObject.GetComponent<Traps>().m_armor);
                    }
                    SoundManager.Instance.WeaponWallHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    hitSomething = true;
                }
            }

            if (collider.CompareTag("Wall"))
            {
                if (chargedAttack == false)
                {
                    //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[1]);
                    //Debug.Log("Ouch axe");
                }
                else if (chargedAttack == true)
                {
                    //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[1] * 2);
                    //Debug.Log("Ouch axe charged !");
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
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>());
                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Nexus>().m_armor);
                        }
                        else if (chargedAttack == true)
                        {
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>());
                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Nexus>().m_armor);
                        }
                        SoundManager.Instance.NexusHitPlay(gameObject);
                        arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    }
                }
            
            }
            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject.name != arm.GetComponentInParent<EntityPlayer>().gameObject.name)
                {

                    if (chargedAttack == false)
                    {
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(0.5f);
                    }
                    else if (chargedAttack == true)
                    {
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(1f);
                    }
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    comboStreak++;
                    timerCombo = 0;
                    hitSomething = true;
                }
                // Debug.Log("PlayerHit");
            }

            if (collider.CompareTag("Invocation"))
            {
                if (collider.gameObject.GetComponent<comportementGeneralIA>().fromPlayercolor != GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
                {
                    if (chargedAttack == false)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                        // Debug.Log("Ouch axe");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(0.5f);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                        //Debug.Log("Ouch axe charged");
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(1f);

                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
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
        if (attack == 3 && comboCanHit == true)
        {
            // Debug.Log(collider.gameObject.tag);
            if (collider.CompareTag("Ennemi"))
            {

                if (chargedAttack == false)
                {
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        Debug.Log("hit du taunt");
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                    // Debug.Log("Ouch axe");
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(0.5f);

                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Ennemi>().m_armor);
                }
                else if (chargedAttack == true)
                {
                    collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                    {
                        Debug.Log("hit du taunt");
                        collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                    //Debug.Log("Ouch axe charged");
                    collider.gameObject.GetComponent<Ennemi>().StopEnemy(1f);

                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Ennemi>().m_armor);
                }
                SoundManager.Instance.WeaponEnnemiHitPlay(gameObject);
                arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                comboStreak++;
                timerCombo = 0;
            }

            if (collider.CompareTag("Trap"))
            {
                if (collider.gameObject.GetComponentInParent<Traps>().gameObject.GetComponentInChildren<TrapAttack>().player.name != gameObject.GetComponentInParent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().gameObject.name)
                {
                    if (chargedAttack == false)
                    {
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1], collider.gameObject.GetComponent<Traps>().m_armor);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[1] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        //Debug.Log("Ouch sword charged !");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[1] * 2, collider.gameObject.GetComponent<Traps>().m_armor);
                    }
                    SoundManager.Instance.WeaponWallHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                }
            }

            if (collider.CompareTag("Wall"))
            {
                if (chargedAttack == false)
                {
                    //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[1]);
                    //Debug.Log("Ouch axe");
                }
                else if (chargedAttack == true)
                {
                    //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[1] * 2);
                    //Debug.Log("Ouch axe charged !");
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
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>());
                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Nexus>().m_armor);
                        }
                        else if (chargedAttack == true)
                        {
                            collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>());
                            InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<Nexus>().m_armor);
                        }
                        SoundManager.Instance.NexusHitPlay(gameObject);
                        arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    }
                }

            }
            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject.name != arm.GetComponentInParent<EntityPlayer>().gameObject.name)
                {

                    if (chargedAttack == false)
                    {
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(0.5f);
                    }
                    else if (chargedAttack == true)
                    {
                        EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                        tempPlayer.RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, tempPlayer.m_armor);
                        collider.gameObject.GetComponent<TpsController>().StopPlayer(1f);
                    }
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    comboStreak++;
                    timerCombo = 0;
                }
                // Debug.Log("PlayerHit");
            }

            if (collider.CompareTag("Invocation"))
            {
                if (collider.gameObject.GetComponent<comportementGeneralIA>().fromPlayercolor != GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
                {
                    if (chargedAttack == false)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                        // Debug.Log("Ouch axe");
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(0.5f);
                    }
                    else if (chargedAttack == true)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0] * 2, gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                        if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                        {
                            collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                        }
                        collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 7, ForceMode.Impulse);
                        //Debug.Log("Ouch axe charged");
                        collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(1f);

                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0] * 2, collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                    }
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    comboStreak++;
                    timerCombo = 0;
                }
            }

        }
    }

}
