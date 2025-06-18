using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

//gère les unités invoqués par le joueur
public class SpawnPlayerInvocation : MonoBehaviour
{
    StateGame m_stateGame;
    public GameObject m_prefabInvocationAttaquant;
    public GameObject m_prefabInvocationDefenseur;
    public GameObject m_prefabInvocationDestructeur;
    public GameObject m_prefabInvocationTueur;
    public GameObject m_prefabPreviewInvocation;
    public Transform m_tSpawnInvocation;
    public GameObject preview = null;
    [SerializeField] Text targetTeam;
    public bool isPosOk = false;
    [SerializeField] Shader previewGood;
    [SerializeField] Shader previewBad;


    public Camp redTeam = null;
    public Camp blueTeam = null;
    public Camp greenTeam = null;
    public Camp yellowTeam = null;
    public Camp AucuneTeam = null;


    public string colorTargetTeam;
    public string myColor;
    public int playerId;
    public int playerCount;

    EntityPlayer m_entityPlayer;
    TpsController m_TPSController;

    int indexTeam = 0;
    public Camp[] listTarget;
    public Camp actualTarget;

    public int indexInvoc = 0;
    public GameObject[] listInvoc;
    public GameObject actualInvoc;


    public int[] listCost;
    public int actualInvocCost;

    public string[] listInvocName;
    public string actualInvocName;

    public int[] listStatsDefender;
    public int[] listStatsAttacker;
    public int[] listStatsDestroyer;
    public int[] listStatsKiller;
    public int[] listStatsActualUnite;

    bool isAlreadyChanged = false;

    public UIPlayer m_UIPlayer;

    public ParticleSystem particleSpawn;

    List<GameObject> tempParticleSpawn = new List<GameObject>();

    float m_cooldownClique = 0.0f;
    float m_cooldownCliqueMax = 0.5f;


    bool CanClick()
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


    // Start is called before the first frame update
    void Start()
    {
        m_stateGame = FindObjectOfType<StateGame>();

        playerId = GetComponent<EntityPlayer>().m_playerId;
        playerCount = DataManager.Instance.m_prefab.Count;
        m_entityPlayer = GetComponent<EntityPlayer>();
        m_TPSController = gameObject.GetComponentInParent<TpsController>();

        //Click cooldown
        m_cooldownClique = 0;

        //Target
        myColor = m_entityPlayer.m_sColor;

        if (playerCount > 1)
        {
            listTarget = new Camp[(int)playerCount - 1];
        }
        //        Debug.Log("listTarget size = "+ listTarget.Length);

        int indexCamp = 0;

        for (int i = 0; i < playerCount; i++)
        {
            switch (m_stateGame.playerGO[i].GetComponent<EntityPlayer>().m_sColor)
            {
                case "Red":
                    if (myColor != "Red")
                    {
                        redTeam = GameObject.Find("RedTeam").GetComponent<Camp>();
                        listTarget[indexCamp] = redTeam;
                        indexCamp++;
                        // Debug.Log("red team add");
                    }
                    break;
                case "Blue":
                    if (myColor != "Blue")
                    {
                        blueTeam = GameObject.Find("BlueTeam").GetComponent<Camp>();
                        listTarget[indexCamp] = blueTeam;
                        indexCamp++;
                        //Debug.Log("Blue team add");
                    }
                    break;
                case "Green":
                    if (myColor != "Green")
                    {
                        greenTeam = GameObject.Find("GreenTeam").GetComponent<Camp>();
                        listTarget[indexCamp] = greenTeam;
                        indexCamp++;
                        // Debug.Log("Green team add");
                    }
                    break;
                case "Yellow":
                    if (myColor != "Yellow")
                    {
                        yellowTeam = GameObject.Find("YellowTeam").GetComponent<Camp>();
                        listTarget[indexCamp] = yellowTeam;
                        indexCamp++;
                        // Debug.Log("Yellow team add");
                    }
                    break;
                default:
                    break;
            }
        }

        //for (int i = 0; i < listTarget.Length; i++)
        //{
        //    Debug.Log("target " + i + " Color = " + listTarget[i].m_sColor);

        //}

        if (playerCount > 1)
        {
            actualTarget = listTarget[0];

        }
        else if (playerCount == 1)
        {
            actualTarget = AucuneTeam;
        }

        //Invocation
        listInvoc = new GameObject[(int)4];
        listInvoc[0] = m_prefabInvocationDefenseur;
        listInvoc[1] = m_prefabInvocationAttaquant;
        listInvoc[2] = m_prefabInvocationDestructeur;
        listInvoc[3] = m_prefabInvocationTueur;
        actualInvoc = listInvoc[indexInvoc];


        //listCost = new int[(int)4];
        //listCost[0] = 20; //cout du defenseur
        //listCost[1] = 20; //cout de l'attaquant
        //listCost[2] = 22; //cout du destructeur
        //listCost[3] = 50; //cout du tueur
        actualInvocCost = listCost[indexInvoc];

        listInvocName = new string[(int)4];
        listInvocName[0] = "Defender";
        listInvocName[1] = "Attacker";
        listInvocName[2] = "Destroyer";
        listInvocName[3] = "Killer";
        actualInvocName = listInvocName[indexInvoc];

        ////Stats des différentes invocations 
        //listStatsDefender = new int[(int)4];
        //listStatsDefender[0] = 10; //stats de vie du defenseur
        //listStatsDefender[1] = 2;  //stats d'armure du defenseur
        //listStatsDefender[2] = 3;  //stats de degats sur joueur du defenseur
        //listStatsDefender[3] = 1;  //stats de degats sur objet du defenseur


        //listStatsAttacker = new int[(int)4];
        //listStatsAttacker[0] = 10;  //stats de vie de l'attaquant
        //listStatsAttacker[1] = 2;   //stats d'armure de l'attaquant
        //listStatsAttacker[2] = 5;   //stats de degats sur joueur de l'attaquant
        //listStatsAttacker[3] = 3;   //stats de degats sur objet de l'attaquant


        //listStatsDestroyer = new int[(int)4];
        //listStatsDestroyer[0] = 10;  //stats de vie du destructeur
        //listStatsDestroyer[1] = 1;   //stats d'armure du destructeur
        //listStatsDestroyer[2] = 2;   //stats de degats sur joueur du destructeur
        //listStatsDestroyer[3] = 7;   //stats de degats sur objet du destructeur

        //listStatsKiller = new int[(int)4];
        //listStatsKiller[0] = 10;  //stats de vie du tueur
        //listStatsKiller[1] = 3;   //stats de d'armure du tueur
        //listStatsKiller[2] = 7;   //stats de degats sur joueur du tueur
        //listStatsKiller[3] = 2;   //stats de degats sur objet du tueur

        listStatsActualUnite = new int[(int)4];
        listStatsActualUnite[0] = 0;  //stats de vie de l'unite actuelle
        listStatsActualUnite[1] = 0;  //stats d'armure de l'unite actuelle 
        listStatsActualUnite[2] = 0;  //stats de degats sur joueur de l'unite actuelle 
        listStatsActualUnite[3] = 0;  //stats de degats sur objet de l'unite actuelle 
    }

    // Update is called once per frame
    void Update()
    {
        if (actualInvocName == listInvocName[0])
        {
            listStatsActualUnite[0] = listStatsDefender[0];
            listStatsActualUnite[1] = listStatsDefender[1];
            listStatsActualUnite[2] = listStatsDefender[2];
            listStatsActualUnite[3] = listStatsDefender[3];
        }
        else if (actualInvocName == listInvocName[1])
        {
            listStatsActualUnite[0] = listStatsAttacker[0];
            listStatsActualUnite[1] = listStatsAttacker[1];
            listStatsActualUnite[2] = listStatsAttacker[2];
            listStatsActualUnite[3] = listStatsAttacker[3];
        }
        else if (actualInvocName == listInvocName[2])
        {
            listStatsActualUnite[0] = listStatsDestroyer[0];
            listStatsActualUnite[1] = listStatsDestroyer[1];
            listStatsActualUnite[2] = listStatsDestroyer[2];
            listStatsActualUnite[3] = listStatsDestroyer[3];
        }
        else if (actualInvocName == listInvocName[3])
        {
            listStatsActualUnite[0] = listStatsKiller[0];
            listStatsActualUnite[1] = listStatsKiller[1];
            listStatsActualUnite[2] = listStatsKiller[2];
            listStatsActualUnite[3] = listStatsKiller[3];
        }



        PreviewPosInvocation();
        //if (m_entityPlayer.GetComponent<EntityPlayer>().m_eSelectedCapacity == Capacity.INVOCATION)
        //{

        //}

        // Debug.Log(indexInvoc);
        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }
        //Debug.Log("le nom de l'invoc est :" + actualInvocName);
        if (playerCount != 1)
        {
            if (!actualTarget.m_isAlive)
            {
                ChangeTarget(actualTarget);
            }
        }

        // Destroy Particle Spawn
        foreach (GameObject particle in tempParticleSpawn.ToArray())
        {
            if (particle.GetComponentInChildren<ParticleSystem>().isStopped)
            {
                tempParticleSpawn.Remove(particle);
                Destroy(particle);
            }
        }
    }

    public void SetInvocation(int _index)
    {
        indexInvoc = _index;
        m_UIPlayer.m_needUpdate = true;
        actualInvocCost = listCost[indexInvoc % listInvoc.Length];
        actualInvoc = listInvoc[indexInvoc % listInvoc.Length];
        actualInvocName = listInvocName[indexInvoc % listInvoc.Length];
    }

    public void UpdateInvocationCapacity()
    {
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Vector3 temp = transform.position + Vector3.right * 20;

                if (CanClick() && isPosOk)// && playerCount != 1)
                {
                    //si on est en affrontement
                    if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT)
                    {
                        bool gotEnoughMoney = m_entityPlayer.RemoveMoney(actualInvocCost);
                        if (gotEnoughMoney)
                        {
                            SpawnEnnemy(actualInvocName, actualTarget, temp);
                        }
                        else
                        {
                            GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                        }
                    }
                    else if (DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE && actualInvocName == listInvocName[0])
                    {
                        bool gotEnoughMoney = m_entityPlayer.RemoveMoney(actualInvocCost);
                        if (gotEnoughMoney)
                        {
                            SpawnEnnemy(actualInvocName, AucuneTeam, temp);

                        }
                        else
                        {
                            GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                        }
                    }



                }
            }

        }
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(m_TPSController.device, "Y", true))
            {
                Vector3 temp = transform.position + Vector3.right * 20;
                //Si on as les fonds nécessaire on fait spawn un ennemi
                if (CanClick() && isPosOk)
                {

                    if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT)
                    {
                        bool gotEnoughMoney = m_entityPlayer.RemoveMoney(actualInvocCost);
                        if (gotEnoughMoney)
                        {
                            SpawnEnnemy(actualInvocName, actualTarget, temp);
                        }
                        else
                        {
                            GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                        }
                    }
                    else if (DataManager.Instance.m_gameMode == DataManager.GameMode.SURVIE && actualInvocName == listInvocName[0])
                    {
                        bool gotEnoughMoney = m_entityPlayer.RemoveMoney(actualInvocCost);
                        if (gotEnoughMoney)
                        {
                            SpawnEnnemy(actualInvocName, AucuneTeam, temp);

                        }
                        else
                        {
                            GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                        }
                    }


                }
            }
        }
    }


    private void PreviewPosInvocation()
    {
        if (preview != null)
        {
            Destroy(preview);
        }

        if (gameObject.GetComponent<EntityPlayer>().m_eSelectedCapacity != Capacity.INVOCATION || GetComponent<EntityPlayer>().m_isDead)
        {
            return;
        }

        //if (gameObject.GetComponent<EntityPlayer>().m_eSelectedCapacity == Capacity.INVOCATION )
        //{
        //    preview = Instantiate(actualInvoc, m_tSpawnInvocation.position, m_tSpawnInvocation.rotation);
        //}
        isPosOk = true;

        if (gameObject.GetComponent<EntityPlayer>().m_eSelectedCapacity == Capacity.INVOCATION)
        {
            Collider[] colliders = Physics.OverlapBox(m_tSpawnInvocation.position + new Vector3(0,1,0), new Vector3(2 / 2, 0, 2 / 2)) ;
            foreach (Collider coll in colliders)
            {
                if (coll.transform.tag == "Wall" || coll.transform.tag == "Building" || coll.transform.tag == "Trap" || coll.transform.tag == "Base" || coll.transform.tag == "Decor"
                     || coll.transform.tag == "Ennemi")
                {
                    isPosOk = false;
                }
            }

            preview = Instantiate(m_prefabPreviewInvocation, m_tSpawnInvocation.position, m_tSpawnInvocation.rotation);

        }

        if (preview != null)
        {
            SkinnedMeshRenderer tempSkin = preview.GetComponentInChildren<InvocationAnimatorScript>().GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] mats = tempSkin.materials;
            if (isPosOk)
            {
                mats[0].shader = previewGood;
                mats[1].shader = previewGood;
                mats[2].shader = previewGood;
                Debug.Log("PreviewGood");
                //preview.transform.GetComponent<Renderer>().material.shader = previewGood;
                tempSkin.materials = mats;
            }
            else
            {
                mats[0].shader = previewBad;
                mats[1].shader = previewBad;
                mats[2].shader = previewBad;
                //preview.GetComponentInChildren<Renderer>().material.shader = previewBad;
                Debug.Log("PreviewBad");
                tempSkin.materials = mats;
                //preview.transform.GetComponent<Renderer>().material.shader = previewBad;
            }
            
        }
    }


    public void UpdateNexusSelection()
    {

        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.T) && indexTeam < playerCount - 2)
            {
                indexTeam++;
                m_UIPlayer.m_needUpdate = true;
                actualTarget = listTarget[indexTeam];
                // Debug.Log("actualTarget = " + actualTarget.m_sColor);
            }
            if (Input.GetKeyDown(KeyCode.G) && indexTeam > 0)
            {
                indexTeam--;
                m_UIPlayer.m_needUpdate = true;
                actualTarget = listTarget[indexTeam];

                // Debug.Log("actualTarget = " + actualTarget.m_sColor);
            }

            //if (Input.GetKeyDown(KeyCode.R) && indexInvoc < 3)
            //{
            //    indexInvoc++;
            //    m_UIPlayer.m_needUpdate = true;
            //    actualInvoc = listInvoc[indexInvoc];
            //    actualInvocCost = listCost[indexInvoc];
            //    actualInvocName = listInvocName[indexInvoc];
            //    // Debug.Log("actualTarget = " + actualTarget.m_sColor);
            //    Debug.Log(actualInvocCost);
            //}
            //if (Input.GetKeyDown(KeyCode.F) && indexInvoc > 0)
            //{
            //    indexInvoc--;
            //    m_UIPlayer.m_needUpdate = true;
            //    actualInvoc = listInvoc[indexInvoc];
            //    actualInvocCost = listCost[indexInvoc];
            //    actualInvocName = listInvocName[indexInvoc];
            //    Debug.Log(actualInvocCost);
            //    // Debug.Log("actualTarget = " + actualTarget.m_sColor);
            //}


        }
        if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
        {
            //Changement de cible
            Vector2 ToucheDirectionnel = InputManager.Instance.ValuePAD(m_TPSController.device, "Pad");

            if (ToucheDirectionnel.y == 1)
            {
                if (!isAlreadyChanged) //changing once and not each frame
                {
                    indexTeam++;
                    indexTeam = indexTeam % (playerCount - 1);


                    isAlreadyChanged = true;
                    m_UIPlayer.m_needUpdate = true;
                    if (listTarget[indexTeam].m_isAlive)
                    {
                        actualTarget = listTarget[indexTeam];
                    }
                    else
                    {
                        ChangeTarget(actualTarget);
                    }
                }
            }
            else if (ToucheDirectionnel.y == -1)
            {

                if (!isAlreadyChanged)
                {
                    // Debug.Log("ToucheDirectionnel == -1 ");
                    indexTeam--;
                    if (indexTeam < 0)
                    {
                        indexTeam = playerCount - 2;
                    }
                    // Debug.Log("ToucheDirectionnel " + ToucheDirectionnel + " ,pID : " + m_entityPlayer.m_playerId);
                    // Debug.Log("ToucheDirectionnel.y == -1 ");

                    isAlreadyChanged = true;
                    m_UIPlayer.m_needUpdate = true;
                    if (listTarget[indexTeam].m_isAlive)
                    {
                        actualTarget = listTarget[indexTeam];
                    }
                    else
                    {
                        ChangeTarget(actualTarget);
                    }
                }
            }
            else
            {
                isAlreadyChanged = false;
            }
        }
    }

    void SpawnEnnemy(string _uniteType, Camp _actualTarget, Vector3 _position)
    {
        if (isPosOk)
        {
            SoundManager.Instance.PlayerInvocatePlay(gameObject);
            //spawn ennemy 

            GameObject particle = Instantiate(particleSpawn.gameObject, m_tSpawnInvocation.position, particleSpawn.gameObject.transform.rotation);

            particle.GetComponent<ParticleSystem>().Play();

            tempParticleSpawn.Add(particle);

            //get spawn position
            Vector3 position = _position;



            GameObject newEnnemy = Instantiate(actualInvoc, m_tSpawnInvocation.position, m_tSpawnInvocation.rotation);

            newEnnemy.GetComponent<comportementGeneralIA>().isPartOfWave = false;
            newEnnemy.GetComponent<comportementGeneralIA>().m_entityPlayer = m_entityPlayer;

            newEnnemy.GetComponent<comportementGeneralIA>().uniteType = _uniteType;
            newEnnemy.GetComponent<comportementGeneralIA>().targetTeam = _actualTarget; //give target team to ennemy

            newEnnemy.GetComponent<comportementGeneralIA>().fromPlayercolor = m_entityPlayer.m_sColor;

            newEnnemy.GetComponent<comportementGeneralIA>().m_maxHealth = listStatsActualUnite[0];
            newEnnemy.GetComponent<comportementGeneralIA>().m_armor = listStatsActualUnite[1];
            newEnnemy.GetComponent<comportementGeneralIA>().m_damageOnCharacters = listStatsActualUnite[2];
            newEnnemy.GetComponent<comportementGeneralIA>().m_damageOnObjects = listStatsActualUnite[3];




            //recherche du premier material present dans le SMR
            SkinnedMeshRenderer tempSkin = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().GetComponentInChildren<SkinnedMeshRenderer>();
            Material[] mats = tempSkin.materials;
            //Assignation du material
            switch (m_entityPlayer.m_sColor)
            {
                case "Red":

                    mats[0] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[0];
                    mats[1] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[0];
                    mats[2] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[0];
                    // Debug.Log("material red");
                    break;
                case "Blue":
                    mats[0] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[1];
                    mats[1] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[1];
                    mats[2] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[1];
                    // Debug.Log("material blue");
                    break;
                case "Green":
                    mats[0] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[2];
                    mats[1] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[2];
                    mats[2] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[2];
                    // Debug.Log("material green");
                    break;
                case "Yellow":
                    mats[0] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[3];
                    mats[1] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[3];
                    mats[2] = newEnnemy.GetComponentInChildren<InvocationAnimatorScript>().m_materials[3];
                    //Debug.Log("material yellow");
                    break;

                default:
                    break;
            }

            tempSkin.materials = mats;





        }
    }
    //public void SetInvocation(int _index)
    //{
    //    indexInvoc = _index;
    //    actualInvoc = listInvoc[indexInvoc % listInvoc.Length];
    //}

    //à appeller lorsque le camp ciblé est tué
    public void ChangeTarget(Camp _currentTarget)
    {
        //Si la cible n'as pas encore été changé dans ce script
        if (_currentTarget.m_campId == actualTarget.m_campId)
        {
            //Debug.Log("_currentTarget != actualTarget");
            indexTeam++;
            indexTeam = indexTeam % (playerCount - 1);
            if (listTarget[indexTeam].m_isAlive)
            {
                actualTarget = listTarget[indexTeam];
            }
            else
            {
                indexTeam++;
                indexTeam = indexTeam % (playerCount - 1);
                actualTarget = listTarget[indexTeam];
            }

            m_UIPlayer.m_needUpdate = true;
        }

    }
}
