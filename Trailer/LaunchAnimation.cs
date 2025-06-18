using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAnimation : MonoBehaviour
{
    public Animator[] tabAnimator;
    public KeyCode[] tabKeyCode;
    public TrailerIaFollow[] m_tabTrailerIaFollow;

    // Start is called before the first frame update
    void Start()
    {
        //launchTouchA.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tabKeyCode.Length; i++)
        {
            if (Input.GetKey(tabKeyCode[i]))
            {
                tabAnimator[i].Rebind();

              
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int k = 0; k < m_tabTrailerIaFollow.Length; k++)
            {
                m_tabTrailerIaFollow[k].StartIASpawn();
            }
        }
    }
}
