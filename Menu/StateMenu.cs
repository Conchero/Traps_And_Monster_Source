using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

using TMPro;
using UnityEngine.Video;

public enum MenuState
{
    Home,
    ModeSelection,
    SettingsScreen,
    SkinSelection,
    MapSelection,

    FadeOfMapSelection,
    FadeToLoad,
    Load,
    FadeOfLoad,
}

//public enum ModeSelectionButton
//{
//    AFFRONTEMENT,
//    SURVIE,
//}

public enum MapSelectionButton
{
    MAP1,
    MAP2,
}

public class StateMenu : State
{
    //MenuManager m_pMenuManager;
    public MenuState m_eMenuState;
    // ModeSelectionButton modeSelectionButton;
    MapSelectionButton mapSelectionButton;
    [SerializeField] GameObject m_FolderHome;
    [SerializeField] GameObject m_FolderModeSelection;
    [SerializeField] ButtonSurvivalPopHup m_windowSurvivalPopHup;
    [SerializeField] GameObject m_FolderSkinSelection;
    [SerializeField] SkinSelectionModelManager m_skinSelectionModelManager;
    [SerializeField] GameObject m_FolderMapSelection;
    [SerializeField] GameObject m_folderSettingsScreen;
    [SerializeField] GameObject m_folderUIControllerDetected;

    //permet de verifier si on a deja appelé la corroutine de chargement de la scene suivante
    bool m_gameBeenLaunched;
    public Image m_BlackScreen;
    public Image m_loadScreen;
    public Sprite[] m_randomScreenSprites;
    public ProgressBar m_progressBar;
    private float m_timer;
    private float m_timerImage = 2f;
    private float m_timerFade = 1f;


    [SerializeField] GameObject m_virtualCamera;
    [SerializeField] float[] speedCamera;
    bool m_cameraIsArrived = true;
    //--// Modif Bastien
    //public List<InputDevice> m_deviceConnected = new List<InputDevice>();
    public List<DataManager.Skin> m_playerJoin = new List<DataManager.Skin>();
    public List<string> m_playerSColor = new List<string>();
    // Test Affichage pour le joignage de joueur
    public List<GameObject> uiTextPlayerNum = new List<GameObject>();
    public List<GameObject> uiTextPlayerMeshNum = new List<GameObject>();
    //Creation du tableau qui contient toutes les couleurs
    string[] sColors = { "Red", "Blue", "Green", "Yellow" };
    // Idem
    public GameObject mainPrefab;
    int m_nbDevices;
    //--//


    float m_cooldownClique = 0.0f;
    float m_cooldownCliqueMax = 0.5f;

    public List<Transform> tabLookAt = new List<Transform>();
    public List<int> tabIndexToPath = new List<int>();
    int previousTransform = 0;
    int nextTransform = 0;
    float speedCam = 0;

    //Cinematique
    public VideoPlayer m_studioVideo;
    private float m_cinematiqueVideoLenght;
    private float m_cinematiqueVideoTimer;
    bool cinematiqueLaunched;

    //TutoBase
    public VideoPlayer m_tutoBaseVideo;
    private float m_tutoBaseVideoLenght;
    private float m_tutoBaseVideoTimer;
    bool m_tutoBaseLaunched;
   public GameObject m_infoTutoGO;

    ////TutoAdvanced
    //public VideoPlayer m_tutoAdvancedVideo;
    //private float m_tutoAdvancedVideoLenght;
    //private float m_tutoAdvancedVideoTimer;
    //bool m_tutoAdvancedLaunched;

    //permet d'obtenir la prochaine couleur disponible, en renseignant ou non une couleur actuel en paramettres
    private string GetNextFreeColor(string _sCurrentColor)
    {
        //Si _sColor est null on commence du premier de la liste /Si il y a une couleur disponible
        if (_sCurrentColor == null)
        {
            //pour chaque couleur
            for (int iColor = 0; iColor < 4; iColor++)
            {
                bool free = true;
                //on regarde Dans tous les players si elle est deja associé
                for (int k = 0; k < m_playerSColor.Count; k++)
                {
                    //on regarde si elle est associé
                    if (sColors[iColor] == m_playerSColor[k])
                    {
                        //Si c'est le cas on reconnais cette couleur comme deja prise
                        free = false;
                    }
                }
                if (free)
                {
                    //Debug.Log("color : " + sColors[iColor]);
                    return sColors[iColor];
                }
            }
            //Si on ne trouve pas de couleur dispo
            Debug.Log("Error color");
            return null;

        }

        //Sinon on commence à partir de la couleur suivante
        else
        {
            //On récupère l'id de la couleur actuel
            int currentIdColor = 0;
            switch (_sCurrentColor)
            {
                case "Red":
                    currentIdColor = 0;
                    break;
                case "Blue":
                    currentIdColor = 1;
                    break;
                case "Green":
                    currentIdColor = 2;
                    break;
                case "Yellow":
                    currentIdColor = 3;
                    break;
                default:
                    Debug.Log("error switch");
                    break;
            }
            // Debug.Log("currentIdColor = " + currentIdColor);

            //on compare chaque element de la table (en commenceant par le suivant)
            for (int i = currentIdColor + 1; i < (currentIdColor + 3); i++)
            {
                // Debug.Log("i % 4 : " + i % 4);
                bool free = true;
                //on regarde Dans tous les players si elle est deja associé
                for (int k = 0; k < m_playerSColor.Count; k++)
                {
                    //on regarde si elle est associé
                    if (sColors[i % 4] == m_playerSColor[k])
                    {
                        //Si c'est le cas on reconnais cette couleur comme deja prise
                        free = false;
                    }
                    else
                    {
                        //  Debug.Log("is free");
                    }

                }
                if (free)
                {
                    //  Debug.Log("color : " + sColors[i % 4]);
                    return sColors[i % 4];
                }
            }
            Debug.Log("aucune couleur dispo");
            return _sCurrentColor;
        }


    }

    //permet d'obtenir la précédente couleur disponible, en renseignant ou non une couleur actuel en paramettres
    private string GetPreviousFreeColor(string _sCurrentColor)
    {
        //Si _sColor est null on commence du premier de la liste /Si il y a une couleur disponible
        if (_sCurrentColor == null)
        {


            ////pour chaque couleur
            //for (int iColor = 0; iColor < 4; iColor++)
            //{
            //    bool free = true;
            //    //on regarde Dans tous les players si elle est deja associé
            //    for (int k = 0; k < m_playerSColor.Count; k++)
            //    {
            //        //on regarde si elle est associé
            //        if (sColors[iColor] == m_playerSColor[k])
            //        {
            //            //Si c'est le cas on reconnais cette couleur comme deja prise
            //            free = false;
            //        }
            //    }
            //    if (free)
            //    {
            //        Debug.Log("color : " + sColors[iColor]);
            //        return sColors[iColor];
            //    }
            //}
            //Si on ne trouve pas de couleur dispo
            Debug.Log("Error color");
            return null;

        }

        //Sinon on commence à partir de la couleur suivante
        else
        {
            //On récupère l'id de la couleur actuel
            int currentIdColor = 0;
            switch (_sCurrentColor)
            {
                case "Red":
                    currentIdColor = 0;
                    break;
                case "Blue":
                    currentIdColor = 1;
                    break;
                case "Green":
                    currentIdColor = 2;
                    break;
                case "Yellow":
                    currentIdColor = 3;
                    break;
                default:
                    Debug.Log("error switch");
                    break;
            }
            // Debug.Log("currentIdColor = " + currentIdColor);
            //pour  id == 0
            //on compare chaque element de la table (en commenceant par le precedent)
            for (int i = currentIdColor - 1; i > (currentIdColor - 3); i--)
            {
                // Debug.Log("i : " + i);
                int index;
                if (i < 0)
                {
                    index = 4 + i;
                }
                else
                {
                    index = i;
                }


                //  Debug.Log("index : " + index);
                bool free = true;
                //on regarde Dans tous les players si elle est deja associé
                for (int k = 0; k < m_playerSColor.Count; k++)
                {
                    //on regarde si elle est associé
                    if (sColors[index] == m_playerSColor[k])
                    {
                        //Si c'est le cas on reconnais cette couleur comme deja prise
                        free = false;
                    }
                    else
                    {
                        // Debug.Log("is free");
                    }

                }
                if (free)
                {
                    //Debug.Log("color : " + sColors[index]);
                    return sColors[index];
                }
            }
            Debug.Log("aucune couleur dispo");
            return _sCurrentColor;
        }


    }

    bool canClick()
    {
        if (m_cooldownClique <= 0.0f)
        {
            m_cooldownClique = m_cooldownCliqueMax;
            return true;
        }
        else
        {
            return false;
        }
    }
    protected override void Start()
    {
        DataManager.Instance.m_prefab.Clear();
        DataManager.Instance.m_playerSColor.Clear();
        // m_pMenuManager = FindObjectOfType<MenuManager>();
        //Pour revenir au menu de selection des skins si on sort d'une partie
        if (DataManager.Instance.m_gameBeenLaunched)
        {
            m_eMenuState = MenuState.SkinSelection;
            m_FolderHome.SetActive(false);
            m_FolderModeSelection.SetActive(false);
            m_FolderSkinSelection.SetActive(true);
            m_FolderMapSelection.SetActive(false);
            m_folderSettingsScreen.SetActive(false);
            SoundManager.Instance.StopAllSound();
            SoundManager.Instance.MenuMusicPlay(gameObject);
            // PrintState();
        }
        else
        {
            m_eMenuState = MenuState.Home;
            m_FolderHome.SetActive(true);
            m_FolderModeSelection.SetActive(false);
            m_FolderSkinSelection.SetActive(false);
            m_FolderMapSelection.SetActive(false);
            m_folderSettingsScreen.SetActive(false);
        }

        //--// Modif Bastien
        m_nbDevices = DataManager.Instance.m_deviceConnected.Count;
        //--//
        m_gameBeenLaunched = false;

        //Choix du screen de chargement
        int RandomScreenId = Random.Range(0, m_randomScreenSprites.Length);
        m_loadScreen.sprite = m_randomScreenSprites[RandomScreenId];


        //Cinematique
        m_cinematiqueVideoLenght = (float)m_studioVideo.length;
        m_cinematiqueVideoTimer = 0.0f;
        cinematiqueLaunched = false;
        //TutoBase
        m_tutoBaseVideoLenght = (float)m_tutoBaseVideo.length;
        m_tutoBaseVideoTimer = 0.0f;
        m_tutoBaseLaunched = false;
        //TutoAdvanced
        //m_tutoAdvancedVideoLenght = (float)m_tutoAdvancedVideo.length;
        //m_tutoAdvancedVideoTimer = 0.0f;
        //m_tutoAdvancedLaunched = false;
        //Timer click
        m_cooldownClique = 0.0f;
    }

    void PlayCinematique()
    {
        SoundManager.Instance.MenuMusicStop(gameObject);
        SoundManager.Instance.TrailerMusicPlay(gameObject);
        m_studioVideo.enabled = true;
        m_studioVideo.Play();
        cinematiqueLaunched = true;
        m_cinematiqueVideoTimer = 0;
    }
    void StopCinematique()
    {
        SoundManager.Instance.TrailerMusicStop(gameObject);
        SoundManager.Instance.MenuMusicPlay(gameObject);
        m_studioVideo.enabled = false;
        m_studioVideo.Stop();
        cinematiqueLaunched = false;
        m_cinematiqueVideoTimer = 0;
    }
    void PlayTutoBase()
    {
        // SoundManager.Instance.TrailerMusicPlay(gameObject);
        m_tutoBaseVideo.enabled = true;
        m_tutoBaseVideo.Play();
        m_tutoBaseLaunched = true;
        m_tutoBaseVideoTimer = 0;

        m_folderUIControllerDetected.SetActive(false);
        m_infoTutoGO.SetActive(false);
    }
    void StopTutoBase()
    {
        // SoundManager.Instance.TrailerMusicStop(gameObject);
        m_tutoBaseVideo.enabled = false;
        m_tutoBaseVideo.Stop();
        m_tutoBaseLaunched = false;
        m_tutoBaseVideoTimer = 0;
        m_folderUIControllerDetected.SetActive(true);
        m_infoTutoGO.SetActive(true);
    }
    //void PlayTutoAdvanced()
    //{
    //    // SoundManager.Instance.TrailerMusicPlay(gameObject);
    //    m_tutoAdvancedVideo.enabled = true;
    //    m_tutoAdvancedVideo.Play();
    //    m_tutoAdvancedLaunched = true;
    //    m_tutoAdvancedVideoTimer = 0;
    //    m_folderUIControllerDetected.SetActive(false);
    //}
    //void StopTutoAdvanced()
    //{
    //    // SoundManager.Instance.TrailerMusicStop(gameObject);
    //    m_tutoAdvancedVideo.enabled = false;
    //    m_tutoAdvancedVideo.Stop();
    //    m_tutoAdvancedLaunched = false;
    //    m_tutoAdvancedVideoTimer = 0;
    //    m_folderUIControllerDetected.SetActive(true);
    //}
    protected override void Update()
    {
        //--// Modif Bastien
        if (m_nbDevices != InputSystem.devices.Count)
        {
            DataManager.Instance.m_deviceConnected.Clear();
            //m_playerSColor.Clear();
            //Debug.Log("test1");
            foreach (InputDevice iD in InputSystem.devices)
            {
                DataManager.Instance.m_deviceConnected.Add(iD);
                //  Debug.Log("test2");
                InputManager.Instance.AssignControl(iD.name);
            }

            foreach(InputDevice ID in InputSystem.disconnectedDevices)
            {
                if(m_playerJoin.Count > 0)
                {
                    for (int j = 0; j < m_playerJoin.Count; j++)
                    {
                        if (m_playerJoin[j].device == ID)
                        {
                            m_playerJoin.RemoveAt(j);
                            m_playerSColor.RemoveAt(j);
                            m_skinSelectionModelManager.m_needUpdate = true;
                            break;
                        }
                    }
                }
            }
            
            m_nbDevices = DataManager.Instance.m_deviceConnected.Count;
            InputManager.Instance.ClearSensitivity();
            InputManager.Instance.AssignSensitivity();
        }

        SoundOfNavigation();
        //  Debug.Log(m_eMenuState);
        MoveCameraIntoScene(tabLookAt, tabIndexToPath, m_virtualCamera);

        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }

        m_timer += Time.deltaTime;
        //  m_cinematiqueTimer += Time.deltaTime;

        switch (m_eMenuState)
        {
            case MenuState.Home:
                if (cinematiqueLaunched)
                {
                    m_cinematiqueVideoTimer += Time.deltaTime;
                    if (m_cinematiqueVideoTimer >= m_cinematiqueVideoLenght)
                    {
                        StopCinematique();

                    }
                }
                //pour toutes les manettes co
                for (int i = 0; i < DataManager.Instance.m_deviceConnected.Count; i++)
                {
                    if (m_cameraIsArrived && InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "X", true) && canClick())
                    {
                        PlayCinematique();
                    }
                    if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "B", true) && canClick())
                    {
                        StopCinematique();
                    }
                }
                // m_pMenuManager.UpdateHome();
                //UpdateStateHome();
                break;
            case MenuState.ModeSelection:
                // m_pMenuManager.UpdateModeSelection();
                UpdateStateModeSelection();
                break;
            case MenuState.SettingsScreen:
                //  m_pMenuManager.UpdateSkinSelection();
                UpdateStateSettingsSelection();
                break;
            case MenuState.SkinSelection:
                //  m_pMenuManager.UpdateSkinSelection();
                UpdateStateSkinSelection();

                break;
            case MenuState.MapSelection:
                //m_pMenuManager.UpdateMapSelection();
                UpdateStateMapSelection();
                break;
            case MenuState.FadeOfMapSelection:
                UpdateStateFadeOfMapSelection();
                break;
            case MenuState.FadeToLoad:

                UpdateStateFadeToLoad();
                break;
            case MenuState.Load:
                UpdateStateLoad();
                break;
            case MenuState.FadeOfLoad:

                UpdateStateFadeOfLoad();
                break;

            default:
                break;
        }

        if (m_eMenuState == MenuState.ModeSelection || m_eMenuState == MenuState.SettingsScreen || m_eMenuState == MenuState.SkinSelection)
        {
            //pour toutes les manettes co
            for (int i = 0; i < DataManager.Instance.m_deviceConnected.Count; i++)
            {
                if (m_cameraIsArrived && InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "Start", true) && canClick())
                {
                    switch (DataManager.Instance.listMeshString.Count)
                    {
                        case 1:
                            if (m_playerJoin.Count < 4 && (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT || DataManager.Instance.m_gameMode == DataManager.GameMode.EMPTY))
                            {
                                if (!m_playerJoin.Contains(new DataManager.Skin(DataManager.Instance.m_deviceConnected[i], mainPrefab, DataManager.Instance.listMeshString[0])))
                                {
                                    //Valeur par defaut
                                    // Debug.Log("free color " + GetNextFreeColor(null));
                                    m_playerJoin.Add(new DataManager.Skin(DataManager.Instance.m_deviceConnected[i], mainPrefab, DataManager.Instance.listMeshString[0]));
                                    m_playerSColor.Add(GetNextFreeColor(null));
                                    m_skinSelectionModelManager.m_needUpdate = true;
                                    SoundManager.Instance.InitPlayerPlay(gameObject);

                                }
                                else
                                {
                                    for (int j = 0; j < m_playerJoin.Count; j++)
                                    {
                                        if (m_playerJoin[j].device == DataManager.Instance.m_deviceConnected[i])
                                        {
                                            //  Debug.Log(m_playerJoin[j].device.name);
                                            m_playerJoin.RemoveAt(j);
                                            m_playerSColor.RemoveAt(j);
                                            m_skinSelectionModelManager.m_needUpdate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT || DataManager.Instance.m_gameMode == DataManager.GameMode.EMPTY)
                            {
                                for (int j = 0; j < m_playerJoin.Count; j++)
                                {
                                    if (m_playerJoin[j].device == DataManager.Instance.m_deviceConnected[i])
                                    {
                                        //Debug.Log(m_playerJoin[j].device.name);
                                        m_playerJoin.RemoveAt(j);
                                        m_playerSColor.RemoveAt(j);
                                        m_skinSelectionModelManager.m_needUpdate = true;
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    switch (DataManager.Instance.listMeshString.Count)
                    {
                        case 1:
                            if (m_playerJoin.Count < 1 && DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE)
                            {
                                if (!m_playerJoin.Contains(new DataManager.Skin(DataManager.Instance.m_deviceConnected[i], mainPrefab, DataManager.Instance.listMeshString[0])))
                                {

                                    // Debug.Log("free color 2 " + GetNextFreeColor(null));
                                    //Valeur par defaut
                                    m_playerJoin.Add(new DataManager.Skin(DataManager.Instance.m_deviceConnected[i], mainPrefab, DataManager.Instance.listMeshString[0]));
                                    m_playerSColor.Add(GetNextFreeColor(null));
                                    m_skinSelectionModelManager.m_needUpdate = true;
                                    SoundManager.Instance.InitPlayerPlay(gameObject);
                                }
                                else
                                {
                                    for (int j = 0; j < m_playerJoin.Count; j++)
                                    {
                                        if (m_playerJoin[j].device == DataManager.Instance.m_deviceConnected[i])
                                        {
                                            // Debug.Log(m_playerJoin[j].device.name);
                                            m_playerJoin.RemoveAt(j);
                                            m_playerSColor.RemoveAt(j);
                                            m_skinSelectionModelManager.m_needUpdate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE)
                            {
                                for (int j = 0; j < m_playerJoin.Count; j++)
                                {
                                    if (m_playerJoin[j].device == DataManager.Instance.m_deviceConnected[i])
                                    {
                                        // Debug.Log(m_playerJoin[j].device.name);
                                        m_playerJoin.RemoveAt(j);
                                        m_playerSColor.RemoveAt(j);
                                        m_skinSelectionModelManager.m_needUpdate = true;
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    //Si aucun joueur n'est ajouté
                    if (m_playerJoin.Count == 0)
                    {
                        uiTextPlayerNum[0].SetActive(false);
                        uiTextPlayerMeshNum[0].SetActive(false);
                        uiTextPlayerMeshNum[0].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[1].SetActive(false);
                        uiTextPlayerMeshNum[1].SetActive(false);
                        uiTextPlayerMeshNum[1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[2].SetActive(false);
                        uiTextPlayerMeshNum[2].SetActive(false);
                        uiTextPlayerMeshNum[2].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[3].SetActive(false);
                        uiTextPlayerMeshNum[3].SetActive(false);
                        uiTextPlayerMeshNum[3].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
                    }
                    //Si un joueur est ajouté
                    else if (m_playerJoin.Count == 1)
                    {
                        uiTextPlayerNum[0].SetActive(true);
                        uiTextPlayerMeshNum[0].SetActive(true);
                        uiTextPlayerMeshNum[0].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[1].SetActive(false);
                        uiTextPlayerMeshNum[1].SetActive(false);
                        uiTextPlayerMeshNum[1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[2].SetActive(false);
                        uiTextPlayerMeshNum[2].SetActive(false);
                        uiTextPlayerMeshNum[2].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                        uiTextPlayerNum[3].SetActive(false);
                        uiTextPlayerMeshNum[3].SetActive(false);
                        uiTextPlayerMeshNum[3].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
                    }

                    //Pas utile si on est en mode survie (un joueur)
                    if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT)
                    {
                        if (m_playerJoin.Count == 2)
                        {
                            uiTextPlayerNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[2].SetActive(false);
                            uiTextPlayerMeshNum[2].SetActive(false);
                            uiTextPlayerMeshNum[2].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[3].SetActive(false);
                            uiTextPlayerMeshNum[3].SetActive(false);
                            uiTextPlayerMeshNum[3].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
                        }
                        //Si trois joueurs sont ajoutés
                        else if (m_playerJoin.Count == 3)
                        {
                            uiTextPlayerNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[2].SetActive(true);
                            uiTextPlayerMeshNum[2].SetActive(true);
                            uiTextPlayerMeshNum[2].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[3].SetActive(false);
                            uiTextPlayerMeshNum[3].SetActive(false);
                            uiTextPlayerMeshNum[3].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
                        }
                        //Si quatre joueurs sont ajoutés
                        else if (m_playerJoin.Count == 4)
                        {
                            uiTextPlayerNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].SetActive(true);
                            uiTextPlayerMeshNum[0].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].SetActive(true);
                            uiTextPlayerMeshNum[1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[2].SetActive(true);
                            uiTextPlayerMeshNum[2].SetActive(true);
                            uiTextPlayerMeshNum[2].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];

                            uiTextPlayerNum[3].SetActive(true);
                            uiTextPlayerMeshNum[3].SetActive(true);
                            uiTextPlayerMeshNum[3].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
                        }
                    }
                }



            }

            //pour tous les comptes ajoutés
            for (int i = 0; i < m_playerJoin.Count; i++)
            {
                if (m_cameraIsArrived && InputManager.Instance.isPressed(m_playerJoin[i].device.name, "X", true) && canClick())
                {

                    m_folderUIControllerDetected.GetComponent<UIControleurDetected>().OpenSettingsPlayer(i);
                }
            }


            //foreach (InputDevice id in m_deviceConnected)
            //{
            //    if (m_cameraIsArrived && InputManager.Instance.isPressed(id.name, "Start", true) && canClick())
            //    {
            //        switch (DataManager.Instance.listMeshString.Count)
            //        {
            //            case 1:
            //                if (!m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0])))
            //                {
            //                    //Valeur par defaut
            //                    m_playerJoin.Add(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]));
            //                    uiTextPlayerNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
            //                }
            //                else
            //                {
            //                    for (int i = 0; i < m_playerJoin.Count; i++)
            //                    {
            //                        if (m_playerJoin[i].device == id)
            //                        {
            //                            Debug.Log(m_playerJoin[i].device.name);
            //                            m_playerJoin.RemoveAt(i);
            //                            break;
            //                        }
            //                    }
            //                }
            //                break;
            //            case 2:
            //                if (!m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[1])))
            //                {
            //                    //Valeur par defaut
            //                    m_playerJoin.Add(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]));
            //                    uiTextPlayerNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
            //                }
            //                break;
            //            case 3:
            //                if (!m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[1]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[2])))
            //                {
            //                    //Valeur par defaut
            //                    m_playerJoin.Add(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]));
            //                    uiTextPlayerNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
            //                }
            //                break;
            //            case 4:
            //                if (!m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[1]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[2]))
            //                    && !m_playerJoin.Contains(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[3])))
            //                {
            //                    //Valeur par defaut
            //                    m_playerJoin.Add(new DataManager.Skin(id, mainPrefab, DataManager.Instance.listMeshString[0]));
            //                    uiTextPlayerNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].SetActive(true);
            //                    uiTextPlayerMeshNum[m_playerJoin.Count - 1].GetComponent<TMP_Text>().text = DataManager.Instance.listMeshString[0];
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
        }

    }


    //HOME
    public void ButtonXToContinue()
    {
        if (m_cameraIsArrived && canClick())
        {
            LoadNextState();
        }
    }
    //SELECT MODE
    public void ButtonSelectionAffrontement()
    {
        if (m_cameraIsArrived && canClick())
        {
            DataManager.Instance.m_gameMode = DataManager.GameMode.AFFRONTEMENT;
            LoadNextState();
        }
    }
    public void ButtonSelectionSurvie()
    {
        // Debug.Log("m_playerJoin.Count " + m_playerJoin.Count);
        if (m_cameraIsArrived)
        {

            if (m_playerJoin.Count < 2/* && canClick()*/)
            {

                DataManager.Instance.m_gameMode = DataManager.GameMode.SURVIE;
                LoadNextState();

            }
            else
            {
                m_windowSurvivalPopHup.TriggerDraw();
            }
        }
    }
    public void ButtonQuitter()
    {
        if (m_cameraIsArrived && canClick())
        {
            // Debug.Log("before quit");
            //MESSAGE DE CONFIRMATION POUR QUITTER LE JEU
            Application.Quit();
            // Debug.Log("after quit");
        }
    }

    //public void ButtonSelectionSettings()
    //{
    //    if (m_cameraIsArrived && canClick())
    //    {
    //        m_eMenuState = MenuState.SettingsScreen;
    //        //OUVERTURE DE L'ECRAN SETTING
    //        //  m_bSettingScreenIsOpen = true;
    //        m_folderSettingsScreen.SetActive(true);
    //        m_FolderModeSelection.SetActive(false);

    //    }
    //}

    //SETTINGS SCREEN
    public void ButtonValidationSettings()
    {
        if (m_cameraIsArrived && canClick())
        {
            LoadNextState();

            //m_eMenuState = MenuState.ModeSelection;
            ////fermeture DE L'ECRAN SETTING
            //// m_bSettingScreenIsOpen = false;
            //m_folderSettingsScreen.SetActive(false);
            //m_FolderModeSelection.SetActive(true);

        }
    }

    //SELECT MAP
    public void ButtonMap1()
    {
        if (m_cameraIsArrived && canClick())
        {
            DataManager.Instance.m_mapNum = (int)MapSelectionButton.MAP1;
            LoadNextState();
        }
    }
    public void ButtonMap2()
    {
        if (m_cameraIsArrived && canClick())
        {
            DataManager.Instance.m_mapNum = (int)MapSelectionButton.MAP2;
            LoadNextState();
        }
    }

    void UpdateStateModeSelection()
    {
        //Retour
        if (m_playerJoin.Count > 0)
        {
            if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "B", true) && !m_tutoBaseLaunched /*&& !m_tutoAdvancedLaunched  */&& m_cameraIsArrived && canClick())
            {
                LoadPreviousState();
            }
        }
        else
        {
            if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[0].device.name, "B", true) && !m_tutoBaseLaunched/* && !m_tutoAdvancedLaunched*/ && m_cameraIsArrived && canClick() )
            {
                LoadPreviousState();
            }
        }


        //Tutos
        if (m_tutoBaseLaunched)
        {
            m_tutoBaseVideoTimer += Time.deltaTime;
            if (m_tutoBaseVideoTimer >= m_tutoBaseVideoLenght)
            {
                StopTutoBase();

            }
        }
        //Tutos
        //if (m_tutoAdvancedLaunched)
        //{
        //    m_tutoAdvancedVideoTimer += Time.deltaTime;
        //    if (m_tutoAdvancedVideoTimer >= m_tutoAdvancedVideoLenght)
        //    {
        //        StopTutoAdvanced();

        //    }
        //}
        //pour toutes les manettes co
        for (int i = 0; i < DataManager.Instance.m_deviceConnected.Count; i++)
        {
            if (!m_tutoBaseLaunched /* && !m_tutoAdvancedLaunched*/ && m_cameraIsArrived && InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "LeftBumper", true) && canClick())
            {
                PlayTutoBase();
            }
            //else if (!m_tutoBaseLaunched /*&& !m_tutoAdvancedLaunched*/ && m_cameraIsArrived && InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "RightBumper", true) && canClick())
            //{
            //    PlayTutoAdvanced();
            //}
            if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[i].name, "B", true) && canClick())
            {
                StopTutoBase();
              //  StopTutoAdvanced();
            }
        }
    }
    void UpdateStateSettingsSelection()
    {
        if (m_playerJoin.Count > 0)
        {
            if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "B", true) && canClick() && m_cameraIsArrived)
            {
                LoadPreviousState();
            }
        }
        else
        {
            if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[0].device.name, "B", true) && canClick() && m_cameraIsArrived)
            {
                LoadPreviousState();
            }
        }
    }

    //A modifier pour prendre en compte le mode de jeux
    void UpdateStateSkinSelection()
    {
        // Change skin
        for (int i = 0; i < m_playerJoin.Count; i++)
        {
            if (m_playerJoin[i].device.name != "Keyboard")
            {
                if (InputManager.Instance.isPressed(m_playerJoin[i].device.name, "Pad", true))
                {
                    if (canClick())
                    {
                        if (InputManager.Instance.ValuePAD(m_playerJoin[i].device.name, "Pad").x > 0)
                        {
                            //Changement de la couleur pour la suivante
                            m_playerSColor[i] = GetNextFreeColor(m_playerSColor[i]);
                            m_skinSelectionModelManager.m_needUpdate = true;


                            //DataManager.Skin skin = m_playerJoin[i];
                            //int index = DataManager.Instance.listMeshString.IndexOf(skin.meshName);
                            //if ((index + 1) == DataManager.Instance.listMeshString.Count)
                            //{
                            //    index = -1;
                            //}
                            //index++;
                            //skin.meshName = DataManager.Instance.listMeshString[index];
                            //uiTextPlayerMeshNum[i].GetComponent<TMP_Text>().text = skin.meshName;
                            //m_playerJoin[i] = skin;
                        }
                        else if (InputManager.Instance.ValuePAD(m_playerJoin[i].device.name, "Pad").x < 0)
                        {

                            //Changement de la couleur pour la Precedente
                            m_playerSColor[i] = GetPreviousFreeColor(m_playerSColor[i]);
                            m_skinSelectionModelManager.m_needUpdate = true;

                            //DataManager.Skin skin = m_playerJoin[i];
                            //int index = DataManager.Instance.listMeshString.IndexOf(skin.meshName);
                            //if (index == 0)
                            //{
                            //    index = DataManager.Instance.listMeshString.Count;
                            //}
                            //index--;
                            //skin.meshName = DataManager.Instance.listMeshString[index];
                            //uiTextPlayerMeshNum[i].GetComponent<TMP_Text>().text = skin.meshName;
                            //m_playerJoin[i] = skin;
                        }
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("ArrowHorizontal"))
                {
                    if (Input.GetAxisRaw("ArrowHorizontal") > 0)
                    {
                        //DataManager.Skin skin = m_playerJoin[i];
                        //int index = DataManager.Instance.listMeshString.IndexOf(skin.meshName);
                        //if ((index + 1) == DataManager.Instance.listMeshString.Count)
                        //{
                        //    index = -1;
                        //}
                        //index++;
                        //skin.meshName = DataManager.Instance.listMeshString[index];
                        //uiTextPlayerMeshNum[i].GetComponent<TMP_Text>().text = skin.meshName;
                        //m_playerJoin[i] = skin;

                        //Changement de la couleur pour la suivante
                        m_playerSColor[i] = GetNextFreeColor(m_playerSColor[i]);
                        m_skinSelectionModelManager.m_needUpdate = true;
                    }
                    else if (Input.GetAxisRaw("ArrowHorizontal") < 0)
                    {
                        //DataManager.Skin skin = m_playerJoin[i];
                        //int index = DataManager.Instance.listMeshString.IndexOf(skin.meshName);
                        //if (index == 0)
                        //{
                        //    index = DataManager.Instance.listMeshString.Count;
                        //}
                        //index--;
                        //skin.meshName = DataManager.Instance.listMeshString[index];
                        //uiTextPlayerMeshNum[i].GetComponent<TMP_Text>().text = skin.meshName;
                        //m_playerJoin[i] = skin;


                        //Changement de la couleur pour la Precedente
                        m_playerSColor[i] = GetPreviousFreeColor(m_playerSColor[i]);
                        m_skinSelectionModelManager.m_needUpdate = true;
                    }
                }
            }
        }

        if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT)
        {

            // modif bastien
            if (m_playerJoin.Count > 1)// fin modif bastien
            {
                // Get canvas skinSelection in menu
                m_FolderSkinSelection.transform.GetChild(3).gameObject.SetActive(true);

                // Get sprite "press X" in menu
                m_FolderSkinSelection.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);


                //Si la première manette ayant rejoind clique sur A
                if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "A", true) && m_cameraIsArrived && canClick())
                {
                    for (int i = 0; i < m_playerJoin.Count; i++)
                    {
                        DataManager.Instance.m_prefab.Add(m_playerJoin[i]);
                        DataManager.Instance.m_playerSColor.Add(m_playerSColor[i]);
                    }
                    LoadNextState();
                }
            }
            else if (m_playerJoin.Count == 0)
            {
                // Get canvas skinSelection in menu
                m_FolderSkinSelection.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                // Get canvas skinSelection in menu
                m_FolderSkinSelection.transform.GetChild(3).gameObject.SetActive(true);

                // Get sprite "press X" in menu
                m_FolderSkinSelection.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            }
        }
        else if (DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE)
        {
            // modif bastien
            if (m_playerJoin.Count == 1)// fin modif bastien
            {
                // Get canvas skinSelection in menu
                m_FolderSkinSelection.transform.GetChild(3).gameObject.SetActive(true);

                //Debug.Log("player count == 0");
                //Si la première manette ayant rejoind clique sur A
                if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "A", true) && m_cameraIsArrived && canClick())
                {
                    for (int i = 0; i < m_playerJoin.Count; i++)
                    {
                        DataManager.Instance.m_prefab.Add(m_playerJoin[i]);
                        DataManager.Instance.m_playerSColor.Add(m_playerSColor[i]);
                    }
                    LoadNextState();
                }
            }
            else
            {
                // Get canvas skinSelection in menu
                m_FolderSkinSelection.transform.GetChild(3).gameObject.SetActive(false);
            }
        }

        if (m_playerJoin.Count > 0)
        {
            if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "B", true) && canClick() && m_cameraIsArrived)
            {
                LoadPreviousState();
            }
        }
        else
        {
            if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[0].device.name, "B", true) && canClick() && m_cameraIsArrived)
            {
                LoadPreviousState();
            }
        }
    }

    void UpdateStateMapSelection()
    {
        if (m_cameraIsArrived && !m_gameBeenLaunched)
        {
            if (m_playerJoin.Count > 0)
            {
                if (InputManager.Instance.isPressed(m_playerJoin[0].device.name, "B", true) && canClick())
                {
                    LoadPreviousState();
                }
            }
            else
            {
                if (InputManager.Instance.isPressed(DataManager.Instance.m_deviceConnected[0].device.name, "B", true) && canClick())
                {
                    LoadPreviousState();
                }
            }
        }
    }


    void UpdateStateFadeOfMapSelection()
    {
        Color color = m_BlackScreen.GetComponent<Image>().color;
        if (m_timer >= m_timerFade)
        {
            color.a = 1;
            m_timer = 0;
            LoadNextState();
            m_loadScreen.gameObject.SetActive(true);
        }
        else
        {

            color.a = 1 * m_timer / m_timerFade;
        }
        m_BlackScreen.GetComponent<Image>().color = color;
    }

    void UpdateStateFadeToLoad()
    {
        Color color = m_loadScreen.GetComponent<Image>().color;
        if (m_timer >= m_timerFade)
        {
            color.a = 1;
            m_timer = 0;
            LoadNextState();
            m_progressBar.gameObject.SetActive(true);
        }
        else
        {
            color.a = 1 * m_timer / m_timerFade;
        }
        m_loadScreen.GetComponent<Image>().color = color;
    }

    void UpdateStateLoad()
    {
        if (m_timer >= m_timerImage)
        {
            FindObjectOfType<LoadingScreenMenu>().m_IntroManagIsAbleToFade = true;

            if (FindObjectOfType<LoadingScreenMenu>().m_LSisAbleToFade)
            {
                m_timer = 0;
                m_progressBar.SetProgress(1f);
                LoadNextState();
            }
        }
        else
        {
            m_progressBar.SetProgress(FindObjectOfType<LoadingScreenMenu>().m_asyncProgress);
        }

    }

    void UpdateStateFadeOfLoad()
    {

        //Debug.Log("UpdateStateFadeOfLoad");
        Color color = m_loadScreen.GetComponent<Image>().color;
        if (m_timer >= m_timerFade && !m_gameBeenLaunched)
        {
            m_gameBeenLaunched = true;
            color.a = 0;
            FindObjectOfType<LoadingScreenMenu>().m_IntroManagIsAbleToLoadScene = true;
        }
        else
        {
            color.a = 1 - 1 * m_timer / m_timerFade;
        }
        m_loadScreen.GetComponent<Image>().color = color;
    }

    void SoundOfNavigation()
    {
        if (m_eMenuState == MenuState.ModeSelection || m_eMenuState == MenuState.SettingsScreen || m_eMenuState == MenuState.MapSelection)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SoundManager.Instance.MenuSelect(gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                SoundManager.Instance.MenuSelect(gameObject);
            }

            for (int i = 0; i < DataManager.Instance.m_deviceConnected.Count; i++)
            {
                if (InputManager.Instance.ValueJoyStick(DataManager.Instance.m_deviceConnected[i].device.name, "LeftStick").y > 0.1f && canClick())
                {
                    SoundManager.Instance.MenuSelect(gameObject);
                }
                else if (InputManager.Instance.ValueJoyStick(DataManager.Instance.m_deviceConnected[i].device.name, "LeftStick").y < -0.1f && canClick())
                {
                    SoundManager.Instance.MenuSelect(gameObject);
                }
            }

        }
        if (m_eMenuState == MenuState.SettingsScreen || m_eMenuState == MenuState.MapSelection)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

                SoundManager.Instance.MenuSelect(gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

                SoundManager.Instance.MenuSelect(gameObject);
            }

            for (int i = 0; i < DataManager.Instance.m_deviceConnected.Count; i++)
            {
                if (InputManager.Instance.ValueJoyStick(DataManager.Instance.m_deviceConnected[i].device.name, "LeftStick").x > 0.1f && canClick())
                {
                    SoundManager.Instance.MenuSelect(gameObject);
                }
                else if (InputManager.Instance.ValueJoyStick(DataManager.Instance.m_deviceConnected[i].device.name, "LeftStick").x < -0.1f && canClick())
                {
                    SoundManager.Instance.MenuSelect(gameObject);
                }
            }
        }

    }


    //Button next
    public override void LoadNextState()
    {

        switch (m_eMenuState)
        {
            case MenuState.Home:
                SoundManager.Instance.MenuConfirm(gameObject);
                SoundManager.Instance.MenuMusicPlay(gameObject);
                //Desactive le folder home
                m_FolderHome.SetActive(false);
                //Active le folder mode de jeu
                m_FolderModeSelection.SetActive(true);
                //active l'affiche des manettes detecté
                m_folderUIControllerDetected.SetActive(true);
                //Changement du state
                m_eMenuState = MenuState.ModeSelection;
                break;
            case MenuState.ModeSelection:
                SoundManager.Instance.MenuMusicPlay(gameObject);
                SoundManager.Instance.MenuConfirm(gameObject);
                //Desactive le folder mode de jeu
                m_FolderModeSelection.SetActive(false);
                //Active le folder settings
                m_folderSettingsScreen.SetActive(true);
                //Changement du state
                m_eMenuState = MenuState.SettingsScreen;
                break;
            case MenuState.SettingsScreen:
                SoundManager.Instance.MenuMusicPlay(gameObject);
                SoundManager.Instance.MenuConfirm(gameObject);
                //Desactive le folder actuel
                m_folderSettingsScreen.SetActive(false);
                //Active le folder suivant 
                m_FolderSkinSelection.SetActive(true);

                m_skinSelectionModelManager.m_needUpdate = true;
                //Changement du state
                m_eMenuState = MenuState.SkinSelection;
                break;
            case MenuState.SkinSelection:
                SoundManager.Instance.MenuMusicPlay(gameObject);
                SoundManager.Instance.MenuConfirm(gameObject);
                //Desactive le folder actuel
                m_FolderSkinSelection.SetActive(false);
                //Active le folder suivant 
                m_FolderMapSelection.SetActive(true);
                //Changement du state
                m_eMenuState = MenuState.MapSelection;
                break;
            case MenuState.MapSelection:
                SoundManager.Instance.MenuMusicPlay(gameObject);
                SoundManager.Instance.MenuConfirm(gameObject);
                //Desactive le folder actuel
                m_FolderMapSelection.SetActive(false);
                //recharge du timer de fade
                m_timer = 0;
                //Desactive l'affiche des manettes detecté
                m_folderUIControllerDetected.SetActive(false);

                //Active l'ecran de fade dédié au chargement et non à la scene
                m_BlackScreen.enabled = true;

                //Changement du state
                m_eMenuState = MenuState.FadeOfMapSelection;
                break;
            case MenuState.FadeOfMapSelection:
                //Changement du state
                m_eMenuState = MenuState.FadeToLoad;
                break;
            case MenuState.FadeToLoad:
                //Changement du state
                m_eMenuState = MenuState.Load;
                break;
            case MenuState.Load:
                //Changement du state
                m_eMenuState = MenuState.FadeOfLoad;
                SoundManager.Instance.MenuMusicStop(gameObject);
                SoundManager.Instance.StopAllSound();
                break;
            default:
                Debug.Log("errorInSwitch");
                break;


        }

        if (m_eMenuState == MenuState.Load)
        {
            LoadingScreenMenu LS = FindObjectOfType<LoadingScreenMenu>();
            if (LS != null && !m_gameBeenLaunched)
            {
                //Pour revenir au menu de selection des skins plus tard
                DataManager.Instance.m_gameBeenLaunched = true;
                m_gameBeenLaunched = true;
                LS.LoadGameScene();
            }
        }

    }
    //Button back
    public void LoadPreviousState()
    {
        if (m_eMenuState > MenuState.Home)
        {
            switch (m_eMenuState)
            {
                case MenuState.ModeSelection:
                    //Active le folder home
                    m_FolderHome.SetActive(true);
                    //Desactive le folder actuel
                    m_FolderModeSelection.SetActive(false);
                    //Desactive l'affiche des manettes detecté
                    m_folderUIControllerDetected.SetActive(false);
                    //Changement du state
                    m_eMenuState = MenuState.Home;

                    //Efface les joueurs inscrit
                    DataManager.Instance.m_prefab.Clear();
                    DataManager.Instance.m_playerSColor.Clear();
                    //Valeur par defaut
                    for (int i = 0; i < m_playerJoin.Count; i++)
                    {
                        uiTextPlayerNum[i].SetActive(false);
                        uiTextPlayerMeshNum[i].SetActive(false);
                    }
                    m_playerJoin.Clear();
                    m_playerSColor.Clear();



                    break;
                case MenuState.SettingsScreen:
                    //Active le folder home
                    m_FolderModeSelection.SetActive(true);
                    //Desactive le folder actuel
                    m_folderSettingsScreen.SetActive(false);
                    //Changement du state
                    m_eMenuState = MenuState.ModeSelection;
                    //Efface les joueurs inscrit
                    DataManager.Instance.m_prefab.Clear();
                    DataManager.Instance.m_playerSColor.Clear();
                    //Valeur par defaut
                    for (int i = 0; i < m_playerJoin.Count; i++)
                    {
                        uiTextPlayerNum[i].SetActive(false);
                        uiTextPlayerMeshNum[i].SetActive(false);
                    }
                    m_playerJoin.Clear();
                    m_playerSColor.Clear();

                    DataManager.Instance.m_gameMode = DataManager.GameMode.EMPTY;

                    break;
                case MenuState.SkinSelection:
                    //Active le folder SettingsScreen
                    m_folderSettingsScreen.SetActive(true);
                    //Desactive le folder actuel
                    m_FolderSkinSelection.SetActive(false);
                    //Changement du state
                    m_eMenuState = MenuState.SettingsScreen;

                    break;
                case MenuState.MapSelection:
                    //Active le folder precedent
                    m_FolderSkinSelection.SetActive(true);
                    //Desactive le folder actuel
                    m_FolderMapSelection.SetActive(false);
                    ////active l'affiche des manettes detecté
                    //m_folderUIControllerDetected.SetActive(true);
                    //Changement de state
                    m_eMenuState = MenuState.SkinSelection;

                    //reset du timer click
                    m_cooldownClique = m_cooldownCliqueMax;


                    //Efface les joueurs inscrit
                    DataManager.Instance.m_prefab.Clear();
                    DataManager.Instance.m_playerSColor.Clear();
                    //Valeur par defaut
                    for (int i = 0; i < m_playerJoin.Count; i++)
                    {
                        uiTextPlayerNum[i].SetActive(false);
                        uiTextPlayerMeshNum[i].SetActive(false);
                    }
                    m_playerJoin.Clear();
                    m_playerSColor.Clear();

                    m_skinSelectionModelManager.m_needUpdate = true;
                    break;

                default:
                    Debug.Log("errorInSwitch");
                    break;
            }
        }

    }
    public override void PrintState()
    {
        //Debug.Log("menu state : " + m_menuState.ToString());
    }


    float timer = 0;
    public void MoveCameraIntoScene(List<Transform> _tabLookAt, List<int> _tabIndexToPath, GameObject _virtualCam)
    {
        foreach (Transform t in _tabLookAt)
        {
            if (t.parent.gameObject.activeSelf)
            {
                _virtualCam.GetComponent<CinemachineVirtualCamera>().LookAt = t;

                foreach (int i in _tabIndexToPath)
                {
                    if (_tabLookAt.IndexOf(t) == _tabIndexToPath.IndexOf(i))
                    {
                        nextTransform = i;

                        if (previousTransform > nextTransform)
                        {
                            //Debug.Log("Next");
                            //if (nextTransform < speedCamera.Length)
                            {
                                speedCam = speedCamera[_tabIndexToPath.IndexOf(nextTransform)];
                                //  Debug.Log("speedCamNext " + speedCam);
                            }
                        }
                        else if (previousTransform < nextTransform)
                        {
                            //Debug.Log("Previous");
                            //if (previousTransform < speedCamera.Length)
                            {
                                speedCam = speedCamera[_tabIndexToPath.IndexOf(previousTransform)];
                                //                                Debug.Log("speedCamPrevious " + speedCam);
                            }
                        }
                    }
                }
            }
        }
        float pathPosition = _virtualCam.GetComponent<CinemachineDollyCart>().m_Position;
        pathPosition = Mathf.Lerp(previousTransform, nextTransform, timer);
        _virtualCam.GetComponent<CinemachineDollyCart>().m_Position = pathPosition;

        if (_virtualCam.GetComponent<CinemachineDollyCart>().m_Position == nextTransform)
        {
            m_cameraIsArrived = true;
            timer = 0;
            previousTransform = nextTransform;
        }
        else
        {
            m_cameraIsArrived = false;
            timer += speedCam * Time.deltaTime;
        }
    }
}
