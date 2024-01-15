using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public static SplashScreen S;
    [SerializeField] private float _duration = 1f;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (S == null)
            S = this;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Hide(Action onComplete)
    {
        Debug.Log("1");
        _canvasGroup.DOFade(0, _duration).OnComplete( () => { _canvasGroup.blocksRaycasts = false; onComplete.Invoke(); });
    }
}
