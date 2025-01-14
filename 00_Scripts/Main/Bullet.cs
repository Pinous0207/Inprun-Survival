using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float explositionRadius = 5.0f;
    public int damage;
    public GameObject ExplosionParticle;
    public LayerMask monsterLayer;
    Transform m_Target;
    public void Init(Transform target)
    {
        m_Target = target; 
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(m_Target.position.x, m_Target.position.y + 1.5f, m_Target.position.z);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ExplosionParticle, transform.position, Quaternion.identity);

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explositionRadius, monsterLayer);
        Debug.Log(nearbyObjects.Length);
        foreach(Collider hitCollider in nearbyObjects)
        {
            Monster monster = hitCollider.GetComponent<Monster>();
            if(monster != null)
            {
                monster.GetDamage(damage);
            }
        }

        Destroy(this.gameObject);
    }
}
