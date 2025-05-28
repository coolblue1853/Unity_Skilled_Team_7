using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private Camera cam;

    private Vector3 moveDirection;
    public Vector3 MoveDirection { get => moveDirection; }

    private Vector3 lookDirection;
    public Vector3 LookDirection { get => lookDirection; }

    public void Init(Player player)
    {
        this.player = player;
        cam = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (player.CurState != PlayerState.Walk)
                player.ChangeState(PlayerState.Walk);

            Vector2 moveInput = context.ReadValue<Vector2>();

            moveDirection = Vector3.right * moveInput.x + Vector3.forward * moveInput.y;
            moveDirection = moveDirection.normalized;
        }

        else if (context.phase == InputActionPhase.Canceled)
        {
            moveDirection = Vector3.zero;
            player.ChangeState(PlayerState.Idle);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // Dash 기능
            player.Dash();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = context.ReadValue<Vector2>();

        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float point))
        {
            Vector3 hitPoint = ray.GetPoint(point);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0;

            if (direction.magnitude < 0.9f)
            {
                lookDirection = Vector3.zero;
            }
            else
            {
                lookDirection = direction.normalized;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (player.OnVehicle)
            {
                player.SetVehicle(null);
                return;
            }

            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                if (hit.collider.TryGetComponent(out VehicleController vehicle))
                {
                    player.SetVehicle(vehicle);
                }
            }
        }
    }

    public void OnCommand(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

        }
    }

    public void OnSelectSlot(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        string keyString = context.control.name;

        if (int.TryParse(keyString, out int keyNum))
        {
            int slotIdx = keyNum - 1;

            // 선택한 슬롯 인덱스 이벤트 호출
            player.PlayerEvents.RaisedSeletSlot(slotIdx);
        }
    }
}
