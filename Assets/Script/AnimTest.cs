using UnityEngine;

public class AnimTest : MonoBehaviour
{
    [SerializeField] private bool isRunningStraight;
    [SerializeField] private bool isIdleStraight = true;
    [SerializeField] private bool isRunningUp;
    [SerializeField] private bool isIdleUp;
    [SerializeField] private Animator anim;
    [SerializeField] Animator headAnim;

    private void Start()
    {
        anim.SetBool("isRunning", false);
        headAnim.SetBool("isRunning", false);
        headAnim.SetBool("isStraight", true);
    }
    private void Update()
    {
        if (isRunningStraight)
        {
            anim.SetBool("isRunning", true);
            headAnim.SetBool("isRunning", true);
            headAnim.SetBool("isStraight", true);
        }
        if (isIdleStraight)
        {
            anim.SetBool("isRunning", false);
            headAnim.SetBool("isRunning", false);
            headAnim.SetBool("isStraight", true);
        }
        if (isRunningUp)
        {
            anim.SetBool("isRunning", true);
            headAnim.SetBool("isRunning", true);
            headAnim.SetBool("isStraight", false);
        }
        if (isIdleUp)
        {
            anim.SetBool("isRunning", false);
            headAnim.SetBool("isRunning", false);
            headAnim.SetBool("isStraight", false);
        }
    }
    private void Default()
    {
        anim.SetBool("isRunning", false);
        headAnim.SetBool("isRunning", false);
        headAnim.SetBool("isStraight", true);
    }

}
