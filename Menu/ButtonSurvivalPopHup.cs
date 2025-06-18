using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSurvivalPopHup : MonoBehaviour
{
    [SerializeField] Image m_background;
    [SerializeField] Image m_logo;
    [SerializeField] TMP_Text m_text;
    bool m_needToDraw;
    bool m_needToFade;
    float m_timerDraw;
    float m_timerFade;
    float m_timerDrawMax = 2f;
    float m_timerFadeMax = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_needToDraw = false;
        m_needToFade = false;
        m_timerDraw = 0f;
        m_timerFade = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_needToDraw)
        {
            if (m_timerDraw > 0f)
            {
                m_timerDraw -= Time.deltaTime;
            }
            else
            {
                m_needToDraw = false;
                m_needToFade = true;
                m_timerFade = m_timerFadeMax;
            }
        }
        if (m_needToFade)
        {
            if (m_timerFade > 0f)
            {
                float alpha = 1 * m_timerFade / m_timerFadeMax;
                m_background.color = new Color(1, 1, 1, alpha);
                m_logo.color = new Color(1, 1, 1, alpha);
                m_text.color = new Color(1, 1, 1, alpha);

                m_timerFade -= Time.deltaTime;
            }
            else
            {
                m_background.color = new Color(1, 1, 1, 0);
                m_logo.color = new Color(1, 1, 1, 0);
                m_text.color = new Color(1, 1, 1, 0);
                m_needToFade = false;
            }
        }
    }


    public void TriggerDraw()
    {
        m_needToDraw = true;
        m_timerDraw = m_timerDrawMax;
        m_background.color = Color.white;
        m_logo.color = Color.white;
        m_text.color = Color.white;
        //m_text.CrossFadeColor(new Color(1, 1, 1, 1), 2.0f, false, true);
    }



    private void OnEnable()
    {
        m_needToDraw = false;
        m_timerDraw = 0;
        m_background.color = new Color(1, 1, 1, 0);
        m_logo.color = new Color(1, 1, 1, 0);
        m_text.color = new Color(1, 1, 1, 0);
    }
}
