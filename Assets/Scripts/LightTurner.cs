using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTurner : MonoBehaviour
{
	[SerializeField] private float minIntensityValue = 0.5F;
	[SerializeField] private float maxIntensityValue = 2F;
	[SerializeField] private float timeTurnedOn = 5F;
	[SerializeField] private float timeToTurnOn = 3F;
	private Light light;
	private ParticleSystem sparksEffect = default;

	private void Awake()
	{
		sparksEffect = GetComponentInChildren<ParticleSystem>();
		light = GetComponent<Light>();
		AssignIntensity();
		sparksEffect.Stop();
		StopAllCoroutines();
		StartCoroutine(TurnOffTheLight());
	}

	private IEnumerator TurnOffTheLight()
	{
		yield return new WaitForSeconds(timeTurnedOn);
		sparksEffect.Play();
		light.intensity = Mathf.Lerp(light.intensity,0.1F,1F);
		StartCoroutine(TurnOnTheLight());
	}

	private IEnumerator TurnOnTheLight()
	{
		yield return new WaitForSeconds(timeToTurnOn);
		sparksEffect.Stop();
		AssignIntensity();
		StartCoroutine(TurnOffTheLight());
	}

	private void AssignIntensity()
	{
		light.intensity = Mathf.Lerp(minIntensityValue, maxIntensityValue, 1F);
	}
}
