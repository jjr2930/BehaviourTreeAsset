using UnityEngine;
using UnityEditor;
using System.Collections;


namespace BehaviourTreeAsset
{
    public class CreateNewBeahviourTree : EditorWindow
    {
        [MenuItem( "Tools/BehaviourTree/CreateNewBehaviourTree" )]
        public static void CreateNewBT()
        {
            string fullPath = EditorUtility.SaveFilePanel( "Save as Behaviour Tree", "", "NewTask", "asset" );
            int temp = fullPath.IndexOf("Assets",0);
            string loadPath = fullPath.Remove(0,temp);

            AssetDatabase.CreateAsset( CreateInstance<Task>(), loadPath );
            AssetDatabase.ImportAsset( loadPath );
            //CreateNewBeahviourTree newWindow = GetWindow<CreateNewBeahviourTree>();
        }
    }
}