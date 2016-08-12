using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_Exploder : MonoBehaviour {

    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    private GameObject ManegerObject;

    // Use this for initialization
    void Start() {
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddExplodeList(this);
    }
	// Update is called once per frame
	void Update () {
	

	}

    void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GravityZone")
        {
            Debug.Log("aa");
            List<Vector3> Center = new List<Vector3>();
            Center.Add(this.gameObject.transform.position);

            for (int aIndex = 0; aIndex < collision.contacts.Length; ++aIndex)
            {
                Center.Add(collision.contacts[aIndex].point);
            }

            //暫定的に一番z座標が高いものを残して残りは削除
            int z_max_index = -1;
            float z_max_position = this.gameObject.transform.position.z;
            for (int i = 0; i < Center.Count; i++)
            {
                if (z_max_position < Center[i].z)
                {
                    z_max_index = i;
                    z_max_position = Center[i].z;
                }
            }

            //z座標が一番高いもの以外を削除
            for (int i = 0; i < Center.Count; i++)
            {
                if (z_max_index != i)
                {
                    Object.Destroy(collision.gameObject);
                }
            }

            if(z_max_index!=-1)
            {
                Object.Destroy(this.gameObject);
            }
        }
    }


    //重心位置計算
    public Vector3 CalculatePosition(List<Vector3> position)
    {
        Vector3 CenterPositon=new Vector3(0,0,0);
        for (int i=0;i<position.Count;i++)
        {
            CenterPositon += position[i];
        }

        CenterPositon = CenterPositon / position.Count;

        return CenterPositon;
    }


    public void SetPosition(Vector3 position)
    {
        this.gameObject.transform.position = position;
    }

    public void SetDestroy()
    {
        Object.Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EnemyGravityManeger.RemoveExplodeList(this);
    }
}
