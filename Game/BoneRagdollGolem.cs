using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneRagdollGolem : MonoBehaviour
{
    Vector3 currentVelocity;
    Vector3 lastPosition;


    // Update is called once per frame
    void Update()
    {
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public void Apply()
    {
        //Changement de layer
        //gameObject.layer = LayerMask.NameToLayer("Cadavre");
        Rigidbody rb = GetComponent<Rigidbody>();
        MeshCollider col = GetComponentInChildren<MeshCollider>();
        col.gameObject.layer = LayerMask.NameToLayer("Cadavre");

        rb.isKinematic = false;
        col.enabled = true;
        //rb.velocity = new Vector3(0, 0, 0);
        rb.velocity = currentVelocity;
    }
}
