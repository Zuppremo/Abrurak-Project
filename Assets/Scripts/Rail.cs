using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
	[SerializeField] private Vector3[] nodes;
	[SerializeField] private int nodesCount;


    void Start()
    {
		nodesCount = transform.childCount;
		nodes = new Vector3[nodesCount];

		for (int i = 0; i < nodesCount; i++)
		{
			nodes[i] = transform.GetChild(i).position;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (nodesCount > 1)
		{
			for (int i = 0; i < nodesCount - 1; i++)
			{
				Debug.DrawLine(nodes[i], nodes[i + 1],Color.green);
			}
		}
    }
}
