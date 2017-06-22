using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAsset
{
    [System.Serializable]
    public class Task : ScriptableObject
    {
        protected List<Task> childs = new List<Task>();
        bool isInit = false;
        public bool AddTask( Task newTask )
        {
            if ( !childs.Contains( newTask ) )
            {
                childs.Add( newTask );
                return true;
            }
            Debug.LogErrorFormat( "Task.AddTask=> already have {0}", newTask );
            return false;
        }

        public void RemoveTask( Task oldTask )
        {
            if ( !childs.Contains( oldTask ) )
            {
                Debug.LogErrorFormat( "Task.RemoveTask=> not contain {0}", oldTask );
                return;
            }
            childs.Remove( oldTask );
        }

        public void Init()
        {
            OnInit();
        }

        public List<Task> GetChilds()
        {
            return childs;
        }

        public void Excute()
        {
            if(!isInit)
            {
                Init();
            }
            OnExcute();
            return;
        }

        public TaskResult Update()
        {
            return OnUpdate();
        }

        #region for child
        protected virtual void OnInit() { }
        protected virtual void OnExcute() { }
        protected virtual TaskResult OnUpdate() { return TaskResult.Running; }
        #endregion
    }
}