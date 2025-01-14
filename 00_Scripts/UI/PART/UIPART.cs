using NUnit.Framework;
using UnityEngine;

public class UIPART : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    public virtual void Open()
    {
        gameObject.SetActive(true);
        Canvas_Holder.Uis.Enqueue(this);
    }

    public virtual void Close()
    {
        if (IsActive == false)
        {
            Debug.LogWarning("Not Active this UI");
            return;
        }
        Canvas_Holder.Uis.Dequeue();
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("Out");
            return;
        }
        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (IsActive)
            Close();
        else Open();
    }
}
