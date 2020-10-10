using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RailManager : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera camera = default;
	[SerializeField] private float movementSpeed = 2.0F;
	[SerializeField] private GameObject[] node;
	[SerializeField] private float[] cameraRotationY;
	private int currentCameraIndex = 0;


	private CinemachineTrackedDolly dolly;
	[SerializeField] private bool isAllEnemiesDied;
	private int currentWaypoint = 0;
	
    private void Awake()
    {
		dolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
		dolly.m_PathPosition = 0;
    }

    private void Update()
    {
		KilledEnemies();
		CameraRotation();
		if (dolly.m_PathPosition < currentWaypoint)
		{
			dolly.m_PathPosition += Time.deltaTime * movementSpeed;
		}
		else if (isAllEnemiesDied)
		{
			isAllEnemiesDied = false;
			currentWaypoint++;
		}
		Debug.Log(node[0].gameObject.transform.childCount);
    }

	private void KilledEnemies()
	{
		for (int i = 0; i < node.Length; i++)
		{
			if (node[i].gameObject.transform.childCount < 1)
			{
				isAllEnemiesDied = true;
			}
		}
		//isAllEnemiesDied = false;
	}

	private void CameraRotation()
	{
		//for (int i = 0; i < dolly.m_PathPosition; i++)
		//{
		//	cameraRotationY[i] = Random.Range(0, 180);
		//} 

		if (dolly.m_PathPosition > 0)
		{
			currentCameraIndex++;
			camera.transform.LookAt(node[currentCameraIndex].transform);
		}

		for (int i = 0; i < node.Length; i++)
		{
			int numberOfNode = 0;
			node[i] = GameObject.Find("Node" + numberOfNode.ToString());
			numberOfNode++;
			Debug.Log(node[i]);
		} 

	}


	//private 
}
