using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunSounds : MonoBehaviour
{
	[SerializeField] private AudioClip gunFiredClip = default;
	[SerializeField] private AudioClip gunReloadRequired = default;
	[SerializeField] private AudioClip gunReloadStarted = default;

	private Gun gun;
	private AudioSource audioSource;
	
    private void Awake()
    {
		gun = GetComponent<Gun>();
		audioSource = gameObject.AddComponent<AudioSource>();
		gun.Fired += OnGunFired;
		gun.ReloadRequired += OnReloadRequired;
		gun.ReloadStarted += OnReloadStarted;
    }

	private void OnGunFired(bool hasAppliedDamage)
	{
		audioSource.PlayOneShot(gunFiredClip);
	}

	private void OnReloadRequired()
	{
		audioSource.PlayOneShot(gunReloadRequired);
	}

	private void OnReloadStarted(float reloadTime)
	{
		audioSource.PlayOneShot(gunReloadStarted);
	}

	private void OnDestroy()
	{
		gun.Fired -= OnGunFired;
		gun.ReloadRequired -= OnReloadRequired;
		gun.ReloadStarted -= OnReloadStarted;
	}

}
