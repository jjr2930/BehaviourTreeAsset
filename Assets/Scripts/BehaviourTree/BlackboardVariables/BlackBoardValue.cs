using UnityEngine;
using System.Collections;

namespace BehaviourTreeAsset
{



    /// <summary>
    /// this class for runtime
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class BlackBoardValue<T> : BlackBoardValueBase
    {
        public T value;
    }

}