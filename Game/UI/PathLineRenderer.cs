using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathLineRenderer : MonoBehaviour
{
    NavMeshAgent nav = null;
    LineRenderer lr = null;
    public string team;
    float timer = 0;
    float refreshTime = 0.1f;
    public Vector3 standPosition; 

    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        lr = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > refreshTime)
        {
            GetComponent<Transform>().position = standPosition;
            //update line with agent path
            lr.positionCount = nav.path.corners.Length;
            lr.SetPositions(nav.path.corners);
        }



    }
}
