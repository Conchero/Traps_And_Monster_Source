using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    //Stock la postion à l'ecran de la barre, à renseigner
    public Vector2 m_pos;
    //pos en fonction de la taille de l'ecran
    private Vector2 m_drawPos;

    //textures vide à afficher, à renseigne
    public Texture2D m_emptyTex;
    //Texture qui prendra en compte le fade
  //  private Texture2D m_finalEmptyTex;
    //textures pleine à afficher, à renseigne
    public Texture2D m_fullTex;
    //Texture qui prendra en compte le fade
   // private Texture2D m_finalFullTex;
    //permet de gerer les fades
    public Color m_color;

    //Creer les elements GUI necessaire pour l'affichage
    public GUISkin m_currentGui;
    public GUIStyle m_currentStyle;
    //taille de la texture, à renseigner
    public Vector2 m_sizeTex;
    //largeur du screen, à renseigner
    public int m_screenWidth;

    //Permet de sourstraire la size à l'axe si on souhaite centrer (coin haut gauche pris par default), à renseigner
    public bool m_bAxeOnX = false;
    public bool m_bAxeOnY = false;

    //Addapte l'Ui en fonction de la taille d'ecran sur lequel il a été configuré
    private float m_ratioScreen;
    //Variable à utiliser en code pour renseigner l'avancement du chargement
    public float m_currentProgress = 0;

    //public float m_fadeFactor;

    void Start()
    {
        //Ratio de l'ecran à appliquer à l'affichage
        m_ratioScreen = (float)Screen.width / (float)m_screenWidth;
        //pos en fonction de la taille de l'ecran
        m_drawPos = m_pos;
        //Si on as précisé qu'on centrait la positon
        //En X
        if (m_bAxeOnX)
        {
            m_drawPos.x -= m_sizeTex.x / 2;
        }
        //En Y
        if (m_bAxeOnY)
        {
            m_drawPos.y -= m_sizeTex.y / 2;
        }

        // m_fadeFactor = 0f;

        //m_finalEmptyTex =  new Texture2D((int)m_sizeTex.x, (int)m_sizeTex.y);
        //m_finalFullTex =  new Texture2D((int)m_sizeTex.x, (int)m_sizeTex.y);
    }

    private void Update()
    {
        //m_finalEmptyTex = m_emptyTex;
        //m_finalFullTex = m_fullTex;
        ////fade de la texture
        ////Paint map
        //for (int i = 0; i < (int)(m_sizeTex.x); i++)
        //{
        //    for (int j = 0; j < (int)(m_sizeTex.y); j++)
        //    {
        //        Color colorEmpty = m_finalEmptyTex.GetPixel(i,j);
        //        Color colorFull = m_finalFullTex.GetPixel(i,j);


        //        //Ajouter le fade à l'alpha
        //        colorEmpty.a = Mathf.Clamp(m_fadeFactor, 0, 1);
        //        colorFull.a = Mathf.Clamp(m_fadeFactor, 0, 1);

        //        m_finalEmptyTex.SetPixel(i, j, colorEmpty);
        //        m_finalFullTex.SetPixel(i, j, colorFull);
        //    }
        //}
    }

    //Affichage de la barre
    void OnGUI()
    {

        //Assignation du skin
        GUI.skin = m_currentGui;
        //draw the background:
        GUI.BeginGroup(new Rect(m_drawPos.x * m_ratioScreen, m_drawPos.y * m_ratioScreen, m_sizeTex.x * m_ratioScreen, m_sizeTex.y * m_ratioScreen), m_currentStyle);
        GUI.DrawTexture(new Rect(0, 0, m_sizeTex.x * m_ratioScreen, m_sizeTex.y * m_ratioScreen), m_emptyTex, ScaleMode.ScaleToFit);

        GUI.EndGroup();
        //draw the filled-in part:
        //Taille du group en fonction de l'avancement du chargement
        GUI.BeginGroup(new Rect(m_drawPos.x * m_ratioScreen, m_drawPos.y * m_ratioScreen, m_sizeTex.x * m_ratioScreen * m_currentProgress, m_sizeTex.y * m_ratioScreen), m_currentStyle);
        GUI.DrawTexture(new Rect(0, 0, m_sizeTex.x * m_ratioScreen, m_sizeTex.y * m_ratioScreen), m_fullTex, ScaleMode.ScaleToFit);
        GUI.EndGroup();

    }

    //à utiliser en précisant l'avancement du chargement (valeur de 0 à 1)
    public void SetProgress(float _progress)
    {
        m_currentProgress = Mathf.Clamp(_progress, 0.0f, 1.0f);
    }
}

