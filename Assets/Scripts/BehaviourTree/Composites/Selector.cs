using UnityEngine;
using System.Collections;

namespace BehaviourTreeAsset
{
    public class Selector : Task
    {
        int currentIndex = 0;
        protected override void OnInit()
        {

        }
        
        protected override void OnExcute()
        {
            currentIndex = 0;
            childs[currentIndex].Excute();
            return;
        }

        protected override TaskResult OnUpdate()
        {
            var result = childs[currentIndex].Update();
            if(result == TaskResult.Success)
            {
                return TaskResult.Success;
            }

            if ( result == TaskResult.Fail )
            {
                currentIndex++;
                if ( IsLastTask() )
                {
                    return result;
                }
                //if next is not null, excute next task
                childs[currentIndex].Excute();
            }

            return TaskResult.Running;
        }

        bool IsLastTask()
        {
            return currentIndex == childs.Count; 
        }
    }
}