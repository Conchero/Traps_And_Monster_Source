using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAvatarAccount : MonoBehaviour
{
    public int m_id;
    public Image m_image;
    public Sprite[] m_sprite;
    public bool m_UINeedUpdate = false;

    // Update is called once per frame
    void Update()
    {
        if (m_UINeedUpdate)
        {
            m_UINeedUpdate = !m_UINeedUpdate;
            UpdateUI();

        }


    }


    void UpdateUI()
    {

        if (FindObjectOfType<StateMenu>().m_playerSColor.Count >  m_id)
        {
            switch (FindObjectOfType<StateMenu>().m_playerSColor[m_id])
            {
                case "Red":
                    m_image.sprite = m_sprite[0];
                    break;
                case "Blue":
                    m_image.sprite = m_sprite[1];
                    break;
                case "Green":
                    m_image.sprite = m_sprite[2];
                    break;
                case "Yellow":
                    m_image.sprite = m_sprite[3];

                    break;
                default:
                    break;
            }
        }
    }
}
