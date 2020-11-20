using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GunMuzzleFlash : MonoBehaviour
{
	[SerializeField] private GameObject muzzleFlash = default;
	[SerializeField] private Image flashImage = default;
	[SerializeField] private Color colorImage;
	[SerializeField] private float flashHideDuration = 0.5F;
	private Gun gun;

	private void Awake()
	{
		gun = FindObjectOfType<Gun>();
		muzzleFlash.SetActive(false);
		gun.Fired += OnGunFired;
	}

	private void OnGunFired(bool hasShoot)
	{
		muzzleFlash.SetActive(true);
		flashImage.DOKill();
		flashImage.color = colorImage;
		flashImage.DOFade(0F,flashHideDuration);
		StartCoroutine(turnOff());
	}

	private void OnDestroy()
	{
		gun.Fired -= OnGunFired;
	}

	private IEnumerator turnOff()
	{
		yield return new WaitForSeconds(0.2F);
		flashImage.DOKill();
		flashImage.DOFade(0,flashHideDuration);
		muzzleFlash.SetActive(false);
	}

}
