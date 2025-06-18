using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class State : MonoBehaviour
{

    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
    
    }
    public virtual void PrintState()
    {
    
    }
    public virtual void LoadNextState()
    {
    
    }

    //// Update is called once per frame
    //protected virtual void checkIfDead()
    //{
    //    // Debug.Log("m_nbVie" + m_nbVie);
    //    if (m_nbVie <= 0)
    //    {

    //        Destroy(gameObject);
    //    }
    //}

    //private MenuState m_menuState;
    // private GameState m_gameState;

    // private void Start()
    //{
    //    m_menuState = MenuState.Home;
    //    m_gameState = GameState.Intro;
    //  //  Debug.Log("m_menuState : " + m_menuState.ToString());
    //}


    //  public void SetMenuState(MenuState _state)
    //{
    //    m_menuState = _state;
    //}
    //  public MenuState GetMenuState()
    //{
    //    return m_menuState;
    //}
    //  public void SetGameState(GameState _state)
    //{
    //    m_gameState = _state;
    //}
    //  public GameState GetGameState()
    //{
    //    return m_gameState;
    //}

    // public void PrintCurrentMenuState()
    //{
    //    Debug.Log("m_menuState : " + m_menuState.ToString());
    //}
    //public void PrintCurrentGameState()
    //{
    //    Debug.Log("m_gameState : " + m_gameState.ToString());
    //}

}
