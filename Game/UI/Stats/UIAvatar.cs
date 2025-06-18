using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatar : MonoBehaviour
{
  //  public EntityPlayer m_linkedEntityPlayer;
    public UIPlayer m_UIPlayer;

    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    private int m_playerID;
    private int m_playerCount;

    

    //texture vide qu'on remplira suivant la couleur de l'equipe
    public Image m_image;
    //table des differentes textures de barres de vie à afficher, à renseigne
    public Sprite[] m_sprites;
    //Scale à appliquer sur la texture pour la redimensionner
    // public Vector2 m_scale;

    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1J1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;


    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;




    void Start()
    {
        //Recupère l' ID
        m_playerID = m_UIPlayer.m_linkedEntityPlayer.m_playerId;
        m_playerCount = m_UIPlayer.m_playerCount;
        //Associe la bonne texture
        switch (m_UIPlayer.m_linkedEntityPlayer.m_sColor)
        {
            case "Red":
                m_image.sprite = m_sprites[0];
                break;
            case "Blue":
                m_image.sprite = m_sprites[1];
                break;
            case "Green":
                m_image.sprite = m_sprites[2];
                break;
            case "Yellow":
                m_image.sprite = m_sprites[3];
                break;

            default:
                break;
        }





        if (m_UIPlayer.m_playerCount > 1)
        {
            m_image.rectTransform.localScale /= 2;
        }


        //Redimensionnement de la barre de capacité en fonction du nombre de joueur
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
        //Si on est un joueur
        else if (m_playerCount == 1)
        {
            GetComponent<RectTransform>().position = m_pos1J1;

        }
    }

    private void Update()
    {
       
    }



}
