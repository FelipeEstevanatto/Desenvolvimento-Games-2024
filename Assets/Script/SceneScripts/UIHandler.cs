using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Texts Objects")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI grenadesText;
    public TextMeshProUGUI ammoText;
    [SerializeField] private Slider healthBar;

    [Header("Scripts")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GrenadeManager grenadeManager;
    [SerializeField] private WeaponManager weaponManager;

    //private int score = 0;

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = player.MaxHealth;
        }
    }

 
    void Update()
    {
        //healthText.text = "Health: " + player.Health.ToString() + "/" + player.MaxHealth.ToString();
        if (healthBar != null)
        {
            healthBar.maxValue = player.MaxHealth;
            healthBar.value = player.Health;
        }
        else
        {
            Debug.Log("No Healthbar");
        }
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