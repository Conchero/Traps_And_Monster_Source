using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHitScreen : MonoBehaviour
{
    public UIPlayer m_linkedUIPlayer;
    public EntityPlayer m_linkedEntityPlayer;
    public Nexus m_linkedNexus;

    //player
    public Image m_imagePlayerHit;
    float m_playerLifeHitTimer;
    public float m_playerLifeHitTimerMax;
    float m_playerLifeHitFadeTimer;
    public float m_playerLifeHitFadeTimerMax;

    int m_prevPlayerLife;



    //nexus
    public Image m_imageNexusHit;
    float m_nexusLifeHitTimer;
    public float m_nexusLifeHitTimerMax;
    float m_nexusLifeHitFadeTimer;
    public float m_nexusLifeHitFadeTimerMax;
    int m_prevNexusLife;


    // Start is called before the first frame update
    void Start()
    {
        //Position
        RectTransform rectT = GetComponent<RectTransform>();
        rectT.localPosition = new Vector3(
            m_linkedUIPlayer.m_screenSpace.x - Screen.width / 2,
            -m_linkedUIPlayer.m_screenSpace.y + Screen.height / 2,
            0);
        //Scale
        rectT.localScale = new Vector3(m_linkedUIPlayer.m_screenSpace.width / Screen.width, m_linkedUIPlayer.m_screenSpace.height / Screen.height, 1);

        //Nexus link
        m_linkedNexus = m_linkedEntityPlayer.m_linkedNexus;


        //PlayerLife
        m_prevPlayerLife = m_linkedEntityPlayer.m_health;
        //NexusLife
        m_prevNexusLife = m_linkedNexus.m_health;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerHit();
        UpdateNexusHit();
    }


    void UpdatePlayerHit()
    {
        //Pop image
        //Si la somme d'argent est supperieur à la precedente
        if (m_linkedEntityPlayer.m_health < m_prevPlayerLife)
        {
            DrawPlayerHit();

        }
        //On met à jour la valeur precedente de vie
        m_prevPlayerLife = m_linkedEntityPlayer.m_health;


        //Si le timer d'apparition n'est pas fini
        if (m_playerLifeHitTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_playerLifeHitTimer -= Time.deltaTime;


            if (m_playerLifeHitTimer <= 0)
            {
                m_playerLifeHitFadeTimer = m_playerLifeHitFadeTimerMax;
            }
        }
        //Si le timer de fade n'est pas fini
        if (m_playerLifeHitFadeTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_playerLifeHitFadeTimer -= Time.deltaTime;
            //On addapte l'alpha
            m_imagePlayerHit.color = new Color(1, 1, 1, 1f * m_playerLifeHitFadeTimer / m_playerLifeHitFadeTimerMax);
            if (m_playerLifeHitFadeTimer <= 0)
            {
                //On met l'alpha à 0
                m_imagePlayerHit.color = new Color(1, 1, 1, 0);
            }
        }
    }
    void UpdateNexusHit()
    {
        //if (m_linkedNexus == null)
        //{
        //    //Nexus link
        //    m_linkedNexus = m_linkedEntityPlayer.m_linkedNexus;
        //}

        //Pop image
        //Si la somme d'argent est supperieur à la precedente
        if (m_linkedNexus.m_health < m_prevNexusLife)
        {
            DrawNexusHit();

        }
        //On met à jour la valeur precedente de vie
        m_prevNexusLife = m_linkedNexus.m_health;


        //Si le timer d'apparition n'est pas fini
        if (m_nexusLifeHitTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_nexusLifeHitTimer -= Time.deltaTime;


            if (m_nexusLifeHitTimer <= 0)
            {
                m_nexusLifeHitFadeTimer = m_nexusLifeHitFadeTimerMax;
            }
        }
        //Si le timer de fade n'est pas fini
        if (m_nexusLifeHitFadeTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_nexusLifeHitFadeTimer -= Time.deltaTime;
            //On addapte l'alpha
            m_imageNexusHit.color = new Color(1, 1, 1, 1f * m_nexusLifeHitFadeTimer / m_nexusLifeHitFadeTimerMax);
            if (m_nexusLifeHitFadeTimer <= 0)
            {
                //On met l'alpha à 0
                m_imageNexusHit.color = new Color(1, 1, 1, 0);
            }
        }
    }


    void DrawPlayerHit()
    {
        //On augmente le timer pendant lequel le text apprait
        m_playerLifeHitTimer = m_playerLifeHitTimerMax;
        //On passe l'alpha au max
        m_imagePlayerHit.color = new Color(1, 1, 1, 1);

        m_playerLifeHitFadeTimer = 0;
    }

    void DrawNexusHit()
    {
        //On augmente le timer pendant lequel le text apprait
        m_nexusLifeHitTimer = m_nexusLifeHitTimerMax;
        //On passe l'alpha au max
        m_imageNexusHit.color = new Color(1, 1, 1, 1);

        m_nexusLifeHitFadeTimer = 0;
    }
}
