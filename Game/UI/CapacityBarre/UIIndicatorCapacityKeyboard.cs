using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicatorCapacityKeyboard : MonoBehaviour
{
    //Component présent sur l'objet
    GridLayoutGroup m_gridLayoutGroup;
    //Nombre de player
    int m_playerCount;

   public EntityPlayer m_linkedPlayer;
    //Id du player auquel est associé l'UI
    int m_playerID;

    private void Start()
    {
        //Recuperation du composant Grid
        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();
        //recuperation du nombre de players
        m_playerCount = DataManager.Instance.m_prefab.Count;

        m_playerID = m_linkedPlayer.m_playerId;

       
        //Redimensionnement de la barre de capacité en fonction du nombre de joueur
        //Si on est plus de deux joueurs
        if (m_playerCount > 2)
        {
            Vector2 cellSize;
            Vector2 spacing;
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                case 0:

                   m_gridLayoutGroup.padding.top = -507;
                  m_gridLayoutGroup.padding.left = 108;
                    //Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                        ////spacing
                         spacing = m_gridLayoutGroup.spacing;
                        spacing /= 2;
                        m_gridLayoutGroup.spacing = spacing;
                   
                    break;
                case 1:

                    m_gridLayoutGroup.padding.top = -507;
                    m_gridLayoutGroup.padding.left = 1059;
                    //Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                    ////spacing
                    spacing = m_gridLayoutGroup.spacing;
                    spacing /= 2;
                    m_gridLayoutGroup.spacing = spacing;

                    break;
                case 2:

                  m_gridLayoutGroup.padding.top = 32;
                  m_gridLayoutGroup.padding.left = 108;
                    //Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                    ////spacing
                    spacing = m_gridLayoutGroup.spacing;
                    spacing /= 2;
                    m_gridLayoutGroup.spacing = spacing;


                    break;
                case 3:

               m_gridLayoutGroup.padding.top = 32;
               m_gridLayoutGroup.padding.left = 1059;
                    //Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2;
                        m_gridLayoutGroup.cellSize = cellSize;
                    ////spacing
                    spacing = m_gridLayoutGroup.spacing;
                    spacing /= 2;
                    m_gridLayoutGroup.spacing = spacing;

                    break;
                default:
                    Debug.Log("error switch");
                    break;
            }




        }
        //Si on est deux joueurs
        else if (m_playerCount == 2)
        {
            Vector2 cellSize;
            Vector2 spacing;
            //En fonction de L'id du joueur
            switch (m_playerID)
            {
                case 0:
                    m_gridLayoutGroup.padding.top = -507;
                    m_gridLayoutGroup.padding.left = 108;
                    
                    ////Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                    //spacing
                    spacing = m_gridLayoutGroup.spacing;
                    spacing /= 2f;
                    m_gridLayoutGroup.spacing = spacing;

                    break;
                case 1:

                    m_gridLayoutGroup.padding.left = 108;
                    m_gridLayoutGroup.padding.top = 32;
                    ////Cell size
                    cellSize = m_gridLayoutGroup.cellSize;
                        cellSize /= 2f;
                        m_gridLayoutGroup.cellSize = cellSize;
                    ////spacing
                    spacing = m_gridLayoutGroup.spacing;
                    spacing /= 2f;
                    m_gridLayoutGroup.spacing = spacing;

                    break;
                default:
                    Debug.Log("error switch");
                    break;
            }
        }


    }





}
