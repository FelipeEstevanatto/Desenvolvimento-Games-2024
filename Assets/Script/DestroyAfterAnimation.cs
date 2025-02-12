using UnityEngine;
using System.Collections;


public class DestroyAfterAnimation : MonoBehaviour
{
    // This method is called as an Animation Event at the last frame of the animation,
    // OR you can use a coroutine with the length of the animation clip.
    [SerializeField] private float animationLength = 2f; // Set this to the length of your animation clip

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(animationLength));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
