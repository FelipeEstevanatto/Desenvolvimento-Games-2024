using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string weaponName;
    
    [TextArea(3,5)]
    [SerializeField] private string description;
    public GameObject weaponPrefab;

    public float attackHeightOffset = 0.0f;

    public abstract void Attack(float direction);

}
