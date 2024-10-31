using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerGardening playerGardening;
    [SerializeField] PlayerCombat playerCombat;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;    
    }

    public void OnMove(InputValue value)
    {
        playerMovement.OnNewMoveInput(value.Get<Vector2>());
    }

    public void OnSellPumpkin(InputValue value)
    {
        playerGardening.OnInteract();
    }
    
    public void OnAttackInFront(InputValue value)
    {
        playerCombat.OnAttackInFront();
    }

    public void OnAttackMousePos(InputValue value)
    {
        // Get the position of the mouse in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z));

        // Calculate the direction from the transform's position to the mouse position
        Vector3 direction = mouseWorldPosition - transform.position;

        playerCombat.OnMousePosAttack(direction);
    }
}
