using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMTargetMaster : MonoBehaviour
{
    public GameObject m_TargetAIMInPrefab;
   // public GameObject m_TargetAIMBHead;
    public GameObject m_TargetAIM;
   // public GameObject m_TargetAIMCrossBow;
    // Start is called before the first frame update
    void Start()
    {
        //  transform.position = m_TargetAIMInPrefab.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TargetAIM != null && m_TargetAIMInPrefab != null)
        {
            m_TargetAIM.transform.position = m_TargetAIMInPrefab.transform.position;
            //  transform.position = m_TargetAIMInPrefab.transform.position;
        }
    }
}
