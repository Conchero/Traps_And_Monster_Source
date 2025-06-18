using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UiPause : MonoBehaviour
{
    //liste des bouttons en enfant
    public Button[] m_buttons;
    //Component présent sur l'objet
    GridLayoutGroup[] m_gridLayoutGroup = new GridLayoutGroup[2];
    //Nombre de player
    int m_playerCount;

    EntityPlayer m_linkedPlayer;
    //Id du player auquel est associé l'UI
    int m_playerID;
    //Bouton actuelement selectionné
    int m_currentButton;
    int m_previousButton;
    //Nombre total de button
    int m_nbButton;

    public GridLayoutGroup otherGLG;
    public GameObject UIInput;
    public Slider sliderSensitivity;
	
    public List<Sprite> listSpriteInput = new List<Sprite>();

    public enum PauseButton
    {
        Empty,
        Touches,
        Quitter
    }

    public enum StatePause
    {
        Main,
        Touches
    }

    [HideInInspector]
    public TpsController m_TPSController;
    public PauseButton m_eSelectedPauseButton;
    public StatePause m_eInState;

    StateGame gameState;

    GameObject pauseParent;

    float m_cooldownClique = 0.0f;
    public float m_cooldownCliqueMax = 0.2f;
    public bool CanClick()
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
        pauseParent = transform.parent.gameObject;

        gameState = FindObjectOfType<StateGame>();

        m_eSelectedPauseButton = PauseButton.Touches;
        m_eInState = StatePause.Main;

        //Click cooldown
        m_cooldownClique = 0;

        //Recuperation du composant Grid
        m_gridLayoutGroup[0] = GetComponent<GridLayoutGroup>();
        m_gridLayoutGroup[1] = otherGLG;
        //recuperation du nombre de players
        m_playerCount = DataManager.Instance.m_prefab.Count;

        m_linkedPlayer = transform.GetComponentInParent<EntityPlayer>();
        m_playerID = m_linkedPlayer.m_playerId;

        m_currentButton = 1;
        m_previousButton = m_currentButton;
        //Recuperation du nombre de bouttons
        m_nbButton = m_buttons.Length;
        //Redimensionnement de la barre de capacité en fonction du nombre de joueur
        //Si on est plus de deux joueurs
        for(int i = 0; i < m_gridLayoutGroup.Length; i++)
        {
            if (m_playerCount > 2)
            {
                //En fonction de L'id du joueur
                switch (m_playerID)
                {
                    case 0:
                        // Resize Panel BackGround
                        RectTransform rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(-480, 270, 0);
                        rectTransformBackground.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Panel
                        RectTransform rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 419.9f); // Top Value
                        rectTransform.offsetMin = new Vector2(-764.7f, rectTransform.offsetMin.y); // Left Value
                        rectTransform.localScale = new Vector3(0.15f, 0.36f, 0.3f);

                        // Resize Input
                        RectTransform rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, 51, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, 100, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, 149, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Slider
                        GameObject parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(-465.2f, 282, 0);
                        rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.8f);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = -389;
                        m_gridLayoutGroup[i].padding.left = -888;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    case 1:
                        // Resize Panel BackGround
                        rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(480, 270, 0);
                        rectTransformBackground.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Panel
                        rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 419.9f); // Top Value
                        rectTransform.offsetMax = new Vector2(880, rectTransform.offsetMax.y); // Right Value
                        rectTransform.localScale = new Vector3(0.15f, 0.36f, 0.3f);

                        // Resize Input
                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, 51, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, 100, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, 149, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Slider
                        parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(501.1f, 282, 0);
                        rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.8f);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = -389;
                        m_gridLayoutGroup[i].padding.left = 1007;
                        //Cell size
                        cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    case 2:
                        // Resize Panel BackGround
                        rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(-480, -270, 0);
                        rectTransformBackground.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Panel
                        rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -406.2f); // Bottom Value
                        rectTransform.offsetMin = new Vector2(-764.7f, rectTransform.offsetMin.y); // Left Value
                        rectTransform.localScale = new Vector3(0.15f, 0.36f, 0.3f);

                        // Resize Input
                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, -503, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, -454, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(-869.9f, -405, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Slider
                        parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(-465.2f, -262.5f, 0);
                        rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.8f);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = 666;
                        m_gridLayoutGroup[i].padding.left = -888;
                        //Cell size
                        cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    case 3:
                        // Resize Panel BackGround
                        rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(480, -270, 0);
                        rectTransformBackground.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Panel
                        rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -406.2f); // Bottom Value
                        rectTransform.offsetMax = new Vector2(880, rectTransform.offsetMax.y); // Right Value
                        rectTransform.localScale = new Vector3(0.15f, 0.36f, 0.3f);

                        // Resize Input
                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, -503, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, -454, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(86, -405, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize Slider
                        parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(501.1f, -262.5f, 0);
                        rectTransform.localScale = new Vector3(0.4f, 0.4f, 0.8f);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = 666;
                        m_gridLayoutGroup[i].padding.left = 1007;
                        //Cell size
                        cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    default:
                        Debug.Log("error switch");
                        break;
                }
            }
            //Si on est deux joueurs
            else if (m_playerCount == 2)
            {
                //En fonction de L'id du joueur
                switch (m_playerID)
                {
                    case 0:
                        // Resize Panel BackGround
                        RectTransform rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(0, 270, 0);
                        rectTransformBackground.localScale = new Vector3(1, 0.5f, 1);

                        // Resize Panel
                        RectTransform rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 435.9f); // Top value
                        rectTransform.localScale = new Vector3(0.27f, 0.36f, 1);

                        // Resize Input
                        RectTransform rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, 64, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, 129, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, 194, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        // Resize Slider
                        GameObject parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(0, 282, 0);
                        rectTransform.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = -448;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    case 1:
                        // Resize Panel BackGround
                        rectTransformBackground = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransformBackground.localPosition = new Vector3(0, -270, 0);
                        rectTransformBackground.localScale = new Vector3(1, 0.5f, 1);

                        // Resize Panel
                        rectTransform = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
                        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -430); // Bottom Value
                        rectTransform.localScale = new Vector3(0.27f, 0.36f, 1);

                        // Resize Input
                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, -476, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, -411, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        rectTransformInput = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
                        rectTransformInput.localPosition =
                            new Vector3(rectTransformInput.localPosition.x, -346, rectTransformInput.localPosition.z);
                        rectTransformInput.localScale = new Vector3(0.75f, 0.75f, 1);

                        // Resize Slider
                        parentSlider = sliderSensitivity.gameObject.transform.parent.transform.parent.gameObject;
                        parentSlider.GetComponent<RectTransform>().localScale = new Vector3(0.68f, 0.52f, 1);

                        // Resize Control
                        rectTransform = pauseParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
                        rectTransform.localPosition = new Vector3(0, -277.52f, 0);
                        rectTransform.localScale = new Vector3(0.5f, 0.5f, 1);

                        // Resize button
                        m_gridLayoutGroup[i].padding.top = 655;
                        //Cell size
                        cellSize = m_gridLayoutGroup[i].cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup[i].cellSize = cellSize;
                        ////spacing
                        spacing = m_gridLayoutGroup[i].spacing;
                        spacing /= 2;
                        m_gridLayoutGroup[i].spacing = spacing;
                        break;
                    default:
                        Debug.Log("error switch");
                        break;
                }
            }
        }

        // Change input image in left corner of screen and control image according to device
        switch (InputManager.Instance.GetLayoutDevice(m_TPSController.device))
        {
            case "Keyboard":
                Image img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[0];
                img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[1];
                img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[2];

                UIInput.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = listSpriteInput[6];
                break;
            default:
                // GamePad
                img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[3];
                img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[4];
                img = pauseParent.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<Image>();
                img.sprite = listSpriteInput[5];

                UIInput.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = listSpriteInput[7];
                break;
        }


        //Selectionne le premier object par defaut
        SelectObject(m_currentButton, m_previousButton);
    }

    private void Update()
    {
        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }
    }

    public void SelectObject(int _buttonIndex, int _prevButtonIndex)
    {
        //Selection du button
        EventSystem.current.SetSelectedGameObject(transform.GetChild(_buttonIndex).gameObject);
        Animator animator = transform.GetChild(_prevButtonIndex).gameObject.GetComponent<Animator>();
        
        if (animator != null)
        {
            transform.GetChild(_prevButtonIndex).gameObject.GetComponent<Animator>().SetTrigger("Disabled");
        }

        animator = transform.GetChild(_buttonIndex).gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            transform.GetChild(_buttonIndex).gameObject.GetComponent<Animator>().SetTrigger("Selected");
        }
    }

    //Utilisé depuis les autres script pour changer la selection d'un boutton;
    public void SetCurrentButton(int _newButton)
    {
        if (_newButton >= 1 && _newButton <= m_nbButton)
        {
            m_previousButton = m_currentButton;
            m_currentButton = _newButton;
            SelectObject(m_currentButton, m_previousButton);
        }
    }

    public void UpdateControlUiPause()
    {
        switch (m_eSelectedPauseButton)
        {
            case PauseButton.Touches:
                if (InputManager.Instance.isPressed(m_TPSController.device, "A", true) && CanClick())
                {
                    UIInput.SetActive(true);
                    m_eInState = StatePause.Touches;
                }
                break;
            case PauseButton.Quitter:
                if (InputManager.Instance.isPressed(m_TPSController.device, "A", true) && CanClick())
                {
                    gameState.m_gameState = GameState.Scoreboard;
                    gameState.LoadNextState();
                }
                break;
            default:
                break;
        }

        switch (m_eInState)
        {
            case StatePause.Main:
                sliderSensitivity.value = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode());
                
                if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        sliderSensitivity.value--;
                        InputManager.Instance.SetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode(), sliderSensitivity.value);
                        sliderSensitivity.value = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode());
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        sliderSensitivity.value++;
                        InputManager.Instance.SetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode(), sliderSensitivity.value);
                        sliderSensitivity.value = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode());
                    }
                }
                else if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
                {
                    if (InputManager.Instance.isPressed(m_TPSController.device, "LeftBumper", true) && CanClick())
                    {
                        sliderSensitivity.value--;
                        InputManager.Instance.SetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode(), sliderSensitivity.value);
                        sliderSensitivity.value = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode());
                    }
                    else if (InputManager.Instance.isPressed(m_TPSController.device, "RightBumper", true) && CanClick())
                    {
                        sliderSensitivity.value++;
                        InputManager.Instance.SetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode(), sliderSensitivity.value);
                        sliderSensitivity.value = InputManager.Instance.GetSensitivity(InputSystem.GetDevice(m_TPSController.device).GetHashCode());
                    }
                }
                break;
            case StatePause.Touches:
                if (InputManager.Instance.isPressed(m_TPSController.device, "B", true) && CanClick())
                {
                    UIInput.SetActive(false);
                    m_eInState = StatePause.Main;
                }
                break;
            default:
                break;
        }

        if (m_eInState == StatePause.Main)
        {
            if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) == "Keyboard")
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PreviousButtonPause();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    NextButtonPause();
                }
            }
            else if (InputManager.Instance.GetLayoutDevice(m_TPSController.device) != "Keyboard")
            {
                Vector2 JoystickDirectionnel = InputManager.Instance.ValueJoyStick(m_TPSController.device, "LeftStick");

                if (JoystickDirectionnel.y < -0.3f && CanClick())
                {
                    NextButtonPause();
                }
                else if (JoystickDirectionnel.y > 0.3f && CanClick())
                {
                    PreviousButtonPause();
                }
            }
        }
    }

    void NextButtonPause()
    {
        switch (m_eSelectedPauseButton)
        {
            case PauseButton.Touches:
                m_eSelectedPauseButton = PauseButton.Quitter;
                SetCurrentButton((int)m_eSelectedPauseButton);
                break;
            default:
                break;
        }
    }

    void PreviousButtonPause()
    {
        switch (m_eSelectedPauseButton)
        {
            case PauseButton.Quitter:
                m_eSelectedPauseButton = PauseButton.Touches;
                SetCurrentButton((int)m_eSelectedPauseButton);
                break;
            default:
                break;
        }
    }
}
