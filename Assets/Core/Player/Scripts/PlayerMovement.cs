using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float footstepCooldown = .33f;
    [SerializeField] SoundEffectData footstepSFX;

    float nextAllowedFootstep;

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!PlayerData.CanMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = (PlayerData.currentMoveInput * moveSpeed);


        if (PlayerData.currentMoveInput == Vector2.zero)
            return;

        if (Mathf.Abs(PlayerData.currentMoveInput.y) < .5f)
        {
            if (PlayerData.currentMoveInput.x > 0) PlayerData.currentOrientation = Orientation.East;
            else PlayerData.currentOrientation = Orientation.West;
        }

        else
        {
            if (PlayerData.currentMoveInput.y > 0) PlayerData.currentOrientation = Orientation.North;
            else PlayerData.currentOrientation = Orientation.South;
        }

        if (Time.time > nextAllowedFootstep)
        {
            nextAllowedFootstep = Time.time + footstepCooldown;
            AudioPlayer.Instance.PlayAudio(footstepSFX);
        }
    }

    #region Inputs
    public void OnNewMoveInput(Vector2 newMoveInput)
    {
        PlayerData.currentMoveInput = newMoveInput;
    }
    #endregion
}
