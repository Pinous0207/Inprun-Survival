using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class PORTAL : UIPART
{
    public Unit_Panel[] panels;
    Portal m_Portal;
    [SerializeField] private Image MainIcon;
    [SerializeField] private TextMeshProUGUI MainSpeech;
    [SerializeField] private TextMeshProUGUI MainName;

    [SerializeField] private GameObject Panel;
    [SerializeField] private Transform Content;

    List<GameObject> Gorvage = new List<GameObject>();
    Unit_Scriptable Data;
    private void Start()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].Init(this);
        }
    }

    public void Init(Portal portal)
    {
        m_Portal = portal;
    }

    private void OnEnable()
    {
        for (int i = 0; i < panels.Length; i++) panels[i].transform.GetChild(0).gameObject.SetActive(false);
        if (Gorvage.Count > 0)
        {
            for (int i = 0; i < Gorvage.Count; i++)
            {
                Destroy(Gorvage[i]);
            }
            Gorvage.Clear();
        }
        MainSetActive(false);
    }

    public void SetBuildObject()
    {
        bool CanBuild = true;
        for (int i = 0; i < Data.itemList.Count; i++)
        {
            ITEM item = Data.itemList[i];
            if (ItemFlowController.ItemCount(item.Data.Key) < item.Count)
            {
                CanBuild = false;
                break;
            }
        }
        Portal portal = new Portal();
        portal = m_Portal;

        if (CanBuild == false) return;

        for (int i = 0; i < Data.itemList.Count; i++)
        {
            ITEM item = Data.itemList[i];
            ItemFlowController.REMOVEITEM(item.Data.Key, item.Count);
        }
        Close();
        portal.GetComponent<Building_OBJ>().SetMakeData(Data.Key, Data.timer, 
            () => portal.GetWorker());
        // 생성하기
    }

    private void MainSetActive(bool isActive)
    {
        MainIcon.gameObject.SetActive(isActive);
        MainName.gameObject.SetActive(isActive);
        MainSpeech.gameObject.SetActive(isActive);
    }

    public override void Close()
    {
        Delegate_Holder.OnOutInteraction();
        base.Close();
    }

    public void SetData(Unit_Scriptable m_Data, Unit_Panel panel)
    {
        Data = m_Data;
        for (int i = 0; i < panels.Length; i++) panels[i].transform.GetChild(0).gameObject.SetActive(false);
        panel.transform.GetChild(0).gameObject.SetActive(true);

        if(Gorvage.Count > 0)
        {
            for(int i = 0; i < Gorvage.Count; i++)
            {
                Destroy(Gorvage[i]);
            }
            Gorvage.Clear();
        }

        MainSetActive(true);

        MainIcon.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        MainName.text = Utils.Localization_Text(String_Table.Unit, m_Data.Key);
        MainSpeech.text = Utils.Localization_Text(String_Table.Unit, m_Data.Key + "_Speech_Value");

        for(int i = 0; i < m_Data.itemList.Count; i++)
        {
            Item_Scriptable itemData = m_Data.itemList[i].Data;
            var go = Instantiate(Panel, Content);
            go.SetActive(true);

            Utils.FindBase<Image>(go.transform, "Icon").sprite = Asset_Mng.Get_Atlas(itemData.Key);
            Utils.FindBase<TextMeshProUGUI>(go.transform, "Title").text =
                Utils.Localization_Text(String_Table.Item, itemData.Key);

            var goText = Utils.FindBase<TextMeshProUGUI>(go.transform, "Count");

            goText.text = string.Format("({0}/{1})", m_Data.itemList[i].Count, ItemFlowController.ItemCount(itemData.Key));
            goText.color = ItemFlowController.ItemCount(itemData.Key) > m_Data.itemList[i].Count ? Color.green : Color.red;
            Gorvage.Add(go);
        }
    }
}
