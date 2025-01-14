using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Building_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Building_Scriptable m_Data;

    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_Text;

    BUILDING parentPanel;
    public void Init(Building_Scriptable Data, BUILDING building)
    {
        m_Data = Data;
        parentPanel = building;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parentPanel.GetClick == false)
        {
            parentPanel.GetClick = true;
            parentPanel.AnimationChange("Click");
        }
        parentPanel.GetItemsData(m_Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel == null) return;
        parentPanel.SetItemClickAnimation(this);

        Canvas_Holder.instance.GetPopUp().Set_PopUP(String_Table.Building, m_Data.Key, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentPanel == null) return;

        if (parentPanel.ItemClickTap.activeSelf == true)
            parentPanel.ItemClickTap.SetActive(false);

        Canvas_Holder.instance.DestroyPopup();
    }

    public void SetData()
    {
        gameObject.SetActive(true);
        m_Icon.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        m_Text.text = Utils.Localization_Text(String_Table.Building, m_Data.Key);
    }


    // OnEnable -> 오브젝트의 액티브값이 활성화 됐을 때
    // OnDisable -> 오브젝트의 액티브값이 비활성화 됐을 때
}
