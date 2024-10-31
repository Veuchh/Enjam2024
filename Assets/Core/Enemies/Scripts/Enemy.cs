using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const string PLAYER_ATTACK_TAG = "PlayerDamageCollider";

    [SerializeField] Enemy ratPrefab;
    [SerializeField] float reevaluateTargetDelay = .5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float attackRange = 1;
    [SerializeField] int maxHP = 1;
    [SerializeField] SoundEffectData ratDeathSFX;
    [SerializeField] SoundEffectData ratSplatterSFX;

    // VFX
    [SerializeField] GameObject blood;

    int currentHP;
    float nextReevaluateTime = float.MinValue;
    bool isAttacking = false;
    PlantSlot targetPlant;
    const float ratDeathSFXcooldown = .2f;
    static float nextAllowedSFX;
    public static int enemyAmount;

    private void Awake()
    {
        currentHP = maxHP;
        enemyAmount++;
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
        targetPlant.onPumpkinDestroyed += OnTargetDestroyed;
    }

    private void OnTargetDestroyed()
    {
        isAttacking = false;
        targetPlant.onPumpkinDestroyed -= OnTargetDestroyed;

        if (enemyAmount < 150 && ratPrefab != null && this != null)
        {
            Instantiate(ratPrefab, transform.position + Vector3.left, Quaternion.identity);
            Instantiate(ratPrefab, transform.position + Vector3.right, Quaternion.identity);
        }
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

            float slotDistance = Vector2.SqrMagnitude(transform.position - slot.transform.position);

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
        if (Time.time > nextAllowedSFX)
        {
            nextAllowedSFX = Time.time + ratDeathSFXcooldown;
            AudioPlayer.Instance.PlayAudio(ratDeathSFX);
            AudioPlayer.Instance.PlayAudio(ratSplatterSFX);

        }
        if (isAttacking)
        {
            targetPlant.RemoveAttacker();
            targetPlant.onPumpkinDestroyed -= OnTargetDestroyed;
        }
        enemyAmount--;
        Destroy(gameObject);

        Instantiate(blood, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_ATTACK_TAG))
            return;

        DamageEnemy();
    }
}
