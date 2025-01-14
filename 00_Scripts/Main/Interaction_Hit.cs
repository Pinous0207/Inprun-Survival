using System.Collections;
using UnityEngine;

public class Interaction_Hit : M_Object
{
    float shakeAmount = 5.0f;
    float shakeDuration = 0.5f;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
        HP = m_Data.HP;
    }

    public override void Interaction(Character character)
    {
        base.Interaction(character);
        character.AnimationChange(m_Data.m_Type.ToString());
        character.EquipmentChange(m_Data.m_Type, true);
    }

    public override void OnHit(Character character)
    {
        base.OnHit(character);
        ShakeTree(transform.position - P_Movement.instance.transform.position);

        if(HP <= 0)
        {
            var items = ItemFlowController.DROPITEMLIST(m_Data.Drop_Items);
            for(int i = 0; i < items.Count; i++)
            {
                var go = Instantiate(Item_Prefab, transform.position, Quaternion.identity);
                go.Init(items[i]);
            }
        }
    }
    private void ShakeTree(Vector3 attackDirection)
    {
        Vector3 oppositeDirection = attackDirection.normalized;

        // transform.rotation, transform.eulerAngles

        Quaternion targetRotation = Quaternion.Euler(
            originalRotation.eulerAngles.x + oppositeDirection.z * shakeAmount,
            originalRotation.eulerAngles.y,
            originalRotation.eulerAngles.z + oppositeDirection.x * shakeAmount);

        StopAllCoroutines();
        StartCoroutine(ShakeAnimation(targetRotation));
    }

    private IEnumerator ShakeAnimation(Quaternion targetRotation)
    {
        float elapsedTime = 0.0f;

        while(elapsedTime < shakeDuration / 2) 
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / (shakeDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;
        while(elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / (shakeDuration / 2)); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
