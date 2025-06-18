using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvocationAnimatorScript : MonoBehaviour
{

    NavMeshAgent m_agent;
    [SerializeField] bool m_eventDead = false;
    Animator m_animator;

    [SerializeField] GameObject Fireball;
    public bool fireballPlay = false;

    //Assignation du material de la couleur de la team
    public Material[] m_materials = new Material[4];
    float timeAnim;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_agent = GetComponentInParent<NavMeshAgent>();


    }
    private void Update()
    {
        if (m_eventDead)
        {
            Kill();
            m_eventDead = false;
        }


        if (gameObject.transform.parent.name == "Imp(Clone)")
        {
            if (m_animator.GetBool("Attack3") == true)
            {
                timeAnim += Time.deltaTime;
                if (timeAnim >= 0.567 && fireballPlay == false)
                {
                    ParticleSystem[] particles = Fireball.GetComponentsInChildren<ParticleSystem>();
                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].Play();
                    }
                    SoundManager.Instance.EnnemiFireballPlay(gameObject);
                    fireballPlay = true;
                }

                if (timeAnim >= 0.967)
                {
                    timeAnim = 0;
                    fireballPlay = false;
                }
            }
         

        }

    }
    private void OnAnimatorIK(int layerIndex)
    {
        // m_animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + Vector3.up * 17.0f + transform.right * 100.0f);
        //m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //  m_animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(transform.forward));
        //  m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.9f);
    }


    public void Kill()
    {
        m_animator.enabled = false;
        //m_agent.enabled = false;


        foreach (var bone in GetComponentsInChildren<BoneRagdoll>())
        {
            bone.Apply();
        }

        Destroy(this);
    }


    public void SuperKill()
    {
        m_animator.enabled = false;
        m_agent.enabled = false;


        foreach (var bone in GetComponentsInChildren<BoneRagdoll>())
        {
            bone.Apply();
            bone.GetComponent<Rigidbody>().AddExplosionForce(3000.0f, transform.position + transform.forward + Vector3.up * 0.5f, 5.0f);
        }

        Destroy(this);

    }
}
