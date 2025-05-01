using UnityEngine;
using DG.Tweening;
public abstract class ItemMove : MonoBehaviour, IUITarget,IItemMove
{
    [HideInInspector]
    public RectTransform Target{get;set;}
    [SerializeField] float duration = 1.5f;

    Vector3 screenPos;
    Vector3 worldTarget;

    void Start()
    {
         // UI의 화면 좌표 → 월드 좌표 변환
        screenPos = Target.position;
        worldTarget = Camera.main.ScreenToWorldPoint(screenPos);
    }

    public virtual void Move(Transform transform)
    {
       

        worldTarget.z = 0;
        
       // 코인 이동
        transform.DOMove(worldTarget, duration)
            .SetEase(Ease.InOutQuad);
        transform.DOScale(Vector3.zero, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                gameObject.SetActive(false);
                transform.localScale = Vector3.one;
            });
    }
}
public interface IUITarget
{
    RectTransform Target { get;set;}
}

public interface IItemMove
{
    public void Move(Transform transform);
}