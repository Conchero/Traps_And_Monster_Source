using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class comportementGeneralIA : MonoBehaviour
{
    StateGame m_stateGame;

    NavMeshAgent m_NMAgent;
    public Nexus recupNexus;
    Vector3 targetNexusPosition;
    public Camp targetTeam;   //ajouté par Max
    //spawnScript parentScript;

    //pour savoir quel est le type de l'unite invoquee
    public string uniteType;
    //pour connaitre de quel joueur vient l'invocation 
    public string fromPlayercolor;
    //pour savoir quand le joueur peut attaquer et quand il ne peut pas
    public bool attackAvailable;
    public attackerStatsLever m_stats;

    //pour savoir si lentite est provoquee ou pas
    public bool m_isTaunted = false;

    public float m_coolDownHitMaxNexus;
    public float m_coolDownHitMaxEnnemis;
    public float m_coolDownHitMaxPlayer;
    public float m_coolDownHitMaxTrap;
    private float m_coolDownHitTimer = 0;
    [SerializeField] string Name = "Dummy";

    //l'ennemi qui sera ciblé par le joueur
    GameObject tempEnnemi;
    GameObject tempObject;

    //different degats selon ce quon tape
    public int m_damageOnCharacters;
    public int m_damageOnObjects;

    public attackerStatsLever asl;
    public defenderStatsLever dfsl;
    public destroyerStatsLever desl;
    public killerStatsLever ksl;


    public int m_maxArmor;
    public int m_armor;

    public int m_maxHealth;
    public int m_health;

    //GEMMES
    public int m_killValue;
    bool m_bDropGemme;

    float nexusMaxMeleeDistance = 3.5f;
    float uniteMaxMeleeDistance = 2f;

    bool RunInvocationIsPlaying = false;

    /////DEAD
    [SerializeField] bool m_eventDead = false;
    [SerializeField] bool m_eventMegaDead = false;
    public bool m_isDead;
    //Temps laissé au ragdoll pour s'update avant de supprimer l'ennemi
    private float m_deathTimer;
    [SerializeField] float m_deathTimerMax;
    [SerializeField] CapsuleCollider m_headCollider;
    [SerializeField] CapsuleCollider m_bodyCollider;

    //determine si l'entite se bat deja
    public bool alreadyFighting = false;
    //stock la position de départ pour le défenseur
    public Vector3 startPosition;
    //determine si l'entite est spawn par une vague ou par un joueur (Modif Axel)
    public bool isPartOfWave;
    //Joueur invocateur
    public EntityPlayer m_entityPlayer;

    //script statistiques
    //Permet de savoir si la cible est toujours en vie
    bool m_targetIsAlive = true;

    //Animation 
    Animator m_animator = null;

    //lifeBar
    [SerializeField] Image[] lifeVisual;

    public int Armor { get => m_armor; set => m_armor = value; }

    bool isHit = false;

    float timerStop = 0;

    float timeAnim;
    bool playAnim;
    //Permet de savoir si les degats on déjà été infligés sur le coup actuel
    bool m_damageApplied;

    // Start is called before the first frame update
    void Start()
    {

        m_stateGame = FindObjectOfType<StateGame>();

        m_NMAgent = gameObject.GetComponent<NavMeshAgent>();

        //  m_health = m_maxHealth;

        //if (uniteType == "defender")
        //{
        //    getDefenderLeverScript();
        //}
        //else if (uniteType == "attacker")
        //{
        //    getAttackerLeverScript();
        //}
        //else if (uniteType == "destroyer")
        //{
        //    getDestroyerLeverScript();
        //}
        //else if (uniteType == "killer")
        //{
        //    getKillerLeverScript();
        //}


        //destination.y = 0.0f;   //make sure target is at ground level

        if (uniteType == "Defender")
        {
            startPosition = transform.position;
            m_NMAgent.SetDestination(startPosition);
        }
        else if (uniteType == "Killer")
        {
            Vector3 destination = targetTeam.m_linkedNexus.transform.position;
            m_NMAgent.SetDestination(targetTeam.m_linkedPlayer.transform.position);
        }
        else if (uniteType == "Attacker" || uniteType == "Destroyer")
        {
            Vector3 destination = targetTeam.m_linkedNexus.transform.position;
            m_NMAgent.SetDestination(destination);
            targetNexusPosition = destination;
        }

        //Death
        m_deathTimer = 0;

        //Animation 
        m_animator = GetComponentInChildren<Animator>();


        m_damageApplied = true;
        playAnim = false;

        //Money
        m_bDropGemme = GameplaySettings.Instance.m_customSettings.GemmeDrop;

    }


    // Update is called once per frame
    void Update()
    {
        if (uniteType == "Defender")
        {
            //Debug.Log(alreadyFighting);
            //DefenderBehavior();
        }
        else if (uniteType == "Destroyer" || uniteType == "Attacker")
        {
            if (tempEnnemi == null && m_isTaunted == false && m_isDead == false)
            {
                float dist = Vector3.Distance(targetNexusPosition, gameObject.transform.position);
                if (dist < nexusMaxMeleeDistance)
                {
                    tempEnnemi = targetTeam.m_linkedNexus;
                }
            }

        }
        else if (uniteType == "Killer" && m_isDead == false)
        {
            m_NMAgent.SetDestination(targetTeam.m_linkedPlayer.transform.position);
        }

        if (m_isDead == false)
        {
            if (tempEnnemi != null)
            {
                //Debug.Log("Voici le tag du tempEnnemi : " + tempEnnemi.tag);
                //Debug.Log("Est ce que l'entité est taunt ?  " + m_isTaunted);
                if (m_isTaunted == true)
                {

                    float attackRange = 0f;
                    if (tempEnnemi.tag == "Player" || tempEnnemi.tag == "Invocation" || tempEnnemi.tag == "Nexus")
                    {
                        attackRange = uniteMaxMeleeDistance;
                    }
                    else if (tempEnnemi.tag == "Nexus")
                    {
                        attackRange = nexusMaxMeleeDistance;
                    }
                    float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                    if (dist > attackRange)
                    {
                        m_NMAgent.SetDestination(tempEnnemi.transform.position);
                        m_NMAgent.updateRotation = true;
                    }
                    if (dist <= attackRange && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                    {

                        m_NMAgent.isStopped = true;
                        //m_animator.SetBool("Attack", true);
                        lookAt(tempEnnemi.transform.position);


                        playAnim = true;
                        //indique qu'il faut aplliquer les degats
                        m_damageApplied = false;

                        m_animator.SetTrigger("Attack");
                        //m_animator.SetBool("Attack", false);

                        m_coolDownHitTimer = m_coolDownHitMaxEnnemis;
                    }
                }
                else if (m_isTaunted == false && uniteType == "Defender")
                {
                    if (tempEnnemi != null)
                    {
                        float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                        if (dist <= uniteMaxMeleeDistance && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                        {

                            m_NMAgent.isStopped = true;
                            //m_animator.SetBool("Attack", true);
                            lookAt(tempEnnemi.transform.position);


                            playAnim = true;
                            //indique qu'il faut aplliquer les degats
                            m_damageApplied = false;

                            m_animator.SetTrigger("Attack");
                            //m_animator.SetBool("Attack", false);

                            m_coolDownHitTimer = m_coolDownHitMaxEnnemis;
                        }
                    }
                }
                else if (m_isTaunted == false && uniteType == "Destroyer" && tempEnnemi.tag == "Trap")
                {
                    if (tempEnnemi != null)
                    {
                        float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                        if (dist <= nexusMaxMeleeDistance && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                        {

                            m_NMAgent.isStopped = true;
                            //m_animator.SetBool("Attack", true);
                            lookAt(tempEnnemi.transform.position);


                            playAnim = true;
                            //indique qu'il faut aplliquer les degats
                            m_damageApplied = false;

                            m_animator.SetTrigger("Attack");
                            //m_animator.SetBool("Attack", false);

                            m_coolDownHitTimer = m_coolDownHitMaxEnnemis;
                        }
                    }
                }
                else if (m_isTaunted == false && tempEnnemi.tag == "Nexus")
                {
                    if (tempEnnemi != null)
                    {
                        float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                        if (dist <= nexusMaxMeleeDistance && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                        {

                            m_NMAgent.isStopped = true;
                            //m_animator.SetBool("Attack", true);
                            lookAt(tempEnnemi.transform.position);


                            playAnim = true;
                            //indique qu'il faut aplliquer les degats
                            m_damageApplied = false;

                            m_animator.SetTrigger("Attack");
                            //m_animator.SetBool("Attack", false);

                            m_coolDownHitTimer = m_coolDownHitMaxEnnemis;
                        }
                    }
                }
                else if (m_isTaunted == false && tempEnnemi.tag == "Player" && uniteType == "Killer")
                {
                    if (tempEnnemi != null)
                    {
                        float dist = Vector3.Distance(tempEnnemi.transform.position, gameObject.transform.position);
                        if (dist <= uniteMaxMeleeDistance && m_coolDownHitTimer <= 0.0f && playAnim == false) //modif Axel
                        {

                            m_NMAgent.isStopped = true;
                            //m_animator.SetBool("Attack", true);
                            lookAt(tempEnnemi.transform.position);


                            playAnim = true;
                            //indique qu'il faut aplliquer les degats
                            m_damageApplied = false;

                            m_animator.SetTrigger("Attack");
                            //m_animator.SetBool("Attack", false);

                            m_coolDownHitTimer = m_coolDownHitMaxEnnemis;
                        }
                    }
                }

            }
        }

        //Death
        if (m_isDead)
        {
            m_deathTimer += Time.deltaTime;
            if (m_deathTimer >= m_deathTimerMax)
            {
                //Kill
                Destroy(this.gameObject);
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


        //UPDATE IN GAME
        if (m_stateGame.m_gameState == GameState.Game)
        {
            m_animator.SetFloat("Velocity", Mathf.Sqrt(Mathf.Pow(m_NMAgent.velocity.x, 2) + Mathf.Pow(m_NMAgent.velocity.y, 2)));

            if (Mathf.Sqrt(Mathf.Pow(m_NMAgent.velocity.x, 2) + Mathf.Pow(m_NMAgent.velocity.y, 2)) > 0.2f)
            {
                if (RunInvocationIsPlaying == false)
                {
                    SoundManager.Instance.InvocationRunPlay(gameObject);
                    RunInvocationIsPlaying = true;
                }
            }
            else
            {
                SoundManager.Instance.InvocationRunStop(gameObject);
                RunInvocationIsPlaying = false;
            }
            HandleNexusDamage(m_entityPlayer);

            //Colldown Hit
            if (m_coolDownHitTimer > 0)
            {
                m_coolDownHitTimer -= Time.deltaTime;

            }


            if (m_isDead == false)
            {
                if (playAnim == true && tempEnnemi != null)
                {
                    timeAnim += Time.deltaTime;
                    //Debug.Log("je rentre dans la boucle d'animation d'attaque");

                    if (uniteType == "Defender")
                    {
                        if (timeAnim >= 0.7 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }
                        else if (timeAnim >= 1.0)
                        {
                            timeAnim = 0;
                            playAnim = false;

                        }
                    }
                    else if (uniteType == "Killer")
                    {
                        if (timeAnim >= 0.700 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }
                        else if (timeAnim >= 1.000)
                        {
                            timeAnim = 0;
                            playAnim = false;

                        }
                    }
                    else if (uniteType == "Attacker")
                    {
                        if (timeAnim >= 0.600 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }
                        else if (timeAnim >= 1.0)
                        {
                            timeAnim = 0;
                            playAnim = false;

                        }

                    }
                    else if (uniteType == "Destroyer")
                    {
                        if (timeAnim >= 0.600 && !m_damageApplied)
                        {
                            m_damageApplied = !m_damageApplied;
                            InfligeDamageToTarget(transform.position, tempEnnemi.transform.position);
                        }
                        else if (timeAnim >= 0.833)
                        {
                            timeAnim = 0;
                            playAnim = false;

                        }



                    }

                }
            }

        }

        else if (m_stateGame.m_gameState == GameState.Scoreboard)
        {
            m_animator.SetFloat("Velocity", 0.0f);
            // m_animator.SetBool("Attack", false);
            m_NMAgent.enabled = false;
            //Anim Idle
        }

        foreach (Image img in lifeVisual)
        {
            img.fillAmount = (float)m_health / m_maxHealth;
        }


        if (m_isDead == false)
        {
            if (isHit == true)
            {
                if (m_isDead == false)
                {
                    m_NMAgent.isStopped = true;
                }
                timerStop -= Time.deltaTime;
                if (timerStop <= 0)
                {
                    if (m_isDead == false)
                    {
                        m_NMAgent.isStopped = false;
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
            if (tempEnnemi.tag == "Player" && dist < uniteMaxMeleeDistance)
            {
                tempEnnemi.GetComponent<EntityPlayer>().RemoveLife(m_damageOnCharacters, m_entityPlayer.gameObject, null);
                if (tempEnnemi.GetComponent<EntityPlayer>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    if (uniteType == "Defender")
                    {
                        tempEnnemi = null;
                        alreadyFighting = false;
                    }
                    else if (uniteType == "Killer")
                    {
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetTeam.m_linkedPlayer.transform.position);
                    }
                    else
                    {
                        m_isTaunted = false;
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetNexusPosition);
                    }
                }
            }
            else if (tempEnnemi.tag == "Invocation" && dist < uniteMaxMeleeDistance)
            {
                SoundManager.Instance.InvocationHitPlay(gameObject);
                tempEnnemi.GetComponent<comportementGeneralIA>().RemoveLife(m_damageOnCharacters, m_entityPlayer.gameObject);
                if (tempEnnemi.GetComponent<comportementGeneralIA>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    if (uniteType == "Defender")
                    {
                        tempEnnemi = null;
                        alreadyFighting = false;
                    }
                    else if (uniteType == "Killer")
                    {
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetTeam.m_linkedPlayer.transform.position);
                    }
                    else
                    {
                        m_isTaunted = false;
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetNexusPosition);
                    }
                }
            }
            else if (tempEnnemi.tag == "Ennemi" && dist < uniteMaxMeleeDistance)
            {
                tempEnnemi.GetComponent<Ennemi>().RemoveLife(m_damageOnCharacters, m_entityPlayer.gameObject);
                if (tempEnnemi.GetComponent<Ennemi>().m_isDead)
                {
                    m_NMAgent.updateRotation = true;
                    if (uniteType == "Defender")
                    {
                        tempEnnemi = null;
                        alreadyFighting = false;
                    }
                    else if (uniteType == "Killer")
                    {
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetTeam.m_linkedPlayer.transform.position);
                    }
                    else
                    {
                        m_isTaunted = false;
                        tempEnnemi = null;
                        m_NMAgent.SetDestination(targetNexusPosition);
                    }

                }
            }
            else if (tempEnnemi.tag == "Nexus" && dist < nexusMaxMeleeDistance)
            {
                //Debug.Log("je suis dans le infligeDamageToTarget et dans le tag nexus");
                tempEnnemi.GetComponent<Nexus>().RemoveLife(m_damageOnObjects, m_entityPlayer);
                SoundManager.Instance.InvocationHitPlay(gameObject);
                SoundManager.Instance.NexusHitPlay(targetTeam.m_linkedNexus);

                if (tempEnnemi.GetComponent<Nexus>().m_eventDead)
                {
                    ChangeInvocationTarget();
                    if (isHit == false)
                    {
                        m_NMAgent.isStopped = false;
                    }

                }
            }
            else if (tempEnnemi.tag == "Trap" && dist < nexusMaxMeleeDistance)
            {

                //enlève la vie au trap 
                tempEnnemi.GetComponentInParent<Traps>().RemoveLife(m_damageOnObjects, m_entityPlayer.gameObject);
                SoundManager.Instance.WeaponWallHitPlay(tempEnnemi);
                if (tempEnnemi.GetComponentInParent<Traps>().isDestroy || tempEnnemi == null)
                {
                    m_NMAgent.updateRotation = true;
                    m_NMAgent.SetDestination(targetNexusPosition);
                }
            }
        }
    }


    void InfligeDamageToTarget()
    {
        //verifie la distance avec la target
        {
            //si  elle est bonne on inflige les degats à la target
        }
    }

    public void StopInvoc(float _duration)
    {
        if (m_isDead == false)
        {
            timerStop = _duration;
            isHit = true;
        }
    }


    void ChangeInvocationTarget()
    {
        m_entityPlayer.GetComponent<SpawnPlayerInvocation>().ChangeTarget(targetTeam);
        targetTeam = m_entityPlayer.GetComponent<SpawnPlayerInvocation>().actualTarget;

        if (uniteType != "Defender")
        {
            Vector3 destination = targetTeam.m_linkedNexus.transform.position;
            destination.y = 0.0f;   //make sure target is at ground level

            m_NMAgent.SetDestination(destination);
            targetNexusPosition = destination;
        }

    }


    //renvois true si la cible est en vie après un coup
    public bool RemoveLife(int _health, GameObject _player)
    {
        //Verifie que le nombre de dégats - la defense est supperieur à 0 pour le pas donner de vie
        if ((_health - m_armor) > 0)
        {
            m_health -= (_health - m_armor);
            SoundManager.Instance.InvocationHurtPlay(gameObject);
        }
        else
        {
            return !m_isDead;
        }
        if (m_health <= 0)
        {
            m_health = 0;
            //Death
            if (!m_isDead)
            {
                KillIA();

                if (!m_bDropGemme)
                {
                    _player.GetComponent<EntityPlayer>().AddMoney(m_killValue);
                }

                // _player.GetComponent<EntityPlayer>().AddMoney(m_killValue);

                SoundManager.Instance.BodyDropPlay(gameObject);
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
    }

    //Death
    void KillIA()
    {
        //Gemmes   
        if (m_bDropGemme)
        {
            FindObjectOfType<CollectableManager>().CreateGemme(transform.position, m_killValue);
        }


        InvocationAnimatorScript IAS = GetComponentInChildren<InvocationAnimatorScript>();
        //Lancement de l'anim de mort
        IAS.Kill();

        m_NMAgent.enabled = false;
        m_isTaunted = false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        m_headCollider.enabled = false;
        m_bodyCollider.enabled = false;

        m_isDead = true;

    }


    void MegaKillIA()
    {
        InvocationAnimatorScript IAS = GetComponentInChildren<InvocationAnimatorScript>();
        //Lancement de l'anim de mort
        IAS.Kill();
        m_NMAgent.enabled = false;
        m_isTaunted = false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        m_headCollider.enabled = false;
        m_bodyCollider.enabled = false;
        m_isDead = true;
    }


    void HandleNexusDamage(EntityPlayer _entityPlayer)
    {
        if (!m_isDead && !m_isTaunted)
        {

            float dist = Vector3.Distance(gameObject.transform.position, targetNexusPosition);

            if (!m_targetIsAlive)
            {
                ChangeInvocationTarget();
                m_targetIsAlive = true;
                if (isHit == false)
                {
                    m_NMAgent.isStopped = false;
                }
            }


            //  m_animator.SetBool("Attack", true);


            else if (dist > nexusMaxMeleeDistance)
            {
                ////ORIENTATION DE L'IA
                //active l'orientation vers le path 
                m_NMAgent.updateRotation = true;


                m_NMAgent.isStopped = false;

                // m_animator.SetBool("Attack", false);
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
        if (transform.rotation != rotation)
        {
            transform.rotation = rotation;
        }
    }

    void getAttackerLeverScript()
    {
        //chope le levier de l'attaquant et l'applique aux stats
        if (gameObject.GetComponent<attackerStatsLever>() != null)
        {
            asl = gameObject.GetComponent<attackerStatsLever>();
        }
        else
        {
            gameObject.AddComponent<attackerStatsLever>();
        }
        m_maxHealth = asl.attackerHealth;
        m_health = m_maxHealth;
        m_armor = asl.attackerArmor;
        nexusMaxMeleeDistance = asl.attackerNexusMeleeDist;
    }


    void getDefenderLeverScript()
    {
        //chope le levier du défenseur et l'applique aux stats
        if (gameObject.GetComponent<defenderStatsLever>() != null)
        {
            dfsl = gameObject.GetComponent<defenderStatsLever>();
        }
        else
        {
            gameObject.AddComponent<defenderStatsLever>();
        }
        m_maxHealth = dfsl.defenderHealth;
        m_health = m_maxHealth;
        m_armor = dfsl.defenderArmor;
        nexusMaxMeleeDistance = dfsl.defenderNexusMeleeDist;
    }


    void getDestroyerLeverScript()
    {
        //chope le levier du destructeur et l'applique aux stats
        if (gameObject.GetComponent<destroyerStatsLever>() != null)
        {
            desl = gameObject.GetComponent<destroyerStatsLever>();
        }
        else
        {
            gameObject.AddComponent<destroyerStatsLever>();
        }
        m_maxHealth = desl.destroyerHealth;
        m_health = m_maxHealth;
        m_armor = desl.destroyerArmor;
        nexusMaxMeleeDistance = desl.destroyerNexusMeleeDist;
    }


    void getKillerLeverScript()
    {
        //chope le levier du tueur et l'applique aux stats
        if (gameObject.GetComponent<killerStatsLever>() != null)
        {
            ksl = gameObject.GetComponent<killerStatsLever>();
        }
        else
        {
            gameObject.AddComponent<killerStatsLever>();
        }
        m_maxHealth = ksl.killerHealth;
        m_health = m_maxHealth;
        m_armor = ksl.killerArmor;
        nexusMaxMeleeDistance = ksl.killerNexusMeleeDist;
    }



    void OnTriggerEnter(Collider other)
    {
        //si l'unité est un défenseur
        if (uniteType == "Defender")
        {
            //si l'entité qui rentre dans le trigger est une invocation, et a une couleur differente de celle du défenseur
            if (other.tag == "Invocation" && fromPlayercolor != other.GetComponent<comportementGeneralIA>().fromPlayercolor)
            {

                //et que le défenseur ne se bat pas déjà
                if (alreadyFighting == false)
                {
                    //alors l'ennemi devient le tempEnnemi
                    tempEnnemi = other.gameObject;
                    lookAt(tempEnnemi.transform.position);
                    if (tempEnnemi.GetComponent<comportementGeneralIA>().m_isTaunted == false)
                    {
                        tempEnnemi.GetComponent<comportementGeneralIA>().TauntEnnemi(gameObject);
                    }

                    //Debug.Log(tempEnnemi.GetComponent<comportementGeneralIA>().m_isTaunted);
                    //le defenseur passe en already fighting true
                    alreadyFighting = true;
                }
            }
            //si l'entité qui entre dans le trigger est un ennemi, et a une couleur différente de celle du défenseur
            if (other.tag == "Ennemi" && fromPlayercolor != other.GetComponent<Ennemi>().fromPlayercolor)
            {
                //et que le défenseur ne se bat pas déjà
                if (alreadyFighting == false)
                {
                    //alors l'ennemi devient le tempEnnemi
                    tempEnnemi = other.gameObject;
                    lookAt(tempEnnemi.transform.position);
                    if (tempEnnemi.GetComponent<Ennemi>().m_isTaunted == false)
                    {
                        tempEnnemi.GetComponent<Ennemi>().TauntEnnemi(gameObject);
                    }

                    //le defenseur passe en already fighting true
                    alreadyFighting = true;
                }

            }
            //si l'entité qui entre dans le trigger est un joueur, et a une couleur différente de celle du défenseur
            if (other.tag == "Player" && fromPlayercolor != other.GetComponent<EntityPlayer>().m_sColor)
            {
                //et que le défenseur ne se bat pas déjà
                if (alreadyFighting == false)
                {
                    //alors l'ennemi devient le tempEnnemi
                    tempEnnemi = other.gameObject;
                    lookAt(tempEnnemi.transform.position);
                    //le defenseur passe en already fighting true
                    alreadyFighting = true;
                }

            }
        }

        //si l'unité est du type destructeur
        if (uniteType == "Destroyer")
        {
            //et que l'entité qui rendre dans le trigger est un piège; et qu'il a un navmeshobstacle actif
            if (other.tag == "Trap" && tempEnnemi == null)
            {
                //alors l'ennemi devient le tempObject
                tempEnnemi = other.gameObject;
                lookAt(tempEnnemi.transform.position);
                //se met à se diriger vers l'objet en question
                m_NMAgent.SetDestination(tempEnnemi.transform.position);

            }
        }
        if (uniteType == "Killer")
        {
            if (other.tag == "Player" && fromPlayercolor != other.GetComponent<EntityPlayer>().m_sColor)
            {
                //et que le défenseur ne se bat pas déjà

                //alors l'ennemi devient le tempEnnemi
                tempEnnemi = other.gameObject;
                lookAt(tempEnnemi.transform.position);
                //le defenseur passe en already fighting true
            }
        }

        if (other.tag == "Nexus")
        {
            tempEnnemi = other.gameObject;
            lookAt(tempEnnemi.transform.position);
            m_NMAgent.SetDestination(tempEnnemi.transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //si l'unité est du type et que l'ennemi temporaire n'est pas null
        if (uniteType == "Defender" && tempEnnemi != null)
        {
            //if (other.tag == "Invocation")
            //{
            //    if (other.GetComponent<comportementGeneralIA>().m_isTaunted == true)
            //    {
            //        Debug.Log("une invoc sort");
            //        m_NMAgent.SetDestination(startPosition);
            //        alreadyFighting = false;
            //        tempEnnemi = null;
            //    }
            //}
            //et que le tag de l'unité sortant est comme le tag de l'unité temporaire, cad joueur, et que le défenseur se bat déjà 
            if (other.tag == "Player" && tempEnnemi.tag == "Player" && alreadyFighting)
            {
                //alors, l'unité ne se bat plus, on passe le alreadyfighting en false
                alreadyFighting = false;
                //et l'ennemi temporaire devient null
                tempEnnemi = null;
            }
            else if (other.tag == "Ennemi" && tempEnnemi.tag == "Ennemi" && alreadyFighting)
            {
                alreadyFighting = false;
                //et l'ennemi temporaire devient null
                tempEnnemi = null;
            }
        }
    }
}
