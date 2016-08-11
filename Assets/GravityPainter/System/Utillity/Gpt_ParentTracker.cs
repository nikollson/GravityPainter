using UnityEngine;
using System.Collections;

public class Gpt_ParentTracker : MonoBehaviour {

    public GameObject parentObject;
    
    public static T Track<T>(GameObject obj) where T : MonoBehaviour
    {
        T ret = null;

        var parentTracker = obj.GetComponent<Gpt_ParentTracker>();
        if (parentTracker != null) ret = parentTracker.parentObject.GetComponent<T>();
        if (ret == null) ret = obj.GetComponent<T>();

        return ret;
    }
}
