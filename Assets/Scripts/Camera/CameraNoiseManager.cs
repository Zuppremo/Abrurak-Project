using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNoiseManager : MonoBehaviour
{
    [SerializeField] private float bobSmooth = 4F;
    [SerializeField] private float shootShakeDuration = 0.1F;
    [SerializeField] private NoiseSettings shootNoiseSettings = default;

    private CinemachineBrain brainCamera = default;
    private List<CinemachineBasicMultiChannelPerlin> cameraNoises = new List<CinemachineBasicMultiChannelPerlin>();
    private NoiseSettings headBobNoiseSettings = default;
    private bool isShooting = false;
    private Gun gun;

    private void Awake()
    {
        brainCamera = FindObjectOfType<CinemachineBrain>();
        CinemachineVirtualCamera[] vCams = FindObjectsOfType<CinemachineVirtualCamera>();

        for (int i = 0; i < vCams.Length; i++)
        {
            if (vCams[i].TryGetComponent<Node>(out Node node))
                cameraNoises.Add(vCams[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
        }
        if (cameraNoises.Count > 0)
            headBobNoiseSettings = cameraNoises[0].m_NoiseProfile;
        gun = FindObjectOfType<Gun>();
        gun.Fired += OnGunFired;
    }

    private void OnDestroy()
    {
        gun.Fired -= OnGunFired;
    }

    private void OnGunFired(bool hasHit)
    {
        isShooting = true;
        cameraNoises.ForEach(n =>
        {
            n.m_NoiseProfile = shootNoiseSettings;
            n.m_AmplitudeGain = 1;
        });
        CancelInvoke(nameof(CancelShoot));
        Invoke(nameof(CancelShoot), shootShakeDuration);
    }

    private void CancelShoot()
    {
        cameraNoises.ForEach(n => 
        {
            n.m_NoiseProfile = headBobNoiseSettings;
            n.m_AmplitudeGain = brainCamera.IsBlending ? 1 : 0;
        });
        isShooting = false;
    }

    private void Update()
    {
        if (!isShooting)
            cameraNoises.ForEach(n => n.m_AmplitudeGain = Mathf.Lerp(cameraNoises[0].m_AmplitudeGain, brainCamera.IsBlending ? 1 : 0, Time.deltaTime * bobSmooth));
    }
}
