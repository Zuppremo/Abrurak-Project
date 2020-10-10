using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtClickLocation : MonoBehaviour
{
	public Rigidbody bullet;
	public float force = 10f;
	public ForceMode forceMode;

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Quaternion rotation = Quaternion.LookRotation(ray.direction);

			Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

			Rigidbody instance = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
			instance.AddForce(ray.direction * force,forceMode);
		}
    }
}
