using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeappon1 : MonoBehaviour
{
    public EntityPlayer m_entityPlayer;
    Image m_image;
    public Sprite[] m_sprites;
    public bool m_UINeedUpdate;

    //Cross
    bool canUse;
    bool prevCanUse;

    public GameObject m_objectCross;
    //Nombre de player
    int m_playerCount;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();

        m_UINeedUpdate = true;
        canUse = m_entityPlayer.m_canUseWeappons;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_UINeedUpdate)
        {
            //  m_UINeedUpdate = false;
            UpdateUI();
        }

        canUse = m_entityPlayer.m_canUseWeappons;
        if (canUse != prevCanUse)
        {
            if (canUse)
            {
                m_objectCross.SetActive(false);
            }
            else
            {

                m_objectCross.SetActive(true); ;
            }
        }
        prevCanUse = canUse;
    }

    void UpdateUI()
    {
        switch (m_entityPlayer.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().PlayerWeapons[0])
        {
            case "Sword":
                m_image.sprite = m_sprites[0];

                // Debug.Log("sword used");
                break;
            case "Axe":
                m_image.sprite = m_sprites[1];
                break;
            case "Bow":
                m_image.sprite = m_sprites[2];
                break;
            case "LaserSword":
                m_image.sprite = m_sprites[3];
                break;
            case "CrossBow":
                m_image.sprite = m_sprites[4];
                break;
            default:
                break;
        }

    }



}
