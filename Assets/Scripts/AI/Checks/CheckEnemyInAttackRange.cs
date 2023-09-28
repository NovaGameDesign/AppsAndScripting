using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAttackRange : Node
{
    private static int _enemyLayermask = 1 << 6;

    private Transform _tranform;
   // private Animator _animator;

    public CheckEnemyInAttackRange(Transform tranform)
    {
        _tranform = tranform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        Transform target = (Transform)t;
        if (Vector3.Distance(_tranform.position, target.position) < GuardBT.attackRange)
        {
            // Set animator values so that attacking occurs and walking stops. 


            state = NodeState.SUCCESS; return state;
        }

        state = NodeState.FAILURE; return state;
    }
}
