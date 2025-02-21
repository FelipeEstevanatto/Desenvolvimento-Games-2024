using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] commonWeapons;
    [SerializeField] private GameObject[] rareWeapons;
    [SerializeField] private GameObject[] legendaryWeapons;
    private Vector3 spawnPoint;
    private GameObject spawnedWeapon;

    private void Start()
    {
        spawnPoint = transform.position;
        SpawnWeapon();
    }
    private void SpawnWeapon()
    {
        float chance = Random.value; 

        GameObject chosenWeapon;
        if (chance < 0.6f) 
            chosenWeapon = commonWeapons[Random.Range(0, commonWeapons.Length)];
        else if (chance < 0.9f) 
            chosenWeapon = rareWeapons[Random.Range(0, rareWeapons.Length)];
        else 
            chosenWeapon = legendaryWeapons[Random.Range(0, legendaryWeapons.Length)];

        Instantiate(chosenWeapon, spawnPoint, Quaternion.identity);
    }

}
