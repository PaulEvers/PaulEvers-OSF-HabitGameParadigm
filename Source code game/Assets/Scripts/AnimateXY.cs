using DG.Tweening;
using UnityEngine;

public class AnimateXY : MonoBehaviour
{
    public bool isLocal = false;
    public bool atStart = false;
    public bool animateY = false;
    public float targetY = 5f;
    public bool animateX = false;
    public float targetX = 5f;
    public float duration = 2f; 
    public Ease easing = Ease.OutQuad; 

    [SerializeField] private RectTransform uiScoreContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!atStart)
        {
            return;
        }
        StartAnimation();
    }



    public void StartAnimation()
    {
        if (!animateX && !animateY)
        {
            return;
        }

        if (animateX && !animateY)
        {
            transform.DOMoveX(targetX, duration).SetEase(easing);
            return;
        }

        if (!animateX && animateY)
        {
            transform.DOMoveY(targetY, duration).SetEase(easing);
            return;
        }

        if (isLocal)
        {
            transform.DOLocalMoveX(targetX, duration).SetEase(easing);
            transform.DOLocalMoveY(targetY, duration).SetEase(easing);
            return;
        }

        transform.DOMoveX(targetX, duration).SetEase(easing);
        transform.DOMoveY(targetY, duration).SetEase(easing);
    }

    public void AnimateY(float y, float dur)
    {
        if (isLocal)
        {
            transform.DOLocalMoveY(y, dur).SetEase(easing);
            return;
        }
           transform.DOMoveY(y, dur).SetEase(easing);
    }

    public void AnimateX(float x, float dur)
    {
        if (isLocal)
        {
            transform.DOLocalMoveX(x, dur).SetEase(easing);
            return;
        }
        transform.DOMoveX(x, dur).SetEase(easing);
    }
}
