using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon baseWeapon; //primary weapon 
    [SerializeField] private Weapon pickupWeapon; // secundary
    [SerializeField] private Transform weaponHolderPos;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject player;

    private Weapon currentWeapon; //weapon to be equipped
    private GameObject currentWeaponModel; //weapon that will be instantiated

    void Start()
    {
        //start with base weapon in hands
        EquipWeapon(baseWeapon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(baseWeapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pickupWeapon != null)
        {
            EquipWeapon(pickupWeapon);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            float direction = Mathf.Sign(player.transform.localScale.x);  //player faicing direction to use in Attack()
            currentWeapon.Attack(direction);
            playerAnimator.SetTrigger("Shoot");
        }
    }

    void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }

        // instantiate the weapon model ate the weapon holder's position
        if (currentWeapon != null)
        {
            currentWeaponModel = Instantiate(currentWeapon.weaponPrefab, weaponHolderPos.position, Quaternion.identity);
            currentWeaponModel.transform.SetParent(weaponHolderPos); // set the weapon's parent to the weapon holder to follow the position

            // Aajust the weapon's rotation based on the player's direction
            UpdateWeaponRotation();

            // if the weapon is of type 'Gun', initialize its fire point
            if (currentWeapon is Gun gun)
            {
                gun.firePoint = currentWeaponModel.transform.Find("FirePoint");
            }
        }
    }

    void UpdateWeaponRotation()
    {
        float direction = Mathf.Sign(player.transform.localScale.x);

        //adjust the rotation of the weapon to match the player's direction
        currentWeaponModel.transform.localRotation = Quaternion.Euler(0, direction == 1 ? 0 : 180, 0);
    }

    public void PickupWeapon(Weapon weapon)
    {
        //update the pickupWeapon slot when a new weapon is picked up
        pickupWeapon = weapon;
    }

}
