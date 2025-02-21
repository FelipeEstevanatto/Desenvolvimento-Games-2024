using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private AudioClip pistolClip;

    protected override void FireBullet(float direction)
    {
        AudioManager.instance.PlaySFX(pistolClip, 0.75f);

        base.FireBullet(direction); 
    }
}
