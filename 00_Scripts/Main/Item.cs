using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float spreadRadius = 10.0f; // 퍼지는 반경
    [SerializeField] private float arcHeight = 5.0f; // 포물선 높이
    [SerializeField] private float moveSpeed = 5.0f; // 아이템 이동 속도
    [SerializeField] private GameObject GetParticle;
    Transform player;

    ITEM m_ITEM;

    public void Init(ITEM item)
    {
        m_ITEM = item;
    }
    private void Start()
    {
        player = P_Movement.instance.transform;
        StartCoroutine(SpreadAndMoveToPlayer());
    }

    IEnumerator SpreadAndMoveToPlayer()
    {
        Vector3 spreadDirection = Random.insideUnitSphere * spreadRadius;
        Vector3 spreadPosition = transform.position + spreadDirection;

        spreadPosition.y = Mathf.Max(spreadPosition.y, arcHeight);

        float spreadTime = 0.3f;
        float elapsedTime = 0.0f;

        Vector3 startPosition = transform.position;

        while(elapsedTime < spreadTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spreadTime;
            transform.position = Vector3.Lerp(startPosition, spreadPosition, t);
            yield return null;
        }
        StartCoroutine(MoveToplayer(spreadPosition));
    }

    IEnumerator MoveToplayer(Vector3 startPosition)
    {
        float journeyTime;
        float elapsedTime;
        Vector3 endPosition;

        while(true) // 무한루프 조심
        {
            endPosition = player.position + new Vector3(0.0f, 1.0f, 0.0f);
            journeyTime = Vector3.Distance(startPosition, endPosition) / moveSpeed;
            elapsedTime = 0.0f;

            while (elapsedTime < journeyTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / journeyTime;

                Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
                transform.position = currentPos;

                endPosition = player.position + new Vector3(0.0f, 1.0f, 0.0f);

                yield return null;
            }

            if (Vector3.Distance(transform.position, player.position + new Vector3(0.0f, 1.0f, 0.0f)) < 0.5f) 
                break;

            startPosition = transform.position; // 시작 위치를 갱신하여 부드럽게 이동
        }

        Instantiate(GetParticle, transform.position, Quaternion.identity);
        Navigation_Mng.instance.PanelGet_Item(m_ITEM.Data, m_ITEM.Count);
        ItemFlowController.GETITEM(m_ITEM.Data, m_ITEM.Count);
        Destroy(this.gameObject);
    }
}
