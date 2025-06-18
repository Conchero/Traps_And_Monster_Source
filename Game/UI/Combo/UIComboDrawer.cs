using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboDrawer : MonoBehaviour
{
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    EntityPlayer m_entityPlayer;

    public GameObject[] m_objects;
    //permet de savoir si les objets sont actuellement actif
    bool m_isActive;

    // Start is called before the first frame update
    void Start()
    {
        m_entityPlayer = m_UIplayer.m_linkedEntityPlayer;
        m_isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        int comboStreak = 0;
        if (m_entityPlayer.gameObject.GetComponentInChildren<GetHand>() != null && m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected != null)
        {
            if (m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected) != null)
            {
                GameObject weappon = m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_entityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected).gameObject;
                if (weappon != null)
                {
                    if (weappon.name == "Axe")
                    {
                        comboStreak = weappon.GetComponent<Axe>().ComboStreak;
                    }
                    else if (weappon.name == "Bow")
                    {
                        comboStreak = weappon.GetComponent<Bow>().ComboStreak;
                    }
                    else if (weappon.name == "CrossBow")
                    {
                        comboStreak = weappon.GetComponent<CrossBow>().ComboStreak;
                    }
                    else if (weappon.name == "LaserSword")
                    {
                        comboStreak = weappon.GetComponent<LaserSword>().ComboStreak;
                    }
                    else if (weappon.name == "Pistol")
                    {
                        comboStreak = weappon.GetComponent<Pistol>().ComboStreak;
                    }
                    else if (weappon.name == "Sword")
                    {
                        comboStreak = weappon.GetComponent<Sword>().ComboStreak;
                    }
                }
            }
        }

        if (comboStreak == 0)
        {
            if (m_isActive)
            {
                m_isActive = !m_isActive;

                //Desactivation des objets du combo
                for (int i = 0; i < m_objects.Length; i++)
                {
                    m_objects[i].SetActive(false);

                }
            }

        }
        else
        {
            if (!m_isActive)
            {
                m_isActive = !m_isActive;

                //Desactivation des objets du combo
                for (int i = 0; i < m_objects.Length; i++)
                {
                    m_objects[i].SetActive(true);

                }

            }

        }

    }
}
