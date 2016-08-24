using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Gpt_FadeManager : MonoBehaviour
{
    public static Mode settedMode;
    private static Gpt_FadeManager _this;
    private static Action sceneLoadFunc;
    private static Animator animator;
    private static bool setting = false;

    public enum Mode { NONE, WHITE, BLACK }

    public GameObject fadeInWhite;
    public GameObject fadeOutWhite;
    public GameObject fadeInBlack;
    public GameObject fadeOutBlack;
    public GameObject fadeInOutBlack;

    public Mode defaultMode;

    public static void SetFade_White(Action func, bool powerful = false)
    {
        if (!CanStartFade() && !powerful) return;
        settedMode = Mode.WHITE;
        MakeFade(_this.fadeInWhite, func);
    }

    public static void SetFade_Black(Action func, bool powerful = false)
    {
        if (!CanStartFade() && !powerful) return;
        settedMode = Mode.BLACK;
        MakeFade(_this.fadeInBlack, func);
    }

    public static void FadeInOutBlack(Action func, bool powerful = false)
    {
        if (!CanStartFade() && !powerful) return;
        MakeFade(_this.fadeInOutBlack, func);
    }

    void Awake()
    {
        _this = this;
    }

    void Start()
    {
        MakeFadeOut();
    }

    void Update()
    {
        if (animator != null)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime >= 1.0f)
            {
                var copy = sceneLoadFunc;
                sceneLoadFunc = null;
                animator = null;
                setting = false;
                if (copy != null)
                {
                    copy();
                }
            }
        }
    }

    static public bool CanStartFade()
    {
        return !setting;
    }

    void MakeFadeOut()
    {
        Mode nextMode = settedMode != Mode.NONE ? settedMode : defaultMode;
        if (nextMode == Mode.WHITE) MakeFade(fadeOutWhite, null);
        if (nextMode == Mode.BLACK) MakeFade(fadeOutBlack, null);
    }

    public static void MakeFade(GameObject prefab, Action func)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        obj.transform.SetParent(_this.transform, false);
        animator = obj.GetComponent<Animator>();
        sceneLoadFunc = func;
        setting = true;
    }
}
