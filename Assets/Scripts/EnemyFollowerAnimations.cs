using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerAnimations : MonoBehaviour
{
    private Animator animator;
    private EnemyFollower enemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyFollower>();
    }

    void Update()
    {
        if (enemy.IsFollowingPlayer)
            animator.SetBool("isFollow", true);
        else
            animator.SetBool("isFollow", false);

        if (enemy.IsAttacking)
            animator.SetBool("isAttacking", true);
        else 
            animator.SetBool("isAttacking", false);
    }
}
