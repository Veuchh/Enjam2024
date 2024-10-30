using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerGardening playerGardening;
    [SerializeField] PlayerCombat playerCombat;

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
        Vector2 mouseDir = Input.mousePosition;

        mouseDir.x = Mathf.InverseLerp(0, Screen.width, mouseDir.x);
        mouseDir.y = Mathf.InverseLerp(0, Screen.height, mouseDir.y);

        mouseDir -= Vector2.one / 2;
        playerCombat.OnMousePosAttack(mouseDir.normalized);
    }
}
