using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskLookAtTarget : Node
{
    private Transform _transform;
   public TaskLookAtTarget(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != null)
        {
            _transform.LookAt(target);
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
