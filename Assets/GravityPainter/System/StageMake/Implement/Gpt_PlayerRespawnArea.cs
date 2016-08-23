using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawnArea : MonoBehaviour
{
    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;
    public GameObject playerDeleteEffect;

    public HitManager hitManager;
    public Transform respawnPosition;
    public float respawnTimte = 0.4f;

    private Gpt_Camera camera;
    private Gpt_Player player;
    private GameObject effectObject;
    bool hit = false;
    bool respawn = false;

    float count = 0;

    void Start()
    {
        camera = GameObject.FindObjectOfType<Gpt_Camera>();
    }

    void Update()
    {
        if (!hit && hitManager.IsHit)
        {
            player = Gpt_ParentTracker.Track<Gpt_Player>(hitManager.HitCollider.gameObject);
            if (player != null)
            {
                hit = true;
                respawn = false;
                count = 0;
                Gpt_FadeManager.FadeInOutBlack(null);

                effectObject = (GameObject)Instantiate(playerDeleteEffect, player.transform.position, Quaternion.identity);
                effectObject.transform.parent = player.transform;
            }
        }

        if (hit)
        {
            count += Time.deltaTime;

            if(!respawn && count > respawnTimte)
            {
                Destroy(effectObject);

                MakeFloor();
                player.DoRespawn(respawnPosition.position);
                respawn = true;
                hit = false;
            }
        }
    }


    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnFloorPosition.position, Quaternion.identity);
    }

}
