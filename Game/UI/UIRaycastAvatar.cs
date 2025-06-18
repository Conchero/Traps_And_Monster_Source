//script by : Alexis

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRaycastAvatar : MonoBehaviour
{
    //Creer les elements GUI necessaire pour l'affichage
    public GUISkin m_currentGui;
    public GUIStyle m_currentStyle;
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    public EntityPlayer m_linkedEntityPlayer;
    //Stock le rectangle d'affiche de L'ui complete
    private Rect m_UIScreenSpace;
    //  private Rect m_lifelineSpace;

    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    private int m_playerID;
    private int m_playerCount;

    //Stock la position dans l'espace dédié, à renseigner
    public Vector2 m_pos;
    //taille de la texture, à renseigner
    public Vector2 m_sizeTex;
    //largeur du screen sur lequel on travail, à renseigner
    public int m_screenWidth;

    //texture vide qu'on remplira suivant la couleur de l'equipe
    private Texture2D m_currentTexture;
    //table des differentes textures de barres de vie à afficher, à renseigne
    public Texture2D[] m_textures;
    //Scale à appliquer sur la texture pour la redimensionner
    public Vector2 m_scale;




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
    private float m_ratioScreen;


    //pos en fonction de la taille de l'ecran
    private Vector2 m_drawPos;



    Vector2 positionScale;
    float canvasPosX;
    float canvasPosY;
    float canvasWidth;
    float canvasHeight;

    float spriteWidth;
    float spriteHeight;

    // Start is called before the first frame update
    void Start()
    {
        m_UIplayer = GetComponentInParent<UIPlayer>();
        m_linkedEntityPlayer = m_UIplayer.m_linkedEntityPlayer;
        m_UIScreenSpace = m_UIplayer.m_screenSpace;

        //Ratio de l'ecran à appliquer à l'affichage
        m_ratioScreen = (float)Screen.width / (float)m_screenWidth;

        //  m_pos *= m_ratioScreen;

        //Recupère l' ID
        m_playerID = transform.parent.transform.parent.transform.parent.GetComponent<EntityPlayer>().m_playerId;
        m_playerCount = m_UIplayer.m_playerCount;
        //Associe la bonne texture
        switch (m_linkedEntityPlayer.m_sColor)
        {
            case "Red":
                m_currentTexture = m_textures[0];
                break;
            case "Blue":
                m_currentTexture = m_textures[1];
                break;
            case "Green":
                m_currentTexture = m_textures[2];
                break;
            case "Yellow":
                m_currentTexture = m_textures[3];
                break;

            default:
                break;
        }

        if (m_UIplayer.m_playerCount > 1)
        {
            m_scale /= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        //à mettre dans le start une fois fini ////=>
        //pos en fonction de la taille de l'ecran
        m_drawPos = m_pos;
        //Position en fonction du pivot
        //En X
        if (m_playerCount < 3)
        {
            if (m_pivotX == PivotX.Middle)
            {
                m_drawPos.x -= (m_sizeTex.x / 2) * m_scale.x;
            }
            else if (m_pivotX == PivotX.Right)
            {
                m_drawPos.x -= m_sizeTex.x * m_scale.x;
            }
        }
        else
        {
            if (m_pivotX == PivotX.Middle)
            {
                m_drawPos.x -= (m_sizeTex.x * 2 / 2) * m_scale.x;
            }
            else if (m_pivotX == PivotX.Right)
            {
                m_drawPos.x -= m_sizeTex.x * 2 * m_scale.x;
            }

        }
        //En Y
        if (m_playerCount == 1)
        {
            if (m_pivotY == PivotY.Middle)
            {
                m_drawPos.y -= (m_sizeTex.y / 2) * m_scale.y;
            }
            else if (m_pivotY == PivotY.Down)
            {
                m_drawPos.y -= m_sizeTex.y * m_scale.y;
            }
        }
        else
        {
            if (m_pivotY == PivotY.Middle)
            {
                m_drawPos.y -= (m_sizeTex.y * 2 / 2) * m_scale.y;
            }
            else if (m_pivotY == PivotY.Down)
            {
                m_drawPos.y -= m_sizeTex.y * 2 * m_scale.y;
            }

        }


        //Scale de position en fonction du nombre de joueur
        positionScale = new Vector2(m_UIScreenSpace.width / Screen.width, m_UIScreenSpace.height / Screen.height);


        canvasPosX = (m_UIScreenSpace.x + m_drawPos.x * positionScale.x) * m_ratioScreen;
        canvasPosY = (m_UIScreenSpace.y + m_drawPos.y * positionScale.y) * m_ratioScreen;
        canvasWidth = m_sizeTex.x * m_ratioScreen * m_scale.x;
        canvasHeight = m_sizeTex.y * m_ratioScreen * m_scale.y;

        spriteWidth = m_sizeTex.x * m_ratioScreen * m_scale.x;
        spriteHeight = m_sizeTex.y * m_ratioScreen * m_scale.y;
        ////<=
        ///


        //Assignation du skin GUI
        GUI.skin = m_currentGui;
        GUI.BeginGroup(new Rect(canvasPosX, canvasPosY, canvasWidth, canvasHeight), m_currentStyle);
        GUI.DrawTexture(new Rect(0, 0, spriteWidth, spriteHeight), m_currentTexture, ScaleMode.ScaleToFit);
        GUI.EndGroup();

    }
}
