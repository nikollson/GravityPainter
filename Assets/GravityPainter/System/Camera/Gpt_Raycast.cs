using UnityEngine;
using System.Collections;

public class Gpt_Raycast : MonoBehaviour {

    public Material changeMaterial;
    private Material preserveMaterial;
    public Material testMaterial;
    private Renderer meshRenderer;

    private string preserveName;
    private Renderer preserveRenderer;

    private bool isTouch;
    private bool oneTouch=false;

    public Transform Camera;
    // Use this for initialization
    void Start () {

	}
	

	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            //Debug.Log("hit!:" + hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "DoorSystem")
            {

                //isTouch = false;

                //if (!oneTouch)
                //{
                //    preserveName = hit.transform.gameObject.name;
                //    preserveRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                //    preserveMaterial = preserveRenderer.material;

                //}
                //else
                //{
                //    if (preserveName != hit.transform.gameObject.name)
                //    {
                //        //preserveRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                //        //preserveMaterial = meshRenderer.material;
                //        isTouch = true;
                //    }else
                //    {
                //        preserveRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                //        preserveMaterial = meshRenderer.material;
                //    }
                //}
                //oneTouch = true;
                //meshRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                //meshRenderer.sharedMaterial = changeMaterial;
                Camera.transform.position = hit.point;
            }

            //マテリアルを基に戻す
            if (isTouch)
            {
                Debug.Log("hit!:" + hit.transform.gameObject.name);
                preserveRenderer.sharedMaterial = testMaterial;
            }
            isTouch = false;

        }

    }
}
