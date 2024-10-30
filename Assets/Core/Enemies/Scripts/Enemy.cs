using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const string PLAYER_ATTACK_TAG = "PlayerDamageCollider";

    [SerializeField] float reevaluateTargetDelay = .5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float attackRange = 1;
    [SerializeField] int maxHP = 1;

    int currentHP;
    float nextReevaluateTime = float.MinValue;
    bool isAttacking = false;
    PlantSlot targetPlant;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        if (!isAttacking && Time.time > nextReevaluateTime)
        {
            nextReevaluateTime = Time.time + reevaluateTargetDelay;
            EvaluateBestTarget();
        }

        if (targetPlant == null)
            return;

        if (targetPlant.slotState == SlotState.empty)
        {
            targetPlant = null;
            isAttacking = false;
        }

        if (targetPlant == null)
            return;

        float distanceToSlot = Vector2.Distance(targetPlant.transform.position, transform.position);

        if (distanceToSlot >= attackRange)
            Move();
        else if (!isAttacking)
            Attack();
    }

    private void Attack()
    {
        isAttacking = true;
        targetPlant.AddAttacker();
    }

    void Move()
    {
        transform.position += (targetPlant.transform.position - transform.position).normalized * speed * Time.deltaTime;
    }

    private void EvaluateBestTarget()
    {
        PlantSlot bestCandidate = null;
        float bestDistance = float.MaxValue;

        foreach (var slot in PlantSlot.slots)
        {
            if (slot.slotState != SlotState.planted)
                continue;

            float slotDistance = Vector2.Distance(transform.position, slot.transform.position);

            if (slotDistance < bestDistance)
            {
                bestDistance = slotDistance;
                bestCandidate = slot;
            }
        }

        targetPlant = bestCandidate;
    }

    void DamageEnemy()
    {
        currentHP--;

        if (currentHP > 0)
            return;

        KillEnnemy();
    }

    private void KillEnnemy()
    {
        if (isAttacking)
        {
            targetPlant.RemoveAttacker();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_ATTACK_TAG))
            return;

        DamageEnemy();
    }
}
