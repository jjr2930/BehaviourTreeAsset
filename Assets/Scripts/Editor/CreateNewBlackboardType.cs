using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;
using System;
using System.Reflection;
public class CreateNewBlackboardType : EditorWindow
{
    List<Type> typeList = new List<Type>();
    Vector2 scrollCoor = Vector2.zero;
    [MenuItem( "Tools/BehaviourTree/CreateNewType" )]
    static void CreateNewType()
    {
        CreateNewBlackboardType newWindow = EditorWindow.GetWindow<CreateNewBlackboardType>();
    }

    /// <summary>
    /// 어셈블리에서 타입 이름들을 모두 가져온다.
    /// </summary>
    private void Awake()
    {
        GetAssembly();
    }

    private void GetAssembly()
    {
        Debug.Log( "get assembly" );
        var assembly = typeof(int).Assembly;
        var types = assembly.GetTypes();

        typeList.Clear();
        for ( int i = 0 ; i < types.Length ; i++ )
        {
            typeList.Add( types[i] );
        }
    }

    /// <summary>
    /// 리플렉션을 이용하여 타입 목록을 만들고, 현재 만들 블랙보드 변수를 선택하게 한다.
    /// </summary>
    private void OnGUI()
    {
        DrawBody();
    }


    void DrawBody()
    {
        if(typeList.Count ==0 )
        {
            GetAssembly();
        }
        scrollCoor = EditorGUILayout.BeginScrollView( scrollCoor );
        {
            for ( int i = 0 ; i < typeList.Count ; i++ )
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField( typeList[i].ToString() );
                        DrawAddButton( i );
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void DrawAddButton(int index)
    {
        if ( GUILayout.Button( "+", GUILayout.MinWidth( 25 ), GUILayout.MaxWidth( 25 ) ) )
        {
            ProcessCreate( typeList[index] );
        }
    }

    private static void ProcessCreate( System.Type t )
    {
        string streamingPath = Application.streamingAssetsPath;
        string templatePath = string.Format(@"file://{0}/{1}",streamingPath,"CreateTypeTemplate.txt");
        string typeName = t.ToString().Replace('.','_');
        string fileName = string.Format("BlackBoardValue{0}.cs",typeName);
        string saveDirectory = "Assets/Scripts/BehaviourTree/BlackboardVariables";
        string currentDirectory = System.IO.Directory.GetCurrentDirectory();
        string filePath = string.Format(@"{0}/{1}/{2}",currentDirectory,saveDirectory,fileName);

        string txt = GetScriptString(t);
        Debug.Log( txt );
        byte[] bytes = UTF8Encoding.UTF8.GetBytes(txt.ToCharArray());
        System.IO.FileStream oFileStream = null;
        oFileStream = new System.IO.FileStream( filePath, System.IO.FileMode.OpenOrCreate );
        oFileStream.Write( bytes, 0, bytes.Length );
        oFileStream.Close();
    }

    static string GetScriptString(System.Type t)
    {
        string fileTypeName = t.ToString().Replace('.','_');
        StringBuilder sb = new StringBuilder();
        sb.Append( "namespace BehaviourTreeAsset\n" );
        sb.Append( "{\n" );
        sb.Append( "[System.Serializable]\n" );
        sb.AppendFormat( "public class BlackBoardValue{0}:BlackBoardValueBase\n",fileTypeName );
        sb.Append( "{\n" );
        sb.Append( "[UnityEngine.SerializeField]\n" );
        sb.AppendFormat( "public {0} value;\n", t.ToString() );
        sb.Append( "}\n" );
        sb.Append( "}\n" );

        return sb.ToString();
    }
}
