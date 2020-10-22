using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected enum State
    {
        NonActivated = 0,
        Cinematic = 1,
        MovingToReadyPoint = 2,
        MovingToAttackPoint = 3,
        AttackReady = 4,
        Attacking = 5,
        Dead = 6,
        Pain = 7
    }

    public System.Action Died;
    public System.Action ReadyToAttack;
    public System.Action AttackEnded;

    [Header("Health Parameters")]
    [SerializeField] private int maxHitPoints = 3;
    //[SerializeField, Range(0, 1)] private float painChance = 0.5F;

    private int currentHP = default;
    protected Animator animator = default;
    private DamageTransfer[] allDamageTransfers;

    protected State CurrentState { get; set; }
    public bool IsDead => CurrentState == State.Dead;
    public bool IsReadyToAttack => CurrentState == State.AttackReady;
    public bool IsAttacking => CurrentState == State.Attacking || CurrentState == State.MovingToAttackPoint;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHitPoints;
        allDamageTransfers = GetComponentsInChildren<DamageTransfer>();
    }

    protected virtual void OnDestroy()
    {

    }

    private void LateUpdate()
    {
        if (CurrentState != State.Cinematic)
            RefreshAnimations();
    }

    protected virtual void RefreshAnimations()
    {
        animator.SetBool("IsMoving", CurrentState == State.MovingToReadyPoint || CurrentState == State.MovingToAttackPoint);
        animator.SetBool("IsAttacking", CurrentState == State.Attacking);
        animator.SetBool("IsAttackReady", CurrentState == State.AttackReady);
        animator.SetBool("IsDead", CurrentState == State.Dead);
    }

    public void DoDamage(int damage)
    {
        if (currentHP <= 0)
            return;

        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHitPoints);

        if (currentHP <= 0)
            OnDead();
        else
            OnDamageReceived();
    }

    protected virtual void OnDamageReceived()
    {
        //if (Random.Range(0, 1) < painChance)
        //    CurrentState = State.Pain;
    }

    protected virtual void OnDead()
    {
        CurrentState = State.Dead;
        System.Array.ForEach(allDamageTransfers, d => d.Disable());
        Died?.Invoke();
    }

    public abstract void Activate();
    public abstract void StartAttackSequence(Vector3 attackPoint, System.Action AttackEnded);
}
