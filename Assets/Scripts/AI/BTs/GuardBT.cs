using BehaviorTree;
using System.Collections.Generic;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 3f;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>         //1.1 ------------- Order of operation
        {
            new Sequence(new List<Node>                 //2.1
            {
                new CheckEnemyInAttackRange(transform), //3.1.1
                new TaskAttack(transform),              //3.1.2
            }),
            new Sequence(new List<Node>                 //2.2
            {
                new CheckEnemyInFOVRange(transform),    //3.2.1
                new TaskGoToTarget(transform),          //3.2.2
            }),
            new TaskPatrol(transform, waypoints)      //1.2
        });         
          
        return root;
    }
}
