using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackStartupDelay = .05f;
    [SerializeField] float attackHitboxDuration = .3f;
    [SerializeField] float attackStopDelay = .05f;
    [SerializeField] GameObject northCollider;
    [SerializeField] GameObject southCollider;
    [SerializeField] GameObject westCollider;
    [SerializeField] GameObject eastCollider;

    public void OnAttackInFront()
    {
        Attack();
    }

    public void OnMousePosAttack(Vector2 mousePosRatio)
    {
        if (Mathf.Abs(mousePosRatio.y) < .5f)
        {
            if (mousePosRatio.x > 0) PlayerData.currentOrientation = Orientation.East;
            else PlayerData.currentOrientation = Orientation.West;
        }

        else
        {
            if (mousePosRatio.y > 0) PlayerData.currentOrientation = Orientation.North;
            else PlayerData.currentOrientation = Orientation.South;
        }

        Attack();
    }

    void Attack()
    {
        if (!PlayerData.CanAttack)
            return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        PlayerData.IsAttacking = true;
        yield return new WaitForSeconds(attackStartupDelay);
        ToggleDamageCollider(true);
        yield return new WaitForSeconds(attackHitboxDuration);
        ToggleDamageCollider(false);
        yield return new WaitForSeconds(attackStopDelay);
        PlayerData.IsAttacking = false;
    }

    void ToggleDamageCollider(bool toggle)
    {
        northCollider.SetActive(false);
        southCollider.SetActive(false);
        westCollider.SetActive(false);
        eastCollider.SetActive(false);

        if (toggle)
        {
            switch (PlayerData.currentOrientation)
            {
                case Orientation.North:
                    northCollider.SetActive(true);
                    break;
                case Orientation.South:
                    southCollider.SetActive(true);
                    break;
                case Orientation.West:
                    westCollider.SetActive(true);
                    break;
                case Orientation.East:
                    eastCollider.SetActive(true);
                    break;
            }
        }
    }
}
