using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerMessage : MonoBehaviour
{
    public EntityPlayer m_linkendEntityPlayer;
    public int m_id;
    public Image m_image;
    public Sprite[] m_sprite;
    public bool m_UINeedUpdate = false;


    // Update is called once per frame
    void Update()
    {
       // if (m_UINeedUpdate)
       // {
            m_UINeedUpdate = !m_UINeedUpdate;
            UpdateUI();

       // }


    }


    void UpdateUI()
    {
        StateMenu SM = FindObjectOfType<StateMenu>();
        StateGame SG = FindObjectOfType<StateGame>();

        if (SM != null)
        {
            if (SM.m_playerJoin.Count > m_id)
            {
                if (SM.m_playerJoin[m_id].device.name == "Keyboard")
                {
                    m_image.sprite = m_sprite[0];
                }
                if (SM.m_playerJoin[m_id].device.name != "Keyboard")
                {
                    m_image.sprite = m_sprite[1];
                }
            }
        }
        else if (SG != null)
        {
            if (SG.playerGO.Count > m_linkendEntityPlayer.m_playerId)
            {
                if (SG.playerGO[m_linkendEntityPlayer.m_playerId].GetComponent<TpsController>().device == "Keyboard")
                {
                    m_image.sprite = m_sprite[0];
                }
                if (SG.playerGO[m_linkendEntityPlayer.m_playerId].GetComponent<TpsController>().device != "Keyboard")
                {
                    m_image.sprite = m_sprite[1];
                }
            }
        }
    }
}
