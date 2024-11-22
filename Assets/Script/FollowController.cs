using UnityEngine;
using System.Collections;

public class FollowController : MonoBehaviour
{
    // What we are following
    public Transform target;

    Vector3 velocity = Vector3.zero;

    // Time to follow target
    public float smoothTime = 0.15f;

    // Enable and set max Y value
    public bool YMaxEnabled = false;
    public float YMaxValue = 0;

    // Enable and set min Y value
    public bool YMinEnabled = false;
    public float YMinValue = 0;

    // Enable and set max X value
    public bool XMaxEnabled = false;
    public float XMaxValue = 0;

    // Enable and set min X value
    public bool XMinEnabled = false;
    public float XMinValue = 0;

    // Fixed update is called in sync with physics
    void FixedUpdate()
    {
        // Target position
        Vector3 targetPos = target.position;

        // Vertical
        if (YMinEnabled && YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, YMaxValue);
        }
        else if (YMinEnabled)
        {
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, target.position.y);
        }
        else if (YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, YMaxValue);
        }

        // Horizontal 
        if (XMaxEnabled && XMinEnabled)
        {
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, XMaxValue);
        }
        else if (XMaxEnabled) {
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, XMaxValue);
        }
        else if (XMinEnabled)
        {
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, target.position.x);
        }

        // align camera
        targetPos.z = transform.position.z;

        // using smooth damp to follow target
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
