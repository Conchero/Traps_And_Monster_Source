using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Profiling;
using Cinemachine;

public class TpsController : MonoBehaviour
{
    float moveSpeed = 10f;
    float sensitivitySpeed;
    [SerializeField] float minMoveSpeed ;
    [SerializeField] float moyMoveSpeed ;
    [SerializeField] float maxMoveSpeed ;
    [SerializeField] float jumpSpeed = 8.0f;
    public float lookXLimitMin = 115.0f;
    public float lookXLimitMax = 100.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] public Transform playerCameraParent;
    [SerializeField] public int defaultFOV = 60;
    [SerializeField] int sprintFOV = 80;
    [SerializeField] int AimingFOV = 40;

    //modif Bastien
    [SerializeField] CinemachineVirtualCamera virtualCameraRight;
    [SerializeField] CinemachineVirtualCamera virtualCameraLeft;
    [SerializeField] int force = 0;//
    [SerializeField] GameObject UIPause;

    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    [HideInInspector]
    public Vector2 rotation = Vector2.zero;
   public bool canMove = true;
    [HideInInspector]
    public bool canShoot = true;
    bool canRun = true;
    bool isSprint = false;
    bool isRightAimingCamera = true;
    bool tempBool = false;
    bool playerRunSoundPlay = false;
    bool isAttacking = false;
    int blockageAttack = 1;
    float sensitivity;
    float sensitivityAim;

    public string device;
    Vector2 leftStick;
    Vector2 rightStick;
    Camera cam;

    Axe axe = null;

    [HideInInspector]
    public StateGame gameState;
    ShopActivation shopActivation;

    Animator animator = null;

    bool playerInShop = false;

    public bool PlayerInShop { get => playerInShop; set => playerInShop = value; }
    public bool IsSprint { get => isSprint; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    [HideInInspector]
    public CinemachineDollyCart dollyCart;
    [HideInInspector]
    public CinemachineCollider colliderCam;

    //optit 
    string m_deviceType;

    [HideInInspector]
    public CinemachineVirtualCamera camTower;
    bool canBeEagle = true;
    public GameObject uiToDisable;

    [HideInInspector]
    public bool inPause = false;

    private float m_timer;
    float m_cooldownClique = 0.0f;
    float m_cooldownCliqueMax = 0.5f;

    bool isHit = false;
    float timerStop;
	
    float currentSpeedY = 0;
    float currentSpeedX = 0;
    public float acceleration = 0;
    public float deceleration = 0;

    Vector3 forward;
    Vector3 forwardGlobal;
    Vector3 right;
    Vector3 rightGlobal;
    bool isInDeceleration;

    //  CustomSampler m_sampler1;
    bool canClick()
    {
        if (m_cooldownClique <= 0.0f)
        {
            m_cooldownClique = m_cooldownCliqueMax;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Awake()
    {
        dollyCart = virtualCameraRight.GetComponent<CinemachineDollyCart>();
        colliderCam = virtualCameraRight.GetComponent<CinemachineCollider>();
    }

    void Start()
    {
        //  m_sampler1 = CustomSampler.Create("m_sampler1");
        moveSpeed = moyMoveSpeed;
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        gameState = FindObjectOfType<StateGame>();
        shopActivation = FindObjectOfType<ShopActivation>();


        cam = GetComponentInChildren<Camera>();

        m_deviceType = InputManager.Instance.GetLayoutDevice(device);
    }

    void Update()
    {
        //  m_sampler1.Begin();
        // Sensibilité
        sensitivity = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(device).GetHashCode());
        sensitivityAim = sensitivity / 2;
        sensitivitySpeed = sensitivity;

        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }

        m_timer += Time.deltaTime;

        //        Debug.Log("update");
        //Debug.Log(gameObject.name +": "+ playerInShop);
        if (animator == null && GetComponentInChildren<Animator>() != null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (dollyCart.m_Position == 1)
        {
            dollyCart.enabled = false;
            colliderCam.enabled = true;
        }
		
		// Acceleration
        if (leftStick.y != 0 && (canBeEagle || !inPause))
        {
            isInDeceleration = false;
            currentSpeedY = Mathf.Lerp(currentSpeedY, moveSpeed * leftStick.y, Time.deltaTime * acceleration);
        }

        if (leftStick.x != 0 && (canBeEagle || !inPause))
        {
            isInDeceleration = false;
            currentSpeedX = Mathf.Lerp(currentSpeedX, moveSpeed * leftStick.x, Time.deltaTime * acceleration);
        }

        if (gameState.m_gameState == GameState.Game && canBeEagle && !inPause)
        {
            if (GetComponentInChildren<GetHand>().hand.transform.Find("Axe") != null && axe == null)
            {
                axe = GetComponentInChildren<GetHand>().hand.transform.Find("Axe").GetComponent<Axe>();
            }

            //   m_sampler1.End();

            if (m_deviceType != "Keyboard")
            {
                leftStick = InputManager.Instance.ValueJoyStick(device, "LeftStick");

                rightStick = InputManager.Instance.ValueJoyStick(device, "RightStick");
            }
            else
            {
                leftStick = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                leftStick.Normalize();
				
                rightStick = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }

            if (characterController.isGrounded)
            {
                animator.SetBool("Jump", false);
                // Deceleration 
                if (leftStick.y == 0 && (canBeEagle || !inPause))
                {
                    isInDeceleration = true;
                    currentSpeedY = Mathf.Lerp(currentSpeedY, 0, Time.deltaTime * deceleration);
                }

                if (leftStick.x == 0 && (canBeEagle || !inPause))
                {
                    isInDeceleration = true;
                    currentSpeedX = Mathf.Lerp(currentSpeedX, 0, Time.deltaTime * deceleration);
                }
                if(currentSpeedX >= 0.1f
                    || currentSpeedY >= 0.1f
                    || currentSpeedX <= -0.1f
                    || currentSpeedY <= -0.1f)
                {
                    isInDeceleration = false;
                }

               
                
                // We are grounded, so recalculate move direction based on axes
                if((leftStick.x != 0f
                    || leftStick.y != 0f) && !isInDeceleration)
                {
                    forward = transform.TransformDirection(Vector3.forward);
                    forwardGlobal = transform.InverseTransformDirection(Vector3.forward);
                    right = transform.TransformDirection(Vector3.right);
                    rightGlobal = transform.InverseTransformDirection(Vector3.right);
                }

                if (isAttacking == false)
                {
                    blockageAttack = 1;
                }
                else
                {
                    blockageAttack = 0;
                }

                float curSpeedX = canMove ? currentSpeedY * blockageAttack : 0;
                float curSpeedY = canMove ? currentSpeedX * blockageAttack : 0;

                if (curSpeedX > 0.1 || curSpeedY > 0.1)
                {
                    if (playerRunSoundPlay == false)
                    {
                        SoundManager.Instance.RunPlayerPlay(gameObject);
                        playerRunSoundPlay = true;
                    }
                }
                else if (curSpeedX < -0.1 || curSpeedY < -0.1)
                {
                    if (playerRunSoundPlay == false)
                    {
                        SoundManager.Instance.RunPlayerPlay(gameObject);
                        playerRunSoundPlay = true;
                    }
                }
                else
                {
                    SoundManager.Instance.RunPlayerStop(gameObject);
                    playerRunSoundPlay = false;
                }

                if (axe != null && axe.ComboEngaged == false)
                {
                    moveDirection = (forward * curSpeedX) + (right * curSpeedY);
                }
                else if (axe != null && axe.ComboEngaged == true)
                {
                    moveDirection = (forwardGlobal * curSpeedX) + (rightGlobal * curSpeedY);
                }
                else if (axe == null)
                {
                    moveDirection = (forward * curSpeedX) + (right * curSpeedY);
                }


                if (m_deviceType != "Keyboard")
                {
                    if (InputManager.Instance.isPressed(device, "A", true) && canMove)
                    {
                        //empecher lors d'attaques normal et chargé des armes mais pas pour le combo de la hache
                        if (
                            //sword
                            animator.GetBool("SwordAttack1") == false
                            && animator.GetBool("SwordAttack2") == false
                            && animator.GetBool("SwordAttack3") == false
                            && animator.GetBool("SwordAttack4") == false
                            && animator.GetBool("ChargedSwordAttack") == false
                            && animator.GetBool("SwordCombo") == false
                            //axe
                            && animator.GetBool("AxeAttack1") == false
                            && animator.GetBool("AxeAttack2") == false
                            && animator.GetBool("AxeAttack3") == false
                            && animator.GetBool("ChargedAxeAttack") == false
                            //LaserSword
                            && animator.GetBool("ShootLaser") == false
                            && animator.GetBool("LaserSwordAttack") == false

                            //Bow
                            && animator.GetBool("BowShoot") == false
                            && animator.GetBool("ChargedBow") == false

                            //CrossBow
                            && animator.GetBool("CrossbowShoot") == false
                            && animator.GetBool("ChargedCrossBow") == false
                            )
                        {
                            // animator.SetBool(GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected + "Idle", false);
                            moveDirection.y = jumpSpeed;
                            //  if (animator.GetBool("Jump") == false)
                            //  {
                            //animator.Play("Jump");
                            animator.SetBool("Jump", true);
                        }
                    }

                }
                else
                {
                    if (Input.GetButton("Jump") && canMove)
                    {
                        //empecher lors d'attaques normal et chargé des armes mais pas pour le combo de la hache
                        if (
                            //sword
                            animator.GetBool("SwordAttack1") == false
                            && animator.GetBool("SwordAttack2") == false
                            && animator.GetBool("SwordAttack3") == false
                            && animator.GetBool("SwordAttack4") == false
                            && animator.GetBool("ChargedSwordAttack") == false
                            && animator.GetBool("SwordCombo") == false
                            //axe
                            && animator.GetBool("AxeAttack1") == false
                            && animator.GetBool("AxeAttack2") == false
                            && animator.GetBool("AxeAttack3") == false
                            && animator.GetBool("ChargedAxeAttack") == false
                            //LaserSword
                            && animator.GetBool("ShootLaser") == false
                            && animator.GetBool("LaserSwordAttack") == false

                            //Bow
                            && animator.GetBool("BowShoot") == false
                            && animator.GetBool("ChargedBow") == false

                            //CrossBow
                            && animator.GetBool("CrossbowShoot") == false
                            && animator.GetBool("ChargedCrossBow") == false
                            )
                        {
                            // animator.SetBool(GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected + "Idle", false);
                            moveDirection.y = jumpSpeed;

                            //{
                            // animator.Play("Jump");
                            animator.SetBool("Jump", true);
                        }
                    }

                }

                if (GetComponentInChildren<Animator>() != null)
                {
                    animator.SetFloat("Velocity", 1 * curSpeedX / maxMoveSpeed);
                    animator.SetFloat("VelocityY", 1 * curSpeedY / maxMoveSpeed);
                }

                if (GetComponentInChildren<PlayerAnimatorScript>().NeedJump == true)
                {
                    moveDirection.y = GetComponentInChildren<PlayerAnimatorScript>().JumpForce;
                }


            }
            else if (!characterController.isGrounded)
            {
                // We are grounded, so recalculate move direction based on axes
                float curSpeedX = canMove ? currentSpeedY : 0;
                float curSpeedY = canMove ? currentSpeedX : 0;
                moveDirection.x = ((forward * curSpeedX) + (right * curSpeedY)).x;
                moveDirection.z = ((forward * curSpeedX) + (right * curSpeedY)).z;
                SoundManager.Instance.RunPlayerStop(gameObject);
                playerRunSoundPlay = false;
            }


            // Player and Camera rotation
            if (canMove)
            {
                rotation.y += rightStick.x * sensitivitySpeed;
                rotation.x += -rightStick.y * sensitivitySpeed;
                //                Debug.Log(defaultFOV * (cam.rect.height / cam.rect.width));
                //    Debug.Log("camY : " + cam.rect.height);
                float currentPos = defaultFOV * (cam.rect.height / cam.rect.width);
                // Camera in sprint
                if (canRun)
                {
                    if (characterController.velocity.magnitude < 3)
                    {
                        isSprint = false;
                    }

                    if (m_deviceType != "Keyboard")
                    {
                        if (InputManager.Instance.isPressed(device, "LeftStickPress", true))
                        {
                            isSprint = true;
                        }
                        if (isSprint)
                        {
                            moveSpeed = maxMoveSpeed;
                            currentPos = sprintFOV * (cam.rect.height / cam.rect.width);
                        }
                        else
                        {
                            isSprint = false;
                            moveSpeed = moyMoveSpeed;
                            currentPos = defaultFOV * (cam.rect.height / cam.rect.width);
                            canShoot = true;
                        }
                    }
                    else
                    {
                        if (Input.GetButton("Run"))
                        {
                            isSprint = true;
                            moveSpeed = maxMoveSpeed;
                            currentPos = sprintFOV * (cam.rect.height / cam.rect.width);
                        }
                        else
                        {
                            isSprint = false;
                            moveSpeed = moyMoveSpeed;
                            currentPos = defaultFOV * (cam.rect.height / cam.rect.width);
                            canShoot = true;
                        }
                    }
                }

                if (m_deviceType != "Keyboard")
                {
                    // Aiming camera
                    if (canShoot)
                    {
                        if (InputManager.Instance.isPressed(device, "LeftTrigger", true))
                        {
                            sensitivitySpeed = sensitivityAim;
                            currentPos = AimingFOV * (cam.rect.height / cam.rect.width);
                            moveSpeed = minMoveSpeed;
                            canRun = false;
                            isSprint = false;
                        }
                        else if (InputManager.Instance.isPressed(device, "LeftTrigger", false))
                        {
                            sensitivitySpeed = sensitivity;
                            canRun = true;
                        }

                    }

                    //modif Bastien
                    virtualCameraRight.m_Lens.FieldOfView = Mathf.Lerp(virtualCameraRight.m_Lens.FieldOfView, currentPos, Time.deltaTime * 15f);
                    virtualCameraLeft.m_Lens.FieldOfView = Mathf.Lerp(virtualCameraLeft.m_Lens.FieldOfView, currentPos, Time.deltaTime * 15f);
                    //
                    rotation.x = Mathf.Clamp(rotation.x, -lookXLimitMin, lookXLimitMax);
                }
                else
                {
                    // Aiming camera
                    if (canShoot)
                    {
                        if (Input.GetButton("MouseButton2"))
                        {
                            sensitivitySpeed = sensitivityAim;
                            currentPos = AimingFOV * (cam.rect.height / cam.rect.width);
                            moveSpeed = minMoveSpeed;
                            canRun = false;
                            isSprint = false;
                        }
                        else if (!Input.GetButton("MouseButton2"))
                        {
                            sensitivitySpeed = sensitivity;
                            canRun = true;
                        }
                    }

                    //modif Bastien
                    virtualCameraRight.m_Lens.FieldOfView = Mathf.Lerp(virtualCameraRight.m_Lens.FieldOfView, currentPos, Time.deltaTime * 15f);
                    virtualCameraLeft.m_Lens.FieldOfView = Mathf.Lerp(virtualCameraLeft.m_Lens.FieldOfView, currentPos, Time.deltaTime * 15f);
                    //
                    rotation.x = Mathf.Clamp(rotation.x, -lookXLimitMin, lookXLimitMax);

                }

                playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
                if (axe != null && axe.ComboEngaged == false)
                {
                    transform.eulerAngles = new Vector2(0, rotation.y);
                }
                else if (axe == null)
                {
                    transform.eulerAngles = new Vector2(0, rotation.y);
                }

            }

            //modif Bastien
            if (m_deviceType != "Keyboard")
            {
                if (InputManager.Instance.isPressed(device, "RightStickPress", true) && !tempBool)
                {
                    virtualCameraRight.enabled = !virtualCameraRight.enabled;
                    virtualCameraLeft.enabled = !virtualCameraLeft.enabled;

                    isRightAimingCamera = !isRightAimingCamera;
                    tempBool = true;
                }
                else if (InputManager.Instance.isPressed(device, "RightStickPress", false))
                {
                    tempBool = false;
                }
            }
            else
            {
                if (InputManager.Instance.isPressed(device, "RightStickPress", true) && !tempBool)
                {
                    virtualCameraRight.enabled = !virtualCameraRight.enabled;
                    virtualCameraLeft.enabled = !virtualCameraLeft.enabled;

                    isRightAimingCamera = !isRightAimingCamera;
                    tempBool = true;
                }
                else if (InputManager.Instance.isPressed(device, "RightStickPress", false))
                {
                    tempBool = false;
                }
            }//
        }
        else if (gameState.m_gameState == GameState.Scoreboard)
        {
            UIPause.transform.GetChild(0).gameObject.SetActive(false);
            UIPause.transform.GetChild(1).gameObject.SetActive(false);
            UIPause.transform.GetChild(2).gameObject.SetActive(false);
            UIPause.transform.GetChild(3).gameObject.SetActive(false);
            inPause = false;
            moveDirection = Vector3.zero;
        }
        else if (!canBeEagle || inPause)
        {
            forward = transform.TransformDirection(Vector3.forward);
            forwardGlobal = transform.InverseTransformDirection(Vector3.forward);
            right = transform.TransformDirection(Vector3.right);
            rightGlobal = transform.InverseTransformDirection(Vector3.right);
            float curSpeedX = canMove ? currentSpeedY * blockageAttack : 0;
            float curSpeedY = canMove ? currentSpeedX * blockageAttack : 0;
            
            leftStick = new Vector2(0, 0);

            if (GetComponentInChildren<Animator>() != null)
            {
                animator.SetFloat("Velocity", 1 * curSpeedX / maxMoveSpeed);
                animator.SetFloat("VelocityY", 1 * curSpeedY / maxMoveSpeed);
            }
            if (characterController.isGrounded)
            {
                animator.SetBool("Jump", false);
            }
            moveDirection.x = ((forward * curSpeedX) + (right * curSpeedY)).x;
            moveDirection.z = ((forward * curSpeedX) + (right * curSpeedY)).z;
        }

        if (!playerInShop)
        {

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller

            characterController.Move(moveDirection * Time.deltaTime);
        }

        // Pause System
        if (gameState.m_gameState == GameState.Game)
        {
            if (m_deviceType != "Keyboard")
            {
                if (inPause)
                {
                    if (InputManager.Instance.isPressed(device, "Start", true) && canClick())
                    {
                        UIPause.transform.GetChild(0).gameObject.SetActive(false);
                        UIPause.transform.GetChild(1).gameObject.SetActive(false);
                        UIPause.transform.GetChild(2).gameObject.SetActive(false);
                        UIPause.transform.GetChild(3).gameObject.SetActive(false);

                        uiToDisable.transform.GetChild(0).gameObject.SetActive(true);
                        uiToDisable.transform.GetChild(1).gameObject.SetActive(true);

                        inPause = false;
                    }

                    /*if (UIPause.transform.GetChild(1).gameObject.activeSelf && UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eInState == UiPause.StatePause.Main)
                    {
                        if (InputManager.Instance.isPressed(device, "B", true) && UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().CanClick())
                        {
                            UIPause.transform.GetChild(0).gameObject.SetActive(false);
                            UIPause.transform.GetChild(1).gameObject.SetActive(false);
                            UIPause.transform.GetChild(2).gameObject.SetActive(false);
                            UIPause.transform.GetChild(3).gameObject.SetActive(false);
                            inPause = false;
                        }
                    }*/
                }
                else
                {
                    if (InputManager.Instance.isPressed(device, "Start", true) && canClick())
                    {
                        UIPause.transform.GetChild(0).gameObject.SetActive(true);
                        UIPause.transform.GetChild(1).gameObject.SetActive(true);
                        UIPause.transform.GetChild(2).gameObject.SetActive(true);
                        UIPause.transform.GetChild(3).gameObject.SetActive(false);

                        uiToDisable.transform.GetChild(0).gameObject.SetActive(false);
                        uiToDisable.transform.GetChild(1).gameObject.SetActive(false);

                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eSelectedPauseButton = UiPause.PauseButton.Touches;
                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eInState = UiPause.StatePause.Main;
                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().SelectObject(1, 1);

                        inPause = true;
                    }
                }
            }
            else
            {
                if (inPause)
                {
                    if (Input.GetKeyDown(KeyCode.Tab) && canClick())
                    {
                        UIPause.transform.GetChild(0).gameObject.SetActive(false);
                        UIPause.transform.GetChild(1).gameObject.SetActive(false);
                        UIPause.transform.GetChild(2).gameObject.SetActive(false);
                        UIPause.transform.GetChild(3).gameObject.SetActive(false);

                        uiToDisable.transform.GetChild(0).gameObject.SetActive(true);
                        uiToDisable.transform.GetChild(1).gameObject.SetActive(true);

                        inPause = false;
                    }

                    /*if (UIPause.transform.GetChild(1).gameObject.activeSelf && UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eInState == UiPause.StatePause.Main)
                    {
                        if (InputManager.Instance.isPressed(device, "B", true) && canClick())
                        {
                            UIPause.transform.GetChild(0).gameObject.SetActive(false);
                            UIPause.transform.GetChild(1).gameObject.SetActive(false);
                            UIPause.transform.GetChild(2).gameObject.SetActive(false);
                            UIPause.transform.GetChild(3).gameObject.SetActive(false);
                            inPause = false;
                        }
                    }*/
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Tab) && canClick())
                    {
                        UIPause.transform.GetChild(0).gameObject.SetActive(true);
                        UIPause.transform.GetChild(1).gameObject.SetActive(true);
                        UIPause.transform.GetChild(2).gameObject.SetActive(true);
                        UIPause.transform.GetChild(3).gameObject.SetActive(false);

                        uiToDisable.transform.GetChild(0).gameObject.SetActive(false);
                        uiToDisable.transform.GetChild(1).gameObject.SetActive(false);

                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eSelectedPauseButton = UiPause.PauseButton.Touches;
                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().m_eInState = UiPause.StatePause.Main;
                        UIPause.transform.GetChild(1).gameObject.GetComponent<UiPause>().SelectObject(1, 1);

                        inPause = true;
                    }
                }
            }
        }

        if (isHit == true)
        {
            timerStop -= Time.deltaTime;
            canMove = false;
            if (timerStop <= 0)
            {
                isHit = false;
                canMove = true;
            }
        }
    }

    public void StopPlayer(float _duration)
    {
        isHit = true;
        timerStop = _duration;
    }

    float timer = 0f;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Trigger")
        {
            if (InputManager.Instance.isPressed(device, "X", true) && canClick() && gameState.m_gameState == GameState.Game && !inPause)
            {
                if (canBeEagle)
                {
                    if (virtualCameraLeft.enabled)
                    {
                        virtualCameraLeft.enabled = false;
                        camTower.enabled = true;
                    }
                    else if (virtualCameraRight.enabled)
                    {
                        virtualCameraRight.enabled = false;
                        camTower.enabled = true;
                    }
                }
                else
                {
                    if (!virtualCameraRight.enabled)
                    {
                        camTower.enabled = false;
                        virtualCameraRight.enabled = true;
                    }
                }
                uiToDisable.SetActive(!uiToDisable.activeSelf);
                canBeEagle = !canBeEagle;
            }
        }

        if (hit.gameObject.name == "SpawnDemons")
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                GetComponent<EntityPlayer>().RemoveLife(3, null, hit.gameObject);
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }

        if (hit.gameObject.tag == "Portal")
        {
            Portal portalScript = hit.gameObject.GetComponent<Portal>();
            Vector3 newPos = transform.position;
            newPos = portalScript.portalToTP.transform.position + portalScript.portalToTP.transform.forward;
            transform.position = newPos;
            rotation = (Vector2)portalScript.portalToTP.transform.eulerAngles;
            SoundManager.Instance.PortalTravelPlay(gameObject);
        }
    }
}
