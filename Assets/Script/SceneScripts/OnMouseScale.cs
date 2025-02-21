using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseScale : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalScale = transform.localScale;
        Debug.Log("OnMouseScale script started. Original scale: " + originalScale);
        // Check if the GameObject has a collider
        if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
        {
            Debug.LogError("No Collider or Collider2D component found on " + gameObject.name);
        }
    }

    public void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        // activate the hover sprite

        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleHoverSprite(originalScale * 1.25f, 0.2f)); // Scale up to 1.25x of the original scale
    }

    public void OnMouseExit()
    {
        if (scaleCoroutine != null)
        {

            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleHoverSprite(originalScale, 0.2f)); // Scale back to original
    }

    private IEnumerator ScaleHoverSprite(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}