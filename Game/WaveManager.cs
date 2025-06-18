/*Fait par Max Bonnaud*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    struct instruction
    {
        public string instructionType;
        public string additionnalInstructions;
        public float floatNum;
    }
    LinkedList<instruction> instructionList;
    List<GameObject> playerList;
    List<GameObject> lineRendererList;
    bool isInstructionListEmpty;

    Stats sbStats = null;

    string[] availableOrders =
    {
        "spawn",                //spawn ennemy named after additionnalInstructions variable for each active team
        "changeSpawnDelay",     //change delay between spawns to value of floatNum
        "changeSpawnDirection"  //change spawn origin between center, left and right
    };

    [SerializeField] Canvas uICanvas;
    [SerializeField] GameObject fadingTextPrefab;

    //ennemy team counter
    public struct EnnemyCounter
    {
        public int red;
        public int blue;
        public int green;
        public int yellow;
        public int total;
    }
    public EnnemyCounter ennemyCounter;

    float timer;
    float spawnDelay;

    public bool isWaveActive;
    string spawnDirection;
    public float breakTimer;

    public int currentWave;

    Camp redTeam = null;
    Camp blueTeam = null;
    Camp greenTeam = null;
    Camp yellowTeam = null;

    [SerializeField] GameObject stateGame;

    //levers
    WaveManagerLevers wml;
    int startingEnnemyPoints = 5;
    int additionnalPointPerWave = 3;
    public float timeBetweenWaves = 60.0f;

    //ennemy Units
    GameObject ennemyFolder;
    GameObject corpseFolder;

    public Transform ennemyFolderTransform;
    public Transform corpseFolderTransform;

    [SerializeField] GameObject ennemyGrunt;
    [SerializeField] GameObject ennemyHeavy;

    int gruntSpawnWeight = 3;
    int gruntSpawnValue = 1;

    int heavySpawnWeight = 1;
    int heavySpawnValue = 5;

    [SerializeField] Material redLineMat;
    [SerializeField] Material blueLineMat;
    [SerializeField] Material greenLineMat;
    [SerializeField] Material yellowLineMat;

    //spawn position levers

    enum SpawnerSwitchBehaviour
    {
        fixedSpawn, cycleSpawns, randomizeSpawns
    };
    [SerializeField] SpawnerSwitchBehaviour spawnSwitchBehaviour = SpawnerSwitchBehaviour.fixedSpawn;

    [SerializeField] GameObject scoreBoardStats;

    [SerializeField] GameObject waveLineRenderer;
    //popup messages
    //[SerializeField] string waveStartMessage = "Horde spawned!";
    //[SerializeField] string waveEndMessage = "Ennemis repelled!"; //not working

    int countDownIndicator = 5;

    bool areLineRenderersPlaced = false;

    void Start()
    {
        GetLeverScript();
        GetPlayers();
        ennemyCounter = new EnnemyCounter();

        //instantiate ennemy folder
        ennemyFolder = new GameObject("Ennemy Folder");
        ennemyFolderTransform = ennemyFolder.GetComponent<Transform>();

        corpseFolder = new GameObject("Corpse Folder");
        corpseFolderTransform = corpseFolder.GetComponent<Transform>();

        //get teamManagers
        redTeam = GameObject.Find("RedTeam").GetComponent<Camp>();
        blueTeam = GameObject.Find("BlueTeam").GetComponent<Camp>();
        greenTeam = GameObject.Find("GreenTeam").GetComponent<Camp>();
        yellowTeam = GameObject.Find("YellowTeam").GetComponent<Camp>();

        instructionList = new LinkedList<instruction>();

        spawnDirection = "center";  //"center", "left" or "right"
        isInstructionListEmpty = true;
        timer = 0.0f;
        spawnDelay = 0.5f;
        //waveBreakTime = 60.0f;
        breakTimer = timeBetweenWaves;
        isWaveActive = false;

        currentWave = 0;

        //get scoreboard
        sbStats = (Stats)GameObject.FindObjectOfType(typeof(Stats));
        
    }

    void Update()
    {
        //disabled on demand from Ben
        /*
        if(!areLineRenderersPlaced)
        {
            areLineRenderersPlaced = true;
            PlaceLineRenderers();
        }*/

        //debug wave spawner
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentWave++;
            SpawnWave(startingEnnemyPoints + currentWave * additionnalPointPerWave);
            breakTimer = timeBetweenWaves;
            isWaveActive = true;
        }

        if (isWaveActive)
        {
            ManageActiveWave();    //ennemy spawning, spawn switching, etc...
        }
        else
        {
            ManageInactiveWave();   //wait for everyone to be ready, spawn next wave, etc...
        }
    }

    void SpawnEnnemy(string _ennemyType, string _team)
    {
        Camp team = null;

        //get Team based on color
        switch (_team)
        {
            case "Red":
                team = redTeam;
                UpdateEnnemyCounter(_team, 1);
                break;
            case "Blue":
                team = blueTeam;
                UpdateEnnemyCounter(_team, 1);
                break;
            case "Green":
                team = greenTeam;
                UpdateEnnemyCounter(_team, 1);
                break;
            case "Yellow":
                team = yellowTeam;
                UpdateEnnemyCounter(_team, 1);
                break;
            default:
                Debug.LogWarning("WaveManager:SpawnEnnemy: unknown team selected: " + _team);
                break;
        }

        //spawn ennemy 
        Vector3 position = Vector3.zero;
        GameObject newEnnemy = null;
        switch (_ennemyType)
        {
            case "grunt":
                //get spawn position
                //Vector3 position = Vector3.zero;
                switch (spawnDirection)
                {
                    case "center":
                        position = team.spawnMid.transform.position;
                        break;
                    case "left":
                        position = team.spawnLeft.transform.position;
                        break;
                    case "right":
                        position = team.spawnRight.transform.position;
                        break;
                }

                newEnnemy = Instantiate(ennemyGrunt, ennemyFolderTransform);
                newEnnemy.GetComponent<Transform>().position = position;
                newEnnemy.GetComponent<Ennemi>().isPartOfWave = true; //modif Axel
                newEnnemy.GetComponent<Ennemi>().targetTeam = team; //give target team to ennemy
                newEnnemy.GetComponent<Ennemi>().recupNexus = team.m_linkedNexus.GetComponent<Nexus>();
                newEnnemy.GetComponent<Ennemi>().waveManager = gameObject;


                break;

            case "heavy":
                //get spawn position
                //Vector3 position = Vector3.zero;
                switch (spawnDirection)
                {
                    case "center":
                        position = team.spawnMid.transform.position;
                        break;
                    case "left":
                        position = team.spawnLeft.transform.position;
                        break;
                    case "right":
                        position = team.spawnRight.transform.position;
                        break;
                }

                newEnnemy = Instantiate(ennemyHeavy, ennemyFolderTransform);
                newEnnemy.GetComponent<Transform>().position = position;
                newEnnemy.GetComponent<Ennemi>().isPartOfWave = true; //modif Axel
                newEnnemy.GetComponent<Ennemi>().targetTeam = team; //give target team to ennemy
                newEnnemy.GetComponent<Ennemi>().recupNexus = team.m_linkedNexus.GetComponent<Nexus>();
                newEnnemy.GetComponent<Ennemi>().waveManager = gameObject;
                break;

            default:
                Debug.LogWarning("WaveManager:SpawnEnnemy: unknown ennemy requested: " + _ennemyType);
                break;
        }


    }

    void SwitchWaveState()
    {
        //switch bool state
        if (isWaveActive)
        {
            //end current wave
            isWaveActive = false;
            breakTimer = timeBetweenWaves;

            //update "waves survived" counter
            if (redTeam.m_isAlive)
            {
                sbStats.nbVaguesSurvive[redTeam.m_sColor]++;
            }
            if (blueTeam.m_isAlive)
            {
                sbStats.nbVaguesSurvive[blueTeam.m_sColor]++;
            }
            if (greenTeam.m_isAlive)
            {
                sbStats.nbVaguesSurvive[greenTeam.m_sColor]++;
            }
            if (yellowTeam.m_isAlive)
            {
                sbStats.nbVaguesSurvive[yellowTeam.m_sColor]++;
            }


        }
        else
        {
            isWaveActive = true;
        }

        //reset timer
        timer = 0.0f;
    }

    //main functions
    void ManageActiveWave()
    {
        //go through order list
        if (instructionList.Count > 0)
        {
            GoThroughOrderList();
        }
        else
        {

            if (ennemyFolder.transform.childCount == 0)
            {
                //wave done
                SwitchWaveState();
                SpawnText("Ennemis repelled!", 1.0f, 30);
                countDownIndicator = 5;
                //change spawn if applicable
                switch (spawnSwitchBehaviour)
                {
                    case SpawnerSwitchBehaviour.cycleSpawns:
                        CycleSpawns();        //get next spawn
                        break;

                    case SpawnerSwitchBehaviour.randomizeSpawns:
                        RandomizeSpawns();    //get random spawn
                        break;
                }


            }
        }

        //move dead ennemies to corpse folder
        int childCount = ennemyFolderTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Ennemi ennemy = ennemyFolderTransform.GetChild(i).GetComponent<Ennemi>();
            if (ennemy.m_isDead)
            {
                //Debug.Log("WaveManager: ManageActiveWave(): moved dead ennemy to corpse storage parent");
                UpdateEnnemyCounter(ennemy.targetTeam.m_sColor, -1);

                ennemy.GetComponent<Transform>().parent = corpseFolderTransform;
                i--;
                childCount--;
            }
        }

    }
    void ManageInactiveWave()
    {
        //check for player readyness

        //manage timer
        breakTimer -= Time.deltaTime;
        if (ArePlayersReady())
        {
            if (breakTimer > 5.0f)
            {
                breakTimer = 5.0f;
            }
        }

        //popup countdown
        if (breakTimer <= 5.0f && countDownIndicator > 0)
        {
            if (breakTimer <= countDownIndicator)
            {
                SpawnText("" + countDownIndicator, 1.0f, 30);
                countDownIndicator--;
            }
        }

        if (IsWaveReady())
        {
            currentWave++;
            SpawnWave(startingEnnemyPoints + currentWave * additionnalPointPerWave);

            breakTimer = timeBetweenWaves;
            isWaveActive = true;

            //set all players ready status to false
            //test
            foreach (GameObject player in playerList)
            {
                player.GetComponent<EntityPlayer>().isReadyForNextWave = false;
            }
        }
    }


    void SpawnWave(int _power)
    {

        //NEW
        int remainingPower = _power;
        //Debug.Log("WaveManager: SpawnWave(): entering units selection loop");
        while (remainingPower > 0)
        {
            //Debug.Log("WaveManager: SpawnWave(): remaining power = " + remainingPower);
            //choose ennemy
            int totalweight = gruntSpawnWeight + heavySpawnWeight;
            int choice = Random.Range(1, totalweight + 1);

            if (choice <= gruntSpawnWeight)
            {
                //check value
                if (remainingPower >= gruntSpawnValue)
                {
                    //spawn
                    instruction tempOrder = new instruction();
                    tempOrder.instructionType = "spawn";
                    tempOrder.additionnalInstructions = "grunt";
                    instructionList.AddLast(tempOrder);

                    //remove power count
                    remainingPower -= gruntSpawnValue;
                }
            }
            else
            {
                //check value
                if (remainingPower >= heavySpawnValue)
                {
                    //spawn
                    instruction tempOrder = new instruction();
                    tempOrder.instructionType = "spawn";
                    tempOrder.additionnalInstructions = "heavy";
                    instructionList.AddLast(tempOrder);

                    //remove power count
                    remainingPower -= heavySpawnValue;
                }
            }

        }

        SpawnText("Horde spawned!", 1.0f, 30);
    }

    bool IsWaveReady()
    {
        bool result = false;
        //debug command: force next wave
        if (Input.GetKeyDown(KeyCode.F1))
        {
            result = true;
            SoundManager.Instance.WaveAnnouncerPlay(gameObject);
        }

        //Timer end start
        if (currentWave > 0 || ArePlayersReady())
        {
            if (breakTimer <= 0.0f)    //end of break timer
            {
                result = true;
                SoundManager.Instance.WaveAnnouncerPlay(gameObject);
            }
        }

        return result;
    }
    void SpawnOrder(instruction _order)
    {
        //Debug.Log("WaveManager: SpawnOrder(): spawn order");  //ok
        //spawn ennemy for every active team
        if (redTeam.m_isAlive)
        {
            SpawnEnnemy(_order.additionnalInstructions, "Red");
        }
        if (blueTeam.m_isAlive)
        {
            SpawnEnnemy(_order.additionnalInstructions, "Blue");
        }
        if (greenTeam.m_isAlive)
        {
            SpawnEnnemy(_order.additionnalInstructions, "Green");
        }
        if (yellowTeam.m_isAlive)
        {
            SpawnEnnemy(_order.additionnalInstructions, "Yellow");
        }
    }

    bool ArePlayersReady()
    {
        bool result = false;
        if (playerList.Count > 0)
        {
            result = true;


            for (int i = 0; i < playerList.Count; i++)
            {
                //check for player readyness
                if (!playerList[i].GetComponent<EntityPlayer>().isReadyForNextWave)
                {
                    result = false;
                }

            }
        }

        return result;
    }

    void ChangeSpawnDirection(string _direction)
    {
        switch (_direction)
        {
            case "center":
                spawnDirection = _direction;
                break;
            case "right":
                spawnDirection = _direction;
                break;
            case "left":
                spawnDirection = _direction;
                break;
            default:
                Debug.LogError("WaveManager:ChangeSpawnDirection: invalid direction command: " + _direction + ", please use center, right or left");
                break;
        }
    }

    void GoThroughOrderList()
    {
        timer += Time.deltaTime;
        if (timer >= spawnDelay)
        {
            //read and execute first order
            instruction order = instructionList.First.Value;

            switch (order.instructionType)
            {
                case "spawn":
                    SpawnOrder(order);
                    timer = 0.0f;    //reset timer
                    break;

                case "changeSpawnDelay":
                    spawnDelay = order.floatNum;
                    break;

                case "changeSPawnDirection":
                    ChangeSpawnDirection(order.additionnalInstructions);
                    break;

                default:
                    Debug.LogError("WaveManager:order processing error: invalid instruction type: " + order.instructionType);
                    break;
            }

            //remove order from list
            instructionList.RemoveFirst();
        }
    }

    void GetLeverScript()
    {
        //get lever script
        if (gameObject.GetComponent<WaveManagerLevers>() != null)
        {
            wml = gameObject.GetComponent<WaveManagerLevers>();
        }
        else
        {
            gameObject.AddComponent<WaveManagerLevers>();
            wml = gameObject.GetComponent<WaveManagerLevers>();
        }
        startingEnnemyPoints = wml.startingEnnemyPoints;
        additionnalPointPerWave = wml.additionnalPointPerWave;
        timeBetweenWaves = wml.timeBetweenWaves;
    }


    public void UpdateEnnemyCounter(string _color, int _count)
    {
        //get Team based on color
        switch (_color.ToLower())
        {
            case "red":
                ennemyCounter.red += _count;
                ennemyCounter.total += _count;
                break;
            case "blue":
                ennemyCounter.blue += _count;
                ennemyCounter.total += _count;
                break;
            case "green":
                ennemyCounter.green += _count;
                ennemyCounter.total += _count;
                break;
            case "yellow":
                ennemyCounter.yellow += _count;
                ennemyCounter.total += _count;
                break;
            default:
                Debug.LogWarning("WaveManager:UpdateManager(): unknown team selected: " + _color);
                break;
        }
        //Debug.Log("ratio: " + ennemyCounter.red + ":" + ennemyCounter.green + ":" + ennemyCounter.blue + ":" + ennemyCounter.yellow + ":" + ennemyCounter.total);
    }

    void GetPlayers()
    {
        //get players
        playerList = stateGame.GetComponent<StateGame>().playerGO;
    }

    //Spawn cycle
    void RandomizeSpawns()
    {
        //1 center, 2 left, 3 right
        int choice = Random.Range(1, 4);

        switch (choice)
        {
            case 1:
                spawnDirection = "center";
                break;
            case 2:
                spawnDirection = "left";
                break;
            case 3:
                spawnDirection = "right";
                break;
        }
    }
    void CycleSpawns()
    {
        //get next spawn
        string newSpawn = "";
        if (spawnDirection == "center")
        {
            newSpawn = "right";
        }
        if (spawnDirection == "right")
        {
            newSpawn = "left";
        }
        if (spawnDirection == "left")
        {
            newSpawn = "center";
        }
        spawnDirection = newSpawn;
    }

    void SpawnText(string _text, float _duration, int _fontSize)
    {
        //Debug.Log("WaveManager: SpawnText(): spawning text");
        GameObject newFadingText = Instantiate(fadingTextPrefab, uICanvas.GetComponent<Transform>());
        newFadingText.GetComponent<FadingText>().duration = _duration;
        newFadingText.GetComponent<FadingText>().GetComponent<TextMeshProUGUI>().text = _text;
        newFadingText.GetComponent<FadingText>().fontSize = _fontSize;
    }

    void PlaceLineRenderers()
    {

        //spawn ennemy for every active team
        if (redTeam.m_isAlive)
        {
            Debug.Log("WaveManager: PlaceLineRenderer(): placing red line renderer");
            GameObject newEnemyLineRenderer = Instantiate(waveLineRenderer);//spawn renderer
            Vector3 position = Vector3.zero;   //set position on red spawn
            switch (spawnDirection)
            {
                case "center":
                    position = redTeam.spawnMid.transform.position;
                    break;
                case "left":
                    position = redTeam.spawnLeft.transform.position;
                    break;
                case "right":
                    position = redTeam.spawnRight.transform.position;
                    break;
            }
            newEnemyLineRenderer.GetComponent<Transform>().position = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().SetDestination(redTeam.GetComponent<Camp>().m_linkedNexus.transform.position);    //set objective
            newEnemyLineRenderer.GetComponent<PathLineRenderer>().standPosition = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().isStopped = true;
            newEnemyLineRenderer.GetComponent<Renderer>().material = redLineMat;

            lineRendererList.Add(newEnemyLineRenderer);

        }
        if (blueTeam.m_isAlive)
        {

            GameObject newEnemyLineRenderer = Instantiate(waveLineRenderer);//spawn renderer
            Vector3 position = Vector3.zero;   //set position on red spawn
            switch (spawnDirection)
            {
                case "center":
                    position = blueTeam.spawnMid.transform.position;
                    break;
                case "left":
                    position = blueTeam.spawnLeft.transform.position;
                    break;
                case "right":
                    position = blueTeam.spawnRight.transform.position;
                    break;
            }
            newEnemyLineRenderer.GetComponent<Transform>().position = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().SetDestination(blueTeam.GetComponent<Camp>().m_linkedNexus.transform.position);    //set objective
            newEnemyLineRenderer.GetComponent<PathLineRenderer>().standPosition = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().isStopped = true;
            newEnemyLineRenderer.GetComponent<Renderer>().material = blueLineMat;

            lineRendererList.Add(newEnemyLineRenderer);
        }
        if (greenTeam.m_isAlive)
        {
            GameObject newEnemyLineRenderer = Instantiate(waveLineRenderer);//spawn renderer
            Vector3 position = Vector3.zero;   //set position on red spawn
            switch (spawnDirection)
            {
                case "center":
                    position = greenTeam.spawnMid.transform.position;
                    break;
                case "left":
                    position = greenTeam.spawnLeft.transform.position;
                    break;
                case "right":
                    position = greenTeam.spawnRight.transform.position;
                    break;
            }
            newEnemyLineRenderer.GetComponent<Transform>().position = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().SetDestination(greenTeam.GetComponent<Camp>().m_linkedNexus.transform.position);    //set objective
            newEnemyLineRenderer.GetComponent<PathLineRenderer>().standPosition = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().isStopped = true;
            newEnemyLineRenderer.GetComponent<Renderer>().material = greenLineMat;

            lineRendererList.Add(newEnemyLineRenderer);
        }
        if (yellowTeam.m_isAlive)
        {
            GameObject newEnemyLineRenderer = Instantiate(waveLineRenderer);//spawn renderer
            Vector3 position = Vector3.zero;   //set position on red spawn
            switch (spawnDirection)
            {
                case "center":
                    position = yellowTeam.spawnMid.transform.position;
                    break;
                case "left":
                    position = yellowTeam.spawnLeft.transform.position;
                    break;
                case "right":
                    position = yellowTeam.spawnRight.transform.position;
                    break;
            }
            newEnemyLineRenderer.GetComponent<Transform>().position = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().SetDestination(yellowTeam.GetComponent<Camp>().m_linkedNexus.transform.position);    //set objective
            newEnemyLineRenderer.GetComponent<PathLineRenderer>().standPosition = position;
            newEnemyLineRenderer.GetComponent<NavMeshAgent>().isStopped = true;
            newEnemyLineRenderer.GetComponent<Renderer>().material = yellowLineMat;

            lineRendererList.Add(newEnemyLineRenderer);
        }
    }

}
