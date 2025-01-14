using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Monster : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    NavMeshAgent agent;

    [SerializeField] private float Range;
    [SerializeField] private GameObject DestroyParticle;
    Coroutine hit_Coroutine;
 
    Renderer renderer;
    Animator animator;

    Transform m_Target;

    bool isAttack = false;
    bool isDead = false;
    Vector3 myPos;
    Monster_Spawner parentSpawner;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        HP = MaxHP;
        renderer = transform.GetComponentInChildren<Renderer>();
        myPos = transform.position;
        AnimationChange("IDLE", false);
    }

    private void AnimationChange(string temp, bool isTrigger = false)
    {
        animator.SetBool("IDLE", false);
        animator.SetBool("WALK", false);

        if(isTrigger)
        {
            animator.SetTrigger(temp);
        }
        else
        {
            animator.SetBool(temp, true);
        }
    }

    public void Init(Monster_Spawner spawner)
    {
        parentSpawner = spawner;
    }

    private void Attack()
    {
        P_Movement.instance.GetDamage(5);
    }

    private void Update()
    {
        if (isDead) return;
        if (m_Target == null) 
        {
            if(agent.remainingDistance <= 2.0f)
            {
                StopMovement(true);
                AnimationChange("IDLE", false);
            }
            return;
        }
        float distance = Vector3.Distance(m_Target.position, transform.position);
    
        if(distance < 2.0f)
        {
            StopMovement(true);
            if (isAttack == false)
            {
                AttackPlayer();
            }
        }
        else
        {
            StopMovement(false);
            AnimationChange("WALK", false);
            agent.SetDestination(m_Target.position);
        }
    }

    private void StopMovement(bool Can)
    {
        agent.isStopped = Can;
        if (Can)
        {
            agent.velocity = Vector3.zero;
        }
    }

    private void AttackPlayer()
    {
        isAttack = true;
        AnimationChange("ATTACK", true);
        Invoke("AttackReturn", 1.0f);
    }

    public void GetPlayer(Transform target)
    {
        m_Target = target;
        AnimationChange("WALK", false);
    }

    public void RemovePlayer()
    {
        m_Target = null;
        StopMovement(false);
        AnimationChange("WALK", false);
        agent.SetDestination(myPos);
    }
  
    private void AttackReturn() => isAttack = false;
    public void GetDamage(int dmg)
    {
        if (isDead) return;

        var playerPos = P_Movement.instance.transform.position;

        Canvas_Holder.instance.GetText(dmg.ToString(), Color.yellow, transform.position);
        HP -= dmg;
        Canvas_Holder.instance.AddSlider(this);
        P_Movement.instance.GetComponent<Character>().GetHitParticle();

        if (hit_Coroutine != null) StopCoroutine(hit_Coroutine);
        hit_Coroutine = StartCoroutine(GetHitCoroutine());

        if (HP <= 0)
        {
            isDead = true;
            StopAllCoroutines();
            StopMovement(true);
            Instantiate(DestroyParticle, 
                new Vector3(transform.position.x,
                transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            parentSpawner.spawnedMonsters.Remove(this);
            Canvas_Holder.instance.RemoveSlider(this);
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            AnimationChange("DIE", true);
            Destroy(this.gameObject, 1.5f);
        }
    }


    IEnumerator GetHitCoroutine()
    {
        float current = 0.0f;
        float percent = 0.0f;
        Color startColor = Color.black;
        Color endColor = Color.white;
        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color lerpColor = Color.Lerp(startColor, endColor, percent);
            renderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }
        current = 0.0f;
        percent = 0.0f;
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;

            Color lerpColor = Color.Lerp(endColor, startColor, percent);
            renderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }
    }
    
}
