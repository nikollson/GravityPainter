using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_WarpStartPoint : MonoBehaviour
{
    private Gpt_Player player;
    public Transform apperPosition;
    public Transform waitPosition;
    public MeshRenderer yukaRenderer;

    public float playerOnTiming = 2.0f;
    public float playerMoveTiming = 3.5f;
    private float time;

    bool appear = false;
    bool move = false;
    public bool canControlAfter = true;

    void Start()
    {
        player = FindObjectOfType<Gpt_Player>();
        player.playerColor.MeshRendererOff();
        time = 0f;
    }
    
    void Update()
    {
        time += Time.deltaTime;
        if (time < playerOnTiming)
        {
            player.canControl = false;
            player.transform.position = waitPosition.position;
        }
        if(!appear && time > playerOnTiming)
        {
            appear = true;
            player.transform.position = apperPosition.position;
            player.playerColor.MeshRendererOn();
        }
        if(!move && time > playerMoveTiming)
        {
            player.canControl = canControlAfter;
        }
        
    }

}
