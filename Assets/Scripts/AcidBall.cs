using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBall : MonoBehaviour
{
	private Rigidbody rb;
	private PlayerHealth playerHealth;
	public float speed;
	public int damage = 1;
	

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		playerHealth = FindObjectOfType<PlayerHealth>();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
		//transform.position += transform.forward * speed * Time.deltaTime;
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			playerHealth.TakeDamage(damage);
			Destroy(gameObject, 1f);
		}
		Destroy(gameObject, 5f);
	}
}
