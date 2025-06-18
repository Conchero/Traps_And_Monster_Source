using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSizeManager : MonoBehaviour
{
    [SerializeField] GameObject firstShop;
    [SerializeField] GameObject secondShop;
    [SerializeField] GameObject thirdShop;
    [SerializeField] GameObject fourthShop;

    int nbPlayer = 0; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nbPlayer == 0)
        {
        //    if (GameObject.Find("Player 4") != null)
        //    {
        //        nbPlayer = 4;
        //    }
        //    else if (GameObject.Find("Player 3") != null)
        //    {
        //        nbPlayer = 3;
        //    }
        //    else if (GameObject.Find("Player 2") != null)
        //    {
        //        nbPlayer = 2;
        //    }
        //    else if (GameObject.Find("Player 1") != null)
        //    {
        //        nbPlayer = 1;
        //    }
          nbPlayer = DataManager.Instance.m_prefab.Count;
        }



        if (nbPlayer == 1)
        {
            firstShop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            firstShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            secondShop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            secondShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            thirdShop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            thirdShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            fourthShop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            fourthShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        else if (nbPlayer == 2)
        {
            firstShop.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5035f, 1);
            firstShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 97.5f, 0);

            secondShop.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5035f, 1);
            secondShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -97.5f, 0);

            thirdShop.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5035f, 1);
            thirdShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -97.5f, 0);

            fourthShop.GetComponent<RectTransform>().localScale = new Vector3(1, 0.5035f, 1);
            fourthShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 97.5f, 0);
        }
        else if (nbPlayer == 3)
        {
            firstShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            firstShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(-175, 97.5f, 0);

            secondShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            secondShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(175, 97.5f, 0);

            thirdShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            thirdShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(-175, -97.5f, 0);

            fourthShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            fourthShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(175, -97.5f, 0);
        }
        else if (nbPlayer == 4)
        {
            firstShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            firstShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(-175, 97.5f, 0);

            secondShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            secondShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(175, 97.5f, 0);

            thirdShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            thirdShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(-175, -97.5f, 0);

            fourthShop.GetComponent<RectTransform>().localScale = new Vector3(0.503f, 0.5035f, 1);
            fourthShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(175, -97.5f, 0);
        }


    }
}
