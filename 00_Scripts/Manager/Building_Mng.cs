using UnityEngine;

public class Building_Mng : MonoBehaviour
{
    Camera cam;
    [SerializeField] private float rayDistance = 100.0f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask layer;

    [HideInInspector] public Building_OBJ BuildingObject;

    float ignoreTime = 0.3f;
    float timer;
    public void SetBuild(Building_Scriptable Data)
    {
        BuildingObject = Instantiate(Data.obj);
        BuildingObject.m_Data = Data;
        BuildingObject.SetMaterial(Material_Type.Transparent);
        BuildingObject.SetTrigger(true);
        timer = Time.time + ignoreTime;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (BuildingObject == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, layer))
        {
            BuildingObject.transform.position = hitInfo.point;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0)
        {
            BuildingObject.transform.Rotate(0f, scroll * rotationSpeed * Time.deltaTime, 0f);
        }

        if (Time.time < timer) return;

        if(Input.GetMouseButtonUp(0))
        {
            if (BuildingObject.CanBuild == false) return;

            ConfirmPlacement();
        }
    }

    private void ConfirmPlacement()
    {
        BuildingObject.SetTrigger(false);
        BuildingObject.Confirm();
        BuildingObject = null;
    }
}
