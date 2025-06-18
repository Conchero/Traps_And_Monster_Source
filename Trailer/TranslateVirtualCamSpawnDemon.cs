using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateVirtualCamSpawnDemon : MonoBehaviour
{
    public Transform around;
    float timer = 0f;
    public float speed = 0f;
    public float width = 0f;
    public float height = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;

        float x = Mathf.Cos(timer) * width;
        float y = height;
        float z = Mathf.Sin(timer) * width;
        
        transform.position = new Vector3(x + around.position.x, y + around.position.y, z + around.position.z);
    }
}
