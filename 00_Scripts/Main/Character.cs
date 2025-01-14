using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

public class Character : MonoBehaviour
{
    public bool MainPlayer = false;
    
    public int HP;
    public int MaxHP;

    [SerializeField] protected GameObject[] Equipments;
    protected Animator animator;
    public M_Object m_Object = null;
    public Collider[] colliders;
    [SerializeField] protected GameObject HitParticle;
    [SerializeField] private Transform GetParticleTransform;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        HP = MaxHP;
    }

    public void AnimationWeight(int layer, float value)
    {
        animator.SetLayerWeight(layer, value);
    }


    public virtual void Hit()
    {
        if (m_Object == null) return;
       
        m_Object.HP -= 20;

        GetHitParticle();

        m_Object.OnHit(this);
    }

    public virtual void Attack()
    {
        GetHitParticle();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Monster>().GetDamage(40);
        }
    }
    
    public virtual void Bullet()
    {

    }
    public void GetHitParticle()
    {
        Vector3 pos = new Vector3(
         GetParticleTransform.position.x + Random.Range(-0.5f, 0.5f),
         GetParticleTransform.position.y,
         GetParticleTransform.position.z + Random.Range(-0.5f, 0.5f));
        Instantiate(HitParticle, pos, Quaternion.identity);
    }
    public void EquipmentAllDeactive()
    {
        for (int i = 0; i < Equipments.Length; i++)
        {
            Equipments[i].SetActive(false);
        }
    }
    public void EquipmentChange(Object_Type type, bool Active)
    {
        Equipments[(int)type].SetActive(Active);
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }
}
