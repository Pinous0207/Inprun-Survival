using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class Utils : MonoBehaviour
{
    public static string Localization_Text(String_Table table, string key)
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(table.ToString(), key, currentLanguage);
        return localizedString;
    }

    public static string Timer(float time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);

        return timer;
    }

    public static T FindBase<T>(Transform parent, string key)
    {
        return parent.Find(key).GetComponent<T>();
    }

    public static void SetLayer(string layer, GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(layer);
    }
}
