using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Intro,
    Game,
    Scoreboard
}

public class StateGame : State
{
    public GameState m_gameState;
    [SerializeField] GameObject m_FolderIntro;
    [SerializeField] GameObject m_FolderGame;
    [SerializeField] GameObject m_FolderScoreboard;
    [SerializeField] GameObject m_ImageScoreboard;
    [SerializeField] GameObject m_TextScoreboard;
    public Text[] m_TextStatScoreboard;
    [SerializeField] Image m_WinImg;
    [SerializeField] List<Sprite> listWinImg = new List<Sprite>();
    [SerializeField] float timerBetweenDeathAndScoreBoard;

    [SerializeField] public Camp[] camps;

    //Assignation du material de la couleur de la team
    public Material[] m_playerMaterials = new Material[4];

    float m_cooldownClique = 0.0f;
    float m_cooldownCliqueMax = 0.5f;
    private float m_introTimer;
    private float m_introTimerMAx = 2.0f;

    private bool m_isLoadingMenu;

    public List<GameObject> playerGO = new List<GameObject>();

    public List<CinemachineVirtualCamera> listCamTower = new List<CinemachineVirtualCamera>();

    public GameObject camIn3J;

    public float m_foliageCullingDistance;

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

    public static void ChangeLayersRecursively(Transform[] _transform, string name)
    {
        foreach (Transform child in _transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(name);
        }
    }
    protected override void Start()
    {
        //Active le premier folder et desactive les autres
        m_FolderIntro.SetActive(true);
        m_FolderGame.SetActive(false);
        m_FolderScoreboard.SetActive(false);
        //Timer D'intro
        m_introTimer = 0.0f;
        //On recharge le cooldown de clique
        m_cooldownClique = m_cooldownCliqueMax;

        //Pour chaque joueur present dans la liste
        for (int i = 0; i < DataManager.Instance.m_prefab.Count; i++)
        {
            //objet qui stockera le prefab player
            GameObject goLocal = null;

            //On recherche le camp qui correspond à la couleur du joueur traité
            for (int indexCamp = 0; indexCamp < camps.Length; indexCamp++)
            {
                //Si la couleur du camp correspond à la couleur du player
                if (camps[indexCamp].m_sColor == DataManager.Instance.m_playerSColor[i])
                {
                    //Creation du prefab de player
                    goLocal = Instantiate(DataManager.Instance.m_prefab[i].go, camps[indexCamp].spawnPlayer.transform.position, camps[indexCamp].spawnPlayer.transform.rotation);

                    //Set de l'id
                    goLocal.GetComponent<EntityPlayer>().m_playerId = i;
                    //On associe l'id du camp à l'id du player
                    camps[indexCamp].m_campId = i;

                    //Recuperation de la couleur choisi
                    goLocal.GetComponent<EntityPlayer>().m_sColor = DataManager.Instance.m_playerSColor[i];

                    //Link du camp dans le player
                    goLocal.GetComponent<EntityPlayer>().m_linkedCamp = camps[indexCamp];
                    //Link du nexus dans le player
                    goLocal.GetComponent<EntityPlayer>().m_linkedNexus = camps[indexCamp].m_linkedNexus.GetComponent<Nexus>();




                    //Link du player dans le camp
                    camps[indexCamp].m_linkedPlayer = goLocal.GetComponent<EntityPlayer>();
                    //Link du player dans le nexus
                    camps[indexCamp].m_linkedNexus.GetComponent<Nexus>().m_linkedPlayer = goLocal.GetComponent<EntityPlayer>();

                    //On garde Le spawn en memoire;
                    camps[indexCamp].m_linkedPlayer.m_transformToSpawn = camps[indexCamp].spawnPlayer.transform;
                    //on indique que le camp est utilisé et en vie
                    camps[indexCamp].m_isUsed = true;
                    camps[indexCamp].m_isAlive = true;
                }
            }
            //On recupère le meshPrefab
            GameObject modelChoose = DataManager.Instance.listPrefabModel[DataManager.Instance.listMeshString.IndexOf(DataManager.Instance.m_prefab[i].meshName)];
            //on l'instantie
            GameObject localModelChoose = Instantiate(modelChoose, goLocal.transform);
            //changement des layers dans le mesh
            ChangeLayersRecursively(localModelChoose.transform.Find("Group_RIG_CharacterName").GetComponentsInChildren<Transform>(true), "P" + (i + 1));

            //MODIF BEN
            //on applique le layer au parent du prefab du player
            goLocal.layer = LayerMask.NameToLayer("P" + (i + 1));
            //on applique le layer au parent du prefab mesh du player
            localModelChoose.gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));

            // Change Layer of virtualCam from nexus
            GameObject mainNexus = goLocal.GetComponent<EntityPlayer>().m_linkedNexus.gameObject.transform.parent.gameObject;
            mainNexus.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));

            //on applique le layer aux Mesh de collisions
            localModelChoose.transform.Find("Head").gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));
            localModelChoose.transform.Find("Body").gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));

            ////--///
            //Material
            //
            SkinnedMeshRenderer tempSkin = localModelChoose.GetComponentInChildren<Animator>().transform.Find("LOW_grp4group_GEO_player").GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] mats = tempSkin.materials;
            //Assignation du material
            switch (goLocal.GetComponent<EntityPlayer>().m_sColor)
            {
                case "Red":

                    mats[0] = m_playerMaterials[0];
                    mats[1] = m_playerMaterials[0];
                    mats[2] = m_playerMaterials[0];
                    // Debug.Log("material red");
                    break;
                case "Blue":
                    mats[0] = m_playerMaterials[1];
                    mats[1] = m_playerMaterials[1];
                    mats[2] = m_playerMaterials[1];
                    // Debug.Log("material blue");
                    break;
                case "Green":
                    mats[0] = m_playerMaterials[2];
                    mats[1] = m_playerMaterials[2];
                    mats[2] = m_playerMaterials[2];
                    // Debug.Log("material green");
                    break;
                case "Yellow":
                    mats[0] = m_playerMaterials[3];
                    mats[1] = m_playerMaterials[3];
                    mats[2] = m_playerMaterials[3];
                    //Debug.Log("material yellow");
                    break;
                default:
                    break;
            }
            tempSkin.materials = mats;

            //Modif ben fin
            //LAYERS
            // Init layers masks
            LayerMask layerMaskP1 = 0;
            LayerMask layerMaskP2 = 0;
            LayerMask layerMaskP3 = 0;
            LayerMask layerMaskP4 = 0;
            LayerMask layerMaskDecor = 0;
            LayerMask layerMaskTraps = 0;
            LayerMask layerMaskInvoc = 0;
            LayerMask layerMaskDemons = 0;
            //Decor traps, invoc et demons
            layerMaskDecor = LayerMask.GetMask("Decor");
            layerMaskTraps = LayerMask.GetMask("Trap");

            Camera cam = goLocal.GetComponentInChildren<Camera>();
            //modif Bastien
            CinemachineVirtualCamera[] virtualCam = goLocal.GetComponentsInChildren<CinemachineVirtualCamera>();
            virtualCam[0].gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));
            virtualCam[1].gameObject.layer = LayerMask.NameToLayer("P" + (i + 1));
            virtualCam[0].gameObject.GetComponent<CinemachineCollider>().m_CollideAgainst
                = LayerMask.GetMask("Default")
                + LayerMask.GetMask("TransparentFX")
                + LayerMask.GetMask("Ignore Raycast")
                + LayerMask.GetMask("Water")
                + LayerMask.GetMask("UI")
                + LayerMask.GetMask("Cadavre")
                + LayerMask.GetMask("Decor")
                + LayerMask.GetMask("GrassRed")
                + LayerMask.GetMask("GrassBlue")
                + LayerMask.GetMask("GrassGreen")
                + LayerMask.GetMask("GrassYellow")
                + LayerMask.GetMask("Trap");
            virtualCam[1].gameObject.GetComponent<CinemachineCollider>().m_CollideAgainst
                = LayerMask.GetMask("Default")
                + LayerMask.GetMask("TransparentFX")
                + LayerMask.GetMask("Ignore Raycast")
                + LayerMask.GetMask("Water")
                + LayerMask.GetMask("UI")
                + LayerMask.GetMask("Cadavre")
                + LayerMask.GetMask("Decor")
                + LayerMask.GetMask("GrassRed")
                + LayerMask.GetMask("GrassBlue")
                + LayerMask.GetMask("GrassGreen")
                + LayerMask.GetMask("GrassYellow")
                + LayerMask.GetMask("Trap");
            //
            cam.cullingMask =
                (1 << LayerMask.NameToLayer("Default"))
                | (1 << LayerMask.NameToLayer("TransparentFX"))
                | (1 << LayerMask.NameToLayer("Ignore Raycast"))
                | (1 << LayerMask.NameToLayer("Water"))
                | (1 << LayerMask.NameToLayer("UI"))
                | (1 << LayerMask.NameToLayer("P" + (i + 1)))
                | (1 << LayerMask.NameToLayer("Cadavre"))
                | (1 << LayerMask.NameToLayer("Decor"))
                | (1 << LayerMask.NameToLayer("GrassRed"))
                | (1 << LayerMask.NameToLayer("GrassBlue"))
                | (1 << LayerMask.NameToLayer("GrassGreen"))
                | (1 << LayerMask.NameToLayer("GrassYellow"))
                | (1 << LayerMask.NameToLayer("Foliage"))
                | (1 << LayerMask.NameToLayer("Trap"))
                | (1 << LayerMask.NameToLayer("Demons"))
                | (1 << LayerMask.NameToLayer("Invoc"));
            goLocal.GetComponent<TpsController>().device = DataManager.Instance.m_prefab[i].device.name;

            //Culling distance
            float[] distances = new float[32];
            distances[LayerMask.NameToLayer("GrassRed")] = m_foliageCullingDistance;
            distances[LayerMask.NameToLayer("GrassBlue")] = m_foliageCullingDistance;
            distances[LayerMask.NameToLayer("GrassGreen")] = m_foliageCullingDistance;
            distances[LayerMask.NameToLayer("GrassYellow")] = m_foliageCullingDistance;
            cam.layerCullDistances = distances;


            //Modif bastien
            switch (DataManager.Instance.m_prefab.Count)
            {
                case 1:
                    cam.rect =
                        new Rect(0f, 0, 1f, 1f);
                    break;// fin modif bastien
                case 2:
                    goLocal.GetComponent<TpsController>().lookXLimitMax = 90;

                    cam.rect =
                        new Rect(0f, 0.5f - (i * 0.5f), 1f, 0.5f);

                    // Change Position camLeft
                    virtualCam[1].gameObject.transform.localPosition = new Vector3(-0.511f, 0.798786f, -2.564854f);

                    // Change Position camRight
                    CinemachinePath.Waypoint[] waypoint = goLocal.GetComponentInChildren<CinemachinePath>().m_Waypoints;
                    waypoint[1].position = new Vector3(0.5121778f, 1.911786f, -3.119854f);
                    break;
                case 3:
                    cam.rect =
                        new Rect(i % 2 / 2f, -((int)(i / 2) / 2f - 0.5f), 0.5f, 0.5f);
                    break;
                default:
                    cam.rect =
                        new Rect(i % 2 / 2f, -((int)(i / 2) / 2f - 0.5f), 0.5f, 0.5f);
                    break;
            }
            // Change FOV before spawn player
            float currentPos = goLocal.GetComponent<TpsController>().defaultFOV * (cam.rect.height / cam.rect.width);
            virtualCam[0].m_Lens.FieldOfView = currentPos;
            virtualCam[1].m_Lens.FieldOfView = currentPos;

            goLocal.name = "Player " + (i + 1);

            goLocal.GetComponent<TpsController>().camTower = listCamTower[i];

            //Pos of the reference target
            GameObject m_targetInController = goLocal.GetComponent<GetAimTargetInController>().m_AIMtarget;
            //master target in mesh prefab
            AIMTargetMaster m_targetMasterInMesh = localModelChoose.GetComponent<GetAimTargetMaster>().m_targetMaster;
            //reference it in the mesh prefab
            m_targetMasterInMesh.m_TargetAIMInPrefab = m_targetInController;


            ////Association de la cible présente sur le controlleur au mesh
            //localModelChoose.GetComponent<GetAimTargetMaster>().m_targetMaster.m_TargetAIMInPrefab = GetComponent<GetAimTargetInController>().m_AIMtarget;

            ////Association de la target head sur le mesh
            //localModelChoose.GetComponent<GetHeadPivot>().m_controller.GetComponent<TargetAIMhead>().m_target = goLocal.GetComponent<GetHeadPivot>().m_controller;
            
            // En mode 3J Cam en vue d'aigle
            if(DataManager.Instance.m_prefab.Count == 3)
            {
                camIn3J.SetActive(true);
            }
            else
            {
                camIn3J.SetActive(false);
            }

            playerGO.Add(goLocal);
        }
        //Modif ben
        //detruit le nexus si le camp n'est pas utilisé
        for (int i = 0; i < camps.Length; i++)
        {
            //  Debug.Log("dans la boucle");
            if (!camps[i].m_isUsed)
            {
                camps[i].m_needToDestroyNexus = true;
                // Debug.Log("suppr");
            }

        }

        m_isLoadingMenu = false;
        //fin Modif ben
        SoundManager.Instance.GameMusicPlay = true;
    }
    protected override void Update()
    {
        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }
        switch (m_gameState)
        {
            case GameState.Intro:
                // m_gameManager.UpdateIntro();
                UpdateIntro();
                break;
            case GameState.Game:

                // m_gameManager.UpdateGame();
                break;
            case GameState.Scoreboard:
                // m_gameManager.UpdateScoreboard();
                //  Debug.Log("in scorboard state");
                SoundManager.Instance.GameMusicPlay = false;
                if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT)
                {
                    SoundManager.Instance.MusicVictoryPlay = true;
                }
                if (DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE)
                {
                    SoundManager.Instance.MusicDefeatPlay = true;
                }

                timerBetweenDeathAndScoreBoard -= Time.deltaTime;

                if (timerBetweenDeathAndScoreBoard <= 0)
                {
                    // Change color alpha of scoreBoard and text
                    Color color = m_ImageScoreboard.GetComponent<Image>().color;
                    color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 1);
                    m_ImageScoreboard.GetComponent<Image>().color = color;
                    color = m_TextScoreboard.GetComponent<Image>().color;
                    color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 1);
                    m_TextScoreboard.GetComponent<Image>().color = color;

                    foreach (Text textStat in m_TextStatScoreboard)
                    {
                        color = textStat.color;
                        color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 1);
                        textStat.color = color;
                    }

                    if (m_ImageScoreboard.GetComponent<Image>().color.a >= 0.75f
                        && m_TextScoreboard.GetComponent<Image>().color.a >= 0.75f)
                    {
                        if (InputManager.Instance.isPressed(DataManager.Instance.m_prefab[0].device.name, "A", true))
                        {
                            if (canClick())
                            {
                                LoadNextState();
                            }
                        }
                    }
                }
                break;
        }
    }

    void UpdateIntro()
    {
        for (int i = 0; i < playerGO.Count; i++)
        {
            if (!playerGO[i].GetComponent<TpsController>().dollyCart.enabled && i == (playerGO.Count - 1))
            {
                LoadNextState();
            }
        }
    }


    public override void PrintState()
    {
        // Debug.Log("game state : " + m_gameState.ToString());
    }
    public override void LoadNextState()
    {
        if (m_gameState < GameState.Scoreboard)
        {
            switch (m_gameState)
            {
                case GameState.Intro:
                    m_FolderIntro.SetActive(false);
                    m_FolderGame.SetActive(true);
                    break;
                case GameState.Game:
                    m_FolderGame.SetActive(false);
                    m_FolderScoreboard.SetActive(true);
                    break;
            }
            m_gameState++;
            PrintState();
        }
        else if (m_gameState == GameState.Scoreboard)
        {
            if (!m_isLoadingMenu)
            {
                LoadingScreenGame LS = FindObjectOfType<LoadingScreenGame>();
                if (LS != null)
                {
          
                    LS.LoadMenuScene();
                    m_isLoadingMenu = true;
                }
            }
        }
    }

    public void UpdateAliveCamp()
    {
        int campCount = 0;
        int indexLastCampAlive = 0;
        for (int i = 0; i < 4; i++)
        {
            if (camps[i].m_isAlive)
            {
                campCount++;
                indexLastCampAlive = i;
            }
        }

        if (campCount <= 1)
        {
            // Debug.Log("update des camps alive : il reste un camp en vie");
            if (m_gameState == GameState.Game)
            {
                foreach (GameObject player in playerGO)
                {
                    player.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(3).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(4).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(5).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(6).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(7).gameObject.SetActive(false);
                    player.transform.GetChild(3).transform.GetChild(8).gameObject.SetActive(false);

                    if (player.GetComponent<EntityPlayer>().m_campIsAlive)
                    {
                        player.transform.GetChild(3).transform.GetChild(9).gameObject.SetActive(true);
                        player.transform.GetChild(3).transform.GetChild(9).transform.GetChild(1).gameObject.SetActive(false);
                        player.transform.GetChild(3).transform.GetChild(9).transform.GetChild(2).gameObject.SetActive(false);
                        player.transform.GetChild(3).transform.GetChild(9).transform.GetChild(3).gameObject.SetActive(true);
                        player.transform.GetChild(3).transform.GetChild(9).transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                
                switch (camps[indexLastCampAlive].m_linkedPlayer.m_sColor)
                {
                    case "Red":
                        m_WinImg.sprite = listWinImg[0];
                        break;
                    case "Green":
                        m_WinImg.sprite = listWinImg[1];
                        break;
                    case "Blue":
                        m_WinImg.sprite = listWinImg[2];
                        break;
                    case "Yellow":
                        m_WinImg.sprite = listWinImg[3];
                        break;
                    default:
                        break;
                }

                LoadNextState();
            }
        }

    }

}
