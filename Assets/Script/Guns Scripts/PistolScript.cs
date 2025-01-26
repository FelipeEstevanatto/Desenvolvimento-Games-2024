using UnityEngine;

public class Pistol : Gun
{
    protected override void FireBullet(float direction)
    {
        base.FireBullet(direction); // Comportamento padrão de disparo
    }
}
