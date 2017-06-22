using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAsset
{
    public class Sequencer : Task
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
            if ( result == TaskResult.Fail )
            {
                return TaskResult.Fail;
            }

            if ( result == TaskResult.Success)
            {
                currentIndex++;
                if ( IsLastTask() )
                {
                    return result;
                }
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
