using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameObject weapon;

    int weaponState = 0;

    [SerializeField] Transform handT;

    GameObject weaponSpawned;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon();
        }
    }

    void EquipWeapon()
    {

    }

    void SpawnWeapon()
    {
        if (weaponSpawned == null)
        {
            weaponState = 1;
            weaponSpawned = Instantiate(weapon, handT.position, handT.rotation * weapon.transform.rotation);
            weaponSpawned.transform.parent = handT;
        }

    }

    void DeSpawnWeapon()
    {
        Destroy(weaponSpawned);
    }
}
