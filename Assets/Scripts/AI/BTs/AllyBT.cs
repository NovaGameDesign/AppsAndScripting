using BehaviorTree;
using System.Collections.Generic;


public class AllyBT : Tree
{

    public static float speed = 2f;
    public static float fovRange = 5f;
    public static float attackRange = 5f;
    public S_Player player;
    

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>         
        {            
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform, attackRange),
                new TaskLookAtTarget(transform),
                new TaskRangedAttack(transform, S_EnemyBase),
            }),
            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(player.transform, "Enemy", fovRange),
                
            }),
            new TaskMoveToPlayer(transform, player),
        });         
          
        return root;
    }
}
