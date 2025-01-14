using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MarkerInfo
{
    public Transform targetTransform;
    public RectTransform markerUI;
    public string key;
    public Image markerIcon;
    public TextMeshProUGUI markerText;
    public MarkerInfo(Transform target, RectTransform ui, string m_Key)
    {
        targetTransform = target;
        markerUI = ui;
        key = m_Key;
        markerIcon = markerUI.Find("Icon").GetComponent<Image>();
        markerText = markerUI.Find("'M'Text").GetComponent<TextMeshProUGUI>();

        markerIcon.sprite = Asset_Mng.Get_Atlas(key);
    }
}
public class CompassBar : MonoBehaviour
{
    private Transform playerTransform;
    public TextMeshProUGUI westText, northText, eastText, southText;

    public float compassWidth = 700.0f;

    [Header("## Settings")]
    public float maxAlpha;
    public float minAlpha;
    public float maxScale;
    public float minScale;
    public float maxDistance;

    [Header("## Other Transform")]
    public static GameObject markerPrefab;
    public static Transform markerParent;

    public static List<MarkerInfo> activeMarkers = new List<MarkerInfo>();

    private void Start()
    {
        playerTransform = P_Movement.instance.transform;
        markerPrefab = transform.Find("CompassMarker").gameObject;
        markerPrefab.SetActive(false);
        markerParent = transform.Find("Mask").transform;
    }

    private void Update()
    {
        UdpateCompass();
        UpdateMarkers();
    }

    private void UdpateCompass()
    {
        float heading = playerTransform.eulerAngles.y;

        SetPositions(westText, heading, 90.0f);
        SetPositions(northText, heading, 180.0f);
        SetPositions(eastText, heading, 270.0f);
        SetPositions(southText, heading, 0.0f);
    }

    private void SetPositions(TextMeshProUGUI text, float heading, float offset)
    {
        // 기준 각도에 따라 각 텍스트가 중앙 기준으로 이동하도록 계산
        float relativeAngle = (heading - offset + 360.0f) % 360.0f; // 각도 보정
        float normalizedAngle = relativeAngle / 360.0f; // 0~1사이의 값으로 정규화

        float xPosition = Mathf.Lerp(-compassWidth, compassWidth, normalizedAngle);
        text.rectTransform.anchoredPosition = new Vector2(xPosition, text.rectTransform.anchoredPosition.y);

        // 중심에서의 거리 계산 (0이 중앙, 1이 최대 거리)
        float distanceFromCenter = Mathf.Abs(xPosition / compassWidth);
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, distanceFromCenter);
        float scale = Mathf.Lerp(maxScale, minScale, distanceFromCenter);

        Color color = text.color;
        color.a = alpha;
        text.color = color;

        text.rectTransform.localScale = Vector3.one * scale;
    }

    public static void AddMarker(Transform targetTransform, string key)
    {
        if (activeMarkers.Exists(m => m.targetTransform == targetTransform))
            return;

        GameObject marker = Instantiate(markerPrefab, markerParent);
        marker.SetActive(true);
        marker.name = "Marker:" + targetTransform.name;
        RectTransform markerRect = marker.GetComponent<RectTransform>();
        activeMarkers.Add(new MarkerInfo(targetTransform, markerRect, key));
    }

    public void UpdateMarkers()
    {
        for (int i = activeMarkers.Count -1; i >= 0; i--)
        {
            MarkerInfo markerInfo = activeMarkers[i];
            if(markerInfo.targetTransform == null)
            {
                Destroy(markerInfo.markerUI.gameObject);
                activeMarkers.RemoveAt(i);
                continue;
            }

            float heading = playerTransform.eulerAngles.y;
            Vector3 directionToTarget = markerInfo.targetTransform.position - playerTransform.position;
            float distance = Vector3.Distance(markerInfo.targetTransform.position, playerTransform.position);
            float targetAngle = Mathf.Atan2(-directionToTarget.x, -directionToTarget.z) * Mathf.Rad2Deg; // 각도 생성

            float relativeAngle = (heading - targetAngle + 360f) % 360f;

            float normalizedAngle = relativeAngle / 360.0f;
            float xPosition = Mathf.Lerp(-compassWidth, compassWidth, normalizedAngle);
            bool MarkerActive = distance <= maxDistance ? false : true;

            markerInfo.markerUI.gameObject.SetActive(MarkerActive);
            markerInfo.markerUI.anchoredPosition = new Vector2(xPosition, markerInfo.markerUI.anchoredPosition.y);
            markerInfo.markerText.text = string.Format("{0:0.0} m", distance);
        }
    }
}
