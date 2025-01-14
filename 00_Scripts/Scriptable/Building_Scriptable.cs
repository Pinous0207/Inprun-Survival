using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Building_Scriptable", menuName = "Scriptable Objects/Building_Scriptable")]
public class Building_Scriptable : Scriptable_Base
{
    public float timer;
    public List<ITEM> m_Items = new List<ITEM>();

    public Building_OBJ obj;
}
