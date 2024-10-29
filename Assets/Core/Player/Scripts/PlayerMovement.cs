using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 1;

    private void FixedUpdate()
    {
        if (PlayerData.CanMove)
            Move();
    }

    private void Move()
    {
        rb.linearVelocity = (PlayerData.currentMoveInput * moveSpeed);
    }

    #region Inputs
    public void OnNewMoveInput(Vector2 newMoveInput)
    {
        PlayerData.currentMoveInput = newMoveInput;
    }
    #endregion
}
