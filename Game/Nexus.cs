using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ecris par Ben
public class Nexus : MonoBehaviour
{

    public Camp m_linkedCamp;
    public EntityPlayer m_linkedPlayer;
    public string m_sColor;

    //Stats
    public int m_healthMax;
    public int m_healthStart;
    public int m_health;
    public int m_armor;
    public int m_armorStart;
    public int m_armorMax;

    //money
    bool m_bDropGemme;
    //determine l'argent qu'on percois lorsqu'on tue cette entitée
    public int m_killValue;

    //Death
    private bool m_isDead;
    public float m_deathTimerMax;
    private float m_deathTimer;
    public ShaderCampGround m_linkedShaderCampGround;
    //Anim
    public Animator m_animator;



    //private Material m_material;
    //Stock la couleur initial du material
    public Color m_color;

    SphereCollider sphereCollider;
    MeshCollider[] meshColliders;
    Rigidbody[] rigidbodies;
    ParticleSystem[] particles;
    ParticleSystem particleExplosion;
    ParticleSystem particleLaser;
    ParticleSystem particleTrail1;
    ParticleSystem particleTrail2;

    //Camp camp;
    //Debug
    public bool m_eventDead = false;

    // ScoreBoard
    public Stats scoreboardStats;

    private void Start()
    {
        //Stats 
        m_health = m_healthStart;
        m_armor = m_armorStart;

        m_deathTimer = m_deathTimerMax;
        //m_material = GetComponent<MeshRenderer>().material;
        //m_color = m_material.GetColor("ColorNexus");
        SoundManager.Instance.NexusIdlePlay(gameObject);

        sphereCollider = GetComponent<SphereCollider>();
        meshColliders = GetComponentsInChildren<MeshCollider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        particles = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].name == "Explosion")
            {
                particleExplosion = particles[i];
            }
            else if (particles[i].name == "LaserBeam")
            {
                particleLaser = particles[i];
            }
            else if (particles[i].name == "Trails")
            {
                particleTrail1 = particles[i];
            }
            else if (particles[i].name == "Trails (1)")
            {
                particleTrail2 = particles[i];
            }
        }

        //camp = GetComponentInParent<Camp>();

        //if(camp.m_sColor == "Red")
        //{
        //    m_color = new Color(1, 0, 0, 1);
        //}
        //else if (camp.m_sColor == "Blue")
        //{
        //    m_color = new Color(0, 0, 1, 1);
        //}
        //else if (camp.m_sColor == "Green")
        //{
        //    m_color = new Color(0, 1, 0, 1);
        //}
        //else if (camp.m_sColor == "Yellow")
        //{
        //    m_color = new Color(1, 1, 0, 1);
        //}


        //Money
        m_bDropGemme = GameplaySettings.Instance.m_customSettings.GemmeDrop;


        // ScoreBoard
        scoreboardStats = FindObjectOfType<Stats>();
    }

    float currentNoiseStrength = 0;
    private void Update()
    {
        if (m_isDead)
        {
            //Desactive la collision utilisé pour les degats subis
            sphereCollider.enabled = false;

            //meshColliders[0].enabled = true;
            //meshColliders[1].enabled = true;
            //// Armature 1
            //rigidbodies[1].isKinematic = false;
            //// Armature 2
            //rigidbodies[2].isKinematic = false;

            if (m_deathTimer > 0)
            {

                m_deathTimer -= Time.deltaTime;

                //Si le timer n'est pas fini
                if (m_deathTimer > 0)
                {
                    Color color = m_color;
                    color.r = color.r / m_deathTimerMax * m_deathTimer;
                    color.g = color.g / m_deathTimerMax * m_deathTimer;
                    color.b = color.b / m_deathTimerMax * m_deathTimer;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        ParticleSystem.MainModule module = particles[i].main;
                        module.startColor = color;
                    }


                    //Met a jour le fade des lumière et ==> rajouter le fade des particules dasn lanimator
                    m_animator.SetFloat("Fadding", 1f - 1f / m_deathTimerMax * m_deathTimer);

                }
                //Si le timer se termine
                else
                {
                    Destroy(gameObject);
                }

                //if (m_deathTimer <= 0.0f)
                //{
                //Fin de la décoloration
                //    Destroy(gameObject);
                //}
            }
        }

        if (m_eventDead)
        {
            m_eventDead = !m_eventDead;
            Kill();

        }

        ParticleSystem.MainModule main = particleLaser.main;
        main.startSize = (7.51f / m_healthMax) * m_health;

        ParticleSystem.NoiseModule trail = particleTrail1.noise;
        if (m_health == m_healthMax)
        {
            currentNoiseStrength = 1;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 175)
        {
            currentNoiseStrength = 6;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 150)
        {
            currentNoiseStrength = 12;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 125)
        {
            currentNoiseStrength = 18;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 100)
        {
            currentNoiseStrength = 24;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 75)
        {
            currentNoiseStrength = 30;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 50)
        {
            currentNoiseStrength = 36;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else if (m_health >= 25)
        {
            currentNoiseStrength = 42;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }
        else
        {
            currentNoiseStrength = 48;
            trail.strength = Mathf.Lerp(trail.strength.constant, currentNoiseStrength, Time.deltaTime * 5);
        }

        ParticleSystem.NoiseModule noise = particleLaser.noise;
        noise.strength = (10.7f * m_healthMax) / m_health;
    }

    //renvois true si la cible est en vie après un coup
    public bool RemoveLife(int _degats, EntityPlayer _player)
    {

        if (!m_isDead)
        {
            //(Si le joueur n'est pas null et que le joueur n'est pas le protecteur du nexus) ou si le joueur est null
            //Debug.Log("remove life");
            if ((_player != null && _player.m_sColor != GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Camp>().m_sColor) || _player == null)
            {
                //Activation des particules de hit si le nexus est en vie
                particleExplosion.Play();

                // if (_player.m_playerId != GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Camp>().m_campId)
                //  {
                //Debug.Log("remove life");
                //Si les degats sont superieur à l'armure
                if (_degats - m_armor > 0)
                {
                    m_health -= (_degats - m_armor);
                    //  Debug.Log("Nexus health : "+ health); ;
                }
                //Sinon les degats ne sont pas appliqué et on retourne l'état is alive comme tel
                else
                {
                    return !m_isDead;
                }
                //Si la vie est en dessous de 0
                if (m_health <= 0)
                {

                    //on la bloque à 0
                    m_health = 0;
                    //Si l'entity qui tape est relié a un player
                    if (_player != null)
                    {
                        //on donne l'argent au joueur tueur

                        if (!m_bDropGemme)
                        {
                            _player.AddMoney(m_killValue);
                        }




                        scoreboardStats.nbKillNexus[_player.m_sColor] += 1;
                    }
                    //Le nexus entame sa destruction
                    Kill();


                }

                //Mise à jour de l' affichage ui de la vie du nexus pour tous les player
                for (int i = 0; i < 4; i++)
                {
                    FindObjectOfType<StateGame>().camps[i].m_needToUpdateUiPlayer = true;
                }

                if (_player != null)
                {
                    m_linkedPlayer.m_killer = _player.gameObject;
                }

                // GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Camp>().m_needToUpdateUiPlayer = true;
            }
        }

        return !m_isDead;
    }



    void Kill()
    {

        if (m_bDropGemme)
        {
            //Gemmes   
            FindObjectOfType<CollectableManager>().CreateGemme(transform.parent.position, m_killValue);
        }

        m_deathTimer = m_deathTimerMax;
        m_isDead = true;
        //Gemmes  
        //On demande au camp d'entamer tout ce qui conserne la destruction visuel/ elimination du player ect
        GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Camp>().KillCamp();


        //kill ground
        m_linkedShaderCampGround.Kill();

        //Declenche la destruction physique
        m_animator.SetBool("Kill", true);
        //sound
        SoundManager.Instance.NexusIdleStop(gameObject);
        SoundManager.Instance.NexusDeathPlay(gameObject);
    }
}
