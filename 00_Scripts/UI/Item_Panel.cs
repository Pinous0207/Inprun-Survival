using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ITEM m_Item;
    public GameObject m_ITEMPANEL;
    public Image Rarity;
    public Image Item_Icon;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemWeightText;
    public INVENTORY parentPanel;
    public void Init(ITEM item, INVENTORY inventory)
    {
        m_Item = item;
        parentPanel = inventory;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel == null) return;
        parentPanel.SetItemClickAnimation(this);
        Canvas_Holder.instance.GetPopUp().Set_PopUP(String_Table.Item, m_Item.Data.Key, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentPanel == null) return;

        if (parentPanel.ItemClickTap.activeSelf == true)
            parentPanel.ItemClickTap.SetActive(false);

        Canvas_Holder.instance.DestroyPopup();
    }

    public void SetItem()
    {
        m_ITEMPANEL.gameObject.SetActive(m_Item.Data == null ? false : true);
        if (m_Item.Data != null)
        {
            Rarity.sprite = Asset_Mng.Get_Atlas(m_Item.Data.rarity.ToString());
            Item_Icon.sprite = Asset_Mng.Get_Atlas(m_Item.Data.Key);
            ItemCountText.text = m_Item.Count.ToString();
            ItemWeightText.text = string.Format("{0:0.0}",ItemFlowController.WeightItem(m_Item.Data.Key));
        }
        else
        {
            Rarity.sprite = Asset_Mng.Get_Atlas("DefaultSquare");
        }
    }
}
