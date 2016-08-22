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


    public enum Mode { NONE, WHITE, BLACK }

    public GameObject fadeInWhite;
    public GameObject fadeOutWhite;
    public GameObject fadeInBlack;
    public GameObject fadeOutBlack;
    public GameObject fadeInOutBlack;

    public Mode defaultMode;

    public static void SetFade_White(Action func)
    {
        sceneLoadFunc = func;
        settedMode = Mode.WHITE;
        MakeFade(_this.fadeInWhite);
    }

    public static void SetFade_Black(Action func)
    {
        sceneLoadFunc = func;
        settedMode = Mode.BLACK;
        MakeFade(_this.fadeInBlack);
    }

    public static void FadeInOutBlack(Action func)
    {
        sceneLoadFunc = func;
        MakeFade(_this.fadeInOutBlack);
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
            if (state.normalizedTime >= 1.0 && sceneLoadFunc != null)
            {
                sceneLoadFunc();
                sceneLoadFunc = null;
            }
        }
    }

    void MakeFadeOut()
    {
        Mode nextMode = settedMode != Mode.NONE ? settedMode : defaultMode;
        if (nextMode == Mode.WHITE) MakeFade(fadeOutWhite);
        if (nextMode == Mode.BLACK) MakeFade(fadeOutBlack);
    }

    public static void MakeFade(GameObject prefab)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        obj.transform.SetParent(_this.transform, false);
        animator = obj.GetComponent<Animator>();
    }
}
