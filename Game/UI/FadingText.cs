//fait par Max Bonnaud
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour
{
    public float duration = 1.0f;
    public int fontSize = 12;
    public string text = "none";

    float lifeTime = 0.0f;
    float alpha = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<TextMeshPro>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;

        if(lifeTime >= duration)
        {
            Destroy(gameObject);
        }
        else
        {
            //update alpha based on %time 
            alpha = 1.0f - (lifeTime / duration);
            Color newColor = new Color(1, 1, 1, alpha);  
            gameObject.GetComponent<TextMeshProUGUI>().color = newColor;
        }
    }
}
