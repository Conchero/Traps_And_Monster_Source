using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    //image progressive
    public Image m_image;
    //Variable à utiliser en code pour renseigner l'etat de la barre de vie
    public float m_fillAmount;

    //à mettre à true lorsqu'on change la valeur de fillAmount
    public bool m_needUpdate;

    // Start is called before the first frame update
    void Start()
    {
        m_fillAmount = 1;
        m_needUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_needUpdate == true)
        {
            m_needUpdate = false;
          
            m_image.fillAmount = m_fillAmount;
        }
    }
}
