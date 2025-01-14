using System.Collections;
using UnityEngine;

public class BonFire : M_Object
{
    public override void Interaction(Character character)
    {
        base.Interaction(character);
        character.AnimationChange("Sitting");
        StartCoroutine(BonFireCoroutine());
    }

    public override void OutInteraction()
    {
        base.OutInteraction();
        StopAllCoroutines();
    }

    IEnumerator BonFireCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        Base_Mng.Game.SetStamina(10);

        StartCoroutine(BonFireCoroutine());
    }
}
