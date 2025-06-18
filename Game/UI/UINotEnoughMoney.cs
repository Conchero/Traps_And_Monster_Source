using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINotEnoughMoney : MonoBehaviour
{
    public UIPlayer m_UIplayer;
    EntityPlayer m_entityPlayer;
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
    // Start is called before the first frame update
    void Start()
    {
        m_entityPlayer = m_UIplayer.m_linkedEntityPlayer;
        m_playerID = m_entityPlayer.m_playerId;

        //recuperation du nombre de players
        m_playerCount = DataManager.Instance.m_prefab.Count;

        if (m_playerCount > 1)
        {
            transform.localScale /= 2;
        }

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
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
