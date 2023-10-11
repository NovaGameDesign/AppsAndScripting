using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected S_EnemyBase S_EnemyBase;
        public static bool runTree = true;

        protected void Start()
        {
            S_EnemyBase = gameObject.GetComponent<S_EnemyBase>();
            _root = SetupTree();           
        }

        private void Update()
        {
            if(runTree)
            {
                if (_root != null)
                {
                    _root.Evaluate();
                }
            }
           
        }

        public static void setRunTree()
        {
            if(runTree)
            {
                runTree = false;
            }
            else
                runTree = true;
        }

        protected abstract Node SetupTree();
    }

}
