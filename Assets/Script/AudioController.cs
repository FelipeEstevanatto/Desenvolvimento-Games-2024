using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private bool isMuted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            isMuted = !isMuted;
            // AudioListener.pause = isMuted;
            AudioListener.volume = isMuted ? 0 : 1;
        }

    }
}
