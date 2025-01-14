using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nav_Item : MonoBehaviour
{
    [SerializeField] private Image Rarity_Image;
    [SerializeField] private Image Item_Icon_Image;
    [SerializeField] private TextMeshProUGUI Item_Name_Text;

    public void Init(Item_Scriptable m_Data, int Count)
    {
        Rarity_Image.sprite = Asset_Mng.Get_Atlas(m_Data.rarity.ToString());
        Item_Icon_Image.sprite = Asset_Mng.Get_Atlas(m_Data.Key);

        Item_Name_Text.text = Utils.Localization_Text(String_Table.Item, m_Data.Key) + "x" + Count.ToString();
    }

    public void Init_Building(Scriptable_Base m_Data, string key)
    {
        Item_Icon_Image.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        Item_Name_Text.text = Utils.Localization_Text(String_Table.UI, key);
    }
}
