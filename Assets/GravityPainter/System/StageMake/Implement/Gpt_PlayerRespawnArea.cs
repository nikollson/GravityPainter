using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawnArea : MonoBehaviour
{
    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;
    public GameObject playerDeleteEffect;
    public GameObject deadMarkerPrefab;
    public Vector3 deadMarkerZure = new Vector3(0, 1, 0);

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

                var obj = (GameObject)Instantiate(deadMarkerPrefab, player.transform.position + deadMarkerZure, Quaternion.identity);
                camera.StartPositionLook(camera.transform, obj.transform);

                if (playerDeleteEffect != null)
                {
                    effectObject = (GameObject)Instantiate(playerDeleteEffect, player.transform.position, Quaternion.identity);
                    effectObject.transform.parent = player.transform;
                }
            }
        }

        if (hit)
        {
            count += Time.deltaTime;

            if (!respawn && count > respawnTimte)
            {
                respawn = true;
                if (effectObject != null) Destroy(effectObject);
                
                MakeFloor();
                player.DoRespawn(respawnPosition.position);
                if (!player.state.IsDead())
                {
                    camera.state = 0;
                }
                hit = false;
            }
        }
    }


    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnFloorPosition.position, Quaternion.identity);
    }

}
