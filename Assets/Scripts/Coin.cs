using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Juice: Gentle vertical floating animation relative to the parent pipe
        transform.DOLocalMoveY(transform.localPosition.y + 0.4f, 0.8f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);

        // Optional Juice: Slight pulsing scale
        transform.DOScale(transform.localScale * 1.1f, 0.6f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutQuad);
    }

    public void Collect()
    {
        // Stop the floating animation
        transform.DOKill();

        // Disable collider so it can't be collected twice
        GetComponent<Collider2D>().enabled = false;

        // Juice: Pop scale up, fade out, then destroy
        transform.DOScale(transform.localScale * 1.5f, 0.2f).SetEase(Ease.OutBack);
        spriteRenderer.DOFade(0f, 0.2f).OnComplete(() => Destroy(gameObject));
    }
}