using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon baseWeaponPrefab; // primary weapon prefab
    [SerializeField] private Weapon pickupWeaponPrefab; // secundary weapon prefab
    [SerializeField] private Transform weaponHolderPos;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject player;

    private Weapon currentWeapon; // equipped weapon
    private Weapon baseWeaponInstance; 
    private Weapon pickupWeaponInstance; 
    private bool isFiring = false;

    void Start()
    {
        //POR ALGUM MOTIVO O INSTANTIATE EST� GERANDO UMA INSTANCIA MUITO MAIOR DO QUE O PREFAB, MESMO COM localScale = (1, 1, 1)
        // instantiate primary weapon 
        if (baseWeaponPrefab != null)
        {
            baseWeaponInstance = Instantiate(baseWeaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
            // correct the instance's transform to the default position 
            baseWeaponInstance.transform.localPosition = Vector3.zero; 
            baseWeaponInstance.transform.localRotation = Quaternion.identity; 
            baseWeaponInstance.transform.localScale = Vector3.one; 
            currentWeapon = baseWeaponInstance;
            EquipWeapon(currentWeapon);
        }

        // instantiate secundary weapon if in inventory and disable the instance initially
        if (pickupWeaponPrefab != null)
        {
            pickupWeaponInstance = Instantiate(pickupWeaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
            pickupWeaponInstance.transform.localPosition = Vector3.zero; 
            pickupWeaponInstance.transform.localRotation = Quaternion.identity; 
            pickupWeaponInstance.transform.localScale = Vector3.one; 
            pickupWeaponInstance.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Esses controles talvez possam ser passados para o PlayerController
        // switch weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(baseWeaponInstance);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pickupWeaponInstance != null)
        {
            EquipWeapon(pickupWeaponInstance);
        }

        if (Input.GetButtonDown("Fire1") && !isFiring)
        {
            isFiring = true;
            float direction = Mathf.Sign(player.transform.localScale.x); // player direction to be used for bullets
            currentWeapon.Attack(direction);
            
            playerAnimator.SetTrigger("Shoot"); //Falta l�gica para nao atirar sem muni��o
        }
        if (Input.GetButtonUp("Fire1")) //one shot per click
        {
            isFiring = false;
        }
    }

    void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false); // disable current weapon
        }

        currentWeapon = weapon;

        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true); // enable new weapon

            // if weapon is a Gun type, find the FirePoint transform and assign it to the Gun.firePoint variable
            if (currentWeapon is Gun currentGun)
            {
                Transform firePointTransform = currentWeapon.transform.Find("FirePoint");
                if (firePointTransform != null)
                {
                    currentGun.firePoint = firePointTransform;
                }
                else
                {
                    Debug.LogWarning("FirePoint n�o encontrado na arma: " + currentGun.name);
                }
            }
        }
    }

    public void PickupWeapon(Weapon weaponPrefab)
    {
        if (pickupWeaponInstance != null)
        {
            Destroy(pickupWeaponInstance.gameObject);
        }

        // instantiate the new weapon in player's hand
        pickupWeaponInstance = Instantiate(weaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
        pickupWeaponInstance.transform.localPosition = Vector3.zero; 
        pickupWeaponInstance.transform.localRotation = Quaternion.identity; 
        pickupWeaponInstance.transform.localScale = Vector3.one; 
        pickupWeaponInstance.gameObject.SetActive(false); // initially disabled
    }
}
