using UnityEngine;
using System.Collections;

public class Gpt_AutoRemove : MonoBehaviour {

    public float time = 1.0f;

    float count = 0;

    void Update()
    {
        count += Time.deltaTime;
        if (count > time) Destroy(this.gameObject);
    }
}
