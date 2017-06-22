using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAsset
{
    public class DebugLog : Action
    {        
        protected override void OnExcute()
        {
            base.OnExcute();
            Debug.Log( "Test" );
            return;
        }
    }
}