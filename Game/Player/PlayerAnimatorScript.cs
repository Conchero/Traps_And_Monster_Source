using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorScript : MonoBehaviour
{
  

    [SerializeField] bool m_eventDead = false;
    [HideInInspector]
    public int id;
    public CinemachineVirtualCamera virtualCameraRagdoll;
    Animator m_animator;
    float IKMultiplicator = 1;
    TpsController controller;

    public bool NeedJump;
    float timeAnim;
    public float JumpForce;
    //UI
    public GameObject m_deathHead;

    GameObject hand;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        virtualCameraRagdoll.gameObject.layer = LayerMask.NameToLayer("P" + id);
        virtualCameraRagdoll.gameObject.GetComponent<CinemachineCollider>().m_CollideAgainst
            = (1 << LayerMask.NameToLayer("Everything"))
            | ~(1 << LayerMask.NameToLayer("Cadavre"));

        controller = GetComponentInParent<TpsController>();
        hand = gameObject.GetComponent<GetHand>().hand.gameObject;
    }
    private void Update()
    {
        if (m_eventDead)
        {
            Kill();
            m_eventDead = false;
        }
        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.ValueJoyStick(controller.device, "RightStick").y > 0.1f && IKMultiplicator < 1.15f)
            {
                IKMultiplicator += 0.005f;
            }
            if (InputManager.Instance.ValueJoyStick(controller.device, "RightStick").y < -0.1f && IKMultiplicator > 0.90f)
            {
                IKMultiplicator -= 0.005f;
            }
            //Debug.Log("Multiplicator: " + IKMultiplicator);
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetAxisRaw("Mouse Y") > 0.1f && IKMultiplicator < 1.15f)
            {
                IKMultiplicator += 0.005f;
            }
            if (Input.GetAxisRaw("Mouse Y") < -0.1f && IKMultiplicator > 0.90f)
            {
                IKMultiplicator -= 0.005f;
            }
            // Debug.Log("Multiplicator: " + IKMultiplicator);
        }
        AnimationMovement();
    }

    void AnimationMovement()
    {
        if (m_animator.GetBool("ChargedAxeAttack") == true)
        {
            timeAnim += Time.deltaTime;
            if (NeedJump == false)
            {
                if (timeAnim >= 0.567)
                {
                    JumpForce = 6.0f;
                    NeedJump = true;
                }
            }
            else
            {
                JumpForce = 0.0f;
            }

            if (timeAnim >= 1.333)
            {
                timeAnim = 0;
                NeedJump = false;
            }
        }
        else if (m_animator.GetBool("SwordAttack4") == true)
        {

            //timeAnim += Time.deltaTime;
            //if (timeAnim >= 0.167 && timeAnim < 0.600f)
            //{
            //    gameObject.transform.Rotate(Vector3.down * Time.deltaTime * 800);
            //}

            //if (timeAnim >= 0.733)
            //{
            //    timeAnim = 0;
            //    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //}

        }
        else if (m_animator.GetBool("SwordCombo") == true)
        {

            timeAnim += Time.deltaTime;
            if (NeedJump == false)
            {
                if (timeAnim >= 0.267)
                {
                    JumpForce = 10.0f;
                    NeedJump = true;
                }
            }
            else
            {
                JumpForce = 0.0f;
            }

            if (timeAnim >= 1.333)
            {
                timeAnim = 0;
                NeedJump = false;
            }

        }
        else
        {

            timeAnim = 0;
            NeedJump = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //if (m_animator.GetBool("BowShoot") == true)
        //{
        //    m_animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + (transform.up) + (transform.right * 0.5f) + (transform.forward));
        //    m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //}

        //if (m_animator.GetBool("ShootLaser") == true)
        //{
        //    m_animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + (transform.up * IKMultiplicator) + (transform.right * 0.4f) + (transform.forward));
        //    m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //}

        if (m_animator.GetBool("CrossbowShoot") == true)
        {
            m_animator.SetIKPosition(AvatarIKGoal.RightHand, hand.transform.position);
            m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        }

        //if (m_animator.GetBool("ChargedBow") == true)
        //{
        //    m_animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + (transform.up * IKMultiplicator * 1.22f) + (transform.right * 0.2f) + (transform.forward));
        //    m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //}


    }


    public void Kill()
    {
        m_animator.enabled = false;
        m_deathHead.SetActive(true);
        foreach (var bone in GetComponentsInChildren<BoneRagdoll>())
        {
            bone.Apply();
        }

        Destroy(this);
    }

    void SuperKill()
    {
        m_animator.enabled = false;


        foreach (var bone in GetComponentsInChildren<BoneRagdoll>())
        {
            bone.Apply();
            bone.GetComponent<Rigidbody>().AddExplosionForce(3000.0f, transform.position + transform.forward + Vector3.up * 0.5f, 5.0f);
        }

        Destroy(this);

    }
}
