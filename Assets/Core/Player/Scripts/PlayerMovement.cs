using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float footstepCooldown = .33f;
    [SerializeField] SoundEffectData footstepSFX;
    [SerializeField] Animator animator;

    float nextAllowedFootstep;

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", PlayerData.currentMoveInput.magnitude);
        Move();

        switch (PlayerData.currentOrientation)
        {
            case Orientation.North:
                animator.SetFloat("X", 0);
                animator.SetFloat("Y", 1);
                break;
            case Orientation.South:
                animator.SetFloat("X", 0);
                animator.SetFloat("Y", -1);
                break;
            case Orientation.West:
                animator.SetFloat("X", -1);
                animator.SetFloat("Y", 0);
                break;
            case Orientation.East:
                animator.SetFloat("X", 1);
                animator.SetFloat("Y", 0);
                break;
        }

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

        if (Mathf.Abs(PlayerData.currentMoveInput.normalized.y) < .5f)
        {
            if (PlayerData.currentMoveInput.normalized.x > 0) PlayerData.currentOrientation = Orientation.East;
            else PlayerData.currentOrientation = Orientation.West;
        }

        else
        {
            if (PlayerData.currentMoveInput.normalized.y > 0) PlayerData.currentOrientation = Orientation.North;
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
