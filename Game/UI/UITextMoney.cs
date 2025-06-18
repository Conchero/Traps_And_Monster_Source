using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextMoney : MonoBehaviour
{
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    EntityPlayer m_entityPlayer;
    public TMP_Text m_text;
   // public Text m_text;

    private int m_playerID;
    private int m_playerCount;



    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;

    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;

   // public int m_fontSize;

    void Start()
    {
        m_UIplayer = GetComponentInParent<UIPlayer>();
        m_entityPlayer = m_UIplayer.m_linkedEntityPlayer;

        m_playerID = m_entityPlayer.m_playerId;
       m_playerCount = m_UIplayer.m_playerCount;

        if (m_playerCount > 1)
        {
            m_text.fontSize /= 2;
        }
        m_text.text = m_entityPlayer.Money.ToString();


        //Mettre dans le start une fois le placement définitif mis en place
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
                    GetComponent<RectTransform>().position = m_pos4J1;
                    break;
                case 1:
                    GetComponent<RectTransform>().position = m_pos4J2;
                    break;
                case 2:
                    GetComponent<RectTransform>().position = m_pos4J3;
                    break;
                case 3:
                    GetComponent<RectTransform>().position = m_pos4J4;

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
                    GetComponent<RectTransform>().position = m_pos2J1;
                    break;
                case 1:
                    GetComponent<RectTransform>().position = m_pos2J2;

                    break;


                default:
                    Debug.Log("error switch");
                    break;
            }

        }
        //Si on est un joueur
        else if (m_playerCount == 1)
        {
            GetComponent<RectTransform>().position = m_pos1;

        }

    }

    private void Update()
    {
       
        m_text.text = m_entityPlayer.Money.ToString();

      

    }

}
