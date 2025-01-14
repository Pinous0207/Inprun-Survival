using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private Dictionary<string, UIPART> uiParts = new Dictionary<string, UIPART>();
    private Dictionary<Monster, Directional_Monster_Slider> monsterSliders = new Dictionary<Monster, Directional_Monster_Slider>();
    public static Queue<UIPART> Uis = new Queue<UIPART>();
    PopUP_Description popup;
    public Directional_Monster_Slider monster_Slider;

    public UIPART GetUIPART(string name)
    {
        if(uiParts.ContainsKey(name))
        {
            return uiParts[name];
        }
        var uiPart = Instantiate(Resources.Load<UIPART>("UI/" + name), UI_PART_PARENT);
        uiParts.Add(name, uiPart);
        uiPart.gameObject.SetActive(false);
        return uiPart;
    }

    public void DestroyPopup()
    {
        if (popup != null) Destroy(popup.gameObject);
    }

    public PopUP_Description GetPopUp()
    {
        DestroyPopup();
        popup = Instantiate(Resources.Load<PopUP_Description>("Prefab/PopUp"), transform);

        return popup;
    }

    public void OpenUI(string uiName)
    {
        if (uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Open();
        }
        else Debug.LogWarning($"UI {uiName} not found.");
    }

    public void CloseUI(string uiName)
    {
        if(uiParts.ContainsKey(uiName))
        {
            uiParts[uiName].Close();
        }
    }

    public void CloseAllUI(string name = "")
    {
        foreach(var part in uiParts)
        {
            if(part.Key != name)
            {
                part.Value.Close();
            }
        }
    }
    [SerializeField] private Transform UI_PART_PARENT;
    [SerializeField] private GameObject Board;
    public Image BoardHpFill, BoardHpWhiteFill;
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image StaminaFill;
    [SerializeField] private Image HPFill;
    Coroutine F_Coroutine;
    private void Start()
    {
        UIPART[] parts = UI_PART_PARENT.GetComponentsInChildren<UIPART>(true);
        foreach(var part in parts)
        {
            uiParts.Add(part.name, part);
        }
        Delegate_Holder.OnInteractionOut += BoardOut;
        Delegate_Holder.OnStamina += StaminaCheck;
        Delegate_Holder.OnHP += HPCheck;
    }

    private void Update()
    {
        CheckSlider();
        CheckUI(KeyCode.I, "INVENTORY");
        CheckUI(KeyCode.B, "BUILDING");
    }
    public void AddSlider(Monster monster)
    {
        if (monsterSliders.ContainsKey(monster))
        {
            monsterSliders[monster].GetSliderCheck();
        }
        else
        {
            var go = Instantiate(monster_Slider, transform);
            go.monster = monster;
            monsterSliders.Add(monster, go);
            monsterSliders[monster].GetSliderCheck();
        }
    }
    public void RemoveSlider(Monster monster)
    {
        monsterSliders[monster].GetComponent<Animator>().SetTrigger("Out");
        monsterSliders.Remove(monster);
    }
    private void CheckSlider()
    {
        foreach(var slider in monsterSliders)
        {
            Vector3 pos = slider.Key.transform.position;
            pos.y += 2.0f;
            slider.Value.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos);
        }
    }

    public void GetText(string temp, Color color, Vector3 posReal)
    {
        posReal.y += 0.5f;
        posReal.x += Random.Range(-0.5f, 0.5f);
        posReal.z += Random.Range(-0.5f, 0.5f);

        var go =Instantiate(Resources.Load<GameObject>("TextObject"), posReal, Quaternion.Euler(55, 0, 0));
        TextMeshPro textObj = go.GetComponent<TextMeshPro>();
        textObj.color = color;
        textObj.text = temp;
    }

    private void HPCheck(int value)
    {
        Character character = P_Movement.instance.transform.GetComponent<Character>();
        HPText.text = character.HP.ToString() + "/" + character.MaxHP.ToString();
        HPFill.fillAmount = (float)character.HP / (float)character.MaxHP;
    }
    private void StaminaCheck(int value)
    {
        StaminaText.text = Base_Mng.Game.Stamina + "/" + Base_Mng.Game.MaxStamina;
        StaminaFill.fillAmount = (float)Base_Mng.Game.Stamina / (float)Base_Mng.Game.MaxStamina;
    }
    void CheckUI(KeyCode key, string uiName)
    {
        if(Input.GetKeyDown(key))
        {
            P_Movement.instance.ReturnCharacterMove();

            CloseAllUI(uiName);
            DestroyPopup();

            uiParts[uiName].Toggle();
        }
    }
    public void GetBoard()
    {
        Board.SetActive(true);
    }

    public void BoardOut() => Board.GetComponent<UI_Animation_Handler>().AnimationChange("Out");

    public void AllStopCoroutine() => StopAllCoroutines();

    public void BoardFill(float hp, float Maxhp)
    {
        BoardHpFill.fillAmount = hp / Maxhp;
        if(F_Coroutine != null)
        {
            StopCoroutine(F_Coroutine);
        }
        F_Coroutine = StartCoroutine(FillCorouitne());
    }

    IEnumerator FillCorouitne()
    {
        while (BoardHpWhiteFill.fillAmount > BoardHpFill.fillAmount)
        {
            BoardHpWhiteFill.fillAmount = Mathf.Lerp(BoardHpWhiteFill.fillAmount,
                BoardHpFill.fillAmount, Time.deltaTime * 5.0f);
            yield return null;
        }
    }
}
