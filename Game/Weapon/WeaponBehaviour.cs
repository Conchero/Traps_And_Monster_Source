using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    IWeapon weapon;

    TpsController controller;


    [SerializeField] public GameObject player;

    [SerializeField] public GameObject[] Weapons;
    [SerializeField] Transform SpawnWeapon;
    int indexFirstWeapon;
    int indexSecondWeapon;

    string[] weaponList = new string[5];
    [SerializeField] string[] playerWeapons = new string[2];
    [HideInInspector]
    public string weaponSelected;
    int currentWeapon;
    bool changed;
    string namePlayer;
    bool haveFirstWeapon;

    public string WeaponSelected { get => weaponSelected; set => weaponSelected = value; }
    public string[] PlayerWeapons { get => playerWeapons; set => playerWeapons = value; }
    public string[] WeaponList { get => weaponList; set => weaponList = value; }
    public int IndexFirstWeapon { get => indexFirstWeapon; set => indexFirstWeapon = value; }
    public int IndexSecondWeapon { get => indexSecondWeapon; set => indexSecondWeapon = value; }
    public bool HaveFirstWeapon { get => haveFirstWeapon; set => haveFirstWeapon = value; }

    //Optit
    StateGame m_stateGame;
    string m_sDeviceType;
    Animator animator = null;

    Quaternion baseRotationRightShoulder;
    Quaternion baseRotationLeftShoulder;

    bool ButtonPressed = false;

   public bool comboEngaged = false;
    // Start is called before the first frame update
    void Start()
    {
        //Opti
        m_stateGame = FindObjectOfType<StateGame>();
        //m_sDeviceType = InputManager.Instance.GetLayoutDevice(controller.device);
        indexFirstWeapon = 0;
        indexSecondWeapon = 0;
        weaponSelected = "BareHand";
        currentWeapon = 0;
        changed = false;


        controller = player.GetComponentInParent<TpsController>();

        weaponList[0] = "Sword";
        weaponList[1] = "Axe";
        weaponList[2] = "Bow";
        weaponList[3] = "LaserSword";
        weaponList[4] = "CrossBow";

        playerWeapons[0] = weaponList[0];
        indexFirstWeapon = 0;
        playerWeapons[1] = weaponList[2];
        indexSecondWeapon = 2;

        InstantiateFirstWeapon();
        InstantiateSecondWeapon();
        namePlayer = "/" + gameObject.transform.parent.gameObject.name + "/" + gameObject.name + "/";
        baseRotationRightShoulder = player.GetComponent<GetHand>().rightShoulder.transform.rotation;
        baseRotationLeftShoulder = player.GetComponent<GetHand>().leftShoulder.transform.rotation;

        if (player != null && player.GetComponentInChildren<Animator>() != null && animator == null)
        {
            animator = player.GetComponentInChildren<Animator>();
        }


        SelectWeapon1();

    }

    public void InstantiateFirstWeapon()
    {
        GameObject weapon1 = Instantiate(Weapons[indexFirstWeapon], SpawnWeapon.position, transform.rotation, transform);
        weapon1.name = playerWeapons[0];
        //List<Transform> transforms = new List<Transform>();
        //weapon1.transform.GetComponentsInChildren<Transform>(true, transforms);
        //for (int i = 0; i < transforms.Count; i++)
        //{
        //    transforms[i].gameObject.layer = LayerMask.NameToLayer("P" + player.GetComponentInParent<EntityPlayer>().gameObject.name.Replace("Player ", ""));
        //}
        weapon1.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void InstantiateSecondWeapon()
    {
        GameObject weapon2 = Instantiate(Weapons[indexSecondWeapon], SpawnWeapon.position, transform.rotation, transform);
        weapon2.name = playerWeapons[1];
        //List<Transform> transforms = new List<Transform>();
        //weapon2.transform.GetComponentsInChildren<Transform>(true, transforms);
        //for (int i = 0; i < transforms.Count; i++)
        //{
        //    transforms[i].gameObject.layer = LayerMask.NameToLayer("P" + player.GetComponentInParent<EntityPlayer>().gameObject.name.Replace("Player ", ""));
        //}
        weapon2.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            //            Debug.Log(animator.GetBool("HasBow"));
        }
        //UpdateControls();
    }

    public void UpdateControls()
    {
        if (player != null && player.GetComponentInChildren<Animator>() != null && animator == null)
        {
            animator = player.GetComponentInChildren<Animator>();
        }
        if (weaponSelected != "BareHand")
        {

           // if (player.GetComponentInParent<TpsController>().IsSprint == false)
          //  {
                if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                {

                    //keyboard
                    if (Input.GetMouseButtonDown(0) == true)
                    {
                        weapon.Attack();

                    }
                    else if (Input.GetMouseButton(2) == true)
                    {
                        weapon.SpecialCaCAttack();
                        weapon.DistanceSpecialAttack();

                    }
                    else if (Input.GetMouseButton(0) == true)
                    {

                        weapon.DistanceAttack();

                    }

                }
                if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                {
                    //PAD
                    if (InputManager.Instance.isPressed(controller.device, "RightTrigger", true) && ButtonPressed == false)
                    {
                        weapon.Attack();
                        weapon.DistanceAttack();
                        ButtonPressed = true;
                    }
                    else if (InputManager.Instance.isPressed(controller.device, "RightBumper", true))
                    {
                        weapon.SpecialCaCAttack();
                        weapon.DistanceSpecialAttack();
                    }
                    else if (InputManager.Instance.isPressed(controller.device, "RightTrigger", false) && InputManager.Instance.isPressed(controller.device, "RightBumper", false))
                    {
                        ButtonPressed = false;
                    }
                }
          //  }
        }

        //if (Input.GetKey(KeyCode.T))
        //{
        //    Time.timeScale = 0.1f;
        //}
        //else if (Input.GetKey(KeyCode.T) == false)
        //{
        //    Time.timeScale = 1f;
        //}

        //ChangeWeaponKeyboard();
        //ChangeWeaponPad();

    }

    public void GetBareHanded()
    {
        if (weaponSelected != "BareHand")
        {
            //Sword
            animator.SetBool("Sword", false);
            animator.SetBool("SwordAttack1", false);
            animator.SetBool("SwordAttack2", false);
            animator.SetBool("SwordAttack3", false);
            animator.SetBool("SwordAttack4", false);
            animator.SetBool("ChargedSwordAttack", false);
            // animator.SetBool("SwordCombo", false);

            //Axe
            animator.SetBool("Axe", false);
            animator.SetBool("AxeAttack1", false);
            animator.SetBool("AxeAttack2", false);
            animator.SetBool("AxeAttack3", false);
            animator.SetBool("ChargedAxeAttack", false);
            // animator.SetBool("AxeCombo", false);

            //LaserSword
            animator.SetBool("LaserSword", false);
            animator.SetBool("ShootLaser", false);
            animator.SetBool("LaserSwordAttack", false);

            //Bow
            animator.SetBool("Bow", false);
            animator.SetBool("BowShoot", false);
            animator.SetBool("ChargedBow", false);

            //CrossBow
            animator.SetBool("CrossBow", false);
            animator.SetBool("CrossbowShoot", false);
            animator.SetBool("ChargedCrossBow", false);


            gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
            weaponSelected = "BareHand";
            gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            currentWeapon = 0;
        }
    }

    void ChangeWeaponKeyboard()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (weaponSelected != playerWeapons[0])
                {
                    //Sword
                    animator.SetBool("Sword", false);
                    animator.SetBool("SwordAttack1", false);
                    animator.SetBool("SwordAttack2", false);
                    animator.SetBool("SwordAttack3", false);
                    animator.SetBool("SwordAttack4", false);
                    animator.SetBool("ChargedSwordAttack", false);
                    // animator.SetBool("SwordCombo", false);

                    //Axe
                    animator.SetBool("Axe", false);
                    animator.SetBool("AxeAttack1", false);
                    animator.SetBool("AxeAttack2", false);
                    animator.SetBool("AxeAttack3", false);
                    animator.SetBool("ChargedAxeAttack", false);
                    // animator.SetBool("AxeCombo", false);

                    //LaserSword
                    animator.SetBool("LaserSword", false);
                    animator.SetBool("ShootLaser", false);
                    animator.SetBool("LaserSwordAttack", false);

                    //Bow
                    animator.SetBool("Bow", false);
                    animator.SetBool("BowShoot", false);
                    animator.SetBool("ChargedBow", false);

                    //CrossBow
                    animator.SetBool("CrossBow", false);
                    animator.SetBool("CrossbowShoot", false);
                    animator.SetBool("ChargedCrossBow", false);

                    if (weaponSelected != "BareHand")
                    {
                        gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
                    }
                    weaponSelected = playerWeapons[0];
                    gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);

                    weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    haveFirstWeapon = true;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.SetBool("SwordAttack", false);
                animator.SetBool("ChargedSwordAttack", false);
                animator.SetBool("AxeAttack", false);
                animator.SetBool("ChargedAxeAttack", false);
                animator.SetBool("LaserSwordAttack", false);
                animator.SetBool("HasBow", false);
                animator.SetBool("ShootLaser", false);
                animator.SetBool("CrossbowShoot", false);
                animator.SetBool("IdleCrossbow", false);

                if (weaponSelected != playerWeapons[1])
                {
                    if (weaponSelected != "BareHand")
                    {
                        gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
                    }
                    weaponSelected = playerWeapons[1];
                    gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);
                    weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    haveFirstWeapon = false;
                }
            }



            //else if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    if (weaponSelected != weaponList[2])
            //    {
            //        if (weaponSelected != "BareHand")
            //        {
            //            gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(false);
            //        }
            //        weaponSelected = weaponList[2];
            //        gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(true);
            //        weapon = gameObject.transform.Find(namePlayer + weaponSelected).GetComponent<IWeapon>();
            //        gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    if (weaponSelected != weaponList[3])
            //    {
            //        if (weaponSelected != "BareHand")
            //        {
            //            gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(false);
            //        }
            //        weaponSelected = weaponList[3];
            //        gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(true);
            //        weapon = gameObject.transform.Find(namePlayer + weaponSelected).GetComponent<IWeapon>();
            //        gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    if (weaponSelected != weaponList[4])
            //    {
            //        if (weaponSelected != "BareHand")
            //        {
            //            gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(false);
            //        }
            //        weaponSelected = weaponList[4];
            //        gameObject.transform.Find(namePlayer + weaponSelected).gameObject.SetActive(true);
            //        weapon = gameObject.transform.Find(namePlayer + weaponSelected).GetComponent<IWeapon>();
            //        gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            //    }
            //}
        }
    }

    void ChangeWeaponPad()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(controller.device, "Pad", true))
            {

                if (InputManager.Instance.ValuePAD(controller.device, "Pad").x >= 1 && currentWeapon < 1 && changed == false)
                {
                    animator.SetBool("SwordAttack1", false);
                    animator.SetBool("SwordAttack2", false);
                    animator.SetBool("ChargedSwordAttack", false);
                    animator.SetBool("AxeAttack", false);
                    animator.SetBool("ChargedAxeAttack", false);
                    animator.SetBool("LaserSwordAttack", false);
                    animator.SetBool("HasBow", false);
                    animator.SetBool("ShootLaser", false);
                    animator.SetBool("CrossbowShoot", false);
                    animator.SetBool("CrossB", false);
                    animator.SetBool("ChargedBow", false);

                    if (weaponSelected != "BareHand")
                    {
                        gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
                        currentWeapon += 1;
                    }
                    weaponSelected = playerWeapons[currentWeapon];
                    gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);
                    weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    changed = true;
                    haveFirstWeapon = true;
                }



                if (InputManager.Instance.ValuePAD(controller.device, "Pad").x <= -1 && currentWeapon > 0 && changed == false)
                {
                    animator.SetBool("SwordAttack", false);
                    animator.SetBool("ChargedSwordAttack", false);
                    animator.SetBool("AxeAttack", false);
                    animator.SetBool("ChargedAxeAttack", false);
                    animator.SetBool("LaserSwordAttack", false);
                    animator.SetBool("HasBow", false);
                    animator.SetBool("ShootLaser", false);
                    animator.SetBool("CrossbowShoot", false);
                    animator.SetBool("HasCrossbow", false);
                    animator.SetBool("ChargedBow", false);

                    if (weaponSelected != "BareHand")
                    {
                        gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
                        currentWeapon -= 1;
                    }
                    weaponSelected = playerWeapons[currentWeapon];
                    gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);
                    weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    changed = true;
                    haveFirstWeapon = false;
                }

            }

            if (InputManager.Instance.ValuePAD(controller.device, "Pad").x == 0 && changed == true)
            {
                changed = false;
            }

        }
    }


    public void SelectWeapon1()
    {
        if (weaponSelected != playerWeapons[0])
        {
            //reset des booléans d'animation sauf pour le jump et les combos
            if (animator != null)
            {
                //Sword
                animator.SetBool("Sword", false);
                animator.SetBool("SwordAttack1", false);
                animator.SetBool("SwordAttack2", false);
                animator.SetBool("SwordAttack3", false);
                animator.SetBool("SwordAttack4", false);
                animator.SetBool("ChargedSwordAttack", false);
                // animator.SetBool("SwordCombo", false);

                //Axe
                animator.SetBool("Axe", false);
                animator.SetBool("AxeAttack1", false);
                animator.SetBool("AxeAttack2", false);
                animator.SetBool("AxeAttack3", false);
                animator.SetBool("ChargedAxeAttack", false);
                // animator.SetBool("AxeCombo", false);

                //LaserSword
                animator.SetBool("LaserSword", false);
                animator.SetBool("ShootLaser", false);
                animator.SetBool("LaserSwordAttack", false);

                //Bow
                animator.SetBool("Bow", false);
                animator.SetBool("BowShoot", false);
                animator.SetBool("ChargedBow", false);

                //CrossBow
                animator.SetBool("CrossBow", false);
                animator.SetBool("CrossbowShoot", false);
                animator.SetBool("ChargedCrossBow", false);
            }
            if (weaponSelected != "BareHand")
            {
                gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
            }

            weaponSelected = playerWeapons[0];
            gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);

            weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();

            gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            haveFirstWeapon = true;

            if (animator != null)
            {

                //if (animator.GetFloat("Velocity") < -0.2)
                //{
                //    animator.Play(weaponSelected + "RunBackward");
                //}
                //else if (animator.GetFloat("Velocity") > 0.2)
                //{
                //    animator.Play(weaponSelected + "RunForward");
                //}
                //else if (animator.GetFloat("Velocity") < 0.2 && animator.GetFloat("Velocity") > -0.2)
                //{
                //animator.Play(weaponSelected + "Idle");
                animator.SetBool(weaponSelected, true);
               // Debug.Log("le weappon = " + animator.GetBool(weaponSelected));
                // }
            }
        }
    }

    public void SelectWeapon2()
    {
        //reset des booléans d'animation sauf pour le jump et les combos
        if (animator != null)
        {
            //Sword
            animator.SetBool("Sword", false);
            animator.SetBool("SwordAttack1", false);
            animator.SetBool("SwordAttack2", false);
            animator.SetBool("SwordAttack3", false);
            animator.SetBool("SwordAttack4", false);
            animator.SetBool("ChargedSwordAttack", false);
            // animator.SetBool("SwordCombo", false);

            //Axe
            animator.SetBool("Axe", false);
            animator.SetBool("AxeAttack1", false);
            animator.SetBool("AxeAttack2", false);
            animator.SetBool("AxeAttack3", false);
            animator.SetBool("ChargedAxeAttack", false);
            // animator.SetBool("AxeCombo", false);

            //LaserSword
            animator.SetBool("LaserSword", false);
            animator.SetBool("ShootLaser", false);
            animator.SetBool("LaserSwordAttack", false);

            //Bow
            animator.SetBool("Bow", false);
            animator.SetBool("BowShoot", false);
            animator.SetBool("ChargedBow", false);

            //CrossBow
            animator.SetBool("CrossBow", false);
            animator.SetBool("CrossbowShoot", false);
            animator.SetBool("ChargedCrossBow", false);
        }
        if (weaponSelected != playerWeapons[1])
        {
            if (weaponSelected != "BareHand")
            {
                gameObject.transform.Find(weaponSelected).gameObject.SetActive(false);
            }
            weaponSelected = playerWeapons[1];
            gameObject.transform.Find(weaponSelected).gameObject.SetActive(true);
            weapon = gameObject.transform.Find(weaponSelected).GetComponent<IWeapon>();
            gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            haveFirstWeapon = false;

            if (animator != null)
            {
                //if (animator.GetFloat("Velocity") < -0.2)
                //{
                //    animator.Play(weaponSelected + "RunBackward");
                //}
                //else if (animator.GetFloat("Velocity") > 0.2)
                //{
                //    animator.Play(weaponSelected + "RunForward");
                //}
                //else if (animator.GetFloat("Velocity") < 0.2 && animator.GetFloat("Velocity") > -0.2)
                //{
               // animator.Play(weaponSelected + "Idle");
                animator.SetBool(weaponSelected, true);
                // }
            }
        }
    }

}
