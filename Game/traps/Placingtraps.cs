//Script by : Alexis

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Placingtraps : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] GameObject smallTrap;
    [SerializeField] GameObject previewSmallTrap;
    [SerializeField] GameObject barrack;
    [SerializeField] GameObject previewBarrack;
    [SerializeField] GameObject smallWallTrap;
    [SerializeField] GameObject previewSmallWallTrap;
    [SerializeField] GameObject roofTrap;
    [SerializeField] GameObject previewRoofTrap;
    [SerializeField] TMP_Text trapUI;

    TpsController controller;

    Traps[] traps = new Traps[4];
    Traps[] previewTraps = new Traps[4];
    public Traps selectdTrap;
    public int indexSelected = 0;
    static List<Traps> allTraps = new List<Traps>();

    public GameObject prevesualize = null;
    [SerializeField] Shader previewGood;
    [SerializeField] Shader previewBad;
    bool canIPlaceTrap = true;
    float rayLegth = 15f;

    public bool isAlreadyChanged = false;
    public bool trapPlaced = false;

    public Stats scoreboardStats;
    WaveManager waveManager;
    GameplaySettings.Settings m_currentSettings;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        traps[0] = smallTrap.GetComponent<Traps>();
        traps[1] = barrack.GetComponent<Traps>();
        traps[2] = smallWallTrap.GetComponent<Traps>();
        traps[3] = roofTrap.GetComponent<Traps>();
        previewTraps[0] = previewSmallTrap.GetComponent<Traps>();
        previewTraps[1] = previewBarrack.GetComponent<Traps>();
        previewTraps[2] = previewSmallWallTrap.GetComponent<Traps>();
        previewTraps[3] = previewRoofTrap.GetComponent<Traps>();
        selectdTrap = traps[indexSelected];

        scoreboardStats = (Stats)GameObject.FindObjectOfType(typeof(Stats));
        waveManager = FindObjectOfType<WaveManager>();
        m_currentSettings = GameplaySettings.Instance.m_customSettings;

    }

    // Update is called once per frame
    public void UpdateControls()
    {

        //UpdateUI();

        InputTraps();
    }

    private void LateUpdate()
    {
        PrevisualizePosTrap();
    }

    private void PlaceATrap()
    {
        if (canIPlaceTrap && prevesualize != null)
        {
            GameObject newTrap = Instantiate(traps[indexSelected].gameObject, prevesualize.transform.position, prevesualize.transform.rotation);
            newTrap.GetComponentInChildren<TrapAttack>().SetPlayer(gameObject);
            gameObject.GetComponent<EntityPlayer>().RemoveMoney(selectdTrap.price);
            allTraps.Add(newTrap.GetComponent<Traps>());
            SoundManager.Instance.TrapSpawnPlay(gameObject);

            //increase trap count
            scoreboardStats.nbPoseTrap[GetComponent<EntityPlayer>().m_sColor] += 1;
        }

    }

    //private void UpdateUI()
    //{
    //    switch (indexSelected)
    //    {
    //        case 0:
    //            trapUI.text = "small trap\ncout : " + traps[0].price.ToString();
    //            break;
    //        case 1:
    //            trapUI.text = "baricade\ncout : " + traps[1].price.ToString();
    //            break;
    //        case 2:
    //            trapUI.text = "small wall trap\ncout : " + traps[2].price.ToString();
    //            break;
    //        default:
    //            trapUI.text = "ceci ne correspond a aucun piege ";
    //            break;
    //    }
    //}

    private void InputTraps()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Y) == true)
            {
                PlaceATrap();
            }
        }
        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(controller.device, "Y", true))
            {
                if (!trapPlaced)
                {
                    PlaceATrap();
                    trapPlaced = true;
                }
            }
            else
            {
                trapPlaced = false;
            }
        }
    }

    private void PrevisualizePosTrap()
    {
        if (prevesualize != null)
        {
            Destroy(prevesualize);
        }


        if (selectdTrap == null || gameObject.GetComponent<EntityPlayer>().m_eSelectedCapacity != Capacity.TRAP || GetComponent<EntityPlayer>().m_isDead)
        {
            return;
        }

        bool isPosOk = true;
        Quaternion rotation = transform.rotation;

        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, rayLegth);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Ground")
            {
                if (selectdTrap.type == Traps.TypeTrap.FLOOR_TRAP)
                {
                    Collider[] colliders = Physics.OverlapBox(hit.point, new Vector3(selectdTrap.length / 2, 0, selectdTrap.length / 2));
                    foreach (Collider coll in colliders)
                    {
                        if (coll.transform.tag == "Wall" || coll.transform.tag == "Building" || coll.transform.tag == "Trap" || coll.transform.tag == "Base")
                        {
                            isPosOk = false;
                        }
                    }

                    prevesualize = Instantiate(previewTraps[indexSelected].gameObject, hit.point, rotation);
                }
                break;
            }
            else if (hit.transform.tag == "Wall")
            {
                if (selectdTrap.type == Traps.TypeTrap.WALL_TRAP)
                {


                    rotation.x = hit.normal.z;
                    rotation.z = -hit.normal.x;
                    rotation.y = 0f;
                    rotation.w = 1f;

                    Collider[] colliders = Physics.OverlapBox(hit.point, new Vector3(selectdTrap.length / 2, selectdTrap.length / 2, selectdTrap.length / 2));
                    foreach (Collider coll in colliders)
                    {
                        if (coll.transform.tag == "Ground" || coll.transform.tag == "Trap")
                        {
                            isPosOk = false;
                        }
                        if (coll.transform.tag == "Wall")
                        {
                            if (coll.transform.rotation.y != hit.transform.rotation.y && coll.transform.rotation.y != -hit.transform.rotation.y) //wall not in the same direction
                            {
                                isPosOk = false;
                            }
                        }
                    }

                    //checks on the corner of the trap
                    RaycastHit hitCorner;
                    //first corner
                    if (rotation.x == 1f || rotation.x == -1f)
                    {
                        Physics.Raycast(hit.point + new Vector3(selectdTrap.length / 2, selectdTrap.length / 2, 1f), Vector3.back, out hitCorner, 2f);
                    }
                    else
                    {
                        Physics.Raycast(hit.point + new Vector3(1f, selectdTrap.length / 2, selectdTrap.length / 2), Vector3.left, out hitCorner, 2f);
                    }
                    if (hitCorner.transform == null)
                    {
                        isPosOk = false;
                    }

                    //second corner
                    if (rotation.x == 1f || rotation.x == -1f)
                    {
                        Physics.Raycast(hit.point + new Vector3(-selectdTrap.length / 2, selectdTrap.length / 2, 1f), Vector3.back, out hitCorner, 2f);
                    }
                    else
                    {
                        Physics.Raycast(hit.point + new Vector3(1f, selectdTrap.length / 2, -selectdTrap.length / 2), Vector3.left, out hitCorner, 2f);
                    }
                    if (hitCorner.transform == null)
                    {
                        isPosOk = false;
                    }

                    prevesualize = Instantiate(previewTraps[indexSelected].gameObject, hit.point, rotation);

                }
                break;
            }
            else if (hit.transform.tag == "Roof")
            {
                if (selectdTrap.type == Traps.TypeTrap.ROOF_TRAP)
                {
                    Vector3 pos = hit.point;
                    pos.y += 0.15f;
                    rotation.SetLookRotation(transform.forward, -transform.up);

                    //verify no overlap
                    Collider[] colliders = Physics.OverlapBox(hit.point, new Vector3(selectdTrap.length / 2, 0, selectdTrap.length / 2));
                    foreach (Collider coll in colliders)
                    {
                        if (coll.transform.tag == "Wall" || coll.transform.tag == "Tower" || coll.transform.tag == "Trap" || coll.transform.tag == "Base")
                        {
                            isPosOk = false;
                        }
                    }

                    //verify corners
                    RaycastHit[] hitCorner = new RaycastHit[4];
                    Physics.Raycast(pos + new Vector3(selectdTrap.length / 2, -0.2f, selectdTrap.length / 2), Vector3.up, out hitCorner[0], 0.5f);
                    Physics.Raycast(pos + new Vector3(-selectdTrap.length / 2, -0.2f, selectdTrap.length / 2), Vector3.up, out hitCorner[1], 0.5f);
                    Physics.Raycast(pos + new Vector3(selectdTrap.length / 2, -0.2f, -selectdTrap.length / 2), Vector3.up, out hitCorner[2], 0.5f);
                    Physics.Raycast(pos + new Vector3(-selectdTrap.length / 2, -0.2f, -selectdTrap.length / 2), Vector3.up, out hitCorner[3], 0.5f);
                    for (int i = 0; i < 4; i++)
                    {
                        if (hitCorner[i].collider == null)
                        {
                            isPosOk = false;
                        }
                    }

                    prevesualize = Instantiate(previewTraps[indexSelected].gameObject, pos, rotation);
                }
                break;
            }
        }

        if (selectdTrap.price > gameObject.GetComponent<EntityPlayer>().Money)
        {
            isPosOk = false;
            GetComponent<EntityPlayer>().DrawNotEnoughMoney();

            if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
            {
                if (Input.GetKeyDown(KeyCode.Y) == true)
                {
                    SoundManager.Instance.CantBuyPlay(gameObject);
                }
            }

            if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
            {
                if (InputManager.Instance.isPressed(controller.device, "Y", true))
                {
                    SoundManager.Instance.CantBuyPlay(gameObject);
                }
            }
        }

        if ((waveManager.isWaveActive && !m_currentSettings.PlayersCanUseTrapInWave) || (!waveManager.isWaveActive && !m_currentSettings.PlayersCanUseTrapOutWave))
        {
            isPosOk = false;
        }

        if (prevesualize != null)
        {
            if (isPosOk)
            {
                //activate shader
                ActivateShader(previewGood, true);
            }
            else
            {
                ActivateShader(previewBad, false);
            }
        }

        canIPlaceTrap = isPosOk;
    }

    public void SetTrap(int _index)
    {
        indexSelected = _index;
        selectdTrap = traps[indexSelected % traps.Length];
    }

    private void ActivateShader(Shader _shader, bool _damageZoneActive)
    {
        int childs = prevesualize.transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            if (prevesualize.transform.GetChild(i).tag == "TrapVisual")
            {
                int secondChilds = prevesualize.transform.GetChild(i).childCount;
                for (int j = 0; j < secondChilds; j++)
                {
                    prevesualize.transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.shader = _shader;
                }
            }
            if (prevesualize.transform.GetChild(i).tag == "DamageZone")
            {
                prevesualize.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = _damageZoneActive;
            }
        }

    }
}
