﻿using UnityEngine;
using Fungus;
using Cinemachine;
using System;

[RequireComponent(typeof(Flowchart), typeof(CinemachineVirtualCamera))]
public class Node : MonoBehaviour
{
    public Action Finished;

    [SerializeField] private float finishLookAtDuration = 0.5F;
    [SerializeField] private float bobSmooth = 4F;

    private CinemachineBrain brainCamera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    private Flowchart flowchart;
    private Vector3 previousFramePosition = Vector3.zero;

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
    }

    private void OnDestroy()
    {
        BlockSignals.OnBlockEnd -= OnBlockEnd;
    }

    private void Update()
    {
        cameraNoise.m_AmplitudeGain = Mathf.Lerp(cameraNoise.m_AmplitudeGain, brainCamera.IsBlending ? 1 : 0, Time.deltaTime * bobSmooth);
    }

    private void OnBlockEnd(Block block)
    {
        if (block.GetFlowchart() == flowchart && block == flowchart.FindBlock("End"))
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
