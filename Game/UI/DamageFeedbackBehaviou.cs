using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackBehaviou : MonoBehaviour
{
    float timerDeath = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce((Vector3.up + Vector3.right)*2,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timerDeath += Time.deltaTime;
        if (timerDeath > 2)
        {
            Destroy(gameObject);
        }
    }
}
