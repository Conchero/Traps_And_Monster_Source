using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems;

//public class TestGUI : Selectable
//{
//override
//}



//public class TestGUI : MonoBehaviour
//{

//    private void Start()
//    {
//        //Navigation procédural pour les bouttons
//        Navigation navigation = GetComponentInChildren<Button>().navigation;
//        //navigation.
//    }

//}


//public class TestGUI : IPointerEnterHandler
//{

//}


////DRAG AND DROP
//public class TestGUI : MonoBehaviour, IBeginDragHandler, IDragHandler
//{
//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        Debug.Log("begin drag");
//       // throw new System.NotImplementedException();
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        GetComponent<RectTransform>().position = eventData.position;
//       // throw new System.NotImplementedException();
//    }
//}


public class AutoSelectGUI : MonoBehaviour
{
    public GameObject m_firstObjectToSelect;
    // bool m_trigger;
    private void Start()
    {
        // m_trigger = false;
        //   EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
        //  Debug.Log("transform.GetChild(0).gameObject" + transform.GetChild(0).gameObject);
        //transform.GetChild(0).GetComponent<Button>().Select();
    }

    private void Update()
    {
        //if (m_trigger)
        //{
        //    m_trigger = !m_trigger;

        //    Animator animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        //    if (animator != null)
        //    {
        //        // Debug.Log(EventSystem.current.currentSelectedGameObject);
        //        Debug.Log("trigger : " + transform.GetChild(0).gameObject.GetComponent<Animator>().GetBool("Selected"));
        //        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Selected");

        //         Debug.Log("trigger : " + transform.GetChild(0).gameObject.GetComponent<Animator>().GetBool("Selected"));

        //        //transform.GetChild(0).gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).
        //    }

        //}


        //  Debug.Log(EventSystem.current.currentSelectedGameObject);
        // EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    IEnumerator DelateSelection()
    {
        yield return 0;
        if (m_firstObjectToSelect != null)
        {
            //Set du premier button à l'activation de l'objet
            EventSystem.current.SetSelectedGameObject(m_firstObjectToSelect);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
        }
    }

    private void OnEnable()
    {
    
        StartCoroutine(DelateSelection());
        ////Set du premier button à l'activation de l'objet
        // EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);



        ////recuperation de l'animator
        //Animator animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        //if (animator != null)
        //{
        //   // Debug.Log(transform.GetChild(0).gameObject.name);
        //    //Debug.Log(transform.GetChild(0).gameObject.name);
        //    // Debug.Log(EventSystem.current.currentSelectedGameObject);
        //  //  Debug.Log("trigger : " + transform.GetChild(0).gameObject.GetComponent<Animator>().GetBool("Selected"));
        //    transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Selected");

        //   // Debug.Log("trigger : " + transform.GetChild(0).gameObject.GetComponent<Animator>().GetBool("Selected"));
        //}




    }


}



