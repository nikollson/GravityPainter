// OpenDoor関数を呼び出すとドア開きます

using UnityEngine;
using System.Collections;

public class Gpt_DoorSystem : MonoBehaviour {

    public GameObject[] doorObj = new GameObject[2];

    enum State
    {
        CLOSE = 0,
        OPENING = 1,
        OPEN = 2
    }
    State state;
    const float MOVE_DOOR_VAL = -4.0f;       // ドア移動範囲
    public float ROT_SPD = 10.0f;       // ドア回転速度

    void Start () {
        state = State.OPENING;
	}
	
	void Update () {

        switch (state) {

            case State.CLOSE:

                //

                break;

            case State.OPENING:

                //OpeningDoorTns();
                OpeningDoorRot();

                break;

            case State.OPEN:

                //

                break;
        }
    }

    void OpeningDoorTns()
    {
        doorObj[0].transform.position -= new Vector3(0, 0, Time.deltaTime);
        doorObj[1].transform.position += new Vector3(0, 0, Time.deltaTime);

        if (doorObj[0].transform.position.z < MOVE_DOOR_VAL)
        {
            state = State.OPEN;
        }
    }

    void OpeningDoorRot()
    {
        if (doorObj[1].transform.eulerAngles.y<=270.0f)
        {
            doorObj[0].transform.eulerAngles += new Vector3(0f, -Time.deltaTime * ROT_SPD, 0f);
            doorObj[1].transform.eulerAngles += new Vector3(0f, Time.deltaTime * ROT_SPD, 0f);
        }
    }


    public void OpenDoor()
    {
        state = State.OPENING;
    }
}
