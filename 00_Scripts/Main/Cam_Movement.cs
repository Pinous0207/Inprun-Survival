using System.Collections;
using UnityEngine;

public class Cam_Movement : MonoBehaviour
{
    public static Cam_Movement instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private Transform player;

    [SerializeField] private float PosX;
    [SerializeField] private float PosY;
    [SerializeField] private float PosZ;

    [SerializeField] private float m_Speed = 2.0f;

    [Header("## Camera Shake")]
    [SerializeField] private float Duration;
    [SerializeField] private float Power;
    Vector3 OriginalPos;
    bool isCameraShake = false;

    private void Start()
    {
        player = P_Movement.instance.transform;
    }

    private void Update()
    {
        if (isCameraShake) return;
        Move();
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            player.transform.position.x + PosX,
            player.transform.position.y + PosY,
            player.transform.position.z + PosZ
            ), Time.deltaTime * m_Speed);
    }

    public void CameraShake()
    {
        if (isCameraShake) return;
        isCameraShake = true;
        StartCoroutine(CameraShake_Coroutine());
    }

    IEnumerator CameraShake_Coroutine()
    {
        OriginalPos = transform.localPosition;
        float timer = 0.0f;
        while(timer <= Duration)
        {
            transform.localPosition = Random.insideUnitSphere * Power + OriginalPos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = OriginalPos;
        isCameraShake = false;
    }
}
