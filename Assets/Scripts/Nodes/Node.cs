using UnityEngine;
using Fungus;
using Cinemachine;
using System;

[RequireComponent(typeof(Flowchart), typeof(CinemachineVirtualCamera))]
public class Node : MonoBehaviour
{
    public Action Finished;

    [SerializeField] private float finishLookAtDuration = 0.5F;
    [SerializeField] private float bobSmooth = 4F;
    [SerializeField] private float enemyKillFinishDelay = 1F;
    [SerializeField] private Enemy[] enemies = default;
    [SerializeField] private Transform attackPoint = default;

    private CinemachineBrain brainCamera = default;
    private CinemachineBasicMultiChannelPerlin cameraNoise = default;
    private Flowchart flowchart = default;
    private Vector3 previousFramePosition = Vector3.zero;
    private int currentEnemyToAttack = 0;
    private bool isInAttackMode = false;

    public bool IsExecuted { get; private set; }
    public bool CanBeExecuted => !IsExecuted && (Vector3.Distance(Camera.main.transform.position, transform.position) < 0.1F);
    public Transform MyTransform { get; private set; }
    public float FinishLookAtDuration => finishLookAtDuration;

    private void Awake()
    {
        brainCamera = FindObjectOfType<CinemachineBrain>();
        CinemachineVirtualCamera vCam = GetComponent<CinemachineVirtualCamera>();
        cameraNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        MyTransform = transform;
        flowchart = GetComponent<Flowchart>();
        BlockSignals.OnBlockEnd += OnBlockEnd;

        if (enemies != null && enemies.Length > 0)
        {
            Array.ForEach(enemies, e => e.Died += OnEnemyDied);
            Array.ForEach(enemies, e => e.ReadyToAttack += OnEnemyReadyToAttack);
        }
    }

    private void OnDestroy()
    {
        BlockSignals.OnBlockEnd -= OnBlockEnd;

        if (enemies != null && enemies.Length > 0)
        {
            Array.ForEach(enemies, e => e.Died -= OnEnemyDied);
            Array.ForEach(enemies, e => e.ReadyToAttack -= OnEnemyReadyToAttack);
        }
    }

    private void OnEnemyReadyToAttack()
    {
        if (Array.Exists(enemies, e => e.IsAttacking))
            return;

        isInAttackMode = true;
        SetEnemyToAttack(enemies[currentEnemyToAttack]);
    }

    private void SetEnemyToAttack(Enemy enemy)
    {
        if (enemy == null)
            return;

        if (enemy.IsDead || !enemy.IsReadyToAttack)
            TrySetNextEnemy();
        else
            enemy.StartAttackSequence(attackPoint.position, TrySetNextEnemy);
    }

    private void TrySetNextEnemy()
    {
        currentEnemyToAttack++;

        if (currentEnemyToAttack >= enemies.Length)
            currentEnemyToAttack = 0;

        SetEnemyToAttack(enemies[currentEnemyToAttack]);
    }

    private void Update()
    {
        cameraNoise.m_AmplitudeGain = Mathf.Lerp(cameraNoise.m_AmplitudeGain, brainCamera.IsBlending ? 1 : 0, Time.deltaTime * bobSmooth);
    }

    private void OnBlockEnd(Block block)
    {
        if (block.GetFlowchart() == flowchart && block.BlockName == "End")
        {
            if (enemies == null || enemies.Length == 0)
                FinishNode();
            else
                Array.ForEach(enemies, e => e.Activate ());
        }
    }

    private void OnEnemyDied()
    {
        bool hasEnemiesAlive = Array.Exists(enemies, e => !e.IsDead);

        if (!hasEnemiesAlive)
            Invoke(nameof(FinishNode), enemyKillFinishDelay);
        else if (isInAttackMode)
            SetEnemyToAttack(Array.Find(enemies, e => e.IsReadyToAttack));
    }

    private void FinishNode()
    {
        Finished?.Invoke();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Execute()
    {
        if (IsExecuted)
            return;

        IsExecuted = true;
        flowchart.ExecuteBlock("Start");
    }
}
