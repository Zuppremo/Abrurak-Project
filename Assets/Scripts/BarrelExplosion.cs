using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrelExplosion : MonoBehaviour, IDamageable
{
	public Action onExplosion;

	[SerializeField] private GameObject explosionParticles = default;
	[SerializeField] private AudioClip explosionSound = default;
	[SerializeField] private float radius = 6F;
	[SerializeField] private float forceOfExplosion = 500F;
	[SerializeField] private int explosionDamage = 1;

	private AudioSource audioSource = default;

	private int health = 2;
	private bool hasExploded = false;

	private void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void Explode()
	{
		if (hasExploded)
			return;

		GameObject particlesGO = Instantiate(explosionParticles, transform.position, transform.rotation);
		audioSource.PlayOneShot(explosionSound);

		Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

		hasExploded = true;

		foreach (Collider nearbyObject in colliders)
		{
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

			if (rb != null)
				rb.AddExplosionForce(forceOfExplosion, transform.position, radius);

			DamageTransfer damageTransfer = nearbyObject.GetComponent<DamageTransfer>();

			if (damageTransfer != null)
				damageTransfer.DoDamage(explosionDamage);
		}

		Destroy(gameObject,1f);
		Destroy(particlesGO, 1f);
	}

	public void DoDamage(int damage)
	{
		health -= damage;

		if (health <= 0 && hasExploded)
			return;

		if (health <= 0 && !hasExploded)
			Explode();
	}
}
