using UnityEngine;
using System.Collections.Generic;
using System;
public class BTUpdateManager : MonoBehaviour
{
    /// <summary>
    /// singletone
    /// </summary>
    static BTUpdateManager instance = null;

    /// <summary>
    /// unity is single thread... have not necessary to lock
    /// </summary>
    public static BTUpdateManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("BTUpdateManager");
                instance = go.AddComponent<BTUpdateManager>();
                DontDestroyOnLoad( go );
            }

            return instance;
        }
    }

    event Action updateMethods;
    
    
    public static void RegisterUpdate(Action newMethod )
    {
        Instance.updateMethods += newMethod;
    }

    public static void RemoveUpdate(Action oldMethod)
    {
        Instance.updateMethods -= oldMethod;
    }


    private void Update()
    {
        if ( null != updateMethods )
        {
            updateMethods();
        }
    }
}
