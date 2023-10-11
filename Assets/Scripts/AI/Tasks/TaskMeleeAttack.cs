using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMeleeAttack : Node
{
    private Transform _lastTarget;
    private S_CharacterStats targetStats;
    private S_EnemyBase _enemyBase;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private bool enemyIsDead;

    public TaskMeleeAttack(Transform transform, S_EnemyBase s_Enemybase)
    {
        _enemyBase = s_Enemybase;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
    
        if(target != _lastTarget)
        {
            targetStats = target.transform.GetComponentInChildren<S_CharacterStats>();
            _lastTarget = target;   

        }

        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime ) 
        {
            _enemyBase.MeleeAttack();
            if(targetStats.health <= 0)
            {
                BehaviorTree.Tree.setRunTree();
                ClearData("target");
                //reset animator
            }
            else
                _attackCounter = 0f;
        }
        
        state = NodeState.RUNNING; 
        return state;
    }
}
