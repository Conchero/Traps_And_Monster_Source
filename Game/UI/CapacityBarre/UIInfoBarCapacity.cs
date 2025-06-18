using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoBarCapacity : MonoBehaviour
{
    public GameObject m_folderName;
    public GameObject m_folderNameText;

    public GameObject m_folderPrice;
    public GameObject m_folderPriceText;
    public GameObject m_folderPriceLogo;

    public GameObject m_folderDegatsSurVivant;
    public GameObject m_folderDegatsSurVivantText;
    public GameObject m_folderDegatsSurVivantLogo;
    public GameObject m_folderDegatsSurObjets; 
    public GameObject m_folderDegatsSurObjetsText; 
    public GameObject m_folderDegatsSurObjetsLogo; 

    public GameObject m_folderLife;
    public GameObject m_folderLifeText;
    public GameObject m_folderLifeLogo;
    public GameObject m_folderArmor;
    public GameObject m_folderArmorText;
    public GameObject m_folderArmorLogo;


    float m_capacityChangeTimer;
    public float m_capacityChangeTimerMax;
    float m_capacityChangeFadeTimer;
    public float m_capacityChangeFadeTimerMax;

    public CapacityBarUI m_linkedCapacityBar;

    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;


    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;


    public UIPlayer m_UIplayer;
    private int m_playerID;
    private int m_playerCount;

    public void DrawInfo(string _name, int _price, int _DPSVivant, int _DPSObjet, int _life, int _armor)
    {
       


        m_capacityChangeTimer = m_capacityChangeTimerMax;
        // Debug.Log("draw ; m_capacityChangeTimer : " + m_capacityChangeTimer);
        //name
        m_folderName.SetActive(true);
        // Debug.Log("m_folderName set activ ");
        m_folderName.GetComponentInChildren<TMP_Text>().text = _name;

        Color color = new Color(1f, 1f, 1f, 1f);

        //Name
        m_folderNameText.GetComponent<TMP_Text>().color = color;



        if (_price != 0)
        {
            m_folderPrice.SetActive(true);
            m_folderPrice.GetComponentInChildren<TMP_Text>().text = _price.ToString(); 
            //price
            if (_price > m_UIplayer.m_linkedEntityPlayer.Money)
            {
                m_folderPriceText.GetComponent<TMP_Text>().color = new Color(1, 0, 0, 1);
            }
            else
            {
                m_folderPriceText.GetComponent<TMP_Text>().color = new Color(0,1, 0, 1);
            }
            m_folderPriceLogo.GetComponent<Image>().color = color;
        }
        else
        {
            m_folderPrice.SetActive(false);
        }
        if (_DPSVivant != 0)
        {
            m_folderDegatsSurVivant.SetActive(true);
            m_folderDegatsSurVivant.GetComponentInChildren<TMP_Text>().text = _DPSVivant.ToString();
            //DSV
            m_folderDegatsSurVivantText.GetComponent<TMP_Text>().color = color;
            m_folderDegatsSurVivantLogo.GetComponent<Image>().color = color;
        }
        else
        {
            m_folderDegatsSurVivant.SetActive(false);
        }
        if (_DPSObjet != 0)
        {
            m_folderDegatsSurObjets.SetActive(true);
            m_folderDegatsSurObjets.GetComponentInChildren<TMP_Text>().text = _DPSObjet.ToString();
            //DSO
            m_folderDegatsSurObjetsText.GetComponent<TMP_Text>().color = color;
            m_folderDegatsSurObjetsLogo.GetComponent<Image>().color = color;
        }
        else
        {
            m_folderDegatsSurObjets.SetActive(false);
        }
        if (_life != 0)
        {
            m_folderLife.SetActive(true);
            m_folderLife.GetComponentInChildren<TMP_Text>().text = _life.ToString();
            //Life
            m_folderLifeText.GetComponent<TMP_Text>().color = color;
            m_folderLifeLogo.GetComponent<Image>().color = color;
        }
        else
        {

            m_folderLife.SetActive(false);
        }
        if (_armor != 0)
        {
            m_folderArmor.SetActive(true);
            m_folderArmor.GetComponentInChildren<TMP_Text>().text = _armor.ToString();
            //Armor
            m_folderArmorText.GetComponent<TMP_Text>().color = color;
            m_folderArmorLogo.GetComponent<Image>().color = color;
        }
        else
        {
            m_folderArmor.SetActive(false);
        }

       
        //On addapte l'alpha
        //m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
        //m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
       
        
       
      
       
       

    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerCount = DataManager.Instance.m_prefab.Count;
        if (m_playerCount > 1)
        {
           GetComponent<RectTransform>().localScale/= 2;
           // Debug.Log("scale " + GetComponent<RectTransform>().localScale);
        }

        m_playerID = m_UIplayer.m_linkedEntityPlayer.m_playerId;



        //Mettre dans le start une fois le placement définitif mis en place
        //Si on est plus de deux joueurs
        if (m_playerCount > 2)
        {
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                //case 1:
                //    GetComponent<RectTransform>().position = m_pos1;
                //    break;
                case 0:
                    GetComponent<RectTransform>().position = m_pos4J1;
                    break;
                case 1:
                    GetComponent<RectTransform>().position = m_pos4J2;
                    break;
                case 2:
                    GetComponent<RectTransform>().position = m_pos4J3;
                    break;
                case 3:
                    GetComponent<RectTransform>().position = m_pos4J4;

                    break;


                default:
                    Debug.Log("error switch");
                    break;
            }



        }
        //Si on est deux joueurs
        else if (m_playerCount == 2)
        {
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                //case 1:
                //    GetComponent<RectTransform>().position = m_pos1;
                //    break;
                case 0:
                    GetComponent<RectTransform>().position = m_pos2J1;
                    break;
                case 1:
                    GetComponent<RectTransform>().position = m_pos2J2;

                    break;


                default:
                    Debug.Log("error switch");
                    break;
            }

        }
        ////Si on est un joueur
        //else if (m_playerCount == 1)
        //{
        //    GetComponent<RectTransform>().position = m_pos1;

        //}
    }

    // Update is called once per frame
    void Update()
    {

        //Si le timer de fade n'est pas fini
        if (m_capacityChangeFadeTimer > 0)
        {
            //On reduit le timer d'apparition 
            m_capacityChangeFadeTimer -= Time.deltaTime;
            Color color = new Color(1f, 1f, 1f, 1f * m_capacityChangeFadeTimer / m_capacityChangeFadeTimerMax);
            //On addapte l'alpha
            //m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
            //m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 1f * m_moneyChangeFadeTimer / m_moneyChangeFadeTimerMax);
          //Name
            m_folderNameText.GetComponent<TMP_Text>().color = color;
            //price
            m_folderPriceText.GetComponent<TMP_Text>().color = color;
            m_folderPriceLogo.GetComponent<Image>().color = color;
            //DSV
            m_folderDegatsSurVivantText.GetComponent<TMP_Text>().color = color;
            m_folderDegatsSurVivantLogo.GetComponent<Image>().color = color;
            //DSO
            m_folderDegatsSurObjetsText.GetComponent<TMP_Text>().color = color;
            m_folderDegatsSurObjetsLogo.GetComponent<Image>().color = color;
            //Life
            m_folderLifeText.GetComponent<TMP_Text>().color = color;
            m_folderLifeLogo.GetComponent<Image>().color = color;
            //Armor
            m_folderArmorText.GetComponent<TMP_Text>().color = color;
            m_folderArmorLogo.GetComponent<Image>().color = color;



            if (m_capacityChangeFadeTimer <= 0)
            {
                m_folderName.SetActive(false);
                m_folderPrice.SetActive(false);
                m_folderDegatsSurVivant.SetActive(false);

                m_folderDegatsSurObjets.SetActive(false);

                m_folderLife.SetActive(false);

                m_folderArmor.SetActive(false);
            }
            //    //On met l'alpha à 0
            //    m_text.faceColor = new Color(0, m_text.faceColor.g, m_text.faceColor.b, 0);
            //    m_text.outlineColor = new Color(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, 0);
            //    m_moneyAddCount = 0;

        }

        if (m_capacityChangeTimer > 0)
        {

          //  Debug.Log("m_capacityChangeTimer : " + m_capacityChangeTimer);
            m_capacityChangeTimer -= Time.deltaTime;

            if (m_capacityChangeTimer <= 0)
            {
                m_capacityChangeFadeTimer = m_capacityChangeFadeTimerMax;
            }
        }




       

    }
}
