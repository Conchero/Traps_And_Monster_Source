using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class LaserSword : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject spawnLaser;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject hugeLaserPrefab;
    GameObject arm;
    GameObject player;
    int attack = 0;

    bool charging = false;
    bool launchingLaser = false;
    float timerSpecial = 0;
    float rotationUpDownArm;

    public float forceImpulse = 1f;

    public int[] stats = new int[4];

    bool comboLaser = false;
    int comboStreak = 0;
    float comboTimer = 0;
    float timerHugeLaser = 5;

    public int[] Stats { get => stats; set => stats = value; }
    public bool ComboLaser { get => comboLaser; set => comboLaser = value; }
    public int ComboStreak { get => comboStreak; set => comboStreak = value; }
    public float ComboTimer { get => comboTimer; set => comboTimer = value; }
    public float TimerHugeLaser { get => timerHugeLaser; set => timerHugeLaser = value; }
    public bool LaunchingLaser { get => launchingLaser; set => launchingLaser = value; }

    TpsController controller;



    Animator animator = null;
    float timerAnim = 0;

    [SerializeField] GameObject DamageFeeback;

    public ParticleSystem particleTrail;
    public ParticleSystem particle;
    public ParticleSystem glow;

    Vector3 projectileDir;

    bool hitSomething;

    CinemachineVirtualCamera virtualCam;
    float shakeTimer;
    Gamepad gamepad;

    [SerializeField] Material RedLaser;
    [SerializeField] Material BlueLaser;
    [SerializeField] Material YellowLaser;
    [SerializeField] Material GreenLaser;

    [SerializeField] Material RedSword;
    [SerializeField] Material BlueSword;
    [SerializeField] Material YellowSword;
    [SerializeField] Material GreenSword;

    bool HugeLaserInitialised;
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

        stats[0] = 4;
        stats[1] = 6;
        stats[2] = 2;
        stats[3] = 3;

        virtualCam = controller.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            gamepad = (Gamepad)InputSystem.GetDevice(controller.device);
        }

        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Red")
        {
            GetComponentInChildren<MeshRenderer>().material = RedSword;
        }
        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Blue")
        {
            GetComponentInChildren<MeshRenderer>().material = BlueSword;
        }
        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Green")
        {
            GetComponentInChildren<MeshRenderer>().material = GreenSword;
        }
        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Yellow")
        {
            GetComponentInChildren<MeshRenderer>().material = YellowSword;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null && animator == null)
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

        switch (attack)
        {
            case 0:
                particleTrail.Stop();
                particle.Stop();
                glow.Stop();
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
                {
                    animator.SetBool("LaserSwordAttack", false);
                }
                gameObject.GetComponent<BoxCollider>().enabled = false;
                break;
            case 1:
                if (timerAnim > 0.16)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = true;

                }
                if (timerAnim > 0.53)
                {
                    attack = 2;
                }
                break;
            case 2:
                gameObject.GetComponent<BoxCollider>().enabled = false;

                if (timerAnim >= 0.867)
                {
                    attack = 0;
                    // animator.SetBool(gameObject.name + "Idle", false);
                    timerAnim = 0;
                    hitSomething = false;
                }

                break;
            case 3:
                charging = false;
                // gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                if (gameObject.transform.Find(arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "HugeLaser") == null)
                {
                    SoundManager.Instance.LaserIgnitionPlay(gameObject);
                    GameObject hugeLaser = Instantiate(hugeLaserPrefab, spawnLaser.transform.position + transform.TransformVector(new Vector3(0, 1.5f, 0)), gameObject.transform.rotation, gameObject.transform);

                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Red")
                    {
                        hugeLaser.GetComponentInChildren<MeshRenderer>().material = RedLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Blue")
                    {
                        hugeLaser.GetComponentInChildren<MeshRenderer>().material = BlueLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Green")
                    {
                        hugeLaser.GetComponentInChildren<MeshRenderer>().material = GreenLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Yellow")
                    {
                        hugeLaser.GetComponentInChildren<MeshRenderer>().material = YellowLaser;
                    }

                    hugeLaser.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "HugeLaser";
                    SoundManager.Instance.ComboActivatedPlay(gameObject);
                    HugeLaserInitialised = true;
                    animator.SetBool("ShootLaser", false);
                    GetComponentInChildren<Animator>().SetBool("Activ", false);
                    attack = 0;
                }
       
                break;
        }






        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (charging == true && InputManager.Instance.isPressed(controller.device, "RightBumper", false))
            {
                charging = false;
                launchingLaser = false;
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                timerSpecial = 0;
                timerAnim = 0;
                // animator.SetBool(gameObject.name + "Idle", false);
                animator.SetBool("ShootLaser", false);
                GetComponentInChildren<Animator>().SetBool("Activ", false);
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (charging == true && Input.GetMouseButton(2) == false)
            {
                charging = false;
                launchingLaser = false;
                gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                timerSpecial = 0;
                timerAnim = 0;
                animator.SetBool("ShootLaser", false);
                GetComponentInChildren<Animator>().SetBool("Activ", false);
                //animator.SetBool(gameObject.name + "Idle", false);
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
            comboTimer += Time.deltaTime;
            if (comboTimer > 2.5f)
            {
                comboTimer = 0;
                comboStreak = 0;
            }
        }

        if (comboStreak >= 3)
        {
            arm.GetComponent<WeaponBehaviour>().comboEngaged = true;
            comboStreak = 3;
            comboTimer = 0;
            if (HugeLaserInitialised == false)
            {
                attack = 3;
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = false;
            timerAnim = 0;
            timerSpecial = 0;
            }

            if (HugeLaserInitialised == true)
            {
                timerHugeLaser -= Time.deltaTime;
                if (timerHugeLaser <= 0)
                {
                    SoundManager.Instance.LaserHumStop(gameObject);
                    SoundManager.Instance.LaserDesactivationPlay(gameObject);
                    timerHugeLaser = 5;
                    Destroy(gameObject.transform.Find(arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "HugeLaser").gameObject);
                    comboStreak = 0;
                    comboTimer = 0;
                    launchingLaser = false;
                    HugeLaserInitialised = false;
                }
            }

            charging = false;
            gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (comboStreak < 3)
        {
            arm.GetComponent<WeaponBehaviour>().comboEngaged = false;

        }

        //Anim management
        if (attack >= 1 && attack < 3)
        {
            timerAnim += Time.deltaTime;
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
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            SoundManager.Instance.WeaponAttackPlay(gameObject);
            attack = 1;
            if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInChildren<Animator>() != null)
            {
                animator.SetBool("LaserSwordAttack", true);
            }
        }
    }

    public void SpecialCaCAttack()
    {

    }

    public void DistanceAttack()
    {

    }

    public void DistanceSpecialAttack()
    {
        if (comboStreak < 3)
        {
            charging = true;
        }
        else
        {
            charging = false;
        }
        if (charging == true)
        {
            projectileDir = gameObject.transform.parent.GetComponent<WeaponBehaviour>().player.transform.parent.Find("UI").gameObject.GetComponentInChildren<UIAim>().m_target - gameObject.transform.position;
            arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<TpsController>().IsAttacking = true;
            if (timerAnim < 0.250)
            {
                timerAnim += Time.deltaTime;
                animator.SetBool("ShootLaser", true);
                

            }
            else if (timerAnim >= 0.250)
            {
                GetComponentInChildren<Animator>().SetBool("Activ", true);
               
                launchingLaser = true;
                timerSpecial += Time.deltaTime;
                if (timerSpecial >= 1.75f)
                {
                    animator.SetTrigger("LaunchLaser");
                    SoundManager.Instance.LaserShootPlay(gameObject);
                    GameObject laser = Instantiate(laserPrefab, spawnLaser.transform.position, Quaternion.identity);

                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Red")
                    {
                        laser.GetComponentInChildren<MeshRenderer>().material = RedLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Blue")
                    {
                        laser.GetComponentInChildren<MeshRenderer>().material = BlueLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Green")
                    {
                        laser.GetComponentInChildren<MeshRenderer>().material = GreenLaser;
                    }
                    if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_sColor == "Yellow")
                    {
                        laser.GetComponentInChildren<MeshRenderer>().material = YellowLaser;
                    }

                    laser.name = arm.GetComponent<WeaponBehaviour>().player.transform.parent.name + "Laser";
                    laser.GetComponent<Rigidbody>().AddForce((projectileDir).normalized * forceImpulse, ForceMode.Impulse);
                    timerSpecial = 0;
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

        ShakeCamera(3, 0.3f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (attack == 1 && hitSomething == false)
        {
            if (collider.CompareTag("Ennemi"))
            {

                collider.gameObject.GetComponent<Ennemi>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>().gameObject);
                if (!collider.gameObject.GetComponent<Ennemi>().m_isTaunted)
                {
                    collider.gameObject.GetComponent<Ennemi>().TauntEnnemi(player);
                }
                collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                SoundManager.Instance.WeaponEnnemiHitPlay(gameObject);
                arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                //Debug.Log("Ouch sword");
                collider.gameObject.GetComponent<Ennemi>().StopEnemy(0.5f);

                InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Ennemi>().m_armor);
                hitSomething = true;
            }
            if (collider.CompareTag("Wall"))
            {

                //collider.gameObject.GetComponentInParent<DestructibleWalls>().RemoveLife(stats[2]);
                //Debug.Log("Ouch sword");
            }

            if (collider.CompareTag("Trap"))
            {
                if (collider.gameObject.GetComponentInParent<Traps>().gameObject.GetComponentInChildren<TrapAttack>().player.name != gameObject.GetComponentInParent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().gameObject.name)
                {
                    collider.gameObject.GetComponentInParent<Traps>().RemoveLife(stats[2], gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    SoundManager.Instance.WeaponWallHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    //Debug.Log("Ouch sword");
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[2], collider.gameObject.GetComponentInParent<Traps>().m_armor);
                    hitSomething = true;
                }
            }

            if (collider.CompareTag("Nexus"))
            {
                if (arm.GetComponent<WeaponBehaviour>().player.GetComponentInParent<EntityPlayer>().m_linkedCamp != collider.GetComponent<Nexus>().m_linkedCamp)
                {
                    if (GameplaySettings.Instance.m_customSettings.PlayersCanHitNexusCAC)
                    {
                        collider.gameObject.GetComponent<Nexus>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>());
                        InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<Nexus>().m_armor);
                    }
                    SoundManager.Instance.NexusHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                }
            }

            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject.name != player.GetComponentInChildren<EntityPlayer>().gameObject.name)
                {
                    // Debug.Log("PlayerHit");
                    EntityPlayer tempPlayer = collider.GetComponentInParent<EntityPlayer>();
                    tempPlayer.RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>().gameObject, null);
                    SoundManager.Instance.WeaponLordHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], tempPlayer.m_armor);
                    collider.gameObject.GetComponent<TpsController>().StopPlayer(0.5f);
                    hitSomething = true;
                }
            }

            if (collider.CompareTag("Invocation"))
            {
                if (collider.gameObject.GetComponent<comportementGeneralIA>().fromPlayercolor != GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
                {
                    collider.gameObject.GetComponent<comportementGeneralIA>().RemoveLife(stats[0], arm.GetComponentInParent<EntityPlayer>().gameObject);
                    if (!collider.gameObject.GetComponent<comportementGeneralIA>().m_isTaunted)
                    {
                        collider.gameObject.GetComponent<comportementGeneralIA>().TauntEnnemi(player);
                    }
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                    SoundManager.Instance.WeaponEnnemiHitPlay(gameObject);
                    arm.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                    collider.gameObject.GetComponent<comportementGeneralIA>().StopInvoc(0.5f);

                    //Debug.Log("Ouch sword");
                    InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, stats[0], collider.gameObject.GetComponent<comportementGeneralIA>().Armor);
                    hitSomething = true;
                }
            }
        }


    }
}
