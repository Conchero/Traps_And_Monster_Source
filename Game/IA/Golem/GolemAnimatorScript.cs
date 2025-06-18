using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimatorScript : MonoBehaviour
{
    public bool m_eventDead = false;
    public Animator m_animator;
    ParticleSystem[] listParticles;

    // Start is called before the first frame update
    void Start()
    {
        // Modif Bastien
        listParticles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_eventDead)
        {
            Kill();
            m_eventDead = false;
            foreach (ParticleSystem particule in listParticles)
            {
                ParticleSystem.MainModule main = particule.main;
                main.loop = false;
            }
        }
    }

    public void Kill()
    {
            m_animator.enabled = false;
            foreach (var bone in GetComponentsInChildren<BoneRagdollGolem>())
            {
                bone.Apply();
            }

            Destroy(this);
        
    }
}
