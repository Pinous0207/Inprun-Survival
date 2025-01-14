using System.Collections.Specialized;
using UnityEngine;

public class Wheather_Mng : MonoBehaviour
{
    [Header("## Sun And Night")]
    public Light directionalLight;
    public Vector3 sunRotationOffset;
    public Gradient sunColorGradient;

    [Range(0, 24)] public float currentTime = 12.0f; // 0~24 ½Ã°£
    public float m_TimeSpeed = 60.0f;

    [Space(20f)]
    [Header("## Rain")]
    public ParticleSystem rainParticleSystem;
    public float minEmissionRate;
    public float maxEmissionRate;
    private ParticleSystem.EmissionModule emissionModule;
    private void Start()
    {
        emissionModule = rainParticleSystem.emission;
        Delegate_Holder.RainIntensityChanged += UpdateRainEmission;
    }

    private void OnDestroy()
    {
        Delegate_Holder.RainIntensityChanged -= UpdateRainEmission;
    }

    private void Update()
    {
        UpdateTime();
        RotateSun();
        UpdateSunColor();
    }

    public void UpdateRainEmission(float intensity)
    {
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, intensity);
        emissionModule.rateOverTime = emissionRate;
    }

    private void UpdateTime()
    {
        float timeSpeed = 24f / m_TimeSpeed;
        currentTime += Time.deltaTime * timeSpeed;
        if (currentTime >= 24.0f) currentTime = 0.0f;
    }

    private void RotateSun()
    {
        float timePercent = currentTime / 24.0f;

        float sunXRotation = Mathf.Lerp(-90.0f, 270.0f, timePercent);
        float sunYRotation = Mathf.Lerp(-45.0f, 45.0f, Mathf.Sin(timePercent * Mathf.PI));

        directionalLight.transform.rotation = Quaternion.Euler(sunXRotation, 
            sunYRotation+sunRotationOffset.y, 0);
    }

    private void UpdateSunColor()
    {
        float timePercent = currentTime / 24.0f;
        directionalLight.color = sunColorGradient.Evaluate(timePercent);
    }
}
