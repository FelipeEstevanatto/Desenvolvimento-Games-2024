using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string weaponName;
    
    [TextArea(3,5)]
    [SerializeField] private string description;
    protected GameObject weaponPrefab;
    public Sprite weaponIcon;


    public float attackHeightOffset = 0.0f;

    public abstract void Attack(float direction);

}
