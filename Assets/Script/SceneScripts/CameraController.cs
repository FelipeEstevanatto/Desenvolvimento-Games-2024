using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private BoxCollider2D boundsBox;
    [SerializeField] private float smoothTime;

    private float halfHeight, halfWidth;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    void Update()
    {
        if (player)
        {
            //targetPosition with bounds restriction 
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(player.transform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
                transform.position.z 
            );

           //smoothing camera 
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
