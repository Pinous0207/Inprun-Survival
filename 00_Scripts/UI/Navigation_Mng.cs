using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using System.Collections.Generic;
public class Navigation_Mng : MonoBehaviour
{
    public static Navigation_Mng instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    [SerializeField] private Transform Content;
    [SerializeField] private int Maximum;
    Nav_Item[] P_Item;


    private void Start()
    {
        P_Item = GetComponentsInChildren<Nav_Item>(true);
    }

    public void PanelGet_Item(Item_Scriptable data, int Count)
    {
        MakeItem(0).Init(data, Count);
    }

    public void PanelGet_Toast(Scriptable_Base data, string key)
    {
        MakeItem(1).Init_Building(data, key);
    }

    private Nav_Item MakeItem(int value)
    {
        var go = Instantiate(P_Item[value], Content);
        go.transform.SetAsFirstSibling();
        go.gameObject.SetActive(true);

        if (Content.childCount > Maximum)
        {
            DestroyImmediate(Content.GetChild(Content.childCount - 1).gameObject);
        }

        return go;
    }
}
