using DG.Tweening;
using UnityEngine;

public class AnimateCamera : MonoBehaviour
{
    public float targetY = 24.5f;
    public float duration = 2f; 
    public float durationScoreContainer = 2f;
    public Ease easing = Ease.OutQuad;
    public Ease easingUi = Ease.OutQuad;

    [SerializeField] private Transform uiScoreContainer;

    [SerializeField] private Transform[] objectsToDelete;

    public void StartAnimation()
    {
        transform.DOMoveY(targetY, duration).SetEase(easing).OnComplete(() =>
        {
            foreach (Transform obj in objectsToDelete)
            {
                if (obj != null)
                    Destroy(obj.gameObject);
            }

            uiScoreContainer.DOLocalMoveY(58.5f, durationScoreContainer)
                    .SetEase(easingUi).OnComplete(() =>
                    {
                        GameManager.Instance.StartEngagingGame();
                    });
        });
    }
}
