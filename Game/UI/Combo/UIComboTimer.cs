using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboTimer : MonoBehaviour
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

    //textures vide à afficher, à renseigne
    public Texture2D m_emptyTex;
    //textures pleine à afficher, à renseigne
    public Texture2D m_fullTex;
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
    //Variable à utiliser en code pour renseigner l'etat de la barre de vie
    float m_currentLife;
    private float m_lifeMax;


    //pos en fonction de la taille de l'ecran
    private Vector2 m_drawPos;

    void Start()
    {
        m_UIplayer = GetComponentInParent<UIPlayer>();
        m_linkedEntityPlayer = m_UIplayer.m_linkedEntityPlayer;
        m_UIScreenSpace = m_UIplayer.m_screenSpace;

        //Ratio de l'ecran à appliquer à l'affichage
        m_ratioScreen = (float)Screen.width / (float)m_screenWidth;

        //  m_pos *= m_ratioScreen;

        //Recupère l' ID
        m_playerID = m_linkedEntityPlayer.m_playerId;
        m_playerCount = DataManager.Instance.m_prefab.Count;


        //TIMER COMBO MAX
        m_lifeMax = 3.5f;

        if (m_UIplayer.m_playerCount > 1)
        {
            m_scale /= 2;
        }
    }

    private void Update()
    {
        //Recuperation de la vie courante
        //m_currentLife = m_UIplayer.m_linkedEntityPlayer.m_health;
        if (m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>() != null && m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected != null)
        {

            if (m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected) != null)
            {
                GameObject weappon = m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.transform.Find(m_linkedEntityPlayer.gameObject.GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>().WeaponSelected).gameObject;
                if (weappon != null)
                {
                    if (weappon.name == "Axe")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<Axe>().TimerCombo);
                    }
                    else if (weappon.name == "Bow")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<Bow>().ComboTimer);
                    }
                    else if (weappon.name == "CrossBow")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<CrossBow>().ComboTimer);
                    }
                    else if (weappon.name == "LaserSword")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<LaserSword>().ComboTimer);
                    }
                    else if (weappon.name == "Pistol")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<Pistol>().ComboTimer);
                    }
                    else if (weappon.name == "Sword")
                    {
                        m_currentLife = (3.5f - weappon.GetComponent<Sword>().TimerCombo);
                    }
                }
            }
        }
    }

    //Affichage de la barre
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



        //Recuperation de la vie courante
        //m_currentLife = m_UIplayer.m_linkedEntityPlayer.m_health;
        //Taille du group en fonction de l'avancement du chargement
        float fichePersoWidth = Mathf.Clamp(((m_sizeTex.x * m_ratioScreen)) * m_currentLife / m_lifeMax, 0, (m_sizeTex.x * m_ratioScreen));

//        Debug.Log("fichePersoWidth : " + fichePersoWidth);

        //Scale de position en fonction du nombre de joueur
        Vector2 positionScale = new Vector2(m_UIScreenSpace.width / Screen.width, m_UIScreenSpace.height / Screen.height);
        //Vector2 positionScale = new Vector2(1, 1);
        //if (m_playerCount == 2)
        //{
        //    positionScale = new Vector2(1, 0.5f);
        //}
        //else if (m_playerCount > 2)
        //{
        //    positionScale = new Vector2(0.5f, 0.5f);
        //}

        float canvasPosX = (m_UIScreenSpace.x + m_drawPos.x * positionScale.x) * m_ratioScreen;
        float canvasPosY = (m_UIScreenSpace.y + m_drawPos.y * positionScale.y) * m_ratioScreen;
        float canvasWidth = fichePersoWidth * m_scale.x;
        float canvasBackgoundWidth = m_sizeTex.x * m_ratioScreen * m_scale.x;
        float canvasHeight = m_sizeTex.y * m_ratioScreen * m_scale.y;

        float spriteWidth = m_sizeTex.x * m_ratioScreen * m_scale.x;
        float spriteHeight = m_sizeTex.y * m_ratioScreen * m_scale.y;
        ////<=
        ///



        //Assignation du skin GUI
        GUI.skin = m_currentGui;
        //draw the background:
        GUI.BeginGroup(new Rect(canvasPosX, canvasPosY, canvasBackgoundWidth, canvasHeight), m_currentStyle);
        GUI.DrawTexture(new Rect(0, 0, spriteWidth, spriteHeight), m_emptyTex, ScaleMode.ScaleToFit);
        GUI.EndGroup();
        //Taille du group en fonction de l'avancement du chargement
        GUI.BeginGroup(new Rect(canvasPosX, canvasPosY, canvasWidth, canvasHeight), m_currentStyle);
        GUI.DrawTexture(new Rect(0, 0, spriteWidth, spriteHeight), m_fullTex, ScaleMode.ScaleToFit);
        GUI.EndGroup();

    }


    //à utiliser en précisant l'avancement du chargement (valeur de 0 à 1)
    //public void SetCurrentLife(float _currentLife)
    //{
    //    m_currentLife = _currentLife;

    //}
}
