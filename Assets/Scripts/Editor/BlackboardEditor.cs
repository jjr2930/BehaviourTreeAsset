using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;


namespace BehaviourTreeAsset
{
    [CustomEditor( typeof( Blackboard ) )]
    public class BlackboardEditor : Editor
    {
        Blackboard blackboard = null;
        private void Awake()
        {
            blackboard = ( Blackboard )target;
        }

        public override void OnInspectorGUI()
        {
            DrawAddBtn();
            DrawBody();
        }


        void DrawAddBtn()
        {
            if ( GUILayout.Button( "+" ) )
            {
                DrawGenericMenu();
            }
        }
        
        private void DrawBody()
        {
            GUILayout.BeginVertical();
            {
                var keys = blackboard.GetKeys();
                var values = blackboard.GetValues();
                for ( int i = 0 ; i < blackboard.GetKeys().Count ; i++ )
                {
                    GUILayout.BeginHorizontal();
                    {
                        //키 출력
                        keys[i] = GUILayout.TextField( keys[i], GUILayout.MinWidth( 150 ),GUILayout.MaxWidth(150) );

                        //키 값과 맞추어서 서브 에셋의 이름 변경
                        if(null != values[i] )
                        {
                            values[i].name = keys[i];
                        }

                        //서브에셋의 값을 출력
                        DrawValue( values[i] );

                        //삭제 버튼 출력
                        DrawRemoveBtn( i );
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            if ( GUI.changed )
            {
                EditorUtility.SetDirty( blackboard.gameObject );
                //AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( blackboard.gameObject ) );
            }
        }
        
        /// <summary>
        /// 리플렉션을 이용하여 value를 가져온다. 에디터에서만 쓰이니 너무 느리지만 않으면 된다.
        /// </summary>
        /// <param name="p"></param>
        void DrawValue(BlackBoardValueBase p)
        {
            var fieldInfo = p.GetType().GetField( "value" );
            var value = fieldInfo.GetValue( p );
            
            GUILayout.Label( value.ToString(), GUILayout.MaxWidth( 150 ) );
        }

        void DrawRemoveBtn( int index )
        {
            if ( GUILayout.Button( "-", GUILayout.MinWidth( 25 ), GUILayout.MaxWidth( 25 ) ) )
            {
                string mainAssetPath = AssetDatabase.GetAssetPath(blackboard.gameObject);
                UnityEngine.Object subAsset = blackboard.GetValues()[index];

                blackboard.GetKeys().RemoveAt( index );
                blackboard.GetValues().RemoveAt( index );
                //remove sub object
                DestroyImmediate( subAsset,true );
                AssetDatabase.ImportAsset( mainAssetPath );
            }
        }

        void DrawGenericMenu()
        {
            Type baseType = typeof(BlackBoardValueBase);
            List<string> menuItemNames = new List<string>();
            Assembly assembly = Assembly.GetAssembly(typeof(BlackBoardValueBase));
            Type[] types = assembly.GetTypes();
            GenericMenu newMenu = new GenericMenu();
            foreach ( var item in types )
            {
                if ( baseType == item.BaseType
                    && item != typeof( BlackBoardValue<> ) )
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

            BlackBoardValueBase instance = ScriptableObject.CreateInstance(t) as BlackBoardValueBase;
            instance.name = "newValue";
            AssetDatabase.AddObjectToAsset( instance, blackboard.gameObject );
            AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( blackboard.gameObject ) );
            blackboard.GetKeys().Add( "newValue" );
            blackboard.GetValues().Add( instance );
        }

    }
}