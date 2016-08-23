// OpenDoor関数を呼び出すとドア開きます

using UnityEngine;
using System.Collections;

public class Gpt_DoorSystem : MonoBehaviour {

    public GameObject cameraObj;
    public GameObject[] doorObj = new GameObject[2];

    public MeshRenderer[] meshRenderer;
    public HitManager cameraHitManager;
    public HitManager playerInChecker;
    public string nextSceneName;
    public float sceneLoadTime = 4.0f;
    public float closeStartTime = 3.0f;
    public float closeEndTime = 3.5f;

    public Transform sceneLoadCamera;
    public Transform sceneLoadLook;
    public Transform camOpenPos;
    public Transform camOpenLook;
    
    private Gpt_Player player;
    AudioSource se;

    float sceneLoadCount = 0;
    bool startSceneLoad = false;

    bool opened = false;

    enum State
    {
        CLOSE = 0,
        OPENING = 1,
        OPEN = 2
    }
    State state;
    const float MOVE_DOOR_VAL = -4.0f;       // ドア移動範囲
    public float ROT_SPD = 75.0f;       // ドア回転速度
    public float ROT_SPD_SECOND = 20.0f;       // ドア回転速度
    public float OPEN_MAX = 120;
    
    float rotCount = 0;

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
        }
        if (state == State.OPEN)
        {
            if (!opened && !startSceneLoad)
            {
                opened = true;
                //cameraObj.GetComponent<Gpt_Camera>().state = 0;
            }
            if (!startSceneLoad && playerInChecker.IsHit)
            {
                startSceneLoad = true;
                SetCamera_SceneLoad();
            }
            player.canControl = true;
        }

        if (startSceneLoad)
        {
            sceneLoadCount += Time.deltaTime;
            if (sceneLoadCount > closeStartTime && sceneLoadCount < closeEndTime)
            {
                float allTime = closeEndTime - closeStartTime;
                float closeTimePar = (sceneLoadCount - closeStartTime) / allTime;
                float time = OPEN_MAX / ROT_SPD;

                doorObj[0].transform.eulerAngles = new Vector3(0f, ROT_SPD_SECOND * time * (1 - closeTimePar), 0f);
                doorObj[1].transform.eulerAngles = new Vector3(0f, -ROT_SPD * time * (1 - closeTimePar), 0f);
            }
            if (sceneLoadCount > sceneLoadTime)
            {
                Gpt_SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    public void SetCamera_SceneLoad()
    {
        cameraObj.GetComponent<Gpt_Camera>().StartPositionLook(sceneLoadCamera, sceneLoadLook, 0.15f, 0.15f);
    }

    void OpeningDoorTns()
    {
        doorObj[0].transform.position += new Vector3(0, 0, Time.deltaTime);
        doorObj[1].transform.position -= new Vector3(0, 0, Time.deltaTime);

        if (doorObj[0].transform.position.z < MOVE_DOOR_VAL)
        {
            state = State.OPEN;
            player.canControl = true;
            cameraObj.GetComponent<Gpt_Camera>().state = 0;
        }
    }

    void OpeningDoorRot()
    {
        rotCount += Time.deltaTime* ROT_SPD;
        if (rotCount <= OPEN_MAX)
        {
            doorObj[0].transform.eulerAngles += new Vector3(0f, Time.deltaTime * ROT_SPD_SECOND, 0f);
            doorObj[1].transform.eulerAngles += new Vector3(0f, -Time.deltaTime * ROT_SPD, 0f);
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
        cameraObj.GetComponent<Gpt_Camera>().StartPositionLook(camOpenPos, camOpenLook);
    }
}
