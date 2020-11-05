using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletCounter : MonoBehaviour
{
	[SerializeField] private Image[] images;
	[SerializeField] private Image noBulletsImage = default;
	[SerializeField] private Text reloadingText = default;
	[SerializeField] private Color noBulletsColor = new Color();
	[SerializeField] private float flashHideDuration = 0.5F;

	private Gun gun;

	private void Awake()
	{
		reloadingText.gameObject.SetActive(false);
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
		noBulletsImage.DOKill();
		noBulletsImage.DOFade(0, flashHideDuration);
	}
	
	private void OnReloadRequired()
	{
		noBulletsImage.DOKill();
		noBulletsImage.color = noBulletsColor; 
		noBulletsImage.DOFade(0,flashHideDuration);
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
