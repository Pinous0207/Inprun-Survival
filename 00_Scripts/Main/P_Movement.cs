using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Processors;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : Character
{
    public static P_Movement instance = null;

    [Header("#Movement Settings")]
    public float moveSpeed = 5.0f;

    [Space(20f)]
    [Header("#Mouse Rotation")]
    public LayerMask groundLayer;
    public float rotationSpeed = 10.0f;

    private CharacterController controller;
    private P_Finder Finder;

    public Bullet bulletObject;
    public Transform bulletTransform;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public override void Start()
    {
        base.Start();

        controller = GetComponent<CharacterController>();
        Finder = GetComponent<P_Finder>();

        Delegate_Holder.OnHPCHange(HP);

        Delegate_Holder.OnInteraction += ReturnCharacterMove;
        Delegate_Holder.OnInteractionOut += () => animator.SetBool("NoneInteraction", false);
    }

    public override void Bullet()
    {
        base.Bullet();

        var go = Instantiate(bulletObject, bulletTransform.position, Quaternion.identity);
        go.Init(Finder.monsterTarget);
    }

    public void ReturnCharacterMove()
    {
        animator.SetBool("NoneInteraction", true);
        animator.SetFloat("a_Speed", 0.0f);
    }
    private void Update()
    {
        if(Finder.OnInteraction)
        {
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.F) && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Delegate_Holder.OnOutInteraction();
            }
            return;
        }
        if (Canvas_Holder.Uis.Count > 0) return;

        Move();
        if (Finder.GetMonster) return;
        RotateTowardsMouse();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraRight * horizontal + cameraForward * vertical;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("a_Speed", currentSpeed);
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0.0f;

            if(direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void GetDamage(int dmg)
    {
        Canvas_Holder.instance.GetText(dmg.ToString(), Color.red, transform.position);
        HP -= dmg;
        Delegate_Holder.OnHPCHange(HP);

    }
}
