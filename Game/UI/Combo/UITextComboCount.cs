using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextComboCount : MonoBehaviour
{
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    EntityPlayer m_entityPlayer;
    public Text m_text;

    private int m_playerID;
    private int m_playerCount;



    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;

    public Vector3 m_pos3J1;
    public Vector3 m_pos3J2;
    public Vector3 m_pos3J3;

    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;

    public int m_fontSize;

    void Start()
    {
        m_UIplayer = GetComponentInParent<UIPlayer>();
        m_entityPlayer = m_UIplayer.m_linkedEntityPlayer;
   
        m_playerID = m_entityPlayer.m_playerId;
        m_playerCount = DataManager.Instance.m_prefab.Count;


        m_text.fontSize = m_fontSize / 2;
        switch (m_playerCount)
        {
            case 1:
                m_text.fontSize = m_fontSize;
                GetComponent<RectTransform>().position = m_pos1;
                break;
            case 2:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos2J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos2J2;
                }
                break;
            case 3:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos3J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos3J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos3J3;
                }
                break;
            case 4:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos4J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos4J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos4J3;
                }
                else if (m_playerID == 3)
                {
                    GetComponent<RectTransform>().position = m_pos4J4;
                }
                break;
            default:
                break;
        }


    }

    private void Update()
    {
        // Debug.Log()
        if (m_entityPlayer.gameObject.GetComponentInChildren<GetHand>() != null && m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected != null )
        {
            if (m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected) != null)
            {
                GameObject weappon = m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected).gameObject;
                if (weappon != null)
                {
                    if (weappon.name == "Axe")
                    {
                        m_text.text = weappon.GetComponent<Axe>().ComboStreak.ToString();
                    }
                    else if (weappon.name == "Bow")
                    {
                        m_text.text = weappon.GetComponent<Bow>().ComboStreak.ToString();
                    }
                    else if (weappon.name == "CrossBow")
                    {
                        m_text.text = weappon.GetComponent<CrossBow>().ComboStreak.ToString();
                    }
                    else if (weappon.name == "LaserSword")
                    {
                        m_text.text = weappon.GetComponent<LaserSword>().ComboStreak.ToString();
                    }
                    else if (weappon.name == "Pistol")
                    {
                        m_text.text = weappon.GetComponent<Pistol>().ComboStreak.ToString();
                    }
                    else if (weappon.name == "Sword")
                    {
                        m_text.text = weappon.GetComponent<Sword>().ComboStreak.ToString();
                    }
                }
            }
        }
        //m_text.text = m_linkedWeaponBehavior.

        m_text.fontSize = m_fontSize / 2;
        switch (m_playerCount)
        {
            case 1:
                m_text.fontSize = m_fontSize;
                GetComponent<RectTransform>().position = m_pos1;
                break;
            case 2:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos2J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos2J2;
                }
                break;
            case 3:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos3J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos3J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos3J3;
                }
                break;
            case 4:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos4J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos4J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos4J3;
                }
                else if (m_playerID == 3)
                {
                    GetComponent<RectTransform>().position = m_pos4J4;
                }
                break;
            default:
                break;
        }
    }
}
