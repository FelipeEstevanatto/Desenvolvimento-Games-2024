using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [Header("Texts Objects")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI grenadesText;
    public TextMeshProUGUI ammoText;

    [Header("Scripts")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GrenadeManager grenadeManager;
    [SerializeField] private WeaponManager weaponManager;

    //private int score = 0;

    void Start()
    {
        //score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.Health.ToString() + "/" + player.MaxHealth.ToString();
        grenadesText.text = grenadeManager.CurrentGrenades.ToString() + "/" + grenadeManager.MaxGrenades.ToString();
        if (weaponManager.currentWeapon is Gun gun)
        {
            if (weaponManager.currentWeapon is Pistol)
            {
                ammoText.text = "Ammo: ∞";
            }
            else
            {
                ammoText.text = "Ammo: " + gun.currentAmmo.ToString() + "/" + gun.ammoCapacity.ToString();
            }
        }
    }
}