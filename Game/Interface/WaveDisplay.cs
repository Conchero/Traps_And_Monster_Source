//Par Max Bonnaud
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
///using UnityEngine.Sprites;

public class WaveDisplay : MonoBehaviour
{
    //get wave manager
    [SerializeField] GameObject waveManager;
    WaveManager wm;
    //get text
    [SerializeField] TMP_Text waveText;
    [SerializeField] GameObject TimerBar;
    [SerializeField] GameObject EnnemyRatioBar;

    //[SerializeField] Material timerBarMat;
    //[SerializeField] Material RatioBarMat;

    bool isWaveActive;
    float timeRatio;

    // Start is called before the first frame update
    void Start()
    {

        waveText.text = "Prep";
        wm = waveManager.GetComponent<WaveManager>();

        isWaveActive = false;
        SetWaveBarTo("timer");
        TimerBar.GetComponent<Image>().material.SetFloat("_WaveTimerBarRatio", 1);
        //TimerBar.GetComponent<CanvasRenderer>().GetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //check if wave is active
    
        if(isWaveActive)
        {
            if (!wm.isWaveActive)
            {
                isWaveActive = false;   //toggle
                SetWaveBarTo("timer");
                waveText.text = "Wave " + waveManager.GetComponent<WaveManager>().currentWave;
            }
            //update ennemy ratio

            if(wm.ennemyCounter.total > 0)
            {
                //Debug.Log("updating ratio bar");
                //get waveManager ennemy counter

                
                WaveManager.EnnemyCounter counter = wm.ennemyCounter;
                Vector4 colorRatio = new Vector4(1f, 1f, 1f ,1f);
                colorRatio.w = (float)counter.yellow / counter.total;   //red
                colorRatio.x = (float)counter.red / counter.total;   //green
                colorRatio.y = (float)counter.green / counter.total;   //blue
                colorRatio.z = (float)counter.blue / counter.total;   //yellow

                //Debug.Log("ratio: " + counter.red + ":" + counter.green + ":" + counter.blue + ":" + counter.yellow);
                
                EnnemyRatioBar.GetComponent<CanvasRenderer>().GetMaterial().SetVector("_ennemyRatio", colorRatio);
            }


        }
        else
        {
            if(wm.isWaveActive)
            {
                isWaveActive = true;    //toggle
                SetWaveBarTo("ennemies");
                waveText.text = "Wave " + waveManager.GetComponent<WaveManager>().currentWave;
            }

            //update next wave timer
            if(wm.timeBetweenWaves != 0.0f)
            {
                timeRatio = wm.breakTimer / wm.timeBetweenWaves;
                TimerBar.GetComponent<CanvasRenderer>().GetMaterial().SetFloat("_WaveTimerBarRatio", timeRatio);
            }
        }

    }
    void SetWaveBarTo(string _barName)
    {
        //display/hide bars
        switch(_barName)
        {
            case "timer":
                TimerBar.GetComponent<CanvasGroup>().alpha = 1;
                EnnemyRatioBar.GetComponent<CanvasGroup>().alpha = 0;
                break;

            case "ennemies":
                TimerBar.GetComponent<CanvasGroup>().alpha = 0;
                EnnemyRatioBar.GetComponent<CanvasGroup>().alpha = 1;
                break;

            default:
                Debug.LogWarning("WaveDisplay: SetWaveBarTo(): unknown name");
                break;
        }
    }
}
