using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    private Transform _lastTarget;
    private S_EnemyBase _enemyBase;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public TaskAttack(Transform transform)
    {

    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if(target != _lastTarget)
        {
            _enemyBase = target.GetComponent<S_EnemyBase>();
            _lastTarget = target;   
        }

        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime ) 
        {
            bool enemyIsDead = true; //damage enemy function to return whether or not it died after being damaged. 
            if(enemyIsDead)
            {
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
