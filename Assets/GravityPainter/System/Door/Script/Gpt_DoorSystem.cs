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

    void Start () {
        state = State.CLOSE;
	}
	
	void Update () {

        switch (state) {

            case State.CLOSE:

                //

                break;

            case State.OPENING:

                doorObj[0].transform.position -= new Vector3(0, 0, Time.deltaTime);
                doorObj[1].transform.position += new Vector3(0, 0, Time.deltaTime);

                if(doorObj[0].transform.position.z < MOVE_DOOR_VAL)
                {
                    state = State.OPEN;
                }

                    break;

            case State.OPEN:

                //

                break;
        }
    }

    public void OpenDoor()
    {
        state = State.OPENING;
    }
}
