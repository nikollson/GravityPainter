using UnityEngine;
using System.Collections;

public class Gpt_YukaBox : MonoBehaviour {

    public Renderer renderer;
    public new Collider collider;
    
    public int HP { get; private set; }
    public Gpt_InkColor Color { get; private set; }

    private Gpt_YukaParts yukaParts;
    private Gpt_YukaManager.TileSetting tileSetting;

    bool isExploding = false;
    float explodeCount = 0;
    float flushTiming = 0;
    float explodeTiming = 0;
    bool isFlushed = false;

    bool isFalling = false;
    float fallCount = 0;
    float reverseTime = 0;
    float reverseEndTime = 0;
    float fallStartY = 0;
    bool isReversing = false;
    
    public void SetColor(Gpt_InkColor color)
    {
        Color = color;
        MaterialUpdate();
    }

    public void AddDamage(int value)
    {
        HP = Mathf.Max(0, HP - value);
        if (HP == 0) StartFall();
        if (HP != 0) MaterialUpdate();
    }

    void MaterialUpdate()
    {
        renderer.sharedMaterial = tileSetting.GetMaterial(Color, HP, false);
    }

    void StartFall()
    {
        fallStartY = this.transform.position.y;
        renderer.enabled = false;
        collider.enabled = false;
        this.transform.position += new Vector3(0, -tileSetting.fallDistance, 0);
        isFalling = true;
        isReversing = false;
    }

    void EndFall1()
    {
        renderer.enabled = true;
        collider.enabled = true;
        HP = tileSetting.StartHP;
        yukaParts.UpdateColor(Time.frameCount);
        SetColor(yukaParts.GetCurrentColor());
        MaterialUpdate();
    }

    void EndFall2()
    {
        this.transform.position = new Vector3(this.transform.position.x, fallStartY, this.transform.position.z);
        fallCount = 0;
    }
    
    public bool CanSetExplode()
    {
        return !isExploding && !isReversing;
    }
    public void SetExplode(float flushTiming, float explodeTiming, float reverseTiming, float reverseEndTiming)
    {
        explodeCount = 0;
        this.explodeTiming = explodeTiming;
        this.flushTiming = flushTiming;
        this.reverseTime = reverseTiming;
        this.reverseEndTime = reverseEndTiming;
        isExploding = true;
        isFlushed = false;
    }

    void Update()
    {
        if (isExploding)
        {
            explodeCount += Time.deltaTime;
            if(!isFlushed && explodeCount > flushTiming)
            {
                isFlushed = true;
                renderer.sharedMaterial = tileSetting.GetMaterial(Color, HP, true);
            }
            if(explodeCount > explodeTiming)
            {
                isExploding = false;
                isFlushed = false;
                explodeCount = 0;
                AddDamage(1);
            }
        }

        if (isFalling)
        {
            fallCount += Time.deltaTime;
            if(!isReversing && fallCount > reverseTime)
            {
                EndFall1();
                isReversing = true;
            }

            if (fallCount > reverseTime)
            {
                Vector3 pos = this.transform.position;
                float restTimeMax = reverseEndTime - reverseTime;
                float restTime = fallCount - reverseTime;
                pos.y = pos.y + (fallStartY - pos.y) * (restTime / restTimeMax);
                this.transform.position = pos;

                if (fallCount > reverseEndTime)
                {
                    EndFall2();
                    isFalling = false;
                    isReversing = false;
                }
            }

        }
    }
    


    public void Setting(Gpt_YukaManager.TileSetting tileSetting, Gpt_YukaParts yukaParts, Gpt_InkColor color)
    {
        this.yukaParts = yukaParts;
        this.tileSetting = tileSetting;
        HP = tileSetting.StartHP;
        SetColor(color);
    }

}
