using UnityEngine;

public class RifleScript : Gun
{
    protected override void FireBullet(float direction)
    {
        base.FireBullet(direction); // Comportamento padrão de disparo
    }
}
