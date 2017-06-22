using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAsset
{
    [System.Serializable]
    public class Blackboard : MonoBehaviour
    {
        [SerializeField]
        List<string> serializedKeys = new List<string>();

        [SerializeField]
        List<BlackBoardValueBase> serializedValues = new List<BlackBoardValueBase>();

        Dictionary<string, BlackBoardValueBase> values
        = new Dictionary<string, BlackBoardValueBase>();

        private void Start()
        {
            values.Clear();
            for ( int i = 0 ; i < serializedKeys.Count ; i++ )
            {
                values.Add( serializedKeys[i], serializedValues[i] );
            }
        }

        public Dictionary<string, BlackBoardValueBase> GetDictionary()
        {
            return values;
        }

        public List<BlackBoardValueBase> GetValues()
        {
            return serializedValues;
        }

        public List<string> GetKeys()
        {
            return serializedKeys;
        }
        

        public void AddValue( string valueName, BlackBoardValueBase value )
        {
            if ( values.ContainsKey( valueName ) )
            {
                Debug.LogErrorFormat( "Blackboard.AddValue=>{0} is already contained", valueName );
                return;
            }

            serializedKeys.Add( valueName );
            serializedValues.Add( value );
            //values.Add( valueName, value );
            return;
        }
        public BlackBoardValue<T> AddValue<T>( string valueName, T value ) where T : new()
        {
            if ( values.ContainsKey( valueName ) )
            {
                Debug.LogErrorFormat( "Blackboard.AddValue=>{0} is already contained", valueName );
                return null;
            }

            BlackBoardValue<T> newValue = new BlackBoardValue<T>();
            newValue.value = value;
            serializedKeys.Add( valueName );
            serializedValues.Add( newValue );
            //values.Add( valueName, newValue );
            return newValue;
        }

        public void RemoveValue<T>( string valueName )
        {
            if ( !valueName.Contains( valueName ) )
            {
                Debug.LogErrorFormat( "Blackboard.RemoveValue=>{0} is not contained", valueName );
                return;
            }

            values.Remove( valueName );
        }

        public T GetValue<T>( string valueName ) where T : new()
        {
            if ( !values.ContainsKey( valueName ) )
            {
                Debug.LogErrorFormat( "Blackboard.RemoveValue=>{0} is not contained", valueName );
                return default( T );
            }

            var blackboardValue = values[valueName] as BlackBoardValue<T>;

            return blackboardValue.value;
        }
    }
}