using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITraps : MonoBehaviour
{
    public enum CurrentTrap
    {
        Tournant,
        Buche,
        Mural,
        Plafond
    }

    public EntityPlayer m_entityPlayer;
    public Placingtraps m_placingtraps;
   
    Image m_image;
    public Sprite[] m_sprites;
    CurrentTrap m_currentTrap;
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
        m_currentTrap = (CurrentTrap)m_placingtraps.indexSelected;
        
        m_image = GetComponent<Image>();

        m_UINeedUpdate = true;

        canUse = m_entityPlayer.m_canUseTraps;
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
            UpdateUI();
        }

        canUse = m_entityPlayer.m_canUseTraps;
        if (canUse && ((m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseTrapInWave) || (!m_waveManager.isWaveActive && m_currentSettings.PlayersCanUseTrapOutWave)))
        {
            m_objectCross.SetActive(false);
        }
        else
        {

            m_objectCross.SetActive(true);
        }
        prevCanUse = canUse;
    }

    void UpdateUI()
    {

        m_currentTrap = (CurrentTrap)m_placingtraps.indexSelected;

        switch (m_currentTrap)
        {
            case CurrentTrap.Tournant:
                m_image.sprite = m_sprites[0];
                break;
            case CurrentTrap.Buche:
                m_image.sprite = m_sprites[1];
                break;
            case CurrentTrap.Mural:
                m_image.sprite = m_sprites[2];
                break;
            case CurrentTrap.Plafond:
                m_image.sprite = m_sprites[3];
                break;
            default:
                break;
        }

    }
}
