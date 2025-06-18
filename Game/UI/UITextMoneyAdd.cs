using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextMoneyAdd : MonoBehaviour
{

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


    int m_moneyAddCount;
    float m_moneyChangeTimer;
    public float m_moneyChangeTimerMax;
    float m_moneyChangeFadeTimer;
    public float m_moneyChangeFadeTimerMax;
    int m_prevMoneyCount;

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

       

        m_moneyAddCount = 0;
        m_prevMoneyCount = m_entityPlayer.Money;


        //On met l'alpha à 0
        m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 0);
        m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 0);
        // m_text.text = m_entityPlayer.Money.ToString();


        //Mettre dans le start une fois le placement définitif mis en place
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
        //Pop text
        //Si la somme d'argent est supperieur à la precedente
        if (m_entityPlayer.Money > m_prevMoneyCount)
        {
            //On ajoute la diference de valeur à la somme des valeur changé
            m_moneyAddCount += (m_entityPlayer.Money - m_prevMoneyCount);
            //On augmente le timer pendant lequel le text apprait
            m_moneyChangeTimer = m_moneyChangeTimerMax;
            //On passe l'alpha au max
            m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 1f);

            m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 1f);
            //On met à jour le text
            m_text.text = "+" + m_moneyAddCount.ToString();

            m_moneyChangeFadeTimer = 0;

        }
        //On met à jour la valeur precedente d'argent
        m_prevMoneyCount = m_entityPlayer.Money;


        //Si le timer d'apparition n'est pas fini
        if (m_moneyChangeTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_moneyChangeTimer -= Time.deltaTime;


            if (m_moneyChangeTimer <= 0)
            {
                m_moneyChangeFadeTimer = m_moneyChangeFadeTimerMax;
            }
        }
        //Si le timer de fade n'est pas fini
        if (m_moneyChangeFadeTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_moneyChangeFadeTimer -= Time.deltaTime;
            //On addapte l'alpha
            m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
            m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
            if (m_moneyChangeFadeTimer <= 0)
            {
                //On met l'alpha à 0
                m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 0);
                m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 0);
                m_moneyAddCount = 0;
            }
        }



      

    }
}
