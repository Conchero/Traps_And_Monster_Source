using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelectionModelManager : MonoBehaviour
{
    public StateMenu m_stateMenu;
    int m_nbDevice;
    int m_nbAccount;


    public PodiumTransform[] m_podiums;

    //Assignation du material de la couleur de la team
    public Material[] m_playerMaterials = new Material[4];
    public bool m_needUpdate;
    // Start is called before the first frame update
    void Start()
    {
        m_needUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_needUpdate)
        {
            m_needUpdate = !m_needUpdate;

            //Nombres de manettes connecté
            m_nbDevice = DataManager.Instance.m_deviceConnected.Count;
            //nombre de joueurs ajouté à la liste
            m_nbAccount = m_stateMenu.m_playerJoin.Count;

            for (int i = 0; i< m_podiums.Length; i++)
            {
                if (i < m_nbAccount)
                {
                 //   Debug.Log("m_podiums[i].sColor.Length" + m_podiums[i].sColor.Length);
                    if(m_podiums[i].sColor.Length == 0)
                    {
                       // Debug.Log("m_podiums[i].sColor "+ m_podiums[i].sColor);
                        m_podiums[i].needToInstantiate = true;
                        m_podiums[i].sColor = m_stateMenu.m_playerSColor[i];

                    }
                   else if (m_podiums[i].sColor != m_stateMenu.m_playerSColor[i])
                    {
                        m_podiums[i].needToUpdateColor = true;
                        m_podiums[i].sColor = m_stateMenu.m_playerSColor[i];

                    }

                }
                else if (m_podiums[i].sColor.Length != 0)
                {
                    //
                    m_podiums[i].needToDestroy = true;

                }

            }



        }
    }
}
