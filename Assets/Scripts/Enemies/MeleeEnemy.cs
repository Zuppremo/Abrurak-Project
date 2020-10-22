using System.Collections;
using UnityEngine;

public class MeleeEnemy : GroundEnemy
{
    [Header("Attack")]
    [SerializeField] private float attackDuration = 1.0F;
    [SerializeField] private float attackMoveSpeedMultiplier = 1.5F;

    private float initialSpeed = 0;

    protected override void Awake()
    {
        base.Awake();
        initialSpeed = agent.speed;
    }

    public override void StartAttackSequence(Vector3 attackPoint, System.Action AttackEnded)
    {
        agent.speed = initialSpeed * attackMoveSpeedMultiplier;
        StopCoroutine(nameof(AttackSequence));
        StartCoroutine(AttackSequence(attackPoint, AttackEnded));
    }

    private IEnumerator AttackSequence(Vector3 attackPoint, System.Action AttackEnded)
    {
        CurrentState = State.MovingToAttackPoint;
        agent.SetDestination(attackPoint);
        agent.isStopped = false;
        yield return new WaitUntil(() => Vector3.Distance(myTransform.position, attackPoint) < 0.1F);
        agent.isStopped = true;
        CurrentState = State.Attacking;
        agent.SetDestination(readyToAttackPoint.position);
        yield return new WaitForSeconds(attackDuration);
        agent.isStopped = false;
        CurrentState = State.MovingToAttackPoint;
        yield return new WaitUntil(() => Vector3.Distance(myTransform.position, readyToAttackPoint.position) < 1.75F);
        CurrentState = State.AttackReady;
        AttackEnded?.Invoke();
        yield return new WaitUntil(() => Vector3.Distance(myTransform.position, readyToAttackPoint.position) < 0.1F);
        agent.isStopped = true;
    }

    protected override void OnDead()
    {
        StopAllCoroutines();
        base.OnDead();
        agent.isStopped = true;
    }
}
