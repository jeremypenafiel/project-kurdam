
using System.Collections.Generic;

using UnityEngine;

public class ScriptableObjectDB<T> : MonoBehaviour where T : ScriptableObject
{
    static Dictionary<string, T> objects;

    public static void Init()
    {
        objects = new Dictionary<string, T>();
        var objectsArray = Resources.LoadAll<T>("");

        foreach ( T obj in objectsArray )
        {
            if (objects.ContainsKey(obj.name))
            {
                continue;
            }
            objects[obj.name] = obj;

        }
    }

    public static T GetObjectByName( string name)
    {
        if ( !objects.ContainsKey(name))
        {
            return null;
        }
        
        return objects[name];
        
    }
    
}
