using UnityEngine;

public class M_Object : MonoBehaviour
{
    [HideInInspector] public Object_Scriptable m_Data;
    public bool GetInteraction = false;

    public int HP;
    public Item Item_Prefab;

    private void Start()
    {
        Delegate_Holder.OnInteractionOut += OutInteraction;
    }

    private void OnDestroy()
    {
        Delegate_Holder.OnInteractionOut -= OutInteraction;
    }

    public virtual void OutInteraction()
    {

    }
    public virtual void Interaction(Character character)
    {
        character.m_Object = this;
        GetInteraction = true;
    }

    public virtual void OnHit(Character character)
    {
        if (character.MainPlayer)
        {
            Canvas_Holder.instance.GetBoard();
            Base_Mng.Game.SetStamina(-10);
            Cam_Movement.instance.CameraShake();
        }
        HP_Init(character);
    }

    public virtual void HP_Init(Character character)
    {
        bool Main = character.MainPlayer;
        if(HP <= 0)
        {
            HP = 0;
            Particle_Handler.instance.OnParticle(transform.GetChild(0).GetComponent<MeshRenderer>());
            if (Main)
            {
                Canvas_Holder.instance.AllStopCoroutine();
                Canvas_Holder.instance.BoardHpWhiteFill.fillAmount = 1.0f;
                Delegate_Holder.OnOutInteraction();
            }
            else
            {
                character.GetComponent<Worker>().StateChange(State.IDLE);
            }
            Base_Mng.Object.RemoveObject(this.gameObject);
            Destroy(this.gameObject);
            return;
        }
        if (Main)
        {
            Canvas_Holder.instance.BoardFill(HP, m_Data.HP);
        }
    }
}
