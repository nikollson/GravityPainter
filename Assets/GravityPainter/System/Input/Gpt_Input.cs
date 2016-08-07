using UnityEngine;
using System.Collections;

public class Gpt_Input : MonoBehaviour {

    static InputGetter inputGetter = new InputGetter();

    public static bool Jump { get { inputGetter.Update(); return inputGetter.Jump; } }
    public static bool Attack { get { inputGetter.Update(); return inputGetter.Attack; } }
    public static bool Skill { get { inputGetter.Update(); return inputGetter.Skill; } }
    public static bool Feever { get { inputGetter.Update(); return inputGetter.Feever; } }
    public static Vector2 Move { get { inputGetter.Update(); return inputGetter.Move; } }
    public static Vector2 CamMove { get { inputGetter.Update(); return inputGetter.CamMove; } }

    [System.Serializable]
    class InputGetter
    {
        public string jumpKey = "Jump";
        public string attackKey = "Fire1";
        public string skillKey = "Fire2";
        public string feeverKey = "Fire3";
        public string xKey = "Horizontal";
        public string yKey = "Vertical";
        public string camXKey = "CamHorizontal";
        public string camYKey = "CamVertical";

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Feever { get; private set; }
        public bool Skill { get; private set; }
        public Vector2 Move { get; private set; }
        public Vector2 CamMove { get; private set; }

        int prevFrame = -1;

        public void Update()
        {
            if (prevFrame == Time.frameCount) return;
            prevFrame = Time.frameCount;

            Jump = Input.GetButton(jumpKey);
            Attack = Input.GetButton(attackKey);
            Feever = Input.GetButton(feeverKey);
            Skill = Input.GetButton(skillKey);

            Move = new Vector2(Input.GetAxis(xKey), Input.GetAxis(yKey));
            CamMove = new Vector2(Input.GetAxis(camXKey), Input.GetAxis(camYKey));
        }
    }

}
