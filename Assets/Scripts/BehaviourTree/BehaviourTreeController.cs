using UnityEngine;
using System.Collections;

namespace BehaviourTreeAsset
{
    public class BehaviourTreeController : MonoBehaviour
    {
        [SerializeField]
        private Task rootTask = null;

        private void Awake()
        {
            rootTask.Excute();    
        }

        private void Update()
        {
            rootTask.Update();
        }
    }
}