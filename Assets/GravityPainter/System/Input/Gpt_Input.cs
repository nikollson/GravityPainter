using UnityEngine;
using System.Collections;

public class Gpt_Input : MonoBehaviour {

    static InputGetter inputGetter = new InputGetter();

    //コントローラーに入力された情報を返すプロパティ
    public static bool Jump { get { inputGetter.Update(); return inputGetter.Jump; } }
    public static bool Attack { get { inputGetter.Update(); return inputGetter.Attack; } }
    public static bool Skill { get { inputGetter.Update(); return inputGetter.Skill; } }
    public static bool Feever { get { inputGetter.Update(); return inputGetter.Feever; } }
    public static Vector2 Move { get { inputGetter.Update(); return inputGetter.Move; } }
    public static Vector2 CamMove { get { inputGetter.Update(); return inputGetter.CamMove; } }
    public static bool MovePush { get { inputGetter.Update(); return inputGetter.MovePush; } }
    public static bool CameraPush { get { inputGetter.Update(); return inputGetter.CameraPush; } }

    //キーが押され始めたフレームを返すプロパティ。押されてないときは0を返す
    public static int JumpStartFrame { get { inputGetter.Update(); return inputGetter.JumpStartFrame; } }
    public static int AttackStartFrame { get { inputGetter.Update(); return inputGetter.AttackStartFrame; } }
    public static int FeeverStartFrame { get { inputGetter.Update(); return inputGetter.FeeverStartFrame; } }
    public static int SkillStartFrame { get { inputGetter.Update(); return inputGetter.SkillStartFrame; } }
    public static int MovePushStartFrame { get { inputGetter.Update(); return inputGetter.MovePushStartFrame; } }
    public static int CameraPushStartFrame { get { inputGetter.Update(); return inputGetter.CameraPushStartFrame; } }

    //キーが押されたフレームだけ、Trueを返すプロパティ
    public static bool JumpDown { get { inputGetter.Update(); return inputGetter.JumpStartFrame == Time.frameCount; } }
    public static bool AttackDown { get { inputGetter.Update(); return inputGetter.AttackStartFrame == Time.frameCount; } }
    public static bool FeeverDown { get { inputGetter.Update(); return inputGetter.FeeverStartFrame == Time.frameCount; } }
    public static bool SkillDown { get { inputGetter.Update(); return inputGetter.SkillStartFrame== Time.frameCount; } }
    public static bool MovePushDown { get { inputGetter.Update(); return inputGetter.MovePushStartFrame == Time.frameCount; } }
    public static bool CameraPushDown { get { inputGetter.Update(); return inputGetter.CameraPushStartFrame == Time.frameCount; } }

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
        public string movePushKey = "MovePush";
        public string cameraPushKey = "CameraPush";

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Feever { get; private set; }
        public bool Skill { get; private set; }
        public Vector2 Move { get; private set; }
        public Vector2 CamMove { get; private set; }
        public bool MovePush { get; private set; }
        public bool CameraPush { get; private set; }

        public int JumpStartFrame { get; private set; }
        public int AttackStartFrame { get; private set; }
        public int FeeverStartFrame { get; private set; }
        public int SkillStartFrame { get; private set; }
        public int MovePushStartFrame { get; private set; }
        public int CameraPushStartFrame { get; private set; }

        int prevFrame = -1;
        
        public void Update()
        {
            if (prevFrame == Time.frameCount) return;
            prevFrame = Time.frameCount;

            Update_Input();
            Update_InputFrame();   
        }

        void Update_Input()
        {
            Jump = Input.GetButton(jumpKey);
            Attack = Input.GetButton(attackKey);
            Feever = Input.GetButton(feeverKey);
            Skill = Input.GetButton(skillKey);

            Move = new Vector2(Input.GetAxis(xKey), Input.GetAxis(yKey));
            CamMove = new Vector2(Input.GetAxis(camXKey), Input.GetAxis(camYKey));

            MovePush = Input.GetButton(movePushKey);
            CameraPush = Input.GetButton(cameraPushKey);
        }


        void Update_InputFrame()
        {
            JumpStartFrame = Jump ? (JumpStartFrame == 0 ? Time.frameCount : JumpStartFrame) : 0;
            AttackStartFrame = Attack ? (AttackStartFrame == 0 ? Time.frameCount : AttackStartFrame) : 0;
            FeeverStartFrame = Feever ? (FeeverStartFrame == 0 ? Time.frameCount : FeeverStartFrame) : 0;
            SkillStartFrame = Skill ? (SkillStartFrame == 0 ? Time.frameCount : SkillStartFrame) : 0;
            MovePushStartFrame = MovePush ? (MovePushStartFrame == 0 ? Time.frameCount : MovePushStartFrame) : 0;
            CameraPushStartFrame = CameraPush ? (CameraPushStartFrame == 0 ? Time.frameCount : CameraPushStartFrame) : 0;
        }
    }

}
