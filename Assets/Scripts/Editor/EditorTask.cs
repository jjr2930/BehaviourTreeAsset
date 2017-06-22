using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BehaviourTreeAsset
{
    /// <summary>
    /// 에디터에서 태스크를 그리기 위해 필요한 데이터들을 저장하는 클래스이다. 데이터에서만 쓰인다.
    /// </summary>
    public class EditorTask 
    {
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

    }
}