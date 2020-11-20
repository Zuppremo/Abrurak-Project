using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReloadText : MonoBehaviour
{

	[SerializeField] private Text reloadText = default;
	[SerializeField] private float flashHideDuration = 1F;
	private Gun gun;

	private void Awake()
	{
		gun = FindObjectOfType<Gun>();
		gun.ReloadRequired += OnReloadRequired;
		gun.ReloadStarted += OnReloadStarted;
		reloadText.gameObject.SetActive(false);
	}

	private void OnReloadRequired()
	{
		reloadText.DOKill();
		reloadText.gameObject.SetActive(true);
		reloadText.DOFade(1, flashHideDuration);
		StopAllCoroutines();
		StartCoroutine(TurnOffReloadText());
	}

	private void OnReloadStarted(float duration)
	{
		reloadText.DOKill();
		reloadText.DOFade(0, flashHideDuration);
	}

	private void OnDestroy()
	{
		gun.ReloadRequired -= OnReloadRequired;
		gun.ReloadStarted -= OnReloadStarted;
	}

	private IEnumerator TurnOffReloadText()
	{
		yield return new WaitForSeconds(0.5F);
		reloadText.gameObject.SetActive(false);
	}
}
