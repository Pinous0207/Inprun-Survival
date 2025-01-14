using UnityEngine;
using UnityEngine.EventSystems;

public class Unit_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Unit_Scriptable m_Data;

    private PORTAL parentPanel;
    public void Init(PORTAL parent_Data)
    {
        parentPanel = parent_Data;
    }

    public void SetData()
    {
        parentPanel.SetData(m_Data, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
