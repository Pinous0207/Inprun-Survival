using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class Portal : M_Object
{
    UIPART part = null;
    [SerializeField] private Worker worker;
    [SerializeField] private Transform WayPoint;
    public override void Interaction(Character character)
    {
        base.Interaction(character);
        part = Canvas_Holder.instance.GetUIPART("PORTAL");
        part.Open();
        part.GetComponent<PORTAL>().Init(this);
    }

    public void GetWorker()
    {
        var go = Instantiate(worker, transform.position, Quaternion.identity);
        go.SetDestination(WayPoint.position, () => go.StateChange(State.IDLE));
    }
}
