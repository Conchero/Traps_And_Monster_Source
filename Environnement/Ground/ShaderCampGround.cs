using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderCampGround : MonoBehaviour
{
   public Material GroundMaterial;
   public GrassDeadArea m_linkedGrassDeadArea;

    public float m_range;

    public float m_timerMax;
     float m_timer;

    public bool m_eventDead;

    StateGame m_stateGame = null;

    //Permet d'appliquer le facteur de vitesse pour syncroniser la destruction du terrain avec celui des plantes 
    //(mettre une valeur inferieur à 1 pour que l'herbe dépop avant la texture de sol)
    public float m_syncfactor;


    // Start is called before the first frame update
    void Start()
    {
        m_range = 0;
        GroundMaterial.SetFloat("RangeGrass", m_range);
        //Debug.Log("set range");
        m_timer = 0;
        m_eventDead = false;

       // m_stateMenu = FindObjectOfType<StateMenu>();
        m_stateGame = FindObjectOfType<StateGame>();
        if (m_stateGame == null)
        {
            Destroy(this);
        }
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
           // Debug.Log("m_timer = "+  m_timer);


            m_range = 1 * (m_timerMax - m_timer)/ m_timerMax;
           
            GroundMaterial.SetFloat("RangeGrass", m_range);
        }

        if(m_eventDead)
        {
            m_eventDead = false;
            Kill();
            m_linkedGrassDeadArea.Kill();
        }

    }

   public void Kill()
    {
        m_timer = m_timerMax;
        m_linkedGrassDeadArea.Kill();

    }
}
