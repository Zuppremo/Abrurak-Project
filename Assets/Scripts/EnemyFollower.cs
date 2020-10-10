using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
	private bool isAttacking = false;
	[SerializeField] private float speed = 3f;
	[SerializeField] private int attackDamage = 1;
	private Transform playerPosition;
	private Rigidbody enemyRigidbody;

    void Start()
    {
		playerPosition = GameObject.Find("Player").GetComponent<Transform>();
		enemyRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
	{
		MoveToThePlayer();
		AttackThePlayer();
		LookThePlayer();
	}

	private void LookThePlayer()
	{
		transform.LookAt(playerPosition.position);
	}

	private void MoveToThePlayer()
	{
		Vector3 positionToMove = Vector3.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);
		enemyRigidbody.MovePosition(positionToMove);
	}

	private void AttackThePlayer()
	{
		if (Vector2.Distance(transform.position, playerPosition.position) < 0.2f && !isAttacking)
		{
			isAttacking = true;
			speed = 0;
			StartCoroutine(secDelay());
			StartCoroutine(attackDelay());
		}
	}

	private IEnumerator attackDelay()
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

	private IEnumerator secDelay()
	{
		yield return new WaitForSeconds(1);
	}
}
