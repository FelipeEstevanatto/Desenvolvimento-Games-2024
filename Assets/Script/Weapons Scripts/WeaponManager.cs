using UnityEngine;
using UnityEngine.EventSystems;

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
        // instantiate primary weapon 
        if (baseWeaponPrefab != null)
        {
            baseWeaponInstance = Instantiate(baseWeaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
            // correct the instance's transform to the default position 
            baseWeaponInstance.transform.localPosition = Vector3.zero; 
            baseWeaponInstance.transform.localRotation = Quaternion.identity; 
            baseWeaponInstance.transform.localScale /= Mathf.Abs(player.transform.localScale.x); // PLAYER COM SCALE (10,10,10) ENTÃO O PREFAB INSTANCIADO SERÁ DE SCALE 10, POIS HERDARÁ O SCALE DO PAI
                                                                                                   // CASO O NOVO SPRITE FOR DE SCALE 1, REMOVER /= player.transform.localScale.x
            currentWeapon = baseWeaponInstance;
            EquipWeapon(currentWeapon);
        }

        // if in inventory, instantiate secundary weapon and disable the instance initially
        if (pickupWeaponPrefab != null)
        {
            pickupWeaponInstance = Instantiate(pickupWeaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
            pickupWeaponInstance.transform.localPosition = Vector3.zero; 
            pickupWeaponInstance.transform.localRotation = Quaternion.identity; 
            pickupWeaponInstance.transform.localScale /= Mathf.Abs(player.transform.localScale.x); 
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

        // If the mouse is over a UI element, return
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // immediately remove the weapon from the player's hand when its ammo is <= 0
        CheckAmmo();

        if (Input.GetButtonDown("Fire1"))
        {
            //
            if (currentWeapon is Gun gun)
            {
                if(gun.currentAmmo > 0 && !isFiring && Time.time >= gun.nextFireTime)
                {
                    isFiring = true;
                    float direction = Mathf.Sign(player.transform.localScale.x); // player direction to be used for bullets
                    currentWeapon.Attack(direction);
            
                    playerAnimator.SetTrigger("Shoot");
                    gun.nextFireTime = Time.time + gun.fireRate;

                } 
            }
        }
        if (Input.GetButtonUp("Fire1")) //one shot per click (TALVEZ SEJA REDUNDANTE COM O FIRERATE)
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
                    Debug.LogWarning("FirePoint não encontrado na arma: " + currentGun.name);
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
        pickupWeaponInstance.transform.localScale /= Mathf.Abs(player.transform.localScale.x); 
        pickupWeaponInstance.gameObject.SetActive(false); // initially disabled
    }

    private void CheckAmmo()
    {
        if (currentWeapon is Gun gun && gun.currentAmmo <= 0)
        {
            if (gun == pickupWeaponInstance) //base gun will always have infinity ammo
            {
                DestroyPickupWeapon();
            }
        }
    }
    private void DestroyPickupWeapon() //Incluir a animação (ou o que quer que seja) da ação de remover a arma da mão
    {
        if (pickupWeaponInstance != null)
        {
            Destroy(pickupWeaponInstance.gameObject);
            pickupWeaponInstance = null;
        }
    }

}


