using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINextWave : MonoBehaviour
{
    [SerializeField] GameObject launchWaveText;
    StateGame m_stateGame;
    WaveManager waveManager;
    EntityPlayer m_entityPlayer;
    TpsController m_TPSController;
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    int m_playerID;
    int m_playerCount;

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
    // Start is called before the first frame update
    void Start()
    {
        m_stateGame = FindObjectOfType<StateGame>();
        waveManager = FindObjectOfType<WaveManager>();

        m_entityPlayer = transform.root.GetComponent<EntityPlayer>();
        m_TPSController = transform.root.GetComponent<TpsController>();

        m_playerCount = m_UIplayer.m_playerCount;
        //Recupère l' ID
        m_playerID = m_entityPlayer.m_playerId;

        if (m_playerCount == 2)
        {
            if (m_playerID == 0)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(0, 472, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(0, -92, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
        else if (m_playerCount > 2)
        {
            if (m_playerID == 0)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(-465, 478, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(529, 490, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 2)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(-465, -55, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 3)
            {
                launchWaveText.GetComponent<RectTransform>().localPosition = new Vector3(529, -55, 0);
                launchWaveText.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }
        ReadyLaunchWave();
        if (!waveManager.isWaveActive && m_stateGame.m_gameState == GameState.Game)
        {
            launchWaveText.SetActive(true);
        }
        else
        {
            launchWaveText.SetActive(false);
        }

        if(m_entityPlayer.isReadyForNextWave)
        {
            launchWaveText.GetComponent<Text>().color = Color.red;
        }
        else
        {
            launchWaveText.GetComponent<Text>().color = Color.white;
        }
    }

    void ReadyLaunchWave()
    {
        if(InputManager.Instance.isPressed(m_TPSController.device, "Select", true) && CanClick())
        {
            m_entityPlayer.isReadyForNextWave = !m_entityPlayer.isReadyForNextWave;
        }
    }
}
