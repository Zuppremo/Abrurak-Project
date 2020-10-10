using UnityEngine;

public class Gun : MonoBehaviour
{
	public float damage = 10f;
	public float range = 100f;
	//public ParticleSystem muzzleFlash;
	public Camera fpCam;
	public GameObject impactParticles;
	private AcidBall acidBall;

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

			Enemy enemy = hit.transform.GetComponent<Enemy>();
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
