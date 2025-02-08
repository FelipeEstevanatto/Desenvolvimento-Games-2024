using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Components")]
    [SerializeField] private Weapon baseWeaponPrefab; // primary weapon prefab
    [SerializeField] private Weapon pickupWeaponPrefab; // secundary weapon prefab
    [SerializeField] private Transform weaponHolderPos;

    [Header("Player Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;

    [Header("UI Components")]
    [SerializeField] private Image weaponSlot1;
    [SerializeField] private Image weaponSlot2;

    public Weapon currentWeapon; // equipped weapon
    [HideInInspector] public Weapon baseWeaponInstance;
    [HideInInspector] public Weapon pickupWeaponInstance; 
    private bool isFiring = false;
    private PlayerController playerController;

    void Start()
    {
        // Get the PlayerController component
        playerController = player.GetComponent<PlayerController>();

        InstantiateWeapons();
    }

    void Update()
    {
        HandleWeaponSwitch();
        HandleWeaponFire();
        HandleWeaponInCrouch();
    }

    private void InstantiateWeapons()
    {
        if (baseWeaponPrefab != null)
        {
            baseWeaponInstance = InstantiateWeapon(baseWeaponPrefab);
            EquipWeapon(baseWeaponInstance);
        }

        if (pickupWeaponPrefab != null)
        {
            pickupWeaponInstance = InstantiateWeapon(pickupWeaponPrefab);
            pickupWeaponInstance.gameObject.SetActive(false);
        }
    }

    private Weapon InstantiateWeapon(Weapon weaponPrefab)
    {
        Weapon weaponInstance = Instantiate(weaponPrefab, weaponHolderPos.position, weaponHolderPos.rotation, weaponHolderPos);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;
        weaponInstance.transform.localScale /= Mathf.Abs(player.transform.localScale.x);
        return weaponInstance;
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(baseWeaponInstance);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pickupWeaponInstance != null)
        {
            EquipWeapon(pickupWeaponInstance);
        }
    }

    private void HandleWeaponFire()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        CheckAmmo();

        if (Input.GetButtonDown("Fire1"))
        {
            TryFireWeapon();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }
    }

    private void HandleWeaponInCrouch()
    {
        if (playerController.IsCrouching)
        {
            if (currentWeapon != null)
            {
                Vector3 newPosition = weaponHolderPos.localPosition;
                newPosition.y = -0.05f;  
                weaponHolderPos.localPosition = newPosition;
            }
        }
        else
        {
            //resets weapon holder position
            if (currentWeapon != null)
            {
                Vector3 newPosition = weaponHolderPos.localPosition;
                newPosition.y = 0.0f; 
                weaponHolderPos.localPosition = newPosition;
            }
        }
    }

    private void TryFireWeapon()
    {
        if (currentWeapon is Gun gun && gun.currentAmmo > 0 && !isFiring && Time.time >= gun.nextFireTime)
        {
            isFiring = true;
            float direction = Mathf.Sign(player.transform.localScale.x);
            currentWeapon.Attack(direction);
            playerAnimator.SetTrigger("Shoot");
            gun.nextFireTime = Time.time + gun.fireRate;
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false); // disable current weapon
        }

        currentWeapon = weapon;

        if (currentWeapon != null)
        {
            if (!currentWeapon.gameObject.activeSelf)
            {
                currentWeapon.gameObject.SetActive(true); // enable new weapon
            }
        }
        UpdateWeaponSlot();
    }

    public void PickupWeapon(Weapon weaponPrefab)
    {
        if (pickupWeaponInstance != null)
        {
            Destroy(pickupWeaponInstance.gameObject);
        }

        pickupWeaponInstance = InstantiateWeapon(weaponPrefab);
        pickupWeaponInstance.gameObject.SetActive(false);
        UpdateWeaponSlot();
    }

    private void CheckAmmo()
    {
        if (currentWeapon is Gun gun && gun.currentAmmo <= 0)
        {
            if (gun == pickupWeaponInstance) //base gun will always have infinity ammo
            {
                RemovePickupWeapon();
                EquipWeapon(baseWeaponInstance); // Switch back to the default weapon
            }
        }
    }
    private void RemovePickupWeapon() //Incluir a anima��o (ou o que quer que seja) da a��o de remover a arma da m�o
    {
        if (pickupWeaponInstance != null)
        {
            Destroy(pickupWeaponInstance.gameObject);
            pickupWeaponInstance = null;
            weaponSlot2.sprite = null;
            weaponSlot2.enabled = false;
        }
    }

    private void UpdateWeaponSlot()
    {
        if (currentWeapon == baseWeaponInstance)
        {
            weaponSlot1.sprite = baseWeaponInstance.weaponIcon;
            weaponSlot1.enabled = true;

            if (pickupWeaponInstance != null)
            {
                weaponSlot2.sprite = pickupWeaponInstance.weaponIcon;
                weaponSlot2.enabled = true;
            }
            else
            {
                weaponSlot2.sprite = null;
                weaponSlot2.enabled = false;
            }
        }
        else if (currentWeapon == pickupWeaponInstance)
        {
            weaponSlot1.sprite = pickupWeaponInstance.weaponIcon;
            weaponSlot1.enabled = true;

            weaponSlot2.sprite = baseWeaponInstance.weaponIcon;
            weaponSlot2.enabled = true;
        }
    }

}




