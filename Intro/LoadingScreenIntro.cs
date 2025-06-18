using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenIntro : MonoBehaviour
{
    public bool m_IntroManagIsAbleToFade;
    public bool m_LSisAbleToFade;

    public bool m_IntroManagIsAbleToLoadScene;

    public float m_asyncProgress;
    private void Start()
    {
        m_IntroManagIsAbleToFade = false;
        m_LSisAbleToFade = false;
        m_IntroManagIsAbleToLoadScene = false;

        m_asyncProgress = 0;
    }
    //static float timerFadeMax = 2.0f;
    //float timerFade;
    //bool start = false;
    //bool Fadding = false;
    //[SerializeField] GameObject GameObjectText;
    //[SerializeField] Text text;
    //[SerializeField] Image blackScreen;
    // Start is called before the first frame update
    //void Start()
    //{
    //    timerFade = timerFadeMax;
    //    start = true;
    //    Fadding = false;
    //}

    //void StartFade()
    //{
    //    timerFade = timerFadeMax;
    //    Fadding = true;
    //    //Mise de l'aplha à 0
    //    Color c = blackScreen.color;
    //    c.a = 0.0f;
    //    blackScreen.color = c;
    //}

    // Update is called once per frame
    void Update()
    {
       // Fadding = false;
        //if (start)
        //{
        //    timerFade -= Time.deltaTime;

        //    if (timerFade <= 0)
        //    {
        //        //Debug.Log("blackScreen.color.a <= 5");
        //        //Mise de l'aplha à 0
        //        Color c = blackScreen.color;
        //        c.a = 0.0f;
        //        blackScreen.color = c;

        //        GameObjectText.SetActive(false);
        //        start = false;

        //        return;
        //    }
        //    Color color = blackScreen.color;
        //    color.a = 1.0f / timerFadeMax * (timerFade);
        //    //Debug.Log("color.a = " + color.a);
        //    blackScreen.color = color;
        //}

        //if (Fadding)
        //{
        //    timerFade -= Time.deltaTime;

        //    if (timerFade <= 0)
        //    {
        //       // Debug.Log("fadding time out");
        //        //Mise de l'alpha à 255
        //        Color c = blackScreen.color;
        //        c.a = 1.0f;
        //        blackScreen.color = c;

        //        Fadding = false;
        //        return;
        //    }
        //    Color color = blackScreen.color;
        //    color.a = 1.0f/timerFadeMax * (timerFadeMax-timerFade);
        //    // Debug.Log("color.a = " + color.a);
        //    blackScreen.color = color;
        //}



    }

    public IEnumerator LoadScene(int SceneNum)
    {
       // StartFade();
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneNum);
        async.allowSceneActivation = false;

       // GameObjectText.SetActive(true);

       // async.completed += (AsyncOperation x) => Debug.Log("Level " + GameManager.Instance.currentLevel + " loaded");

        while (async.progress < 0.9f)
        {
            m_asyncProgress = async.progress;
            m_LSisAbleToFade = true;
           // Debug.Log("m_LSisAbleToFade");
           // text.text = (int)(async.progress * 100) + "%";
            //Debug.Log("< 90");

            //if (async.progress >= 0.9f)
            //{
            //    m_IntroManagIsAbleToFade = true;
            //}
            yield return null;
        }
        //   Debug.Log("> 90");
        //text.text = "100% ";
        yield return new WaitUntil(() => m_IntroManagIsAbleToLoadScene); //Le niveau suivant se lance avec les condition suivante:)
        //-Le niveau est chargé
        //- Le fade(d’environ 1 ou 2 sec) est terminé, et l’écran noir.);
        //if (m_IntroManagIsAbleToLoadScene)
       // {
          //  Debug.Log("in load : m_IntroManagIsAbleToLoadScene");
            async.allowSceneActivation = true;
        //}
    }

    public void LoadMenuScene()
    {
        StartCoroutine(FindObjectOfType<LoadingScreenIntro>().LoadScene(1));
    }
    public void LoadGameScene()
    {
        StartCoroutine(FindObjectOfType<LoadingScreenIntro>().LoadScene(2));
    }
}
