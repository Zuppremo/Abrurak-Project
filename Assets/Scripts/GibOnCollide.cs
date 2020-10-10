using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibOnCollide : MonoBehaviour
{
	public GameObject gib;
	public bool gibOnCollision = true;
	public bool gibOnTrigger = true;

	private void OnTriggerEnter(Collider other)
	{
		if (gibOnTrigger)
			GibNow();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (gibOnCollision)
			GibNow();
	}


	private void GibNow()
	{
		if (gib != null)
			Instantiate(gib, transform.position, Quaternion.identity);
		Destroy(gameObject,1f);
	}

	private void OnDestroy()
	{
		GameObject gibInstace = Instantiate(gib, transform.position, Quaternion.identity);
		Destroy(gibInstace,1f);
	}
}
