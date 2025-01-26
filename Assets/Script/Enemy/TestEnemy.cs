using UnityEngine;

public class TestEnemy : Enemy
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
