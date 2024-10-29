using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    public void OnMove(InputValue value)
    {
        playerMovement.OnNewMoveInput(value.Get<Vector2>());
    }
}
