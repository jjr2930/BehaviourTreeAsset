using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviourTreeAsset
{
    public class BehaviourTreeEditor : EditorWindow
    {

        #region static
        [MenuItem( "Tools/BehaviourTree/Edit BehaviourTree" )]
        static void OpenWindow()
        {
            GetWindow<BehaviourTreeEditor>();

        }

        static Texture taskBackground = null;
        static Texture TaskBackground
        {
            get
            {
                if ( null == taskBackground )
                {
                    taskBackground = ( Texture )EditorGUIUtility.Load( "TaskBackground.png" );
                }
                return taskBackground;
            }
        }

        #endregion

        Task rootTask = null;

        /// <summary>
        /// 코딩의 편의를 위해 Task에셋 별로 EditorTask를 갖는다.
        /// </summary>
        Dictionary<Task, EditorTask> dicEditorTest = new Dictionary<Task, EditorTask>();


        private void OnGUI()
        {
            var foundedTasks = Selection.GetFiltered<Task>( SelectionMode.Assets );
            if ( foundedTasks.Length == 0 )
            {
                DrawSelectObject();
                return;
            }

            rootTask = foundedTasks[0];

            DrawTask( rootTask );
        }

        void DrawSelectObject()
        {
            GUILayout.Label( "Select Behaviour Tree" );
        }

        void DrawTask( Task targetTask )
        {
            //리스트에 추가한다.
            if ( !dicEditorTest.ContainsKey( targetTask ) )
            {
                EditorTask newEditorTask = new EditorTask();
                newEditorTask.refTask = targetTask;
                dicEditorTest.Add( targetTask, newEditorTask );
            }
            EditorTask thisEditorTask = dicEditorTest[targetTask];

            GUILayout.BeginArea( new Rect( Vector2.one * 100, Vector2.one * 50 ), TaskBackground );
            {
                DrawTaskName( thisEditorTask );
                DrawAddTask( thisEditorTask );
                DrawChilds( thisEditorTask );
                DrawConnections();
            };
            GUILayout.EndArea();
        }

        void DrawTaskName( EditorTask targetTask )
        {
            GUILayout.Label( targetTask.refTask.name );
        }

        void DrawAddTask( EditorTask targetTask )
        {
            if ( GUILayout.Button( "+", JGUIUtil.FixRectSIze( 50, 20 ) ) )
            {
                DrawTaskGenericMenu();
            }
        }

        void DrawChilds( EditorTask targetTask )
        {
            int childCount = targetTask.refTask.GetChilds().Count;
            if ( childCount == 0 )
            {
                return;
            }

            for ( int i = 0 ; i < childCount ; i++ )
            {
                DrawTask( targetTask.refTask.GetChilds()[i] );
            }
        }

        void DrawConnections()
        {

        }

        void DrawTaskGenericMenu()
        {
            Type baseType = typeof(Task);
            List<string> menuItemNames = new List<string>();
            Assembly assembly = Assembly.GetAssembly(baseType);
            Type[] types = assembly.GetTypes();
            GenericMenu newMenu = new GenericMenu();
            foreach ( var item in types )
            {
                if ( baseType == item.BaseType )
                {
                    string itemTypeName = item.ToString();
                    menuItemNames.Add( item.ToString() );
                    newMenu.AddItem( new GUIContent( itemTypeName ), false, CreateObjectUsingReflection, item );
                }
            }
            newMenu.ShowAsContext();

        }

        public void CreateObjectUsingReflection( object type )
        {
            Type t = type as Type;
            if ( null == t )
            {
                Debug.LogErrorFormat( "{0} is not type", type );
                return;
            }

            Task instance = ScriptableObject.CreateInstance(t) as Task;
            instance.name = "newTask";
            AssetDatabase.AddObjectToAsset( instance, rootTask );

            AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( rootTask ) );
        }
    }
}