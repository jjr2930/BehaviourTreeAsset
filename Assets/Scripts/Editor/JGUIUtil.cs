using UnityEngine;
using System.Collections;
using UnityEditor;

public static class JGUIUtil 
{
    /// <summary>
    /// get guilayout option, 
    /// this will return fixed rect size
    /// </summary>
    /// <param name="width">rect's width</param>
    /// <param name="height">rect's height</param>
    /// <returns></returns>
    public static GUILayoutOption[] FixRectSIze( float width, float height )
    {
        GUILayoutOption[] layouts = new GUILayoutOption[4];
        layouts[0] = GUILayout.MinWidth( width );
        layouts[1] = GUILayout.MaxWidth( width );
        layouts[2] = GUILayout.MinHeight( height);
        layouts[3] = GUILayout.MaxWidth( height);

        return layouts;
    }
}
