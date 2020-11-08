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
    private CinemachineBasicMultiChannelPerlin cameraNoise = default;
    private NoiseSettings headBobNoiseSettings = default;
    private bool isShooting = false;
    private Gun gun;

    private void Awake()
    {
        brainCamera = FindObjectOfType<CinemachineBrain>();
        CinemachineVirtualCamera vCam = GetComponent<CinemachineVirtualCamera>();
        cameraNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        headBobNoiseSettings = cameraNoise.m_NoiseProfile;
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
        cameraNoise.m_NoiseProfile = shootNoiseSettings;
        cameraNoise.m_AmplitudeGain = 1;
        CancelInvoke(nameof(CancelShoot));
        Invoke(nameof(CancelShoot), shootShakeDuration);
    }

    private void CancelShoot()
    {
        cameraNoise.m_NoiseProfile = headBobNoiseSettings;
        cameraNoise.m_AmplitudeGain = brainCamera.IsBlending ? 1 : 0;
        isShooting = false;
    }

    private void Update()
    {
        if (!isShooting)
            cameraNoise.m_AmplitudeGain = Mathf.Lerp(cameraNoise.m_AmplitudeGain, brainCamera.IsBlending ? 1 : 0, Time.deltaTime * bobSmooth);
    }
}
