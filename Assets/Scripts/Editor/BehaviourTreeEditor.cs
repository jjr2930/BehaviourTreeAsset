using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviourTreeAsset
{
    public class BehaviourTreeEditor : EditorWindow
    {
        #region const

        public float TASK_WIDTH = 100;
        public float TASK_HEIGHT = 50;
        public float TASK_INTERVAL = 20;
        #endregion

        #region static
        [MenuItem( "Tools/BehaviourTree/Edit BehaviourTree" )]
        static void OpenWindow()
        {
            GetWindow<BehaviourTreeEditor>();
        }

        static Task cachedSelectedTask = null;
        #endregion

        Task rootTask = null;

        /// <summary>
        /// 코딩의 편의를 위해 Task에셋 별로 EditorTask를 갖는다.
        /// </summary>
        Dictionary<Task, EditorTask> dicEditorTest = new Dictionary<Task, EditorTask>();
        List<Task> tempTaskList = new List<Task>();
        private void Awake()
        {
            rootTask = GetSelectedTask();
            CollectChildTask( rootTask );
            CreateEditorTask( rootTask );
            SetChildPosition( rootTask );

        }

        private void OnGUI()
        {
            rootTask = GetSelectedTask();

            if(null == rootTask)
            {
                GUILayout.Label( "Select Behaviour tree" );
                return; 
            }

            CollectChildTask( rootTask );
            CreateEditorTask( rootTask );
            SetChildPosition( rootTask );

            DrawTask( rootTask );
        }

        void SetChildPosition(Task targetTask)
        {
            var         editorTask      = dicEditorTest[targetTask];
            EditorTask  childEditorTask = null;
            var         childs          = targetTask.GetChilds();

            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                childEditorTask             = dicEditorTest[childs[i]];
                float newX                  = editorTask.Position.x + 100 * i;
                float newY                  = editorTask.Position.y + 50;
                childEditorTask.Position    = new Vector2( newX, newY );
                SetChildPosition( childs[i] );
            }
        }

        void CreateEditorTask(Task targetTask)
        {
            AddTaskToDictionary( targetTask );
            var editorTask = dicEditorTest[targetTask];
            var childs = targetTask.GetChilds();
            for(int i = 0 ; i<childs.Count ; i++ )
            {
                CreateEditorTask( childs[i] );
            }
        }

        Task GetSelectedTask()
        {
            var currentSelectTasks = Selection.GetFiltered<Task>( SelectionMode.Assets );
            if ( currentSelectTasks.Length != 0 )
            {
                if( currentSelectTasks[0] != cachedSelectedTask)
                {
                    cachedSelectedTask = currentSelectTasks[0];
                }
            }

            return cachedSelectedTask;
        }

        void DrawTask( Task targetTask )
        {
            //리스트에 추가한다.
            foreach ( var item in dicEditorTest )
            {
                item.Value.DrawTask();
            }
        }

        void CollectChildTask( Task targetTask)
        {
            if(!tempTaskList.Contains(targetTask))
            {
                tempTaskList.Add( targetTask );
            }

            var childs = targetTask.GetChilds();
            for(int i=0 ; i<childs.Count ; i++ )
            {
                CollectChildTask( childs[i] );
            }
        }

        void AddTaskToDictionary( Task targetTask )
        {
            if ( !dicEditorTest.ContainsKey( targetTask ) )
            {
                EditorTask newEditorTask = new EditorTask();
                newEditorTask.refTask = targetTask;
                dicEditorTest.Add( targetTask, newEditorTask );
            }
        }

        


        void DrawChilds( EditorTask targetTask )
        {
            var childs = targetTask.refTask.GetChilds();
            if ( childs.Count== 0 )
            {
                return;
            }

            for ( int i = 0 ; i < childs.Count; i++ )
            {
                AddTaskToDictionary( childs[i] );
                EditorTask child = dicEditorTest[childs[i]];
                float newX = targetTask.Position.x + TASK_WIDTH * i + TASK_INTERVAL;
                float newY = targetTask.Position.y + TASK_HEIGHT + TASK_INTERVAL;
                child.Position = targetTask.Position + new Vector2( newX, newY );
                DrawTask( childs[i] );
            }
        }

        void DrawConnections()
        {

        }


       


    }

    
}