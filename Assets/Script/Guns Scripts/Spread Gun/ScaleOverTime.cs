using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float minScale, maxScale;

    Vector2 scale;
    void Start()
    {
        scale = transform.localScale;
    }

    void Update()
    {
        scale = new Vector2(Mathf.Clamp(scale.x += scaleSpeed * Time.deltaTime, minScale, maxScale), scale.y);
        scale = new Vector2(scale.x, Mathf.Clamp(scale.y += scaleSpeed * Time.deltaTime, minScale, maxScale));

        transform.localScale = scale;
    }
}
