using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

//Ecrit par ben
public class Camp : MonoBehaviour
{
    public GameObject m_linkedNexus;
    public bool m_needToDestroyNexus;
    public EntityPlayer m_linkedPlayer;
    public bool m_needToUpdateUiPlayer;
    //permet de prendre en compte ce camp comme appartement à un joueur
    public bool m_isUsed;
    //Permet de savoir si ce camp est en vie
    public bool m_isAlive;

    public GameObject spawnMid;
    public GameObject spawnLeft;
    public GameObject spawnRight;
    public GameObject spawnPlayer;

    public int m_campId;
    public string m_sColor;

    public Sprite spriteDefeat;

    public CinemachineVirtualCamera camVirtualNexus;

    private void Awake()
    {
        m_linkedPlayer = null;
    }
    private void Start()
    {
        m_needToUpdateUiPlayer = false;
    }
    private void Update()
    {
        if (m_needToDestroyNexus == true)
        {
            Destroy(m_linkedNexus);
            SoundManager.Instance.NexusIdleStop(m_linkedNexus);
            m_needToDestroyNexus = false;
        }

        if (m_isUsed && m_isAlive)
        {
            if (m_needToUpdateUiPlayer)
            {
                m_linkedPlayer.m_UIPlayer.m_needUpdate = true;
                m_needToUpdateUiPlayer = false;
            }
        }
    }
    public void KillCamp()
    {
        m_isAlive = false;
		
		m_linkedPlayer.m_campIsAlive = false;
		m_linkedPlayer.KillPlayer();

        // Get sprite Victory or defeat
        m_linkedPlayer.gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(3).gameObject.SetActive(true);
        m_linkedPlayer.gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(3).gameObject.GetComponent<Image>().sprite = spriteDefeat;

        // Timer Respawn text
        m_linkedPlayer.gameObject.transform.GetChild(3).transform.GetChild(9).transform.GetChild(4).gameObject.SetActive(false);

        /// Translate CameraRagdoll to CamNexus
        m_linkedPlayer.gameObject.GetComponentInChildren<PlayerAnimatorScript>().virtualCameraRagdoll.enabled = false;
        //Change Speed Translate
        m_linkedPlayer.gameObject.GetComponentInChildren<CinemachineBrain>().m_DefaultBlend.m_Time = 3.5f;
        camVirtualNexus.enabled = true;
		
        FindObjectOfType<StateGame>().UpdateAliveCamp();
    }


    private void OnTriggerEnter(Collider other)
    {
        EntityPlayer entityPlayer = other.GetComponent<EntityPlayer>();
        if (entityPlayer != null)
        {

            if (entityPlayer.m_sColor != m_sColor)
            {
                // entityPlayer.m_isInEnnemyCamp = true;
                entityPlayer.EnterInEnnemiCamp();
            }
            else
            {
                //entityPlayer.m_isInHisCamp = true;
                entityPlayer.EnterInCamp();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EntityPlayer entityPlayer = other.GetComponent<EntityPlayer>();

        if (entityPlayer != null)
        {
            if (entityPlayer.m_sColor != m_sColor)
            {
                //entityPlayer.m_isInEnnemyCamp = false;
                entityPlayer.GoOutOfEnnemiCamp();
            }

            else
            {
                //entityPlayer.m_isInHisCamp = false;
                entityPlayer.GoOutOfCamp();
            }
        }
    }
}
