using UnityEngine;
using System.Collections;

public class Gpt_BossAI : MonoBehaviour
{
    // 行動パターン
    enum State
    {
        None = 0,           // 完全に何もしない
        Search = 1,         // 探し(周ってプレイヤーを探している、リスポーン地点にいる際に使用する)
        StrikeAtk = 10,     // 攻撃
    }
    State state;

    float cnt = 0.0f;

    void Start()
    {
        state = State.Search;
    }

    void Update()
    {

        cnt += Time.deltaTime;

        // 探し状態
        if (state == State.Search)
        {
            this.transform.eulerAngles = new Vector3(0, cnt*50.0f, 0);
        }
    }
}
