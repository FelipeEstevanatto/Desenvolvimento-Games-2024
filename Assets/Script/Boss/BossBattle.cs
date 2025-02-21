using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController cam;
    [SerializeField] private Transform camPosition;
    [SerializeField] private float camSpeed;
    void Start()
    {
        cam = FindAnyObjectByType<CameraController>();
        cam.enabled = false; //deactivates camera script when object set actives
    }

    void Update()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPosition.position, camSpeed * Time.deltaTime);
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
}
