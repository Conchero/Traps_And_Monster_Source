using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private float speed = 50f;
    private float lifeTimer = 0f;
    private float lifeTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * speed * Time.deltaTime, out hit))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
