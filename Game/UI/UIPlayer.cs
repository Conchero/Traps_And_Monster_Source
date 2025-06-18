using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Defini et met à disposition les coordonées de l'UI du player
public class UIPlayer : MonoBehaviour
{
    //Stock le nombre de joueurs à avoir rejoind la partie
    public int m_playerCount;
    public EntityPlayer m_linkedEntityPlayer;
    private int m_playerID;
    public Nexus m_linkedNexus;
    public SpawnPlayerInvocation m_linkedSpawnPlayerInvocation;
    public Placingtraps m_placingtraps;

    //position et taille de l'ui du player sur l'ecran
    public Rect m_screenSpace;


    public bool m_needUpdate = false;


    void InitForOnePlayer()
    {
        //player 1
        m_screenSpace.x = 0;
        m_screenSpace.y = 0;
        m_screenSpace.width = Screen.width;
        m_screenSpace.height = Screen.height;
    }
    void InitForTwoPlayer()
    {
        //player 1
        if (m_playerID == 0)
        {
            m_screenSpace.x = 0;
            m_screenSpace.y = 0;
            m_screenSpace.width = Screen.width;
            m_screenSpace.height = Screen.height / 2;
        }
        //player 2
        else if (m_playerID == 1)
        {
            m_screenSpace.x = 0;
            m_screenSpace.y = Screen.height / 2;
            m_screenSpace.width = Screen.width;
            m_screenSpace.height = Screen.height / 2;
        }

    }
    void InitForTreeOrFourPlayer()
    {
        //player 1
        if (m_playerID == 0)
        {
            m_screenSpace.x = 0;
            m_screenSpace.y = 0;
            m_screenSpace.width = Screen.width / 2;
            m_screenSpace.height = Screen.height / 2;
        }
        //player 2
        else if (m_playerID == 1)
        {
            m_screenSpace.x = Screen.width / 2;
            m_screenSpace.y = 0;
            m_screenSpace.width = Screen.width / 2;
            m_screenSpace.height = Screen.height / 2;
        }
        //player 3
        else if (m_playerID == 2)
        {
            m_screenSpace.x = 0;
            m_screenSpace.y = Screen.height / 2;
            m_screenSpace.width = Screen.width / 2;
            m_screenSpace.height = Screen.height / 2;
        }
        //player 4
        else if (m_playerID == 3)
        {
            m_screenSpace.x = Screen.width / 2;
            m_screenSpace.y = Screen.height / 2;
            m_screenSpace.width = Screen.width / 2;
            m_screenSpace.height = Screen.height / 2;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        m_playerCount = DataManager.Instance.m_prefab.Count;
//        Debug.Log("m_playerCount = " + m_playerCount);
        m_playerID = m_linkedEntityPlayer.m_playerId;
        m_linkedNexus = m_linkedEntityPlayer.m_linkedNexus;
        
        //Dimensionnement des UI
        switch (m_playerCount)
        {
            case 1:
                InitForOnePlayer();
                break;
            case 2:
                InitForTwoPlayer();
                break;
            case 3:
                InitForTreeOrFourPlayer();
                break;
            case 4:
                InitForTreeOrFourPlayer();
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_needUpdate)
        {
            m_needUpdate = false;
        }
    }
}
