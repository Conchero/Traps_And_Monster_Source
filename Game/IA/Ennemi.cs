using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ennemi : MonoBehaviour
{
    StateGame m_stateGame;

    NavMeshAgent m_NMAgent;
    public Nexus recupNexus;
    Vector3 targetNexusPosition;
    public Camp targetTeam;   //ajouté par Max
    //spawnScript parentScript;
    public float nexusMaxMeleeDistance;
    public float stopDistance;

    public GameObject waveManager = null;

    //Stats
    [SerializeField] int m_maxHealth;
    [SerializeField] int m_maxArmor;
    public int m_health;
    public int m_armor;

    //unité ciblée pour le taunt 
    GameObject tempEnnemi = null;
    //objet ciblé par le golem pour destruction imminente 
    GameObject tempObject = null;


    private float m_coolDownHitTimer = 0;
    [SerializeField] string Name = "Dummy";

    //GEMMES
    //determine l'argent qu'on percois lorsqu'on tue cette entitée
    [SerializeField] int m_killValue;
    bool m_bDropGemme;

    public string fromPlayercolor = "aucune";

    /////DEAD
    [SerializeField] bool m_eventDead = false;
    [SerializeField] bool m_eventMegaDead = false;
    public bool m_isDead;
    //Temps laissé au ragdoll pour s'update avant de supprimer l'ennemi
    private float m_deathTimer;
    [SerializeField] float m_deathTimerMax;
    [SerializeField] CapsuleCollider m_headCollider;
    [SerializeField] CapsuleCollider m_bodyCollider;


    float vsUniteRange = 2;

    //pour savoir si lentite est provoquee ou pas
    public bool m_isTaunted = false;

    //determine si l'entite est spawn par une vague ou par un joueur (Modif Axel)
    public bool isPartOfWave;
    //Joueur invocateur
    public EntityPlayer m_entityPlayer;

    //Permet de savoir si la cible est toujours en vie
    bool m_targetIsAlive = true;

    //Animation 
    Animator m_animator = null;


    bool RunEnnemiIsPlaying = false;

    bool isHit = false;

    float timerStop = 0;

    float timeAnim;
    bool playAnim = false;

    bool isSpawning = false;

    int attackNumber;

    int rangeOfAttackAnime;
    //Permet de savoir si els degats on déjà été infligé sur le coup actuel
    bool m_damageApplied;

    //barre de vie
    [SerializeField] Image[] lifeVisual;

    enum EnemyType
    {
        imp,
        golem
    };

    //type de l'unité
    [Tooltip("Type de l'unité")]
    [SerializeField] EnemyType enemyType;

    public int m_damageOnNexus;
    public int m_damageOnCharacters;
    public float m_coolDownHitMax;

    ParticleSystem[] listParticles;


    // Start is called before the first frame update
    void Start()
    {
        m_stateGame = FindObjectOfType<StateGame>();

        m_NMAgent = gameObject.GetComponent<NavMeshAgent>();



        Vector3 destination = targetTeam.m_linkedNexus.transform.position;
        //destination.y = 0.0f;   //make sure target is at ground level

        m_NMAgent.SetDestination(destination);
        m_NMAgent.stoppingDistance = stopDistance;
        targetNexusPosition = destination;

        //Death
        m_deathTimer = 0;

        //Animation 
        m_animator = GetComponentInChildren<Animator>();


        if (enemyType == EnemyType.imp)
        {
            SoundManager.Instance.EnnemiFirePlay(gameObject);
        }

        if (enemyType == EnemyType.golem)
        {
            SoundManager.Instance.GolemFirePlay(gameObject);

            SoundManager.Instance.GolemIdlePlay(gameObject);

        }


        m_health = m_maxHealth;
        m_armor = m_maxArmor;
        if (enemyType == EnemyType.imp)
        {
            //    m_damageOnNexus = 5;
            //    m_coolDownHitMax = 1.2f;
            rangeOfAttackAnime = 4;
            //    m_maxHealth = 10;
            //    m_health = m_maxHealth;
            //    m_armor = 1;
        }
        else if (enemyType == EnemyType.golem)
        {
            //    m_damageOnNexus = 15;
            //    m_coolDownHitMax = 3.5f;
            rangeOfAttackAnime = 3;
            //    m_maxHealth = 20;
            //    m_health = m_maxHealth;
            //    m_armor = 3;
        }
        attackNumber = Random.Range(1, rangeOfAttackAnime);

        // Modif Bastien
        listParticles = GetComponentsInChildren<ParticleSystem>();

        m_damageApplied = false;

        //Money
        m_bDropGemme = GameplaySettings.Instance.m_customSettings.GemmeDrop;


    }

    // Update is called once per frame
    void Update()
    {
        if (tempEnnemi != null)
        {

            if (tempEnnemi.tag == "Player")
            {
                if (tempEnnemi.GetComponent<EntityPlayer>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    m_isTaunted = false;
                    tempEnnemi = null;


                    if (enemyType == EnemyType.imp)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                        m_animator.SetBool("Attack3", false);
                    }
                    else if (enemyType == EnemyType.golem)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                    }
                }
            }
        }
        //Death
        if (m_isDead)
        {
            // transform.Translate(Vector3.up);
            m_deathTimer += Time.deltaTime;
            if (m_deathTimer >= m_deathTimerMax)
            {
                //Kill
                Destroy(this.gameObject);
            }

            foreach (ParticleSystem particule in listParticles)
            {
                ParticleSystem.MainModule main = particule.main;
                main.loop = false;
            }
        }

        if (m_eventDead)
        {
            KillIA();
            m_eventDead = false;
        }
        if (m_eventMegaDead)
        {
            MegaKillIA();
            m_eventMegaDead = false;
        }

        //check if target team is still alive (modif max)
        if (!targetTeam.m_isAlive)
        {
            if (!m_isDead)
            {
                KillIA();
            }
        }

        //UPDATE IN GAME
        if (m_stateGame.m_gameState == GameState.Game)
        {
            //  Debug.Log("has path " + m_NMAgent.isPathStale);


            m_animator.SetFloat("Velocity", Mathf.Sqrt(Mathf.Pow(m_NMAgent.velocity.x, 2) + Mathf.Pow(m_NMAgent.velocity.y, 2)));

            if (Mathf.Sqrt(Mathf.Pow(m_NMAgent.velocity.x, 2) + Mathf.Pow(m_NMAgent.velocity.y, 2)) > 0.1f)
            {
                if (RunEnnemiIsPlaying == false)
                {
                    if (enemyType == EnemyType.imp)
                    {
                        SoundManager.Instance.RunEnnemiPlay(gameObject);
                    }

                    RunEnnemiIsPlaying = true;
                }
            }
            else
            {
                if (enemyType == EnemyType.imp)
                {
                    SoundManager.Instance.RunEnnemiStop(gameObject);
                }

                RunEnnemiIsPlaying = false;
            }

            HandleNexusDamage(/*m_entityPlayer*/);


            if (m_isDead == false)
            {
                if (tempEnnemi == null && m_isTaunted == false)
                {

                    float dist = Vector3.Distance(targetNexusPosition, gameObject.transform.position);
                    if (dist < nexusMaxMeleeDistance)
                    {
                        tempEnnemi = targetTeam.m_linkedNexus;
                    }
                }
            }

            //Colldown Hit
            if (m_coolDownHitTimer > 0)
            {
                m_coolDownHitTimer -= Time.deltaTime;
            }
        }

        else if (m_stateGame.m_gameState == GameState.Scoreboard)
        {
            if (enemyType == EnemyType.imp)
            {
                m_animator.SetFloat("Velocity", 0.0f);
                m_animator.SetBool("Attack1", false);
                m_animator.SetBool("Attack2", false);
                m_animator.SetBool("Attack3", false);
                m_NMAgent.enabled = false;
            }
            //else if (enemyType == EnemyType.golem)
            //{
            //    m_animator.SetFloat("Velocity", 0.0f);
            //    m_animator.SetTrigger("Attack1");
            //    m_animator.SetTrigger("Attack2");
            //    m_NMAgent.enabled = false;
            //}
            //Anim Idle
        }

        //barre de vie
        foreach (Image img in lifeVisual)
        {
            img.fillAmount = (float)m_health / m_maxHealth;
        }


        if (m_isDead == false && m_health > 0)
        {
            if (isHit == true)
            {
                //if (m_isDead == false && m_health > 0)
                //{
                if (m_NMAgent.enabled)
                {
                    m_NMAgent.isStopped = true;
                }
                //  }
                timerStop -= Time.deltaTime;
                if (timerStop <= 0)
                {
                    //if (m_isDead == false && m_health > 0)
                    //{
                    if (m_NMAgent.enabled)
                    {
                        m_NMAgent.isStopped = false;
                    }
                    // }
                }
            }
        }

        if (m_isDead == false)
        {


            if (tempEnnemi != null)
            {
                float attackRange = 0f;
                if (tempEnnemi.tag == "Player" || tempEnnemi.tag == "Invocation")
                {
                    attackRange = vsUniteRange;
                }
                else if (tempEnnemi.tag == "Nexus" || tempEnnemi.tag == "Trap")
                {
                    attackRange = nexusMaxMeleeDistance;
                }
                //Debug.Log("l'attack range est : " +attackRange);
                float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                if (dist > attackRange)
                {
                    m_NMAgent.SetDestination(tempEnnemi.transform.position);
                    m_NMAgent.updateRotation = true;

                }
                else if (dist <= attackRange && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                {

                    m_NMAgent.isStopped = true;

                    lookAt(tempEnnemi.transform.position);

                    //indique qu'il faut aplliquer les degats
                    m_damageApplied = false;
                    if (enemyType == EnemyType.imp)
                    {
                        m_animator.SetBool("Attack" + attackNumber, true);
                        SoundManager.Instance.EnnemiAttack(gameObject);
                        if (attackNumber == 3)
                        {

                        }
                    }
                    else if (enemyType == EnemyType.golem)
                    {
                        m_animator.SetTrigger("Attack" + attackNumber);
                        SoundManager.Instance.GolemAttack(gameObject);

                        // Debug.Log("atatck golem ");
                    }
                    playAnim = true;

                    m_coolDownHitTimer = m_coolDownHitMax;
                }
            }
            else if (tempEnnemi == null)
            {
                m_NMAgent.SetDestination(targetNexusPosition);
            }

        }



        if (m_isDead == false)
        {
            if (playAnim == true && tempEnnemi != null)
            {
                if (enemyType == EnemyType.imp)
                {
                    //Debug.Log("je rentre dans la boucle d'animation de l'attaque");
                    timeAnim += Time.deltaTime;
                    if (attackNumber != 3)
                    {
                        if (timeAnim >= 0.600 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }
                        if (timeAnim >= 0.800)
                        {
                            m_animator.SetBool("Attack" + attackNumber, false);
                            timeAnim = 0;
                            playAnim = false;
                            attackNumber = Random.Range(1, rangeOfAttackAnime);
                        }
                    }

                    if (attackNumber == 3)
                    {
                        if (timeAnim >= 0.600 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }

                        if (timeAnim >= 0.967)
                        {
                            m_animator.SetBool("Attack" + attackNumber, false);
                            timeAnim = 0;
                            playAnim = false;
                            attackNumber = Random.Range(1, rangeOfAttackAnime);
                        }
                    }
                }
                else if (enemyType == EnemyType.golem)
                {
                    timeAnim += Time.deltaTime;

                    if (timeAnim >= 1 && !m_damageApplied)
                    {
                        m_damageApplied = !m_damageApplied;
                        InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                    }

                    if (timeAnim >= 1.5)
                    {
                        // m_animator.SetTrigger("Attack" + attackNumber);
                        timeAnim = 0;
                        playAnim = false;
                        attackNumber = Random.Range(1, rangeOfAttackAnime);
                    }

                }
            }
        }
    }

    void InfligeDamageToTarget(Vector3 _ennemi, Vector3 _target)
    {
        //Debug.Log("je rentre dans le infligedamageToTarget");
        float dist = Vector3.Distance(_ennemi, _target);
        if (playAnim)
        {
            if (tempEnnemi.tag == "Player" && dist < vsUniteRange)
            {
                tempEnnemi.GetComponent<EntityPlayer>().RemoveLife(m_damageOnCharacters, null, null);
                if (tempEnnemi.GetComponent<EntityPlayer>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    m_isTaunted = false;
                    tempEnnemi = null;
                    m_NMAgent.isStopped = false;
                    m_NMAgent.SetDestination(targetNexusPosition);

                    if (enemyType == EnemyType.imp)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                        m_animator.SetBool("Attack3", false);
                    }
                    else if (enemyType == EnemyType.golem)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                    }
                }
            }
            else if (tempEnnemi.tag == "Invocation" && dist < vsUniteRange)
            {
                tempEnnemi.GetComponent<comportementGeneralIA>().RemoveLife(m_damageOnCharacters, null);
                if (tempEnnemi.GetComponent<comportementGeneralIA>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    m_isTaunted = false;
                    tempEnnemi = null;
                    m_NMAgent.isStopped = false;
                    m_NMAgent.SetDestination(targetNexusPosition);
                    if (enemyType == EnemyType.imp)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                        m_animator.SetBool("Attack3", false);
                    }
                    else if (enemyType == EnemyType.golem)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                    }
                }
            }
            else if (tempEnnemi.tag == "Nexus" && dist < nexusMaxMeleeDistance)
            {
                //Debug.Log("je suis dans le infligeDamageToTarget et dans le tag nexus");
                tempEnnemi.GetComponent<Nexus>().RemoveLife(m_damageOnNexus, null);
                SoundManager.Instance.InvocationHitPlay(gameObject);
                SoundManager.Instance.NexusHitPlay(targetTeam.m_linkedNexus);

                if (tempEnnemi.GetComponent<Nexus>().m_eventDead)
                {
                    if (!m_isDead)
                    {
                        KillIA();
                    }
                }
            }
            else if (tempEnnemi.tag == "Trap" && dist < nexusMaxMeleeDistance)
            {
                //enlève la vie au trap 
                tempEnnemi.GetComponentInParent<Traps>().RemoveLife(m_damageOnNexus, null);
                SoundManager.Instance.WeaponWallHitPlay(tempEnnemi);
                if (tempEnnemi.GetComponentInParent<Traps>().isDestroy || tempEnnemi == null)
                {
                    m_NMAgent.updateRotation = true;
                    m_NMAgent.isStopped = false;
                    m_NMAgent.SetDestination(targetNexusPosition);
                    if (enemyType == EnemyType.imp)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                        m_animator.SetBool("Attack3", false);
                    }
                    else if (enemyType == EnemyType.golem)
                    {
                        m_animator.SetBool("Attack1", false);
                        m_animator.SetBool("Attack2", false);
                    }
                }
            }
        }
    }

    public void StopEnemy(float _duration)
    {
        if (m_isDead == false && m_health > 0)
        {
            timerStop = _duration;
            isHit = true;
        }
    }

    void ChangeInvocationTarget()
    {
        m_entityPlayer.GetComponent<SpawnPlayerInvocation>().ChangeTarget(targetTeam);
        targetTeam = m_entityPlayer.GetComponent<SpawnPlayerInvocation>().actualTarget;

        Vector3 destination = targetTeam.m_linkedNexus.transform.position;
        destination.y = 0.0f;   //make sure target is at ground level

        m_NMAgent.SetDestination(destination);
        targetNexusPosition = destination;
    }

    //renvois true si la cible est en vie après un coup
    public bool RemoveLife(int _health, GameObject _player)
    {
        //Verifie que le nombre de dégats - la defense est supperieur à 0 pour le pas donner de vie
        if ((_health - m_armor) > 0)
        {
            m_health -= (_health - m_armor);
            if (enemyType == EnemyType.imp)
            {
                SoundManager.Instance.EnemyHurtPlay(gameObject);
            }

            if (enemyType == EnemyType.golem)
            {
                SoundManager.Instance.GolemHurtPlay(gameObject);
            }
        }
        else
        {
            return !m_isDead;
        }
        if (m_health <= 0)
        {
            m_health = 0;
            SoundManager.Instance.RunEnnemiStop(gameObject);
            SoundManager.Instance.GolemIdleStop(gameObject);
            SoundManager.Instance.EnnemiFireStop(gameObject);
            SoundManager.Instance.EnnemiFireStop(gameObject);
            //Death
            if (!m_isDead)
            {
                KillIA();
                if (!m_bDropGemme)
                {
                    _player.GetComponent<EntityPlayer>().AddMoney(m_killValue);
                }
                if (enemyType == EnemyType.imp)
                {
                    SoundManager.Instance.BodyDropPlay(gameObject);
                    //_player.GetComponent<EntityPlayer>().AddMoney(10);
                }
                if (enemyType == EnemyType.golem)
                {
                    SoundManager.Instance.GolemDeathPlay(gameObject);
                    // _player.GetComponent<EntityPlayer>().AddMoney(50);
                }
                return !m_isDead;
            }
        }
        return !m_isDead;
    }

    public void TauntEnnemi(GameObject _player)
    {
        if (m_isTaunted == false && tempEnnemi == null)
        {
            m_isTaunted = true;
            tempEnnemi = null;
            tempEnnemi = _player;
            lookAt(tempEnnemi.transform.position);
        }
        else if (m_isTaunted == false && tempEnnemi.tag == "Nexus")
        {
            m_isTaunted = true;
            tempEnnemi = null;
            tempEnnemi = _player;
            lookAt(tempEnnemi.transform.position);
        }
        else if (m_isTaunted == false && tempEnnemi.tag == "Trap")
        {
            m_isTaunted = true;
            tempEnnemi = null;
            tempEnnemi = _player;
            lookAt(tempEnnemi.transform.position);
        }
    }


    //Death
    void KillIA()
    {

        //Gemmes   
        if (m_bDropGemme)
        {
            FindObjectOfType<CollectableManager>().CreateGemme(transform.position, m_killValue);
        }


        if (enemyType == EnemyType.imp)
        {
            GetComponentInChildren<InvocationAnimatorScript>().Kill();
        }
        else if (enemyType == EnemyType.golem)
        {
            GetComponentInChildren<GolemAnimatorScript>().Kill();
        }
        //waveManager.GetComponent<WaveManager>().UpdateEnnemyCounter(targetTeam.m_sColor, -1);
        m_NMAgent.enabled = false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        m_headCollider.enabled = false;
        m_bodyCollider.enabled = false;

        m_isDead = true;
    }

    void MegaKillIA()
    {
        if (enemyType == EnemyType.imp)
        {
            GetComponentInChildren<InvocationAnimatorScript>().Kill();
        }
        else if (enemyType == EnemyType.golem)
        {
            GetComponentInChildren<GolemAnimatorScript>().Kill();
        }
        //waveManager.GetComponent<WaveManager>().UpdateEnnemyCounter(targetTeam.m_sColor, -1);
        m_NMAgent.enabled = false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        m_headCollider.enabled = false;
        m_bodyCollider.enabled = false;

        m_isDead = true;
    }

    void HandleNexusDamage(/*EntityPlayer _entityPlayer*/)
    {
        if (!m_isDead && !m_isTaunted)
        {
            if (isSpawning == false)
            {
                StopEnemy(1);
                isSpawning = true;
            }

            float dist = Vector3.Distance(gameObject.transform.position, targetNexusPosition);

            if (dist > nexusMaxMeleeDistance && !m_NMAgent.hasPath)
            {
                if (!m_isTaunted)
                {
                    // Debug.Log("demon run");
                    Vector3 destination = targetTeam.m_linkedNexus.transform.position;
                    m_NMAgent.SetDestination(destination);
                    m_NMAgent.stoppingDistance = stopDistance;
                    // m_NMAgent.isStopped = false;
                }

            }
        }
    }



    public void lookAt(Vector3 _targetToLookAtPosition)
    {
        m_NMAgent.updateRotation = false;
        //Orientation manuel

        Vector3 direction = (_targetToLookAtPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_NMAgent.angularSpeed);
        rotation.x = 0;
        rotation.z = 0;

        transform.rotation = rotation;

    }


    void OnTriggerEnter(Collider other)
    {
        //et que l'entité qui rendre dans le trigger est un piège; 
        if (other.tag == "Trap" && tempEnnemi == null)
        {

            //alors l'ennemi devient le tempObject
            tempEnnemi = other.gameObject;
            //se met à se diriger vers l'objet en question
            m_NMAgent.SetDestination(tempEnnemi.transform.position);

        }
        else if (other.tag == "Nexus" && tempEnnemi == null)
        {
            tempEnnemi = other.gameObject;
            lookAt(tempEnnemi.transform.position);
            m_NMAgent.SetDestination(tempEnnemi.transform.position);
        }
    }



    public int GetHealth()
    {
        return m_health;
    }
}
