using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerGardening playerGardening;
    public void OnMove(InputValue value)
    {
        playerMovement.OnNewMoveInput(value.Get<Vector2>());
    }

    public void OnSellPumpkin(InputValue value)
    {
        playerGardening.OnInteract();
    }
}
