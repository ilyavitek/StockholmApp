using UnityEngine;
using UnityEngine.Events;

public class MyHoloButton : MonoBehaviour {

    public UnityEvent m_event;

    //[SerializeField]
    //Placeable m_placable;

    public void OnSelect () {

        m_event.Invoke ();

        //m_placable.IsChildrenAutoHideAndShow = !m_placable.IsChildrenAutoHideAndShow;
        //m_placable.SetChildrensToHide (m_placable.IsChildrenAutoHideAndShow);
    }
}
