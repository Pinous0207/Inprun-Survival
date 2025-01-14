using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Directional_Monster_Slider : MonoBehaviour
{
    [SerializeField] private Image Slider01Fill, Slider02Fill;
    public Monster monster;
    Coroutine coroutine;
    public void GetSliderCheck()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SliderCoroutine());
    }
    IEnumerator SliderCoroutine()
    {
        float value = (float)monster.HP / (float)monster.MaxHP;
        Slider02Fill.fillAmount = value;
        float timer = 0.0f;
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            Slider01Fill.fillAmount = Mathf.Lerp(Slider01Fill.fillAmount, Slider02Fill.fillAmount, timer);
            yield return null;
        }
    }

}
