using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollower : MonoBehaviour
{

	public Action Attack;

	[SerializeField] private float speed = 3f;
	[SerializeField] private int attackDamage = 1;
	[SerializeField] private Transform rangeToAttackPlayer;
	private NavMeshAgent navMesh = default;
	private bool isMoving = false;
	private bool attack = false;
	private bool isReadyToAttack = false;

	public bool IsMoving => isMoving;
	public bool canAttack => attack;
	public bool IsReadyToAttack => isReadyToAttack;

	void Start()
	{
		navMesh = GetComponent<NavMeshAgent>();
		navMesh.acceleration = speed;
	}

    private void Update()
	{
		WalkToPlayerSight();
		VerifyDistance();
        //StopInPlayerPosition();
        //Debug.Log(canAttack);

    }

	private void WalkToPlayerSight()
	{
		Vector3 positionToMove = Vector3.MoveTowards(transform.position, rangeToAttackPlayer.position, navMesh.acceleration * Time.deltaTime);
		isMoving = true;
		navMesh.destination = positionToMove;
		VerifyDistance();
	}

	private void VerifyDistance()
	{
		if (Vector2.Distance(transform.position, rangeToAttackPlayer.position) < 0.1f)
		{
			isMoving = false;
			isReadyToAttack = true;
			navMesh.isStopped = true;
			//navMesh.isStopped = false;
			//canAttack = false;
		}
	}

	private void AttackPlayer()
    {
		Node node = FindObjectOfType<Node>();
		if (node.IsExecuted)
		{
			isMoving = false;
			attack = true;
		}
	}

	private void VerifyIfAlreadyAttacking()
    {
		Node node = FindObjectOfType<Node>();

		//if (node.isBeingAttacked)
		//	attack = false;
    }
}
