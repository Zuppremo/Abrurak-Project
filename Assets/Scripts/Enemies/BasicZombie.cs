﻿using Fungus;
using UnityEngine;
using UnityEngine.AI;

public class BasicZombie : Enemy
{
    public System.Action AttackPointReached;

    [Header("References")]
    [SerializeField] private Transform attackPoint = default;
    [Header("Parameters")]
    [SerializeField] private float lookAtSmooth = 1.5F;

    private Transform myTransform;
    private Transform cameraTransform;
    private NavMeshAgent agent;
    private Flowchart cinematicFlowchart;

    protected override void Awake()
    {
        base.Awake();
        myTransform = transform;
        cameraTransform = Camera.main.transform;
        CurrentState = State.NonActivated;
        SetUpAgent();

        if (TryGetComponent(out cinematicFlowchart))
        {
            BlockSignals.OnBlockStart += OnBlockStart;
            BlockSignals.OnBlockEnd += OnBlockEnd;
        }
    }

    private void SetUpAgent()
    {
        attackPoint.SetParent(null);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(attackPoint.position);
        agent.isStopped = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (cinematicFlowchart != null)
        {
            BlockSignals.OnBlockStart -= OnBlockStart;
            BlockSignals.OnBlockEnd -= OnBlockEnd;
        }
    }

    private void Update()
    {
        if (CurrentState == State.MovingToReadyPoint && Vector3.Distance(myTransform.position, attackPoint.position) < 0.1F)
            SetReadyToAttack();

        if (CurrentState == State.AttackReady || CurrentState == State.Attacking)
            LookAtMainCamera();
    }

    private void OnBlockStart(Block block)
    {
        if (block.GetFlowchart() == cinematicFlowchart && block.BlockName == "Start")
            CurrentState = State.Cinematic;
    }

    private void OnBlockEnd(Block block)
    {
        if (block.GetFlowchart() == cinematicFlowchart && block.BlockName == "End")
            GoToReadyPoint();
    }

    public override void Activate()
    {
        if (cinematicFlowchart != null)
            cinematicFlowchart.ExecuteBlock("Start");
        else
            GoToReadyPoint();
    }

    private void GoToReadyPoint()
    {
        CurrentState = State.MovingToReadyPoint;
        agent.isStopped = false;
    }

    private void SetReadyToAttack()
    {
        CurrentState = State.AttackReady;
        agent.isStopped = true;
        agent.updateRotation = false;
        AttackPointReached?.Invoke();
    }

    private void LookAtMainCamera()
    {
        var lookPos = cameraTransform.position - myTransform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rotation, Time.deltaTime * lookAtSmooth);
    }

    protected override void RefreshAnimations()
    {
        base.RefreshAnimations();
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }
}
