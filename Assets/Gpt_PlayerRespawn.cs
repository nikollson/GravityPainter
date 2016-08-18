using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawn : MonoBehaviour {

    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;
    
    public void DoRespawn()
    {
        MakeFloor();
    }

    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnFloorPosition.position, Quaternion.identity);
    }
}
