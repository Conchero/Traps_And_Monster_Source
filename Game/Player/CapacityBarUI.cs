using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems;

public enum BarType
{
    CAPACITY,
    TRAPS,
    INVOCATION
}
public class CapacityBarUI : MonoBehaviour
{
    public BarType m_barType;
    //liste des bouttons en enfant
    public Button[] m_buttons;
    //Component présent sur l'objet
    GridLayoutGroup m_gridLayoutGroup;
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

    //indicateur touches navigation
    public GameObject m_inputKeyboard;
    public GameObject m_inputController;
    public TpsController m_TPSController;
    string controllerType;
    
    private void Start()
    {
        //Recuperation du composant Grid
        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();
        //recuperation du nombre de players
        m_playerCount = DataManager.Instance.m_prefab.Count;

        m_linkedPlayer = transform.GetComponentInParent<EntityPlayer>();
        m_playerID = m_linkedPlayer.m_playerId;

        m_currentButton = 0;
        m_previousButton = m_currentButton;
        //Recuperation du nombre de bouttons
        m_nbButton = m_buttons.Length;
        //Redimensionnement de la barre de capacité en fonction du nombre de joueur
        //Si on est plus de deux joueurs
        if (m_playerCount > 2)
        {
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                case 0:
                    if (m_barType == BarType.CAPACITY)
                    {

                        m_gridLayoutGroup.padding.bottom = 561;
                        //   m_gridLayoutGroup.padding.top = -180;
                        //  m_gridLayoutGroup.padding.left = -888;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = -1547;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = -1430;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    break;
                case 1:
                    if (m_barType == BarType.CAPACITY)
                    {

                        m_gridLayoutGroup.padding.bottom = 561;

                        m_gridLayoutGroup.padding.left = 996;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = 359;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = 475;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    break;
                case 2:
                    if (m_barType == BarType.CAPACITY)
                    {
                        m_gridLayoutGroup.padding.bottom = 24;
                        // m_gridLayoutGroup.padding.top = 899;
                        //  m_gridLayoutGroup.padding.left = -888;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = 687;
                        m_gridLayoutGroup.padding.left = -1547;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = 687;
                        m_gridLayoutGroup.padding.left = -1430;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }

                    break;
                case 3:
                    if (m_barType == BarType.CAPACITY)
                    {
                        m_gridLayoutGroup.padding.bottom = 24;
                        //  m_gridLayoutGroup.padding.top = 899;
                           m_gridLayoutGroup.padding.left = 996;
                        //Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = 687;
                        m_gridLayoutGroup.padding.left = 359;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        //Layer Y position
                        m_gridLayoutGroup.padding.top = 687;
                        m_gridLayoutGroup.padding.left = 475;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }

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
                    if (m_barType == BarType.CAPACITY)
                    {
                        m_gridLayoutGroup.padding.bottom = Screen.height/2 + m_gridLayoutGroup.padding.bottom/2;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        ////Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = -1546;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        ////Layer Y position
                        m_gridLayoutGroup.padding.top = -386;
                        m_gridLayoutGroup.padding.left = -1427;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    break;
                case 1:
                    if (m_barType == BarType.CAPACITY)
                    {
                        m_gridLayoutGroup.padding.bottom /= 2;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;
                    }
                    else if (m_barType == BarType.TRAPS)
                    {
                        ////Layer Y position
                        m_gridLayoutGroup.padding.top = 696;
                        m_gridLayoutGroup.padding.left = -1546;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    else if (m_barType == BarType.INVOCATION)
                    {
                        ////Layer Y position
                        m_gridLayoutGroup.padding.top = 696;
                        m_gridLayoutGroup.padding.left = -1427;
                        ////Cell size
                        Vector2 cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                        Vector2 spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2f;
                        m_gridLayoutGroup.spacing = spacing;

                    }
                    break;
                default:
                    Debug.Log("error switch");
                    break;
            }
        }


        //indicateur touches navigation
        //si on se sert bien d'indicateur de touche
        if (m_TPSController != null)
        {
            controllerType = InputManager.Instance.GetLayoutDevice(m_TPSController.device);
            if (controllerType == "Keyboard")
            {
                m_inputKeyboard.SetActive(true);
                m_inputController.SetActive(false);
            }
            else if (controllerType != "Keyboard")
            {
                m_inputKeyboard.SetActive(false);
                m_inputController.SetActive(true);
            }
        }

        //Selectionne le premier object par defaut
        SelectObject(m_currentButton, m_previousButton);
    }

    void SelectObject(int _buttonIndex, int _prevButtonIndex)
    {

        //Reset du boutton précedent
        m_buttons[_prevButtonIndex].GetComponent<Image>().color = new Color(1, 1, 1, 40f/255f);

        //scale du boutton
       m_buttons[_buttonIndex].GetComponent<Image>().color = new Color(1, 1, 1, 1);

        //Selection du button
        EventSystem.current.SetSelectedGameObject(transform.GetChild(_buttonIndex).gameObject);

    }

    //Utilisé depuis les autres script pour changer la selection d'un boutton;
    public void SetCurrentButton(int _newButton)
    {
        if (_newButton >= 0 && _newButton <= m_nbButton - 1)
        {
            m_previousButton = m_currentButton;
            m_currentButton = _newButton;
            SelectObject(m_currentButton, m_previousButton);
        }
    }




    private void OnEnable()
    {

        //  EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);


    }
}
