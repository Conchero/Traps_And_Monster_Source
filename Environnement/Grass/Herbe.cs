using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbe : MonoBehaviour
{

    [SerializeField] Vector3 m_wind;
    [SerializeField] float m_ondulationFacteur;
    [SerializeField] Material GrassMaterial;
    public float m_timerMax;
    float m_timer;
    float m_baseScale;

    private void Start()
    {

        //Demo mouvement
        //  m_wind.x = Mathf.Cos(Time.time);
        //m_wind.z = Mathf.Sin(Time.time);
        //set du vent au material
        GrassMaterial.SetVector("Wind", m_wind);
        GrassMaterial.SetFloat("OndulationFacteur", m_ondulationFacteur);
        m_baseScale = transform.localScale.x;

    }

    private void Update()
    {
        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = 0;
                Destroy(gameObject);
                //Debug.Log("destroy herbe");
            }
         

            //on modifie la taille du radius
            transform.localScale = new Vector3(
                m_baseScale - m_baseScale * (m_timerMax - m_timer) / m_timerMax,
                m_baseScale - m_baseScale * (m_timerMax - m_timer) / m_timerMax,
                m_baseScale - m_baseScale * (m_timerMax - m_timer) / m_timerMax
            );
        }

    }

    public void Kill()
    {
        m_timer = m_timerMax;
    }


    //private void Update()
    //{

    //    //Demo mouvement
    //  //  m_wind.x = Mathf.Cos(Time.time);
    //    //m_wind.z = Mathf.Sin(Time.time);
    //    //set du vent au material
    //    GrassMaterial.SetVector("Wind", m_wind);
    //    GrassMaterial.SetFloat("OndulationFacteur", m_ondulationFacteur);

    //}


}
