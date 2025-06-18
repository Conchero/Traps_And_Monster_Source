using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;


public enum IntroState
{
    Studio,
    TransitionToCrea,
    Crea,
    TransitionToTrailer,
    Trailer,

    FadeToLogo,
    Logo,
    FadeOfLogo,
    FadeToLoad,
    Load,
    FadeOfLoad,
}

public class IntroManager : MonoBehaviour
{

    bool m_applicationFocus;

    //permet de verifier si on a deja appelé la corroutine de chargement de la scene suivante
    bool m_menuBeenLaunched;
    IntroState m_introState;

    public VideoPlayer m_studioVideo;
    private float m_studioVideoLenght;
    private float m_studioVideoTimer;

    private float m_timerBetweenVideos = 1.0f;
    public VideoPlayer m_creaVideo;
    private float m_creaVideoLenght;
    private float m_creaVideoTimer;

    public VideoPlayer m_trailerVideo;
    private float m_trailerVideoLenght;
    private float m_trailerVideoTimer;

    //à declencher pour passer à la scene de menu en debug (après le chargement)
    public bool m_eventEndOfIntro = false;


    public Image m_logoImg;
    public Image m_loadScreen;
    public Sprite[] m_randomScreenSprites;
    public ProgressBar m_progressBar;
    // public Image m_loadImg;
    // public Image m_loadOverlayImg;


    private float m_timer;
    private float m_timerImage = 2f;
    private float m_timerFade = 1.5f;


    //permet de savoir si le chargement est deja lancé pour ne aps charger plusieurs scenes
    bool m_loadIsLaunched;

    // Start is called before the first frame update
    void Start()
    {
        m_menuBeenLaunched = false;

        m_introState = IntroState.Studio;

        m_studioVideoLenght = (float)m_studioVideo.length;
        m_studioVideoTimer = 0.0f;

        m_creaVideoLenght = (float)m_creaVideo.length;
        m_creaVideoTimer = 0.0f;

        m_trailerVideoLenght = (float)m_trailerVideo.length;
        m_trailerVideoTimer = 0.0f;


        m_timer = 0.0f;


        //Choix du screen de chargement
        int RandomScreenId = Random.Range(0, m_randomScreenSprites.Length);
        m_loadScreen.sprite = m_randomScreenSprites[RandomScreenId];
        //Addaptation de l'image à la taille de l'ecran (fonctionne pas)
        //RectTransform tempRect = m_loadScreen.transform.GetComponent<RectTransform>();
        //tempRect.sizeDelta = new Vector2(m_randomScreenSprites[RandomScreenId].texture.width, m_randomScreenSprites[RandomScreenId].texture.height);

        m_loadIsLaunched = false;

        foreach (InputDevice iD in InputSystem.devices)
        {
            DataManager.Instance.m_deviceConnected.Add(iD);
            InputManager.Instance.AssignControl(iD.name);
        }
    }


    void Studio()
    {

        SoundManager.Instance.IntroAfterLifePlay(gameObject);
        m_studioVideoTimer += Time.deltaTime;
        if (m_studioVideoTimer >= m_studioVideoLenght)
        {
            m_introState++;
            m_studioVideo.gameObject.SetActive(false);
        }
    }

    void TransitionToCrea()
    {
        SoundManager.Instance.IntroAfterLifeStop(gameObject);
        m_timerBetweenVideos -= Time.deltaTime;
        if (m_timerBetweenVideos <= 0)
        {
            m_introState++;
            m_creaVideo.Play();
            m_timerBetweenVideos = 1.0f;
        }
    }
    void Crea()
    {
        SoundManager.Instance.IntroCreaPlay(gameObject);
        m_creaVideoTimer += Time.deltaTime;
        if (m_creaVideoTimer >= m_creaVideoLenght)
        {
            m_introState++;
            m_timer = 0;
            m_creaVideo.gameObject.SetActive(false);

        }
    }
    void TransitionToTrailer()
    {
        SoundManager.Instance.IntroCreaStop(gameObject);
        m_timerBetweenVideos -= Time.deltaTime;
        if (m_timerBetweenVideos <= 0)
        {
            m_introState++;
            m_trailerVideo.Play();
        }
    }
    void Trailer()
    {
        SoundManager.Instance.TrailerMusicPlay(gameObject);
        m_trailerVideoTimer += Time.deltaTime;
        if (m_trailerVideoTimer >= m_trailerVideoLenght)
        {
            m_introState++;
            m_timer = 0;
            m_trailerVideo.gameObject.SetActive(false);

        }
    }
    void FadeToLogo()
    {
        SoundManager.Instance.TrailerMusicStop(gameObject);

        Color color = m_logoImg.GetComponent<Image>().color;
        if (m_timer >= m_timerFade)
        {
            color.a = 1;
            m_timer = 0;
            m_introState++;
        }
        else
        {
            color.a = 1 * m_timer / m_timerFade;
        }
        m_logoImg.GetComponent<Image>().color = color;
    }
    void Logo()
    {
        if (m_timer >= m_timerImage)
        {
            m_timer = 0;
            m_introState++;
        }
    }
    void FadeOfLogo()
    {
        Color color = m_logoImg.GetComponent<Image>().color;
        if (m_timer >= m_timerFade)
        {
            color.a = 0;
            m_timer = 0;
            m_introState++;
            m_loadScreen.gameObject.SetActive(true);
        }
        else
        {

            color.a = 1 - 1 * m_timer / m_timerFade;

        }
        m_logoImg.GetComponent<Image>().color = color;
    }


    void FadeToLoad()
    {
        Color color = m_loadScreen.GetComponent<Image>().color;
        //  Color colorOverlay = m_loadOverlayImg.GetComponent<Image>().color;
        // color.a = 1;
        if (m_timer >= m_timerFade)
        {
            color.a = 1;
            // colorOverlay.a = 1;
            m_timer = 0;
            m_introState = IntroState.Load;
            FindObjectOfType<LoadingScreenIntro>().LoadMenuScene();
            m_progressBar.gameObject.SetActive(true);
        }
        else
        {
            color.a = 1 * m_timer / m_timerFade;
            //  colorOverlay.a = 1 * m_timer / m_timerFade;
        }
        //  m_loadImg.GetComponent<Image>().color = color;
        //  m_loadOverlayImg.GetComponent<Image>().color = colorOverlay;
        m_loadScreen.GetComponent<Image>().color = color;
    }


    void Load()
    {
        if (m_timer >= m_timerImage)
        {
            FindObjectOfType<LoadingScreenIntro>().m_IntroManagIsAbleToFade = true;


            if (FindObjectOfType<LoadingScreenIntro>().m_LSisAbleToFade)
            {
                m_timer = 0;
                m_introState = IntroState.FadeOfLoad;
                //  Vector3 tempScale = m_loadImg.GetComponent<Image>().GetComponent<RectTransform>().localScale;
                //  Vector3 Scale = new Vector3(1 , tempScale.y, tempScale.z);
                //  m_loadImg.GetComponent<Image>().GetComponent<RectTransform>().localScale = Scale;
                m_progressBar.SetProgress(1f);
                m_progressBar.gameObject.SetActive(false);
            }
        }
        else
        {
            //  Vector3 tempScale = m_loadImg.GetComponent<Image>().GetComponent<RectTransform>().localScale;
            // Vector3 Scale = new Vector3(1 * FindObjectOfType<LoadingScreenIntro>().m_asyncProgress / 0.9f, tempScale.y, tempScale.z);
            // m_loadImg.GetComponent<Image>().GetComponent<RectTransform>().localScale = Scale;
            m_progressBar.SetProgress(FindObjectOfType<LoadingScreenIntro>().m_asyncProgress);
        }
    }


    void FadeOfLoad()
    {
        Color color = m_loadScreen.GetComponent<Image>().color;
        // Color colorOverlay = m_loadOverlayImg.GetComponent<Image>().color;
        if (m_timer >= m_timerFade && !m_menuBeenLaunched)
        {
            m_menuBeenLaunched = true;
            color.a = 0;
            //  colorOverlay.a = 0;

            FindObjectOfType<LoadingScreenIntro>().m_IntroManagIsAbleToLoadScene = true;
        }
        else
        {
            color.a = 1 - 1 * m_timer / m_timerFade;
        }
        //  m_loadImg.GetComponent<Image>().color = color;
        //  m_loadOverlayImg.GetComponent<Image>().color = colorOverlay;
        m_loadScreen.GetComponent<Image>().color = color;
    }



    // Update is called once per frame
    void Update()
    {
        // if (m_applicationFocus)
        //  {
        //Quitter l'intro
        if ((Input.GetKeyDown(KeyCode.Escape) && !m_loadIsLaunched) || m_eventEndOfIntro)
        {
            m_loadIsLaunched = true;
            SoundManager.Instance.IntroCreaStop(gameObject);
            SoundManager.Instance.IntroAfterLifeStop(gameObject);
            m_eventEndOfIntro = false;

            m_timer = 0;
            m_introState = IntroState.FadeToLoad;
            m_studioVideo.Stop();
            m_creaVideo.Stop();
            m_trailerVideo.Stop();
        }

        foreach (InputDevice input in DataManager.Instance.m_deviceConnected)
        {
            if ((InputManager.Instance.isPressed(input.device.name, "A", true) && !m_loadIsLaunched) || m_eventEndOfIntro)
            {
                m_loadIsLaunched = true;
                SoundManager.Instance.IntroCreaStop(gameObject);
                SoundManager.Instance.IntroAfterLifeStop(gameObject);
                // SoundManager.Instance.TrailerMusicStop(gameObject);
                m_eventEndOfIntro = false;

                m_timer = 0;
                m_introState = IntroState.FadeToLoad;
                m_studioVideo.Stop();
                m_creaVideo.Stop();
            }
        }

        //Timer
        m_timer += Time.deltaTime;


        switch (m_introState)
        {

            case IntroState.Studio:
                Studio();
                break;
            case IntroState.TransitionToCrea:
                TransitionToCrea();
                break;
            case IntroState.Crea:
                Crea();
                break;
            case IntroState.TransitionToTrailer:
                TransitionToTrailer();
                // TransitionToCrea();
                break;
            case IntroState.Trailer:
                Trailer();
                // Crea();
                break;
            case IntroState.FadeToLogo:
                FadeToLogo();
                break;
            case IntroState.Logo:
                Logo();
                break;
            case IntroState.FadeOfLogo:
                FadeOfLogo();
                break;
            case IntroState.FadeToLoad:
                FadeToLoad();
                break;
            case IntroState.Load:
                Load();
                break;
            case IntroState.FadeOfLoad:
                FadeOfLoad();
                break;
        }
        // }
    }




    //void OnGUI()
    //{
    //    if (isPaused)
    //        GUI.Label(new Rect(100, 100, 50, 30), "Game paused");
    //}

    void OnApplicationFocus(bool hasFocus)
    {
        m_applicationFocus = hasFocus;
    }

    //void OnApplicationPause(bool pauseStatus)
    //{
    //    isPaused = pauseStatus;
    //}
}
