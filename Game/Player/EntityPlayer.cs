using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Capacity
{
    WEAPON1,
    WEAPON2,
    TRAP,
    INVOCATION
}

public enum CurrentTrap
{
    RotationTrap,
    Barricade,
    Walltrap,
    Plafond

}
public enum CurrentInvocation
{
    Attaquant,
    Defenseur,
    Destructeur,
    Tueur

}

public class EntityPlayer : MonoBehaviour
{
    StateGame m_stateGame;

    public Camp m_linkedCamp;
    public Nexus m_linkedNexus;
    //position to spawn
    public Transform m_transformToSpawn;
    //variable qui va recuperer les reglagles de la partie
    GameplaySettings.Settings m_currentSettings;
    TpsController m_TPSController;
    //////Valeur correspondant au camp
    public int m_playerId;
    public string m_sColor;
    /////DEAD
    [SerializeField] bool m_eventDead = false;
    public bool m_isDead;
    public bool m_campIsAlive = true;
    ////Temps laissé au ragdoll pour s'update
    //private float m_deathTimer;
    //private float m_deathTimerMax = 2.0f;
    //Respawn 
    public float m_respawnTimerCoolDown;
    //S'incremente à chaque mort
    float m_respawnTimerTotal;
    //timer qu'on incremente à chaque frame en cas de isDead = true
    float m_respawnTimer;
    public float m_respawnTimerMax;
    //Object to disable for ragdoll
    [SerializeField] CharacterController m_scriptCharacterController;
    [SerializeField] CapsuleCollider m_scriptCapsuleCollider;
    public WeaponBehaviour m_weaponBehavior;
    public NavMeshObstacle m_navMeshObstacle;

    //toggle if ready for next wave or not
    public bool isReadyForNextWave;

    //CAPACITY
    //Stock la capacité selectionné 
    public Capacity m_eSelectedCapacity;
    //Stock L'ui de la barre de capacitée
    public CapacityBarUI m_sCapacityBarUI;
    //Stock L'ui de la barre des traps
    public GameObject m_TrapsBarUI;
    bool m_bTrapBarOpen = false;
    //Stock le piege selectionné
    CurrentTrap m_eSelectedTrap = CurrentTrap.RotationTrap;
    //Stock L'ui de la barre des invocations
    public GameObject m_InvocationBarUI;
    bool m_bInvocBarOpen = false;
    //Stock l'unité selectionné
    CurrentInvocation m_eSelectedInvocation = CurrentInvocation.Attaquant;
    //Stock les component qui représente les capacity
    SpawnPlayerInvocation m_sSpawnPlayerInvocation;
    Placingtraps m_sPlacingtraps;



    //RULES
    //Boolean qui nous dira si on se trouve dans une base ennemis ou non 
    public bool m_isInEnnemyCamp;
    public bool m_isInHisCamp;
    //Boolean en charge de nous dire si on peut se servir de nos armes
    public bool m_canUseWeappons;
    //Boolean en charge de nous dire si on peut se servir de nos pieges
    public bool m_canUseTraps;
    //Boolean en charge de nous dire si on peut se servir de nos pieges
    public bool m_canUseInvocations;
    //Distance invicible
    public bool m_isDistanceInvicible;
    //invicible
    bool m_isInvicible;
    WaveManager m_waveManager;



    //Stats 
    public int m_healthStart;
    public int m_health;
    public int m_armor;
    public int m_armorStart;
    public int m_armorMax;

    //money
    bool m_bDropGemme;
    private int m_money;
    public int m_startMoney;
    public int m_healthMax;
    //determine l'argent qu'on percois lorsqu'on tue cette entitée
    public int m_killValue;
    [SerializeField] Image[] lifeBars;


    //UI temp
    [SerializeField] public UIPlayer m_UIPlayer;

    //UiInfo
    public UIInfoBarCapacity m_UIInfoBarCapacity;
    bool needToReUpdateInfoCap;
    string prevWeapponName;

    //Not enough money
    public Image m_imgNotEnoughMoney;
    float m_timerNotEnoughMoney = 0.0f;
    public float m_timerMaxNotEnoughMoney;
    float m_timerFadeNotEnoughMoney = 0.0f;
    public float m_timerFadeMaxNotEnoughMoney;

    //bool isAlreadyChanged = false;

    //PAUSE
    public UiPause m_sUiPause;

    // Killer
    public GameObject m_killer;
    public List<Sprite> m_KillersSprite = new List<Sprite>();

    public Text timerRespawn;

    // ScoreBoard
    public Stats scoreboardStats;
    bool m_endGame;
    
    Gamepad gamepad;


    float m_cooldownClique = 0.0f;
    public float m_cooldownCliqueMax = 0.2f;
    bool CanClick()
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
    public int Money { get => m_money; }
    // Start is called before the first frame update
    void Start()
    {

        // Debug.Log("end start player nb" + m_playerId + " position : " + transform.position);
        m_stateGame = FindObjectOfType<StateGame>();
        m_currentSettings = GameplaySettings.Instance.m_customSettings;
        m_TPSController = gameObject.GetComponent<TpsController>();
        //Dead
        m_isDead = false;
        //Init de la capacité dans l'état de depart
        m_eSelectedCapacity = Capacity.WEAPON1;
        m_sSpawnPlayerInvocation = GetComponent<SpawnPlayerInvocation>();
        m_sPlacingtraps = GetComponent<Placingtraps>();
        m_weaponBehavior = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();

        //Recup du component PAS
        PlayerAnimatorScript PAS = GetComponentInChildren<PlayerAnimatorScript>();

        PAS.id = m_playerId + 1;

        isReadyForNextWave = false;

        //Stats 
        m_health = m_healthStart;
        m_armor = m_armorStart;
        //Money
        m_bDropGemme = GameplaySettings.Instance.m_customSettings.GemmeDrop;
        m_money = m_startMoney;
        //Prend le temps indiqué dans le serialize
        m_respawnTimerTotal = m_respawnTimerCoolDown;
        m_respawnTimer = 0;
        //Permet de savoir si un joueur se trouve dans un camp ennemis 
        m_isInEnnemyCamp = false;
        m_isInHisCamp = true;
        m_canUseWeappons = m_currentSettings.PlayersCanUseWeapponInCampZone;
        //Update UI
        m_UIPlayer.m_needUpdate = true;

        //Click cooldown
        m_cooldownClique = 0;


        //Rules
        m_waveManager = FindObjectOfType<WaveManager>();

        //Capacity Info
        needToReUpdateInfoCap = false;
        UpdateCapacityInfo();
        prevWeapponName = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().weaponSelected;

        // ScoreBoard
        scoreboardStats = FindObjectOfType<Stats>();
        m_endGame = false;


        // Set gamepad
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
        {
            gamepad = (Gamepad)InputSystem.GetDevice(m_TPSController.device);
        }

        EnterInCamp();
    }

    void UpdateCapacityInfo()
    {
        string name = "";
        int price = 0;
        int DSV = 0;
        int DSO = 0;
        int life = 0;
        int armor = 0;


        switch (m_eSelectedCapacity)
        {
            case Capacity.WEAPON1:
                name = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().weaponSelected;

                if (name == "")
                {
                    needToReUpdateInfoCap = true;
                }
                switch (name)
                {
                    case "Axe":
                        Axe axe = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Axe>();
                        DSV = axe.stats[0];
                        DSO = axe.stats[1];
                        break;
                    case "Sword":
                        Sword sword = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Sword>();
                        DSV = sword.stats[0];
                        DSO = sword.stats[1];
                        break;
                    case "LaserSword":
                        LaserSword laserSword = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<LaserSword>();
                        DSV = laserSword.stats[0];
                        DSO = laserSword.stats[1];
                        name = "Laser Sword";
                        break;
                    case "Bow":
                        Bow bow = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Bow>();
                        DSV = bow.stats[0];
                        //DSO = bow.stats[1];
                        break;
                    case "CrossBow":
                        CrossBow crossBow = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<CrossBow>();
                        DSV = crossBow.stats[0];
                        //DSO = crossBow.stats[1];
                        name = "Crossbow";
                        break;
                    default:
                        break;
                }
                //Debug.Log("name : " + name);

                break;
            case Capacity.WEAPON2:
                name = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().weaponSelected;

                if (name == "")
                {
                    needToReUpdateInfoCap = true;
                }
                switch (name)
                {
                    case "Axe":
                        Axe axe = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Axe>();
                        DSV = axe.stats[0];
                        DSO = axe.stats[1];

                        break;
                    case "Sword":
                        Sword sword = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Sword>();
                        DSV = sword.stats[0];
                        DSO = sword.stats[1];
                        break;
                    case "LaserSword":
                        LaserSword laserSword = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<LaserSword>();
                        DSV = laserSword.stats[0];
                        DSO = laserSword.stats[1];
                        name = "Laser Sword";
                        break;
                    case "Bow":
                        Bow bow = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Bow>();
                        DSV = bow.stats[0];
                        // DSO = bow.stats[1];
                        break;
                    case "CrossBow":
                        CrossBow crossBow = GetComponentInChildren<GetHand>().hand.GetComponentInChildren<CrossBow>();
                        DSV = crossBow.stats[0];
                        name = "Cross Bow";
                        //DSO = crossBow.stats[1];
                        break;
                    default:
                        break;
                }
                break;
            case Capacity.TRAP:
                name = m_sPlacingtraps.selectdTrap.name;
                price = m_sPlacingtraps.selectdTrap.price;
                DSV = m_sPlacingtraps.selectdTrap.GetComponentInChildren<TrapAttack>().damage;
                life = m_sPlacingtraps.selectdTrap.maxLife;
                armor = m_sPlacingtraps.selectdTrap.m_maxArmor;

                break;
            case Capacity.INVOCATION:
                name = m_sSpawnPlayerInvocation.actualInvoc.name;
                price = m_sSpawnPlayerInvocation.actualInvocCost;
                DSV = m_sSpawnPlayerInvocation.listStatsActualUnite[2];
                DSO = m_sSpawnPlayerInvocation.listStatsActualUnite[3];
                life = m_sSpawnPlayerInvocation.listStatsActualUnite[0];
                armor = m_sSpawnPlayerInvocation.listStatsActualUnite[1];
                //switch(name)
                //{
                //    case :
                //        break:
                //    default:
                //        Debug.Log("error switch");
                //        break;
                //}

                //DSV = m_sSpawnPlayerInvocation.actualInvoc.m_degatsSurVivants;
                //life = m_sSpawnPlayerInvocation.actualInvoc.maxLife;
                //armor = m_sSpawnPlayerInvocation.actualInvoc.m_maxArmor;
                break;
            default:
                break;
        }
        //Execute Info
        m_UIInfoBarCapacity.DrawInfo(name, price, DSV, DSO, life, armor);

    }

    public void DrawNotEnoughMoney()
    {

        m_imgNotEnoughMoney.enabled = true;
        m_imgNotEnoughMoney.color = Color.white;
        m_timerNotEnoughMoney = m_timerMaxNotEnoughMoney;
        //  m_timerFadeNotEnoughMoney = m_timerFadeMaxNotEnoughMoney;

    }

    void UpdateNotEnoughMoney()
    {


        //Si le timer de fade n'est pas fini
        if (m_timerFadeNotEnoughMoney > 0)
        {
            //On reduit le timer d'apparition 
            m_timerFadeNotEnoughMoney -= Time.deltaTime;
            Color color = new Color(1f, 1f, 1f, 1f * m_timerFadeNotEnoughMoney / m_timerFadeMaxNotEnoughMoney);
            //On addapte l'alpha

            m_imgNotEnoughMoney.color = color;

            if (m_timerFadeNotEnoughMoney <= 0)
            {
                m_imgNotEnoughMoney.enabled = false;
            }
            //    //On met l'alpha à 0
            //    m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 0);
            //    m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 0);
            //    m_moneyAddCount = 0;

        }

        if (m_timerNotEnoughMoney > 0)
        {

            //  Debug.Log("m_capacityChangeTimer : " + m_capacityChangeTimer);
            m_timerNotEnoughMoney -= Time.deltaTime;

            if (m_timerNotEnoughMoney <= 0)
            {
                m_timerFadeNotEnoughMoney = m_timerFadeMaxNotEnoughMoney;
            }
        }


    }

    void SetAnimatorsToFalse()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            Animator animator = GetComponentInChildren<Animator>();
            //GetComponentInChildren<Animator>().SetBool("SwordIdle", false);
            //GetComponentInChildren<Animator>().SetBool("SwordAttack1", false);
            //GetComponentInChildren<Animator>().SetBool("SwordAttack2", false);
            //GetComponentInChildren<Animator>().SetBool("SwordAttack3", false);
            //GetComponentInChildren<Animator>().SetBool("SwordAttack4", false);
            //GetComponentInChildren<Animator>().SetBool("SwordCombo", false);
            //GetComponentInChildren<Animator>().SetBool("ChargedSwordAttack", false);
            //GetComponentInChildren<Animator>().SetBool("AxeAttack1", false);
            //GetComponentInChildren<Animator>().SetBool("AxeAttack2", false);
            //GetComponentInChildren<Animator>().SetBool("AxeAttack3", false);
            //GetComponentInChildren<Animator>().SetBool("AxeCombo", false);
            //GetComponentInChildren<Animator>().SetBool("AxeIdle", false);
            //GetComponentInChildren<Animator>().SetBool("ChargedAxeAttack", false);
            //GetComponentInChildren<Animator>().SetBool("LaserSwordIdle", false);
            //GetComponentInChildren<Animator>().SetBool("LaserSwordAttack", false);
            //GetComponentInChildren<Animator>().SetBool("BowIdle", false);
            //GetComponentInChildren<Animator>().SetBool("BowShoot", false);
            //GetComponentInChildren<Animator>().SetBool("ChargedBow", false);
            //GetComponentInChildren<Animator>().SetBool("CrossbowShoot", false);
            //GetComponentInChildren<Animator>().SetBool("CrossBowIdle", false);

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

    }

    private void Update()
    {
        // Debug.Log("update player nb" + m_playerId + " position : " + transform.position);

        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }
        //DEAD
        if (m_isDead)
        {
            timerRespawn.text = ((int)(m_respawnTimerTotal - m_respawnTimer)).ToString() + " s";

            SoundManager.Instance.RunPlayerStop(gameObject);
            // transform.Translate(Vector3.up);
            m_respawnTimer += Time.deltaTime;
            if (m_respawnTimer >= m_respawnTimerTotal && m_campIsAlive)
            {
                //Respawn
                m_isDead = false;
                m_respawnTimer = 0;
                if (m_respawnTimerTotal + m_respawnTimerCoolDown < m_respawnTimerMax)
                {
                    m_respawnTimerTotal += m_respawnTimerCoolDown;
                }
                else
                {
                    m_respawnTimerTotal = m_respawnTimerMax;
                }
                Respawn();
                // Debug.Log("Respawn");
            }

        }
        //EVENT DEAD
        if (m_eventDead)
        {
            KillPlayer();
            m_eventDead = false;
        }

        //UPDATE IN GAME
        if (m_stateGame.m_gameState == GameState.Game && !m_isDead)
        {
            if (!m_TPSController.inPause)
            {
                //Fermeture des selection trap et invoc si on utilise la capacitée
                if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
                {
                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        //Fermeture de la barre de selection de piege si on change de capacity
                        DesactiveBarTrap();
                        DesactiveBarInvocation();
                    }
                }
                else
                {
                    if (InputManager.Instance.isPressed(m_TPSController.device, "Y", true))
                    {
                        //Fermeture de la barre de selection de piege si on change de capacity
                        DesactiveBarTrap();
                        DesactiveBarInvocation();
                    }
                }



                //Navigation dans la barre de capacité
                if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
                {
                    if (m_weaponBehavior.comboEngaged == false)
                    {

                        if (Input.GetKeyDown(KeyCode.Alpha1) && CanClick())
                        {
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            m_weaponBehavior.SelectWeapon1();
                            m_eSelectedCapacity = Capacity.WEAPON1;
                            m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                            //Fermeture de la barre de selection de piege si on change de capacity
                            DesactiveBarTrap();
                            //fermeture de la barre d'invocation si on change de capacity
                            DesactiveBarInvocation();

                            //Info
                            UpdateCapacityInfo();

                            SoundManager.Instance.CapacityChangePlay(gameObject);

                        }
                        if (Input.GetKeyDown(KeyCode.Alpha2) && CanClick())
                        {
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            m_weaponBehavior.SelectWeapon2();
                            m_eSelectedCapacity = Capacity.WEAPON2;
                            m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                            //Fermeture de la barre de selection de piege si on change de capacity
                            DesactiveBarTrap();
                            //fermeture de la barre d'invocation si on change de capacity
                            DesactiveBarInvocation();

                            //Info
                            UpdateCapacityInfo();


                            SoundManager.Instance.CapacityChangePlay(gameObject);



                        }
                        if (Input.GetKeyDown(KeyCode.Alpha3) && m_eSelectedCapacity != Capacity.TRAP && CanClick())
                        {
                            SetAnimatorsToFalse();
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            m_weaponBehavior.GetBareHanded();
                            m_eSelectedCapacity = Capacity.TRAP;
                            m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                            //fermeture de la barre d'invocation si on change de capacity
                            DesactiveBarInvocation();
                            //      Debug.Log("activ capacity trap");
                            //Active le mode piege dans le script
                            // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap+1));
                            m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));

                            //Info
                            UpdateCapacityInfo();


                            SoundManager.Instance.CapacityChangePlay(gameObject);
                        }
                        if (Input.GetKeyDown(KeyCode.Alpha4) && m_eSelectedCapacity != Capacity.INVOCATION && CanClick())
                        {
                            SetAnimatorsToFalse();
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            m_weaponBehavior.GetBareHanded();
                            m_eSelectedCapacity = Capacity.INVOCATION;
                            m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                            //Fermeture de la barre de selection de piege si on change de case
                            DesactiveBarTrap();

                            //Info
                            UpdateCapacityInfo();


                            SoundManager.Instance.CapacityChangePlay(gameObject);


                        }
                    }

                }
                if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
                {

                    Vector2 ToucheDirectionnel = InputManager.Instance.ValuePAD(m_TPSController.device, "Pad");
                    if (!m_bInvocBarOpen && !m_bTrapBarOpen)
                    {
                        if (ToucheDirectionnel.x == 1 && CanClick())
                        {
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            //SetAnimatorsToFalse();
                            NextCapacity();
                            //Info
                            UpdateCapacityInfo();

                            SoundManager.Instance.CapacityChangePlay(gameObject);
                        }
                        else if (ToucheDirectionnel.x == -1 && CanClick())
                        {
                            gameObject.GetComponent<TpsController>().IsAttacking = false;
                            //  SetAnimatorsToFalse();
                            PreviousCapacity();
                            //Info
                            UpdateCapacityInfo();

                            SoundManager.Instance.CapacityChangePlay(gameObject);
                        }
                    }
                }
                //Update en fonction de la selection de la barre de capacité
                switch (m_eSelectedCapacity)
                {
                    case Capacity.WEAPON1:
                        if (m_canUseWeappons)
                        {
                            UpdateCapacityWeapon1();
                        }
                        break;
                    case Capacity.WEAPON2:
                        if (m_canUseWeappons)
                        {
                            UpdateCapacityWeapon2();
                        }
                        break;
                    case Capacity.TRAP:


                        UpdateCapacityTraps();
                        if (m_canUseTraps)
                        {
                            if ((m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseTrapInWave) || (!m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseTrapOutWave))
                            {
                                m_sPlacingtraps.UpdateControls();
                            }
                        }
                        break;
                    case Capacity.INVOCATION:

                        UpdateCapacityInvocation();
                        if (m_canUseInvocations)
                        {
                            if ((m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseInvocationInWave) || (!m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseInvocationOutWave))
                            {
                                m_sSpawnPlayerInvocation.UpdateInvocationCapacity();
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (!m_bInvocBarOpen && !m_bTrapBarOpen && m_stateGame.playerGO.Count > 2)
                {
                    m_sSpawnPlayerInvocation.UpdateNexusSelection();
                }
            }
            else
            {
                m_sUiPause.m_TPSController = m_TPSController;
                m_sUiPause.UpdateControlUiPause();
            }
        }
        //Scorboard
        else if(m_stateGame.m_gameState == GameState.Scoreboard && !m_isDead)
        {

          if( ! m_endGame)
            {
                m_endGame = true;
                int rand = Random.Range(1, 4);
                string animName = "EndGame" + rand.ToString();
                GetComponentInChildren<Animator>().SetTrigger(animName);
            }
        }

        foreach (Image img in lifeBars)
        {
            img.fillAmount = (float)m_health / m_healthMax;
        }

        if (needToReUpdateInfoCap)
        {
            needToReUpdateInfoCap = !needToReUpdateInfoCap;
            UpdateCapacityInfo();
        }

        if (prevWeapponName != GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().weaponSelected)
        {
            prevWeapponName = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().weaponSelected;
            UpdateCapacityInfo();
        }

        //Ui message not enough money update
        UpdateNotEnoughMoney();
    }

    void DesactiveBarTrap()
    {
        if (m_bTrapBarOpen)
        {
            m_bTrapBarOpen = false;
            m_TrapsBarUI.SetActive(m_bTrapBarOpen);
            // m_sPlacingtraps.SetTrap(0);
        }

    }
    void DesactiveBarInvocation()
    {
        if (m_bInvocBarOpen)
        {
            m_bInvocBarOpen = false;
            m_InvocationBarUI.SetActive(m_bInvocBarOpen);
        }

    }

    //CAPACITY
    void NextCapacity()
    {
        switch (m_eSelectedCapacity)
        {
            case Capacity.WEAPON1:
                m_weaponBehavior.SelectWeapon2();
                m_eSelectedCapacity++;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                break;
            case Capacity.WEAPON2:
                SetAnimatorsToFalse();
                m_weaponBehavior.GetBareHanded();
                m_eSelectedCapacity++;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                break;
            case Capacity.TRAP:
                m_eSelectedCapacity++;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                //Fermeture de la barre de selection de piege si on change de capacity
                DesactiveBarTrap();

                break;
            case Capacity.INVOCATION:
                // Debug.Log("capacityMax");
                break;
            default:
                break;
        }
    }
    void PreviousCapacity()
    {
        switch (m_eSelectedCapacity)
        {
            case Capacity.WEAPON1:

                //  Debug.Log("capacityMin");
                break;
            case Capacity.WEAPON2:
                m_weaponBehavior.SelectWeapon1();
                m_eSelectedCapacity--;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                break;
            case Capacity.TRAP:
                m_weaponBehavior.SelectWeapon2();
                m_eSelectedCapacity--;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                //Fermeture de la barre de selection de piege si on change de capacity
                DesactiveBarTrap();

                break;
            case Capacity.INVOCATION:
                m_eSelectedCapacity--;
                m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                //fermeture de la barre d'invocation si on change de capacity
                DesactiveBarInvocation();
                break;
            default:
                break;
        }
    }

    //TRAPS
    void NextTrap()
    {
        switch (m_eSelectedTrap)
        {
            case CurrentTrap.RotationTrap:
                m_eSelectedTrap++;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));

                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);
                break;
            case CurrentTrap.Barricade:
                m_eSelectedTrap++;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));

                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);
                break;
            case CurrentTrap.Walltrap:

                m_eSelectedTrap++;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));

                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);

                break;
            default:
                break;
        }
    }
    void PreviousTrap()
    {
        switch (m_eSelectedTrap)
        {
            case CurrentTrap.RotationTrap:
                //  Debug.Log("beggining of traps");
                break;
            case CurrentTrap.Barricade:
                m_eSelectedTrap--;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));
                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);
                break;
            case CurrentTrap.Walltrap:
                m_eSelectedTrap--;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));
                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);
                break;
            case CurrentTrap.Plafond:
                m_eSelectedTrap--;
                m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap));
                // m_sPlacingtraps.SetTrap(((int)m_eSelectedTrap + 1));
                m_TrapsBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedTrap);
                break;
            default:
                break;
        }
    }

    //INVOCATION
    void NextInvocation()
    {
        switch (m_eSelectedInvocation)
        {
            case CurrentInvocation.Attaquant:
                m_eSelectedInvocation++;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            case CurrentInvocation.Defenseur:
                m_eSelectedInvocation++;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            case CurrentInvocation.Destructeur:
                m_eSelectedInvocation++;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            case CurrentInvocation.Tueur:
                // Debug.Log("end of traps");

                break;
            default:
                break;
        }
    }
    void PreviousInvocation()
    {
        switch (m_eSelectedInvocation)
        {
            case CurrentInvocation.Attaquant:
                break;
            case CurrentInvocation.Defenseur:
                m_eSelectedInvocation--;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            case CurrentInvocation.Destructeur:
                m_eSelectedInvocation--;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            case CurrentInvocation.Tueur:
                m_eSelectedInvocation--;

                m_InvocationBarUI.GetComponent<CapacityBarUI>().SetCurrentButton((int)m_eSelectedInvocation);
                m_sSpawnPlayerInvocation.SetInvocation((int)m_eSelectedInvocation);
                break;
            default:
                break;
        }
    }

    void UpdateCapacityWeapon1()
    {
        if (m_weaponBehavior != null)
        {

            m_weaponBehavior.UpdateControls();
        }
        else
        {
            //  Debug.Log("error m_weapponbehavior == null");
            m_weaponBehavior = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
        }
    }
    void UpdateCapacityWeapon2()
    {
        if (m_weaponBehavior != null)
        {

            m_weaponBehavior.UpdateControls();
        }
        else
        {
            // Debug.Log("error m_weapponbehavior == null");

            m_weaponBehavior = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
        }
    }
    void UpdateCapacityTraps()
    {
        //ouverture de la barre de selection de piege
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
        {
            //ouverture /fermeture de la barre de selection de piege
            if (Input.GetKeyDown(KeyCode.Alpha3) && CanClick())
            {

                // Debug.Log("activ trap barre");
                if (m_bTrapBarOpen)
                {

                    SoundManager.Instance.CapacityChangePlay(gameObject);
                    m_bTrapBarOpen = false;
                    m_TrapsBarUI.SetActive(m_bTrapBarOpen);

                }
                else
                {
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                    m_bTrapBarOpen = true;
                    m_TrapsBarUI.SetActive(m_bTrapBarOpen);
                    //Info
                    UpdateCapacityInfo();
                }
            }
            //Navigation dans la barre de selection de piege si elle est active
            if (m_bTrapBarOpen)
            {
                if (Input.GetKeyDown(KeyCode.R) && CanClick())
                {
                    PreviousTrap();
                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                else if (Input.GetKeyDown(KeyCode.F) && CanClick())
                {
                    NextTrap();
                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
            }

        }
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(m_TPSController.device, "LeftBumper", true) && CanClick())
            {
                //ouverture/fermeture de la barre de selection de piege
                if (m_bTrapBarOpen)
                {
                    m_bTrapBarOpen = false;
                    m_TrapsBarUI.SetActive(m_bTrapBarOpen);
                    //  Debug.Log("open");
                }
                else
                {
                    m_bTrapBarOpen = true;
                    m_TrapsBarUI.SetActive(m_bTrapBarOpen);
                    //Info
                    UpdateCapacityInfo();
                }
            }

            //Si la barre des pieges est ouverte
            if (m_bTrapBarOpen)
            {
                Vector2 ToucheDirectionnel = InputManager.Instance.ValuePAD(m_TPSController.device, "Pad");

                // Debug.Log("ToucheDirectionnel" + ToucheDirectionnel);
                if (ToucheDirectionnel.y == 1 && CanClick())
                {
                    // Debug.Log("prev trap");
                    PreviousTrap();
                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                else if (ToucheDirectionnel.y == -1 && CanClick())
                {
                    // Debug.Log("next trap");
                    NextTrap();
                    //Info
                    UpdateCapacityInfo();

                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                // Debug.Log("ToucheDirectionnel" + ToucheDirectionnel);
                if (ToucheDirectionnel.x != 0 && CanClick())
                {
                    //  m_sPlacingtraps.SetTrap(0);
                    m_bTrapBarOpen = false;
                    m_TrapsBarUI.SetActive(m_bTrapBarOpen);
                }
            }
        }


        //m_sPlacingtraps.UpdateControls();
    }
    void UpdateCapacityInvocation()
    {

        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Alpha4) && CanClick())
            {

                //ouverture /fermeture de la barre d'invocation
                if (m_bInvocBarOpen)
                {
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                    m_bInvocBarOpen = false;
                    m_InvocationBarUI.SetActive(m_bInvocBarOpen);
                }
                else
                {
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                    m_bInvocBarOpen = true;
                    m_InvocationBarUI.SetActive(m_bInvocBarOpen);
                    //Info
                    UpdateCapacityInfo();
                }
            }

            //Navigation dans la barre de selection de piege si elle est active
            if (m_bInvocBarOpen)
            {
                // Debug.Log("trap bar ")
                if (Input.GetKeyDown(KeyCode.R) && CanClick())
                {
                    PreviousInvocation();

                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                else if (Input.GetKeyDown(KeyCode.F) && CanClick())
                {
                    NextInvocation();
                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
            }


        }
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(m_TPSController.device, "LeftBumper", true) && CanClick())
            {
                //ouverture /fermeture de la barre d'invocation
                if (m_bInvocBarOpen)
                {
                    m_bInvocBarOpen = false;
                    m_InvocationBarUI.SetActive(m_bInvocBarOpen);
                }
                else
                {
                    m_bInvocBarOpen = true;
                    m_InvocationBarUI.SetActive(m_bInvocBarOpen);
                    //Info
                    UpdateCapacityInfo();
                }
            }

            //Si la barre des pieges est ouverte
            if (m_bInvocBarOpen)
            {
                Vector2 ToucheDirectionnel = InputManager.Instance.ValuePAD(m_TPSController.device, "Pad");

                // Debug.Log("ToucheDirectionnel" + ToucheDirectionnel);
                if (ToucheDirectionnel.y == 1 && CanClick())
                {
                    PreviousInvocation();

                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                else if (ToucheDirectionnel.y == -1 && CanClick())
                {
                    NextInvocation();

                    //Info
                    UpdateCapacityInfo();
                    SoundManager.Instance.CapacityChangePlay(gameObject);
                }
                // Debug.Log("ToucheDirectionnel" + ToucheDirectionnel);
                if (ToucheDirectionnel.x != 0 && CanClick())
                {

                    m_bInvocBarOpen = false;
                    m_InvocationBarUI.SetActive(m_bInvocBarOpen);
                }
            }

        }

      //  m_sSpawnPlayerInvocation.UpdateInvocationCapacity();
    }

    //DEATH
    public void KillPlayer()
    {
        if (m_bDropGemme)
        {
            //Gemmes   
            if (RemoveMoney(m_killValue))
            {
                FindObjectOfType<CollectableManager>().CreateGemme(transform.position, m_killValue);
            }
            else
            {
                FindObjectOfType<CollectableManager>().CreateGemme(transform.position, m_money);
                RemoveMoney(m_money);
            }
        }

        m_health = 0;
        // Debug.Log("Player id : " + m_playerId);
        //Lancement de l'anim de mort
        PlayerAnimatorScript PAS = GetComponentInChildren<PlayerAnimatorScript>();

        CinemachineVirtualCamera[] listVirtualCamera = GetComponentsInChildren<CinemachineVirtualCamera>();

        listVirtualCamera[0].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        listVirtualCamera[1].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        // Reset vibrations
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }

        listVirtualCamera[0].enabled = false;
        listVirtualCamera[1].enabled = false;
        PAS.virtualCameraRagdoll.enabled = true;
        m_TPSController.IsAttacking = false;
        PAS.Kill();

        //Desactivation de l'arme
        GetComponentInChildren<GetHand>().hand.SetActive(false);

        m_TPSController.enabled = false;
        m_scriptCharacterController.enabled = false;
        m_navMeshObstacle.enabled = false;
        m_scriptCapsuleCollider.enabled = false;
        Transform prefabMesh = transform.GetComponentInChildren<Animator>().transform;
        Transform head = prefabMesh.Find("Head");
        Transform body = prefabMesh.Find("Body");
        head.GetComponent<CapsuleCollider>().enabled = false;
        body.GetComponent<CapsuleCollider>().enabled = false;

        Destroy(GetComponent<Rigidbody>());


        // DeathScreen
        gameObject.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(4).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(5).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(6).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(7).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(8).gameObject.SetActive(false);

        gameObject.transform.GetChild(3).transform.GetChild(9).gameObject.SetActive(true);

        // Cas player, invocation or faille
        if (m_killer != null)
        {
            if (m_killer.tag == "Player")
            {
                switch (m_killer.GetComponent<EntityPlayer>().m_sColor)
                {
                    case "Red":
                        // Set Image Killer
                        gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).GetComponent<Image>().sprite =
                            m_KillersSprite[0];
                        break;
                    case "Blue":
                        // Set Image Killer
                        gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).GetComponent<Image>().sprite =
                            m_KillersSprite[1];
                        break;
                    case "Green":
                        // Set Image Killer
                        gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).GetComponent<Image>().sprite =
                            m_KillersSprite[2];
                        break;
                    case "Yellow":
                        // Set Image Killer
                        gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).GetComponent<Image>().sprite =
                            m_KillersSprite[3];
                        break;
                    default:
                        break;
                }
            }
            // Cas faille
            else
            {
                gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        // Cas démons
        else
        {
            gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).GetComponent<Image>().sprite =
                            m_KillersSprite[4];
        }


        m_isDead = true;
    }
    private void Respawn()
    {
        // ScoreBoard
        scoreboardStats.nbDeath[m_sColor] += 1;

        //replacement du prefab player sur son point de spawn
        transform.position = m_transformToSpawn.position;
        transform.rotation = m_transformToSpawn.rotation;

        m_TPSController.rotation.y = transform.eulerAngles.y;
        //Suppression du mesh
        Destroy(GetComponentInChildren<Animator>().gameObject);

        //On recupère le meshPrefab
        GameObject modelChoose = DataManager.Instance.listPrefabModel[DataManager.Instance.listMeshString.IndexOf(DataManager.Instance.m_prefab[m_playerId].meshName)];
        //on l'instantie
        GameObject localModelChoose = Instantiate(modelChoose, transform);
        //changement des layers dans le mesh
        ChangeLayersRecursively(localModelChoose.transform.Find("Group_RIG_CharacterName").GetComponentsInChildren<Transform>(true), "P" + (m_playerId + 1));

        //MODIF BEN

        //on applique le layer au parent du prefab mesh du player
        localModelChoose.gameObject.layer = LayerMask.NameToLayer("P" + (m_playerId + 1));
        //on applique le layer aux Mesh de collisions
        localModelChoose.transform.Find("Head").gameObject.layer = LayerMask.NameToLayer("P" + (m_playerId + 1));
        localModelChoose.transform.Find("Body").gameObject.layer = LayerMask.NameToLayer("P" + (m_playerId + 1));

        ////--///
        //Material
        //
        SkinnedMeshRenderer tempSkin = localModelChoose.GetComponentInChildren<Animator>().transform.Find("LOW_grp4group_GEO_player").GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] mats = tempSkin.materials;
        //Assignation du material
        switch (m_sColor)
        {
            case "Red":

                mats[0] = m_stateGame.m_playerMaterials[0];
                mats[1] = m_stateGame.m_playerMaterials[0];
                mats[2] = m_stateGame.m_playerMaterials[0];
                // Debug.Log("material red");
                break;
            case "Blue":
                mats[0] = m_stateGame.m_playerMaterials[1];
                mats[1] = m_stateGame.m_playerMaterials[1];
                mats[2] = m_stateGame.m_playerMaterials[1];
                // Debug.Log("material blue");
                break;
            case "Green":
                mats[0] = m_stateGame.m_playerMaterials[2];
                mats[1] = m_stateGame.m_playerMaterials[2];
                mats[2] = m_stateGame.m_playerMaterials[2];
                // Debug.Log("material green");
                break;
            case "Yellow":
                mats[0] = m_stateGame.m_playerMaterials[3];
                mats[1] = m_stateGame.m_playerMaterials[3];
                mats[2] = m_stateGame.m_playerMaterials[3];
                //Debug.Log("material yellow");
                break;
            default:
                break;
        }
        tempSkin.materials = mats;


        //Desactivation de l'arme
        //  GetComponentInChildren<GetHand>().hand.SetActive(true);
        m_TPSController.enabled = true;
        m_scriptCharacterController.enabled = true;
        m_scriptCapsuleCollider.enabled = true;

        gameObject.AddComponent(typeof(Rigidbody));
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;


        //Stats Reset
        m_health = m_healthStart;
        m_armor = m_armorStart;

        m_isInHisCamp = true;
        m_UIPlayer.m_needUpdate = true;


        m_weaponBehavior = GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();

        // Reset Camera
        CinemachineVirtualCamera[] listVirtualCamera = GetComponentsInChildren<CinemachineVirtualCamera>();
        listVirtualCamera[0].enabled = true;
        listVirtualCamera[1].enabled = false;
        PlayerAnimatorScript PAS = GetComponentInChildren<PlayerAnimatorScript>();
        PAS.id = m_playerId + 1;
        PAS.virtualCameraRagdoll.enabled = false;


        //Reset de la capacité courrante
        //m_eSelectedCapacity = Capacity.WEAPON1;
        //Stock L'ui de la barre de capacitée
        m_eSelectedCapacity = Capacity.WEAPON1;
        m_sCapacityBarUI.SetCurrentButton((int)m_eSelectedCapacity);

        ////Stock le piege selectionné
        //m_eSelectedTrap = CurrentTrap.RotationTrap;
        ////Stock l'unité selectionné
        //m_eSelectedInvocation = CurrentInvocation.Attaquant;

        //Stock L'ui de la barre des traps
        m_TrapsBarUI.SetActive(false);
        m_bTrapBarOpen = false;
        //Stock L'ui de la barre des invocations
        m_InvocationBarUI.SetActive(false);
        m_bInvocBarOpen = false;


        //RIG
        //Association de la target head sur le mesh
        //localModelChoose.GetComponent<GetHeadPivot>().m_controller.GetComponent<TargetAIMhead>().m_target = GetComponent<GetHeadPivot>().m_controller;
        //Pos of the reference target
        GameObject m_targetInController = GetComponent<GetAimTargetInController>().m_AIMtarget;
        //master target in mesh prefab
        AIMTargetMaster m_targetMasterInMesh = localModelChoose.GetComponent<GetAimTargetMaster>().m_targetMaster;
        //reference it in the mesh prefab
        m_targetMasterInMesh.m_TargetAIMInPrefab = m_targetInController;

        // Reset DeathScreen
        gameObject.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).transform.GetChild(4).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(5).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(6).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(7).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).transform.GetChild(8).gameObject.SetActive(true);

        gameObject.transform.GetChild(3).transform.GetChild(9).gameObject.SetActive(false);
        if (!gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(1).gameObject.activeSelf
            || !gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).gameObject.activeSelf)
        {
            gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).gameObject.SetActive(true);
        }

        // Reset m_killer
        m_killer = null;

        EnterInCamp();
    }
    public static void ChangeLayersRecursively(Transform[] _transform, string name)
    {
        foreach (Transform child in _transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(name);
        }
    }

    //LIFE
    //renvois true si la cible est en vie après un coup
    public bool RemoveLife(int _degats, GameObject _player, GameObject _other)
    {
        if (!m_isInvicible)
        {
            //(Si le joueur n'est pas null et que le joueur n'est pas le protecteur du nexus) ou si le joueur est null
            //Debug.Log("remove life");
            if ((_player != null && _player.GetComponent<EntityPlayer>().m_sColor != m_sColor) || _player == null)
            {
                //Si les degats sont superieur à l'armure
                if (_degats - m_armor > 0)
                {

                    m_health -= (_degats - m_armor);
                    SoundManager.Instance.PlayerHurtPlay(gameObject);
                }
                //Sinon les degats ne sont pas appliqué et on retourne l'état is alive comme tel
                else
                {
                    return !m_isDead;
                }
                //Si la vie est en dessous de 0
                if (m_health <= 0)
                {
                    //Death
                    if (!m_isDead)
                    {
                        m_health = 0;
                        KillPlayer();

                        //Si l'entity qui tape est relié a un player
                        if (_player != null)
                        {
                            if (!m_bDropGemme)
                            {
                                //Gemmes   
                                if (RemoveMoney(m_killValue))
                                {
                                    _player.GetComponent<EntityPlayer>().AddMoney(m_killValue);
                                }
                                else
                                {
                                    _player.GetComponent<EntityPlayer>().AddMoney(m_money);
                                    RemoveMoney(m_money);
                                }
                               
                            }
                            scoreboardStats.nbKillPlayer[_player.GetComponent<EntityPlayer>().m_sColor] += 1;
                        }
                        SoundManager.Instance.RunPlayerStop(gameObject);
                        SoundManager.Instance.PlayerDiePlay(gameObject);
                        SoundManager.Instance.BodyDropPlay(gameObject);

                    }

                }

                // If player
                if (_player != null)
                {
                    m_killer = _player;
                }
                else if (_other != null)
                {
                    m_killer = _other;
                }
                else
                {
                    m_killer = null;
                }

                m_UIPlayer.m_needUpdate = true;
            }
        }

        return !m_isDead;
    }

    //MONEY
    public void AddMoney(int _money)
    {
        if (_money > 0)
        {
            m_money += _money;
        }
        SoundManager.Instance.CoinPickingUpPlay(gameObject);
        m_UIPlayer.m_needUpdate = true;

    }
    //Renvois false si les fonds ne sont pas sufisant ou si on essaye de soustraire une valeur negative
    public bool RemoveMoney(int _money)
    {
        //Si On as les fonds suffisant pour payer
        if (m_money >= _money)
        {
            if (_money > 0)
            {
                m_money -= _money;
            }
            else
            {
                return false;
            }
            m_UIPlayer.m_needUpdate = true;
            return true;
        }
        else
        {
            return false;
        }
    }


    //RULES
    public void EnterInCamp()
    {
        m_isInHisCamp = true;
        //Verifie Si on peut utiliser nos armes dans notre camp
        m_canUseWeappons = m_currentSettings.PlayersCanUseWeapponInCampZone;
        m_canUseTraps = m_currentSettings.PlayersCanUseTrapInCampZone;
        m_canUseInvocations = m_currentSettings.PlayersCanUseInvocationInCampZone;

        m_isDistanceInvicible = m_currentSettings.PlayersCannotBeDistanceHittedInCampZone;
        m_isInvicible = m_currentSettings.PlayersAreInvicibleInCampZone;
    }
    public void GoOutOfCamp()
    {
        m_isInHisCamp = false;
        //Verifie Si on peut utiliser nos armes dans la zone neutre
        m_canUseWeappons = m_currentSettings.PlayersCanUseWeapponInNeutralZone;
        m_canUseTraps = m_currentSettings.PlayersCanUseTrapInNeutralZone;
        m_canUseInvocations = m_currentSettings.PlayersCanUseInvocationInNeutralZone;

        m_isDistanceInvicible = m_currentSettings.PlayersCannotBeDistanceHittedInNeutralZone;
        m_isInvicible = m_currentSettings.PlayersAreInvicibleInNeutralZone;
    }
    public void EnterInEnnemiCamp()
    {
        m_isInEnnemyCamp = true;
        //Verifie Si on peut utiliser nos armes dans la zone neutre
        m_canUseWeappons = m_currentSettings.PlayersCanUseWeapponInEnnemiCampZone;
        m_canUseTraps = m_currentSettings.PlayersCanUseTrapInEnnemiCampZone;
        m_canUseInvocations = m_currentSettings.PlayersCanUseInvocationInEnnemiCampZone;

        m_isDistanceInvicible = m_currentSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone;
        m_isInvicible = m_currentSettings.PlayersAreInvicibleInEnnemiCampZone;
    }
    public void GoOutOfEnnemiCamp()
    {
        m_isInEnnemyCamp = false;
        //Verifie Si on peut utiliser nos armes dans la zone neutre
        m_canUseWeappons = m_currentSettings.PlayersCanUseWeapponInNeutralZone;
        m_canUseTraps = m_currentSettings.PlayersCanUseTrapInNeutralZone;
        m_canUseInvocations = m_currentSettings.PlayersCanUseInvocationInNeutralZone;

        m_isDistanceInvicible = m_currentSettings.PlayersCannotBeDistanceHittedInNeutralZone;
        m_isInvicible = m_currentSettings.PlayersAreInvicibleInEnnemiCampZone;
    }
}
