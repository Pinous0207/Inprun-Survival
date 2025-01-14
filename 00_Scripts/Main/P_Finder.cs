using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class P_Finder : MonoBehaviour
{
    [SerializeField] private float checkRadius = 5.0f;
    [SerializeField] private float checkMonsterRadius = 10.0f;

    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] private GameObject IconPrefab;
    [SerializeField] private float activationDistance = 3.0f;
    [SerializeField] private float attack_speed;
    private Dictionary<Transform, GameObject> activeIcons = new Dictionary<Transform, GameObject>();
    [HideInInspector] public bool OnInteraction = false;
    public bool GetMonster = false;
    bool isAttack = false;

    Transform closetObject;
    public Transform monsterTarget;
    private void Start()
    {
        Delegate_Holder.OnInteraction += OnInteractionVoid;
        Delegate_Holder.OnInteractionOut += OnInteractionOut;
    }

    void OnInteractionVoid()
    {
        OnInteraction = true;
        transform.LookAt(closetObject.transform.position);
        closetObject = null;
        IconInit();
    }

    void OnInteractionOut()
    {
        OnInteraction = false;
        P_Movement.instance.EquipmentAllDeactive();
        activeIcons.Clear();
    }
 
    private void Update()
    {
        if (OnInteraction) return;

        Collider[] monsterObjects = Physics.OverlapSphere(transform.position, checkMonsterRadius, monsterLayer);
        GetMonster = monsterObjects.Length > 0;

        if(GetMonster)
        {
            monsterTarget = null;
            float monsterClosetDistance = Mathf.Infinity;
            foreach(Collider monster in monsterObjects)
            {
                float distance = Vector3.Distance(transform.position, monster.transform.position);
                if(distance < monsterClosetDistance)
                {
                    monsterClosetDistance = distance;
                    monsterTarget = monster.transform;
                }
            }
            if (monsterTarget != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!isAttack)
                    {
                        AttackMonster(monsterObjects);
                        P_Movement.instance.EquipmentChange(Object_Type.Monster, true);
                    }
                }
                transform.LookAt(monsterTarget);
                closetObject = null;
                IconInit();
            }
            return;
        }
        P_Movement.instance.EquipmentChange(Object_Type.Monster, false);

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRadius, interactableLayer);

        closetObject = null;
        float closetDistance = Mathf.Infinity;

        foreach(Collider obj in nearbyObjects)
        {
            Transform targetTransform = obj.transform;

            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if(distance <= activationDistance && distance < closetDistance)
            {
                closetObject = targetTransform;
                closetDistance = distance;
            }
        }
        if (closetObject != null)
        {
            ShowIcon(closetObject);

            if (Input.GetKeyDown(KeyCode.F))
            {
                M_Object subObject = null;

                if(closetObject.GetComponent<M_Object>() == null)
                    subObject = closetObject.transform.parent.GetComponent<M_Object>();
                else subObject = closetObject.GetComponent<M_Object>();

                subObject.Interaction(GetComponent<Character>());
                Delegate_Holder.OnStartInteraction();
            }
        }

        IconInit();
    }

    private void AttackMonster(Collider[] monsters)
    {
        isAttack = true;
        P_Movement.instance.AnimationWeight(1, 1);
        P_Movement.instance.AnimationChange("Attack");
        P_Movement.instance.colliders = monsters;
        Invoke("ReturnAttack", attack_speed);
    }

    private void ReturnAttack()
    {
        P_Movement.instance.AnimationWeight(1, 0);
        isAttack = false;
    }
    private void IconInit()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (var iconEntry in activeIcons)
        {
            if (iconEntry.Key != closetObject || closetObject == null)
            {
                iconEntry.Value.GetComponent<UI_Animation_Handler>().AnimationChange("Out");
                toRemove.Add(iconEntry.Key);
            }
        }
        foreach (var transformToRemove in toRemove)
        {
            activeIcons.Remove(transformToRemove);
        }
    }

    private void ShowIcon(Transform targetTransform)
    {
        if(activeIcons.ContainsKey(targetTransform))
        {
            UpdateIconPosition(targetTransform, activeIcons[targetTransform]);
            return;
        }

        GameObject iconInstance = Instantiate(IconPrefab, uiCanvas.transform);
        activeIcons[targetTransform] = iconInstance;

        UpdateIconPosition(targetTransform, iconInstance);
    }

    private void UpdateIconPosition(Transform targetTransform, GameObject Icon)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(
            new Vector3(
            targetTransform.position.x,
            targetTransform.position.y + 1.5f,
            targetTransform.position.z));

        Icon.GetComponent<RectTransform>().position = screenPosition;
    }
}
