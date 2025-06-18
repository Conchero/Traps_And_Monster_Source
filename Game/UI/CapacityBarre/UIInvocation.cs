using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInvocation : MonoBehaviour
{
    public enum CurrentInvocation
    {
        Defenseur,
        Attaquant,
        Destructeur,
        Tueur

    }

    public EntityPlayer m_entityPlayer;
    public SpawnPlayerInvocation m_spawnPlayerInvocation;
    Image m_image;
    public Sprite[] m_sprites;
    CurrentInvocation m_currentInvoc;
    public bool m_UINeedUpdate;

    bool canUse;
    bool prevCanUse;

    public GameObject m_objectCross;
    //Nombre de player
    int m_playerCount;

    //Rules
    WaveManager m_waveManager;
    //variable qui va recuperer les reglagles de la partie
    GameplaySettings.Settings m_currentSettings;



    // Start is called before the first frame update
    void Start()
    {
        m_currentInvoc = (CurrentInvocation)m_spawnPlayerInvocation.indexInvoc;
        m_image = GetComponent<Image>();

        m_UINeedUpdate = true;

        canUse = m_entityPlayer.m_canUseInvocations;
        if (canUse)
        {
            m_objectCross.SetActive(false);
        }
        else
        {

            m_objectCross.SetActive(true);
        }
        prevCanUse = canUse;

        //recuperation du nombre de players
        m_playerCount = DataManager.Instance.m_prefab.Count;

        if (m_playerCount > 1)
        {
            m_objectCross.GetComponent<RectTransform>().localScale /= 2;
        }


        //Rules
        m_currentSettings = GameplaySettings.Instance.m_customSettings;
        m_waveManager = FindObjectOfType<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_UINeedUpdate)
        {
            // m_UINeedUpdate = false;
            m_currentInvoc = (CurrentInvocation)m_spawnPlayerInvocation.indexInvoc;

            UpdateUIimage();
            UpdateUIcross();
        }

       
    }

    void UpdateUIimage()
    {

        // m_currentInvoc = (CurrentInvocation)m_spawnPlayerInvocation.indexInvoc;

        switch (m_currentInvoc)
        {
            case CurrentInvocation.Defenseur:
                m_image.sprite = m_sprites[0];
                //if (m_playerCount == 1)
                //{
                //    m_objectCross.SetActive(false);
                //}
                break;
            case CurrentInvocation.Attaquant:
                m_image.sprite = m_sprites[1];
                //if (m_playerCount == 1)
                //{
                //    m_objectCross.SetActive(true);
                //}
                break;
            case CurrentInvocation.Destructeur:
                m_image.sprite = m_sprites[2];
                //if (m_playerCount == 1)
                //{
                //    m_objectCross.SetActive(true);
                //}
                break;
            case CurrentInvocation.Tueur:
                m_image.sprite = m_sprites[3];
                //if (m_playerCount == 1)
                //{
                //    m_objectCross.SetActive(true);
                //}
                break;
            default:
                break;
        }

    }
    void UpdateUIcross()
    {
        canUse = m_entityPlayer.m_canUseInvocations;
        //if (canUse != prevCanUse)
        //{
        if (canUse && ((m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseInvocationInWave) || (!m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseInvocationOutWave)))
        {
            m_objectCross.SetActive(false);
        }
        else
        {

            m_objectCross.SetActive(true);
        }
        //}
        prevCanUse = canUse;

        switch (m_currentInvoc)
        {
      
            case CurrentInvocation.Attaquant:
               // m_image.sprite = m_sprites[1];
                if (m_playerCount == 1)
                {
                    m_objectCross.SetActive(true);
                }
                break;
            case CurrentInvocation.Destructeur:
                //  m_image.sprite = m_sprites[2];
                if (m_playerCount == 1)
                {
                    m_objectCross.SetActive(true);
                }
                break;
            case CurrentInvocation.Tueur:
                // m_image.sprite = m_sprites[3];
                if (m_playerCount == 1)
                {
                    m_objectCross.SetActive(true);
                }
                break;
            default:
                break;
        }

    }
}
