using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFicheAccount : MonoBehaviour
{

    public GameObject m_avatar;
    public GameObject m_leaveMessage;
    public GameObject m_optionMessage;

    public bool m_UINeedUpdate;
    // Start is called before the first frame update
    void Start()
    {
        m_UINeedUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_UINeedUpdate)
        {
            m_UINeedUpdate = !m_UINeedUpdate;

            m_avatar.GetComponent<MenuAvatarAccount>().m_UINeedUpdate = true;
            m_leaveMessage.GetComponent<MenuControllerMessage>().m_UINeedUpdate = true;
            m_optionMessage.GetComponent<MenuControllerMessage>().m_UINeedUpdate = true;

        }
    }
}
