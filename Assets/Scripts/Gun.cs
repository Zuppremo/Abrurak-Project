using UnityEngine;

public class Gun : MonoBehaviour
{
	public float damage = 10f;
	public float range = 100f;
	//public ParticleSystem muzzleFlash;
	public GameObject impactParticles;

    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}    
    }

	private void Shoot()
	{
		//muzzleFlash.Play();

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray,out hit,range))
		{
			Debug.Log(hit.transform.name);

			OldEnemy enemy = hit.transform.GetComponent<OldEnemy>();
			AcidBall acidBall = hit.transform.GetComponent<AcidBall>();

			if (enemy != null)
				enemy.TakeDamage(damage);
			else if (acidBall != null)
				Destroy(acidBall.gameObject);

			GameObject particlesGo = Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(particlesGo, 2f);
		}
	}
}
