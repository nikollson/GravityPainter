using UnityEngine;
using System.Collections;

public class Gpt_Input : MonoBehaviour
{
    const float EPS = 0.001f;
    static InputGetter inputGetter = new InputGetter();

    //コントローラーに入力された情報を返すプロパティ
    public static bool Jump { get { inputGetter.Update(); return inputGetter.Jump; } }
    public static bool Attack { get { inputGetter.Update(); return inputGetter.Attack; } }
    public static bool Skill { get { inputGetter.Update(); return inputGetter.Skill; } }
    public static bool IsMoving { get { return Move.magnitude > EPS; } }
    public static Vector2 Move { get { inputGetter.Update(); return inputGetter.Move; } }
    public static Vector2 CamMove { get { inputGetter.Update(); return inputGetter.CamMove; } }
    public static bool MovePush { get { inputGetter.Update(); return inputGetter.MovePush; } }
    public static bool CameraPush { get { inputGetter.Update(); return inputGetter.CameraPush; } }
    public static bool ColorLeft { get { inputGetter.Update(); return inputGetter.ColorLeft; } }
    public static bool ColorRight { get { inputGetter.Update(); return inputGetter.ColorRight; } }
    public static bool Option { get { inputGetter.Update(); return inputGetter.Option; } }
    public static bool Detonate { get { inputGetter.Update(); return inputGetter.Detonate; } }

    public static bool HasAnyKey()
    {
        bool ret = false;
        ret |= Gpt_Input.Attack;
        ret |= Gpt_Input.Skill;
        ret |= Gpt_Input.Jump;
        ret |= Gpt_Input.ColorLeft;
        ret |= Gpt_Input.ColorRight;
        ret |= Gpt_Input.CameraPush;
        ret |= Gpt_Input.MovePush;
        return ret;
    }

    //キーが押され始めたフレームを返すプロパティ。押されてないときは0を返す
    public static int JumpStartFrame { get { inputGetter.Update(); return inputGetter.JumpStartFrame; } }
    public static int AttackStartFrame { get { inputGetter.Update(); return inputGetter.AttackStartFrame; } }
    public static int FeeverStartFrame { get { inputGetter.Update(); return inputGetter.FeeverStartFrame; } }
    public static int SkillStartFrame { get { inputGetter.Update(); return inputGetter.SkillStartFrame; } }
    public static int MovePushStartFrame { get { inputGetter.Update(); return inputGetter.MovePushStartFrame; } }
    public static int CameraPushStartFrame { get { inputGetter.Update(); return inputGetter.CameraPushStartFrame; } }
    public static int ColorLeftStartFrame { get { inputGetter.Update(); return inputGetter.ColorLeftStartFrame; } }
    public static int ColorRightStartFrame { get { inputGetter.Update(); return inputGetter.ColorRightStartFrame; } }
    public static int OptionStartFrame { get { inputGetter.Update(); return inputGetter.OptionStartFrame; } }
    public static int DetonateStartFrame { get { inputGetter.Update(); return inputGetter.DetonateStartFrame; } }

    //キーが押されたフレームだけ、Trueを返すプロパティ
    public static bool JumpDown { get { inputGetter.Update(); return inputGetter.JumpStartFrame == Time.frameCount; } }
    public static bool AttackDown { get { inputGetter.Update(); return inputGetter.AttackStartFrame == Time.frameCount; } }
    public static bool FeeverDown { get { inputGetter.Update(); return inputGetter.FeeverStartFrame == Time.frameCount; } }
    public static bool SkillDown { get { inputGetter.Update(); return inputGetter.SkillStartFrame == Time.frameCount; } }
    public static bool MovePushDown { get { inputGetter.Update(); return inputGetter.MovePushStartFrame == Time.frameCount; } }
    public static bool CameraPushDown { get { inputGetter.Update(); return inputGetter.CameraPushStartFrame == Time.frameCount; } }
    public static bool ColorLeftDown { get { inputGetter.Update(); return inputGetter.ColorLeftStartFrame == Time.frameCount; } }
    public static bool ColorRightDown { get { inputGetter.Update(); return inputGetter.ColorRightStartFrame == Time.frameCount; } }
    public static bool OptionDown { get { inputGetter.Update(); return inputGetter.OptionStartFrame == Time.frameCount; } }
    public static bool DetonateDown { get { inputGetter.Update(); return inputGetter.DetonateStartFrame == Time.frameCount; } }

    [System.Serializable]
    class InputGetter
    {
        const string jumpKey = "Jump";
        const string attackKey = "Fire1";
        const string skillKey = "Fire2";
        const string xKey = "Horizontal";
        const string yKey = "Vertical";
        const string camXKey = "CamHorizontal";
        const string camYKey = "CamVertical";
        const string movePushKey = "MovePush";
        const string cameraPushKey = "CameraPush";
        const string colorLeftKey = "L1";
        const string colorRightKey = "R1";
        const string optionKey = "Option";
        const string detonateKey = "Fire3";

        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Skill { get; private set; }
        public Vector2 Move { get; private set; }
        public Vector2 CamMove { get; private set; }
        public bool MovePush { get; private set; }
        public bool CameraPush { get; private set; }
        public bool ColorLeft { get; private set; }
        public bool ColorRight { get; private set; }
        public bool Option { get; private set; }
        public bool Detonate { get; private set; }

        public int JumpStartFrame { get; private set; }
        public int AttackStartFrame { get; private set; }
        public int FeeverStartFrame { get; private set; }
        public int SkillStartFrame { get; private set; }
        public int MovePushStartFrame { get; private set; }
        public int CameraPushStartFrame { get; private set; }
        public int ColorLeftStartFrame { get; private set; }
        public int ColorRightStartFrame { get; private set; }
        public int OptionStartFrame { get; private set; }
        public int DetonateStartFrame { get; private set; }

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
            Skill = Input.GetButton(skillKey);

            Move = new Vector2(Input.GetAxis(xKey), Input.GetAxis(yKey));
            CamMove = new Vector2(Input.GetAxis(camXKey), Input.GetAxis(camYKey));

            Move = Move.normalized;
            CamMove = CamMove.normalized;

            MovePush = Input.GetButton(movePushKey);
            CameraPush = Input.GetButton(cameraPushKey);

            ColorLeft = Input.GetButton(colorLeftKey);
            ColorRight = Input.GetButton(colorRightKey);
            Option = Input.GetButton(optionKey);
            Detonate = Input.GetButton(detonateKey);
        }


        void Update_InputFrame()
        {
            JumpStartFrame = Jump ? (JumpStartFrame == 0 ? Time.frameCount : JumpStartFrame) : 0;
            AttackStartFrame = Attack ? (AttackStartFrame == 0 ? Time.frameCount : AttackStartFrame) : 0;
            SkillStartFrame = Skill ? (SkillStartFrame == 0 ? Time.frameCount : SkillStartFrame) : 0;
            MovePushStartFrame = MovePush ? (MovePushStartFrame == 0 ? Time.frameCount : MovePushStartFrame) : 0;
            CameraPushStartFrame = CameraPush ? (CameraPushStartFrame == 0 ? Time.frameCount : CameraPushStartFrame) : 0;
            ColorLeftStartFrame = ColorLeft ? (ColorLeftStartFrame == 0 ? Time.frameCount : ColorLeftStartFrame) : 0;
            ColorRightStartFrame = ColorRight ? (ColorRightStartFrame == 0 ? Time.frameCount : ColorRightStartFrame) : 0;
            OptionStartFrame = Option ? (OptionStartFrame == 0 ? Time.frameCount : OptionStartFrame) : 0;
            DetonateStartFrame = Detonate ? (DetonateStartFrame == 0 ? Time.frameCount : DetonateStartFrame) : 0;

        }
    }
}
