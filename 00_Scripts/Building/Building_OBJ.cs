using Mono.Cecil.Cil;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public enum Material_Type
{
    Opaque,
    Transparent
}
public class Building_OBJ : MonoBehaviour
{
    [HideInInspector] public Building_Scriptable m_Data;
    [SerializeField] private ParticleSystem particle;

    Renderer renderer;
    Collider collider;
    private Material Opaque_M, Transparent_M;
    public Material OriginalMaterial;
    private Color[] colors = {new Color(0.0f, 0.02415333f, 0.7490197f, 1.0f) , new Color(1.0f, 0.2688679f, 0.2688679f, 1.0f) };

    public bool CanBuild = true;
    bool Completed = false;
    bool GetTriggerMaterial = true;

    public GameObject Board;
    public GameObject PortalQuad;

    [SerializeField] private Image IconImage;
    [SerializeField] private Image FillSlider;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI PercentageText;

    public bool Working = false;

    private void Awake()
    {
        Opaque_M = Resources.Load<Material>("Opaque_M");
        Transparent_M = Resources.Load<Material>("Transparent_M");
        renderer = GetComponentInChildren<Renderer>();
        collider = GetComponentInChildren<Collider>();
    }

    public void Confirm()
    {
        GetTriggerMaterial = false;
        particle.Play();

        Transform P = Board.transform.parent;
        P.eulerAngles = new Vector3(55.0f, P.eulerAngles.y - transform.eulerAngles.y, 0.0f);

        Board.SetActive(true);
        IconImage.sprite = Asset_Mng.Get_Atlas(m_Data.Key);
        TitleText.text = m_Data.Key;
        SetBuildData(m_Data.timer, BuildCompleted);
        Navigation_Mng.instance.PanelGet_Toast(m_Data, "Confirm");
    }

    private void BuildCompleted()
    {
        SetMaterial(Material_Type.Opaque);
        Board.GetComponent<Animator>().SetTrigger("Out");

        CompassBar.AddMarker(transform, m_Data.Key);

        GetComponent<AudioSource>().enabled = true;

        StartCoroutine(CompletedCoroutine());
        PortalQuad.SetActive(true);
    }

    private IEnumerator CompletedCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float EmissionStart = 1.0f;
        float EmissionEnd = 20.0f;
        Color startColor = Color.white;
        Color endColor = Color.black;
        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            float LerpEmission = Mathf.Lerp(EmissionStart, EmissionEnd, percent);
            renderer.material.SetColor("_EmissionColor", startColor * LerpEmission);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        current = 0.0f;
        percent = 0.0f;
        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            float LerpEmission = Mathf.Lerp(EmissionEnd, EmissionStart, percent);
            Color LerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", LerpColor * LerpEmission);
            yield return null;
        }
        Completed = true;

        Navigation_Mng.instance.PanelGet_Toast(m_Data, "Build_Completed");

        if (OriginalMaterial != null)
            renderer.material = OriginalMaterial;
    }

    public void SetMakeData(string key, float timer, Action action = null)
    {
        Board.SetActive(true);
        IconImage.sprite = Asset_Mng.Get_Atlas(key);
        TitleText.text = Utils.Localization_Text(String_Table.Unit , key);

        Utils.SetLayer("WorkObject", collider.gameObject);
        SetBuildData(timer, action);
    }

    public void SetBuildData(float time, Action action)
    {
        StartCoroutine(SliderFillCoroutine(time, action));
    }

    IEnumerator SliderFillCoroutine(float time, Action action)
    {
        float t = 0.0f;
        while(t <= time)
        {
            t += Time.deltaTime;

            FillSlider.fillAmount = t / time;
            PercentageText.text = string.Format("{0:0.0}%" ,FillSlider.fillAmount * 100.0f);
            yield return null;
        }
        if (action != null)
        {
            Board.GetComponent<Animator>().SetTrigger("Out");
            action?.Invoke();
        }
        Utils.SetLayer("Object", collider.gameObject);
    }

    public void SetTrigger(bool active)
    {
        collider.isTrigger = active;
    }

    public void SetMaterial(Material_Type type)
    {
        switch(type)
        {
            case Material_Type.Opaque: renderer.material = Opaque_M; break;
            case Material_Type.Transparent: renderer.material = Transparent_M; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetTriggerMaterial) return;
        if(other.gameObject.name != "Terrain")
        {
            SetMaterial_Color(1);
            CanBuild = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!GetTriggerMaterial) return;
        if (other.gameObject.name != "Terrain")
        {
            SetMaterial_Color(0);
            CanBuild = true;
        }
    }

    public void SetMaterial_Color(int value)
    {
        renderer.material.SetColor("_EmissionColor", colors[value]);
    }
}
