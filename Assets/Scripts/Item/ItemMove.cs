using UnityEngine;
using DG.Tweening;
public abstract class ItemMove : MonoBehaviour, IUITarget, IItemMove
{
    RectTransform _target;
    
    [HideInInspector]
    public RectTransform Target
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
            {
                SetTarget();
            }
        }
    }

    [SerializeField] float duration = 1.5f;

    Vector3 worldTarget;

    public void SetTarget()
    {
        // UI의 화면 좌표 → 월드 좌표 변환
        Vector3 screenPos = Target.position;

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
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.localScale = Vector3.one;
            });
    }
}
public interface IUITarget
{
    RectTransform Target { get; set; }
}

public interface IItemMove
{
    public void Move(Transform transform);
}