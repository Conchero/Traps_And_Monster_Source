using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComboLogo : MonoBehaviour
{
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    //public EntityPlayer m_linkedEntityPlayer;
    //Stock le rectangle d'affiche de L'ui complete
    //  private Rect m_UIScreenSpace;
    //  private Rect m_lifelineSpace;
    public EntityPlayer m_linkedEntityPlayer;
    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    private int m_playerID;
    private int m_playerCount;

    //Stock la position dans l'espace dédié, à renseigner
    // Vector2 m_pos;
    //taille de la texture, à renseigner
   // public Vector2 m_sizeTex;
    //largeur du screen sur lequel on travail, à renseigner
   // public int m_screenWidth;

    // texture à afficher
    //public Texture2D m_currentTexture;
    //Scale à appliquer sur la texture pour la redimensionner
   // public Vector2 m_scale;

    //Stock la position dans l'espace en focntion du nombre de joueur
    public Vector3 m_pos1;

    public Vector3 m_pos2J1;
    public Vector3 m_pos2J2;

    public Vector3 m_pos3J1;
    public Vector3 m_pos3J2;
    public Vector3 m_pos3J3;

    public Vector3 m_pos4J1;
    public Vector3 m_pos4J2;
    public Vector3 m_pos4J3;
    public Vector3 m_pos4J4;

    public Image m_image;
    private float width;
    private float height;

    public enum PivotX
    {
        Left,
        Middle,
        Right
    }
    public enum PivotY
    {
        Top,
        Middle,
        Down
    }
    //Permet de soustraire la size à l'axe si on souhaite centrer (coin haut gauche pris par default), à renseigner
    public PivotX m_pivotX;
    public PivotY m_pivotY;
    //Addapte l'Ui en fonction de la taille d'ecran sur lequel il a été configuré
   // private float m_ratioScreen;


    //pos en fonction de la taille de l'ecran
    //private Vector2 m_drawPos;



    //Vector2 positionScale;
    //float canvasPosX;
    //float canvasPosY;
    //float canvasWidth;
    //float canvasHeight;

    //float spriteWidth;
    //float spriteHeight;

    void Start()
    {
        //Recupère l' ID
        m_playerID = m_linkedEntityPlayer.m_playerId;
        m_playerCount = m_UIplayer.m_playerCount;
        // Debug.Log("m_playerCount = " + m_playerCount);
       

        if (m_UIplayer.m_playerCount > 1)
        {
            m_image.GetComponent<RectTransform>().sizeDelta = m_image.GetComponent<RectTransform>().sizeDelta / 2;

        }

        switch (m_playerCount)
        {
            case 1:
                // m_text.fontSize = m_fontSize;
                GetComponent<RectTransform>().position = m_pos1;
//                Debug.Log(GetComponent<RectTransform>().position);
                break;
            case 2:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos2J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos2J2;
                }
                break;
            case 3:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos3J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos3J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos3J3;
                }
                break;
            case 4:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos4J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos4J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos4J3;
                }
                else if (m_playerID == 3)
                {
                    GetComponent<RectTransform>().position = m_pos4J4;
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        //à mettre dans le start une fois fini ////=>
        //pos en fonction de la taille de l'ecran
      //  Debug.Log("m_playerCount = " + m_playerCount);
        switch (m_playerCount)
        {
            case 1:
//                Debug.Log("pos : "+GetComponent<RectTransform>().position);
                // m_text.fontSize = m_fontSize;
                GetComponent<RectTransform>().position = m_pos1;
                break;
            case 2:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos2J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos2J2;
                }
                break;
            case 3:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos3J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos3J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos3J3;
                }
                break;
            case 4:
                if (m_playerID == 0)
                {
                    GetComponent<RectTransform>().position = m_pos4J1;
                }
                else if (m_playerID == 1)
                {
                    GetComponent<RectTransform>().position = m_pos4J2;
                }
                else if (m_playerID == 2)
                {
                    GetComponent<RectTransform>().position = m_pos4J3;
                }
                else if (m_playerID == 3)
                {
                    GetComponent<RectTransform>().position = m_pos4J4;
                }
                break;
            default:
                break;
        }
    }
    //Affichage de la barre
   
}
