using System.Collections;
using UnityEngine;

public class Game_Mng : MonoBehaviour
{
    public int Stamina, MaxStamina;

    private void Start()
    {
        Stamina = MaxStamina;
        StartCoroutine(DelayStamina());
    }

    IEnumerator DelayStamina() {
        yield return new WaitForSeconds(0.02f);
        SetStamina(0, false);
    }

    public void SetStamina(int value, bool GetText = true)
    {
        Stamina += value;
        if (GetText)
        {
            Color color = value > 0 ? Color.green : Color.red;
            Canvas_Holder.instance.GetText(value.ToString(), color, P_Movement.instance.transform.position);
        }
        Delegate_Holder.OnStaminaChange(Stamina);
    }
}
