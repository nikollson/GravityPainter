// OpenDoor関数を呼び出すとドア開きます

using UnityEngine;
using System.Collections;

public class Gpt_DoorSystem : MonoBehaviour {

    public GameObject cameraObj;
    public GameObject[] doorObj = new GameObject[2];

    public MeshRenderer[] meshRenderer;
    public HitManager cameraHitManager;

    private Gpt_Player player;
    AudioSource se;

    enum State
    {
        CLOSE = 0,
        OPENING = 1,
        OPEN = 2
    }
    State state;
    const float MOVE_DOOR_VAL = -4.0f;       // ドア移動範囲
    public float ROT_SPD = 75.0f;       // ドア回転速度

    void Start()
    {
        state = State.CLOSE;
        se = this.GetComponent<AudioSource>();
        foreach (var a in GameObject.FindGameObjectsWithTag("Player"))
        {
            var b = a.GetComponent<Gpt_Player>();
            if (b != null) { player = b; break; }
        }
    }

    void Update()
    {

        //Debug.Log("AAAAA" + cameraHitManager.IsHit);
        foreach (var a in meshRenderer)
        {
            a.enabled = !cameraHitManager.IsHit;
        }

        switch (state)
        {

            case State.CLOSE:

                //

                break;

            case State.OPENING:

                //OpeningDoorTns();
                OpeningDoorRot();
                player.canControl = false;
                break;

            case State.OPEN:

                player.canControl = true;
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
        if (doorObj[1].transform.eulerAngles.y <= 150.0f)
        {
            doorObj[0].transform.eulerAngles += new Vector3(0f, -Time.deltaTime * ROT_SPD, 0f);
            doorObj[1].transform.eulerAngles += new Vector3(0f, Time.deltaTime * ROT_SPD, 0f);
        }
        else {
            cameraObj.GetComponent<Gpt_Camera>().state = 0;
            state = State.OPEN;
        }
    }

    public void OpenDoor()
    {
        state = State.OPENING;
        se.Play();
        cameraObj.GetComponent<Gpt_Camera>().state = 1;
    }
}
