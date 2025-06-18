using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifelinePlayer : MonoBehaviour
{
    //Stock L'ui player 
    public UIPlayer m_UIplayer;
    public UIProgressBar m_progressBar;
    EntityPlayer m_entityPlayer;

    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    private int m_playerID;
    private int m_playerCount;
    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1J1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;


    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;


    int m_maxLife;
    int m_currentLife;
    int m_previousLife;

    //à mettre à true lorsqu'on change la valeur de fillAmount
    public bool m_needUpdate;

    void Start()
    {
        m_entityPlayer = m_UIplayer.m_linkedEntityPlayer;

        //Recupère l' ID
        m_playerID = m_entityPlayer.m_playerId;
        m_playerCount = m_UIplayer.m_playerCount;

        m_maxLife = m_entityPlayer.m_healthMax;
        m_currentLife = m_entityPlayer.m_health;
        m_previousLife = m_currentLife;

        if (m_UIplayer.m_playerCount > 1)
        {
           transform.localScale /= 2;
        }

        m_needUpdate = true;

        ////PosX
        //m_pos2J1.x = m_pos1J1.x / 2;
        //m_pos2J2.x = m_pos1J1.x / 2;
        //m_pos4J1.x = m_pos1J1.x / 2;
        //m_pos4J2.x = m_pos1J1.x / 2 + Screen.width / 2;
        //m_pos4J3.x = m_pos1J1.x / 2;
        //m_pos4J4.x = m_pos1J1.x / 2 + Screen.width / 2;
        ////posY
        //m_pos2J1.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2;
        //m_pos2J2.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2 + Screen.height / 2;
        //m_pos4J1.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2;
        //m_pos4J2.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2 + Screen.height / 2;
        //m_pos4J3.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2;
        //m_pos4J4.y = m_pos1J1.y + (Screen.height - m_pos1J1.y) / 2 + Screen.height / 2;

        //Redimensionnement de la barre de capacité en fonction du nombre de joueur
        //Si on est plus de deux joueurs
        if (m_playerCount > 2)
        {
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                //case 1:
                //    GetComponent<RectTransform>().position = m_pos1;
                //    break;
                case 0:
                    transform.position = m_pos4J1;
                    break;
                case 1:
                    transform.position = m_pos4J2;
                    break;
                case 2:
                    transform.position = m_pos4J3;
                    break;
                case 3:
                    transform.position = m_pos4J4;

                    break;


                default:
                    Debug.Log("error switch");
                    break;
            }



        }
        //Si on est deux joueurs
        else if (m_playerCount == 2)
        {
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                //case 1:
                //    GetComponent<RectTransform>().position = m_pos1;
                //    break;
                case 0:
                    transform.position = m_pos2J1;
                    break;
                case 1:
                    transform.position = m_pos2J2;

                    break;


                default:
                    Debug.Log("error switch");
                    break;
            }

        }

        //Si on est un joueur
        else if (m_playerCount == 1)
        {
            transform.position = m_pos1J1;

        }

    }

    private void Update()
    {
       

        m_currentLife = m_entityPlayer.m_health;
        if (m_currentLife != m_previousLife || m_needUpdate)
        {
            m_needUpdate = false;
            m_progressBar.m_fillAmount = 1f * (float)m_currentLife / (float)m_maxLife;
            m_progressBar.m_needUpdate = true;
        }
        m_previousLife = m_currentLife;


    }

}
