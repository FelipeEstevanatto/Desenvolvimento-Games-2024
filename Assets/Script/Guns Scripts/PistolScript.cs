using UnityEngine;

public class Pistol : Gun
{
    protected override void FireBullet(float direction)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.pistolClip, 0.5f);

        base.FireBullet(direction); 
    }
}
