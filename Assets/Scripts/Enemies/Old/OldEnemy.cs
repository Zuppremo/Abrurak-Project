using UnityEngine;

public class OldEnemy : MonoBehaviour
{
	[SerializeField] private GameObject playerHitParticles = default;
	[SerializeField] private GameObject acidBall = default;
	public Transform throwPoint;
	public float health = 50f;
	public float fireRange;
	public Transform playerTarget;
	public float fireRate;
	private float nextFire;
	public int attackDamage = 1;
	private PlayerHealth playerHealth;

	private void Awake()
	{
		playerHealth = FindObjectOfType<PlayerHealth>();
	}

	private void Update()
	{
		{
			//transform.LookAt(playerTarget);
		}

		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;

			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, fireRange))
			{
				Instantiate(acidBall, throwPoint.position, Quaternion.LookRotation(Camera.main.transform.position - throwPoint.position));
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
				Debug.Log("Did hit");
				GameObject hurtParticles = Instantiate(playerHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(hurtParticles, 2f);
			}
			else
			{
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
				Debug.Log("Did not hit");
			}
		}
	}

	public void TakeDamage(float amount)
	{
		health -= amount;

		if (health <= 0)
			Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
