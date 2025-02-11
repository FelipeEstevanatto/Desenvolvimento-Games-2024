using UnityEngine;

public class Rifle : Gun
{
    protected override void FireBullet(float direction)
    {
        base.FireBullet(direction);

        AudioManager.instance.PlaySFX(AudioManager.instance.rifleSingleClip);
    }
}
