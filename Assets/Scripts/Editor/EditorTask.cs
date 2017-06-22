using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviourTreeAsset
{
    /// <summary>
    /// 새로운 태스크를 만들때 필요한 파라미터
    /// </summary>
    public class CreateNewTaskParameter
    {
        public Type newType;
        public Task parent;
    }

    /// <summary>
    /// 에디터에서 태스크를 그리기 위해 필요한 데이터들을 저장하는 클래스이다. 데이터에서만 쓰인다.
    /// </summary>
    public class EditorTask 
    {
        #region const
        const float TASK_WIDTH  = 100;
        const float TASK_HEIGHT = 50;
        #endregion

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

        /// <summary>
        /// 이것이 표현하는 태스크 에셋의 포인터
        /// </summary>
        public Task refTask { get; set; }

        /// <summary>
        /// 이 태스크의 위치
        /// </summary>
        public Vector2 Position  { get; set; }

        /// <summary>
        /// 이 태스크의 사이즈
        /// </summary>
        public Vector2 Size { get; set; }

        public void DrawTask()
        {
            GUILayout.BeginArea( new Rect( Position.x, Position.y, TASK_WIDTH, TASK_HEIGHT ), TaskBackground );
            {
                DrawTaskName();
                DrawAddTask();
            }
            GUILayout.EndArea();
        }

        void DrawTaskName( )
        {
            GUILayout.Label( refTask.name );
        }


        void DrawAddTask( )
        {
            if ( GUILayout.Button( "+", JGUIUtil.FixRectSIze( 50, 20 ) ) )
            {
                DrawTaskGenericMenu( );
            }
        }


        void DrawTaskGenericMenu( )
        {
            Type baseType = typeof(Task);
            List<string> menuItemNames = new List<string>();
            Assembly assembly = Assembly.GetAssembly(baseType);
            Type[] types = assembly.GetTypes();
            GenericMenu newMenu = new GenericMenu();
            foreach ( var item in types )
            {
                if ( baseType == item.BaseType 
                    && !item.IsAbstract)
                {
                    CreateNewTaskParameter p = new CreateNewTaskParameter();
                    p.newType = item;
                    p.parent = refTask;

                    string itemTypeName = item.ToString();
                    menuItemNames.Add( item.ToString() );
                    newMenu.AddItem( new GUIContent( itemTypeName ), false, CreateObjectUsingReflection, p );
                }
            }
            newMenu.ShowAsContext();
        }

        public void CreateObjectUsingReflection( object parameter )
        {
            CreateNewTaskParameter p = parameter as CreateNewTaskParameter;
            Type t = p.newType as Type;
            if ( null == t )
            {
                Debug.LogErrorFormat( "{0} is not type", p.newType.ToString() );
                return;
            }

            Task instance = ScriptableObject.CreateInstance(t) as Task;
            instance.name = "newTask";
            AssetDatabase.AddObjectToAsset( instance, refTask );

            //새로만든 애셋을 자식으로 넣어준다.
            p.parent.AddTask( instance );

            //세이브 하라고 표시해주고
            EditorUtility.SetDirty( refTask);

            //다시 임포트하여 갱신
            AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( refTask) );
        }
    }
}