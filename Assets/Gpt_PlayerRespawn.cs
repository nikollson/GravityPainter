using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawn : MonoBehaviour {

    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;

    Vector3 respawnPosition;
    
    void Start()
    {
        respawnPosition = respawnFloorPosition.position;
    }

    public void DoRespawn()
    {
        MakeFloor();
    }

    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnPosition, Quaternion.identity);
    }
}
