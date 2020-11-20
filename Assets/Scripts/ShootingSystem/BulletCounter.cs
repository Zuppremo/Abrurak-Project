using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletCounter : MonoBehaviour
{
	[SerializeField] private Image[] images;
	[SerializeField] private Text reloadingText = default;
	//[SerializeField] private Text reloadingRequiredText = default;
	//[SerializeField] private Color reloadingTextColor = new Color();
	[SerializeField] private float flashHideDuration = 0.5F;

	private Gun gun;

	private void Awake()
	{
		reloadingText.gameObject.SetActive(false);
		//reloadingRequiredText.gameObject.SetActive(false);
		StopAllCoroutines();
		gun = FindObjectOfType<Gun>();
		images = GetComponentsInChildren<Image>();
		gun.Fired += OnGunFired;
		gun.ReloadStarted += OnReload;
		gun.ReloadRequired += OnReloadRequired;
	}

	private void OnGunFired(bool hasShoot)
	{
		for (int i = 0; i <= images.Length; i++)
		{
			images[gun.CurrentBullets].gameObject.SetActive(false);
		}
	}

	private void OnReload(float reloadTime)
	{
		for (int i = 0; i < images.Length; i++)
		{
			images[i].gameObject.SetActive(true);
		}
		StartCoroutine(ReloadTime());
		//reloadingRequiredText.DOKill();
		//reloadingRequiredText.DOFade(0, flashHideDuration);
	}
	
	private void OnReloadRequired()
	{
		//reloadingRequiredText.DOKill();
		//reloadingRequiredText.color = reloadingTextColor;
		//reloadingRequiredText.DOFade(1,flashHideDuration);
	}

	private void OnDestroy()
	{
		gun.Fired -= OnGunFired;
		gun.ReloadStarted -= OnReload;
		gun.ReloadRequired -= OnReloadRequired;
	}

	private IEnumerator ReloadTime()
	{
		reloadingText.gameObject.SetActive(true);
		yield return new WaitForSeconds(gun.ReloadDuration);
		reloadingText.gameObject.SetActive(false);
	}
}
