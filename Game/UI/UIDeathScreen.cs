using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeathScreen : MonoBehaviour
{
    [SerializeField] GameObject KillerImg1;
    [SerializeField] GameObject KillerImg2;
    [SerializeField] GameObject Victory_DefeatImg;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject TimeRespawn;
    EntityPlayer m_entityPlayer;
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    int m_playerID;
    int m_playerCount;

    // Start is called before the first frame update
    void Start()
    {
        m_entityPlayer = transform.root.GetComponent<EntityPlayer>();

        m_playerCount = m_UIplayer.m_playerCount;
        //Recupère l' ID
        m_playerID = m_entityPlayer.m_playerId;

        if (m_playerCount == 2)
        {
            if (m_playerID == 0)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(0, 270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(0, 163.1f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(0, 453.7f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(0, 55.5f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(0, -270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(0, -372.3f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(0, -102.6f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(0, -270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(0, -482.9f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
        else if (m_playerCount > 2)
        {
            if (m_playerID == 0)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(-480, 270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(-480, 163.1f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(-480, 453.7f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(-480, 270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(-480, 55.5f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(480, 270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(480, 163.1f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(480, 453.7f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(480, 270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(480, 55.5f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 2)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(-480, -270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(-480, -372.3f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(-480, -102.6f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(-480, -270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(-480, -482.9f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 3)
            {
                KillerImg1.GetComponent<RectTransform>().localPosition = new Vector3(480, -270, 0);
                KillerImg1.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                KillerImg2.GetComponent<RectTransform>().localPosition = new Vector3(480, -372.3f, 0);
                KillerImg2.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                Victory_DefeatImg.GetComponent<RectTransform>().localPosition = new Vector3(480, -102.6f, 0);
                Victory_DefeatImg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                panel.GetComponent<RectTransform>().localPosition = new Vector3(480, -270, 0);
                panel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                TimeRespawn.GetComponent<RectTransform>().localPosition = new Vector3(480, -482.9f, 0);
                TimeRespawn.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
