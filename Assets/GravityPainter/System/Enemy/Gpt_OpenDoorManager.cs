using UnityEngine;
using System.Collections;

public class Gpt_OpenDoorManager : MonoBehaviour {

    public Gpt_EnemyPhaseControl phaseControl;
    public Gpt_DoorSystem doorSystem;

    bool opended = false;

    void Update()
    {
        /*
        if (!opended && phaseControl.IsEndAllPhase())
        {
            doorSystem.OpenDoor();
            opended = true;
        }
        */
    }
}
