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

    private void Update()
    {
        animator.SetBool("canAttack", enemy.canAttack);
        animator.SetBool("isMoving", enemy.IsMoving);
    }

}
