using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollower : MonoBehaviour
{
	[SerializeField] private float speed = 3f;
	[SerializeField] private int attackDamage = 1;
	private Transform playerPosition;
	private NavMeshAgent navMesh = default;
	private bool isFollowingPlayer = false;
	private bool isAttacking = false;

	public bool IsFollowingPlayer => isFollowingPlayer;
	public bool IsAttacking => isAttacking;

	void Start()
	{
		playerPosition = GameObject.Find("Player").GetComponent<Transform>();
		navMesh = GetComponent<NavMeshAgent>();
	}

    void FixedUpdate()
	{
		MoveToThePlayer();
		AttackThePlayer();
		LookThePlayer();
		StopInPlayerPosition();
		Debug.Log(isAttacking);
	}

	private void LookThePlayer()
	{
       // transform.LookAt(playerPosition.position);
	}

	private void MoveToThePlayer()
	{
		Vector3 positionToMove = Vector3.MoveTowards(transform.position, playerPosition.position, navMesh.acceleration * Time.deltaTime);
		navMesh.destination = positionToMove;
		isFollowingPlayer = true;
	}

	private void AttackThePlayer()
	{
		if (Vector2.Distance(transform.position, playerPosition.position) < 1f && !isAttacking)
		{
			isAttacking = true;
			navMesh.acceleration = 0;
			StartCoroutine(AttackDelay());
		}
		else
			navMesh.acceleration = navMesh.acceleration;
	}

	private void StopInPlayerPosition()
    {
		if (Vector3.Distance(transform.position, playerPosition.position) < 1.5f)
        {
			speed = 0f;
        }
    }

	private IEnumerator AttackDelay()
	{
		if (isAttacking)
		{
			yield return new WaitForSeconds(1);
			PlayerHealth playerHp = FindObjectOfType<PlayerHealth>();
			playerHp.TakeDamage(attackDamage);
			yield return new WaitForSeconds(2);
			isAttacking = false;
		}
	}
}
