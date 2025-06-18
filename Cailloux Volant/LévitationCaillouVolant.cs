using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LévitationCaillouVolant : MonoBehaviour
{
    float speed;
    public float height;
    float angle;
    Vector3 pos;

    bool startUp;
    bool startRight;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;

        if(Random.Range(0f, 1f) < 0.5f)
        {
            startUp = true;
        }
        else
        {
            startUp = false;
        }

        if (Random.Range(0f, 1f) < 0.5f)
        {
            startRight = true;
        }
        else
        {
            startRight = false;
        }

        speed = Random.Range(0.8f, 2f);
        angle = Random.Range(0.05f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(startUp)
        {
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
            //set the object's Y to the new calculated Y
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if(startRight)
            {
                //calculate what the new Y rotation will be
                float newRotateY = Mathf.Sin(Time.time * angle);
                //set the object's Y to the new calculated Y
                transform.Rotate(Vector3.up, angle);
            }
            else
            {
                //calculate what the new Y rotation will be
                float newRotateY = Mathf.Sin(Time.time * angle);
                //set the object's Y to the new calculated Y
                transform.Rotate(Vector3.up, -angle);
            }
        }
        else
        {
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * -speed) * height + pos.y;
            //set the object's Y to the new calculated Y
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (startRight)
            {
                //calculate what the new Y rotation will be
                float newRotateY = Mathf.Sin(Time.time * angle);
                //set the object's Y to the new calculated Y
                transform.Rotate(Vector3.up, angle);
            }
            else
            {
                //calculate what the new Y rotation will be
                float newRotateY = Mathf.Sin(Time.time * angle);
                //set the object's Y to the new calculated Y
                transform.Rotate(Vector3.up, -angle);
            }
        }
    }
}
