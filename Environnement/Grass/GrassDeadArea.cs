using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDeadArea : MonoBehaviour
{

    public ShaderCampGround m_linkedShaderCampGround;
    //On doit renseigner le collider associé
    public SphereCollider m_sphereCollider;
    //Récupère la taille de base du collider
    float m_colliderBaseSize;

    public float m_range;

    float m_timerMax;
    float m_timer;

    public bool m_eventDead;

    //Permet d'appliquer le facteur de vitesse pour syncroniser la destruction du terrain avec celui des plantes
    float m_syncfactor;

    // Start is called before the first frame update
    void Start()
    {
        m_range = m_linkedShaderCampGround.m_range;

        m_colliderBaseSize = m_sphereCollider.radius;
        m_syncfactor = m_linkedShaderCampGround.m_syncfactor;
        m_timerMax = m_linkedShaderCampGround.m_timerMax  * m_syncfactor;
        m_timer = 0;
        m_eventDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = 0;
            }

            //on modifie la taille du radius
            m_sphereCollider.radius= m_colliderBaseSize - m_colliderBaseSize * (m_timerMax - m_timer) / m_timerMax;
        }

        if (m_eventDead)
        {
            m_eventDead = false;
            Kill();
        }

    }


    private void OnTriggerExit(Collider other)
    {
        Herbe herbe = other.GetComponent<Herbe>();
        if (herbe != null)
        {
         
                herbe.Kill();

            //  Debug.Log("kill grass");

        }


    }


    public void Kill()
    {
        m_timer = m_timerMax;

    }

}
