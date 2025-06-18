using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAim : MonoBehaviour
{
    //Stock L'ui player sur lequel on va draw
    public UIPlayer m_UIplayer;
    //Stock le player ID pour appliquer la bonne couleur de barre de vie
    private int m_playerID;
    private int m_playerCount;
    public GameObject[] m_objects;
    EntityPlayer m_entityPlayer;
    TpsController m_TPSController;
    public Camera cam;
    [HideInInspector]
    public Vector3 m_target;

    List<Vector3> startPos = new List<Vector3>();
    List<Vector3> endPos = new List<Vector3>();

    [HideInInspector]
    public bool hitProjectile = false;
    bool hitProcess = false;

    bool isWhite = true;

    // Start is called before the first frame update
    void Start()
    {
        m_entityPlayer = transform.parent.transform.parent.GetComponent<EntityPlayer>();
        m_TPSController = transform.parent.transform.parent.GetComponent<TpsController>();
        m_playerCount = m_UIplayer.m_playerCount;
        //Recupère l' ID
        m_playerID = m_entityPlayer.m_playerId;
        
        // Re position de la visée
        if(m_playerCount == 2)
        {
            if (m_playerID == 0)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30, 540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30, 540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(0, (540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(0, (540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20, (540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20, (540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20, (540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20, (540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(0, -540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30, -540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30, -540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(0, (-540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(0, (-540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20, (-540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20, (-540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20, (-540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20, (-540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }
        else if(m_playerCount > 2)
        {
            if (m_playerID == 0)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, 540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30 + (-960 / 2), 540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30 + (-960 / 2), 540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, (540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, (540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);


                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (-960 / 2), (540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20 + (-960 / 2), (540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20 + (-960 / 2), (540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (-960 / 2), (540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 1)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, 540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30 + (960 / 2), 540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30 + (960 / 2), 540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, (540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, (540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);


                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (960 / 2), (540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20 + (960 / 2), (540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20 + (960 / 2), (540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (960 / 2), (540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 2)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, -540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30 + (-960 / 2), -540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30 + (-960 / 2), -540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, (-540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(-960 / 2, (-540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);


                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (-960 / 2), (-540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20 + (-960 / 2), (-540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20 + (-960 / 2), (-540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (-960 / 2), (-540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (m_playerID == 3)
            {
                m_objects[0].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, -540 / 2, 0);
                m_objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[1].GetComponent<RectTransform>().localPosition = new Vector3(-30 + (960 / 2), -540 / 2, 0);
                m_objects[1].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[2].GetComponent<RectTransform>().localPosition = new Vector3(30 + (960 / 2), -540 / 2, 0);
                m_objects[2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[3].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, (-540 / 2) + 30, 0);
                m_objects[3].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[4].GetComponent<RectTransform>().localPosition = new Vector3(960 / 2, (-540 / 2) - 30, 0);
                m_objects[4].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);


                m_objects[5].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (960 / 2), (-540 / 2) + 20, 0);
                m_objects[5].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[6].GetComponent<RectTransform>().localPosition = new Vector3(20 + (960 / 2), (-540 / 2) + 20, 0);
                m_objects[6].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[7].GetComponent<RectTransform>().localPosition = new Vector3(20 + (960 / 2), (-540 / 2) - 20, 0);
                m_objects[7].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);

                m_objects[8].GetComponent<RectTransform>().localPosition = new Vector3(-20 + (960 / 2), (-540 / 2) - 20, 0);
                m_objects[8].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            }
        }

        startPos.Add(m_objects[1].GetComponent<RectTransform>().localPosition);
        startPos.Add(m_objects[2].GetComponent<RectTransform>().localPosition);
        startPos.Add(m_objects[3].GetComponent<RectTransform>().localPosition);
        startPos.Add(m_objects[4].GetComponent<RectTransform>().localPosition);

        endPos.Add(new Vector3(startPos[0].x + 10, startPos[0].y, startPos[0].z));
        endPos.Add(new Vector3(startPos[1].x - 10, startPos[1].y, startPos[1].z));
        endPos.Add(new Vector3(startPos[2].x, startPos[2].y - 10, startPos[2].z));
        endPos.Add(new Vector3(startPos[3].x, startPos[3].y + 10, startPos[3].z));


        //Init target
        m_target = new Vector3();

    }

    // Update is called once per frame
    void Update()
    {
        // Distance marker
        if(m_entityPlayer.m_weaponBehavior.weaponSelected == "Bow" ||
            m_entityPlayer.m_weaponBehavior.weaponSelected == "CrossBow") // à mettre les autres
        {
            m_objects[1].SetActive(true);
            m_objects[2].SetActive(true);
            m_objects[3].SetActive(true);
            m_objects[4].SetActive(true);
        }
        else if (m_entityPlayer.m_weaponBehavior.weaponSelected == "LaserSword")
        {
            if (m_entityPlayer.m_weaponBehavior.GetComponentInChildren<LaserSword>().LaunchingLaser)
            {
                m_objects[1].SetActive(true);
                m_objects[2].SetActive(true);
                m_objects[3].SetActive(true);
                m_objects[4].SetActive(true);
            }
            else
            {
                m_objects[1].SetActive(false);
                m_objects[2].SetActive(false);
                m_objects[3].SetActive(false);
                m_objects[4].SetActive(false);
            }
        }
        else
        {
            m_objects[1].SetActive(false);
            m_objects[2].SetActive(false);
            m_objects[3].SetActive(false);
            m_objects[4].SetActive(false);
        }

        // Hit marker
        if (hitProjectile)
        {
            for (int i = 5; i <= 8; i++)
            {
                Color color = m_objects[i].GetComponent<Image>().color;
                m_objects[i].GetComponent<Image>().color
                    = new Color(color.r, color.g, color.b, 1);
            }

            hitProjectile = false;
            hitProcess = true;
        }
        if(hitProcess)
        {
            for (int i = 5; i <= 8; i++)
            {
                if (m_objects[i].GetComponent<Image>().color.a != 0)
                {
                    Color color = m_objects[i].GetComponent<Image>().color;
                    m_objects[i].GetComponent<Image>().color
                        = new Color(color.r, color.g, color.b, Mathf.Lerp(m_objects[i].GetComponent<Image>().color.a, 0, Time.deltaTime * 5));
                }
                else
                {
                    hitProcess = false;
                }
            }
        }


        if (m_TPSController.gameState.m_gameState == GameState.Game)
        {
            // Pos serré quand viser
            if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
            {
                if (m_TPSController.canShoot)
                {
                    if (Input.GetButton("MouseButton2"))
                    {
                        m_objects[1].GetComponent<RectTransform>().localPosition
                               = Vector3.Lerp(m_objects[1].GetComponent<RectTransform>().localPosition, endPos[0], Time.deltaTime * 10f);
                        m_objects[2].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[2].GetComponent<RectTransform>().localPosition, endPos[1], Time.deltaTime * 10f);
                        m_objects[3].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[3].GetComponent<RectTransform>().localPosition, endPos[2], Time.deltaTime * 10f);
                        m_objects[4].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[4].GetComponent<RectTransform>().localPosition, endPos[3], Time.deltaTime * 10f);
                    }
                    else if (!Input.GetButton("MouseButton2"))
                    {
                        m_objects[1].GetComponent<RectTransform>().localPosition
                               = Vector3.Lerp(m_objects[1].GetComponent<RectTransform>().localPosition, startPos[0], Time.deltaTime * 10f);
                        m_objects[2].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[2].GetComponent<RectTransform>().localPosition, startPos[1], Time.deltaTime * 10f);
                        m_objects[3].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[3].GetComponent<RectTransform>().localPosition, startPos[2], Time.deltaTime * 10f);
                        m_objects[4].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[4].GetComponent<RectTransform>().localPosition, startPos[3], Time.deltaTime * 10f);
                    }
                }
            }
            else if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
            {
                if (m_TPSController.canShoot)
                {
                    if (InputManager.Instance.isPressed(m_TPSController.device, "LeftTrigger", true))
                    {
                        m_objects[1].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[1].GetComponent<RectTransform>().localPosition, endPos[0], Time.deltaTime * 10f);
                        m_objects[2].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[2].GetComponent<RectTransform>().localPosition, endPos[1], Time.deltaTime * 10f);
                        m_objects[3].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[3].GetComponent<RectTransform>().localPosition, endPos[2], Time.deltaTime * 10f);
                        m_objects[4].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[4].GetComponent<RectTransform>().localPosition, endPos[3], Time.deltaTime * 10f);
                    }
                    else if (InputManager.Instance.isPressed(m_TPSController.device, "LeftTrigger", false))
                    {
                        m_objects[1].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[1].GetComponent<RectTransform>().localPosition, startPos[0], Time.deltaTime * 10f);
                        m_objects[2].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[2].GetComponent<RectTransform>().localPosition, startPos[1], Time.deltaTime * 10f);
                        m_objects[3].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[3].GetComponent<RectTransform>().localPosition, startPos[2], Time.deltaTime * 10f);
                        m_objects[4].GetComponent<RectTransform>().localPosition
                            = Vector3.Lerp(m_objects[4].GetComponent<RectTransform>().localPosition, startPos[3], Time.deltaTime * 10f);
                    }
                }
            }


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

            //Players
            for (int i = 0; i < 3; i++)
            {
                if (m_entityPlayer.m_playerId != i)
                {
                    switch (i)
                    {
                        case 0:
                            layerMaskP1 = LayerMask.GetMask("P1");
                            break;
                        case 1:
                            layerMaskP2 = LayerMask.GetMask("P2");
                            break;
                        case 2:
                            layerMaskP3 = LayerMask.GetMask("P3");
                            break;
                        case 3:
                            layerMaskP4 = LayerMask.GetMask("P4");
                            break;
                    }
                }
            }
            //Decor traps, invoc et demons
            layerMaskDecor  = LayerMask.GetMask("Decor");
            layerMaskTraps = LayerMask.GetMask("Trap");
            layerMaskInvoc = LayerMask.GetMask("Invoc");
            layerMaskDemons = LayerMask.GetMask("Demons");



            RaycastHit hit;
            bool isTouch =
                Physics.Raycast(cam.gameObject.transform.position + (2.5f * cam.gameObject.transform.forward),
                cam.gameObject.transform.forward,
                out hit,
                88f   ,
             layerMaskP1 + layerMaskP2 + layerMaskP3 + layerMaskP4 + layerMaskDecor + layerMaskTraps + layerMaskInvoc + layerMaskDemons);
            
            if (isTouch)
            {
                //Si on touche, la cible est l'objet touché
                m_target = hit.point;

                if (hit.collider.gameObject.tag == "Player"
                    || hit.collider.gameObject.tag == "EnnemiBody"
                    || hit.collider.gameObject.tag == "EnnemiHead"
                    || hit.collider.gameObject.tag == "PlayerBody"
                    || hit.collider.gameObject.tag == "PlayerHead"
                    || hit.collider.gameObject.tag == "PlayerMesh"
                    || hit.collider.gameObject.tag == "Trap"
                    || hit.collider.gameObject.tag == "Invocation"
                    || hit.collider.gameObject.tag == "InvocationHead"
                    || hit.collider.gameObject.tag == "InvocationBody")
                {
                    for (int i = 0; i < m_objects.Length; i++)
                    {
                        m_objects[i].GetComponent<Image>().color
                            = new Color(1, 0, 0, m_objects[i].GetComponent<Image>().color.a);
                    }
                    isWhite = false;
                }
				else if(hit.collider.gameObject.GetComponent<Nexus>() != null)
				{
					if(m_entityPlayer.m_sColor != hit.collider.gameObject.GetComponent<Nexus>().m_sColor)
                    {
                        for (int i = 0; i < m_objects.Length; i++)
                        {
                            m_objects[i].GetComponent<Image>().color
                                = new Color(1, 0, 0, m_objects[i].GetComponent<Image>().color.a);
                        }
                        isWhite = false;
                    }
                }
                else
                {
                    for (int i = 0; i < m_objects.Length; i++)
                    {
                        m_objects[i].GetComponent<Image>().color
                            = new Color(1, 1, 1, m_objects[i].GetComponent<Image>().color.a);
                    }
                    isWhite = true;
                }
            }
            else
            {
                if(!isWhite)
                {
                    for (int i = 0; i < m_objects.Length; i++)
                    {
                        m_objects[i].GetComponent<Image>().color
                            = new Color(1, 1, 1, m_objects[i].GetComponent<Image>().color.a);
                    }
                    isWhite = true;
                }

                //Si on touche pas, la cible est un point loin sur le ray cast
                m_target = cam.gameObject.transform.position + (cam.gameObject.transform.forward*50);

            }

          //  Debug.DrawRay(cam.gameObject.transform.position + (2.5f * cam.gameObject.transform.forward), cam.gameObject.transform.forward * 88, Color.red);
        }
    }
}
