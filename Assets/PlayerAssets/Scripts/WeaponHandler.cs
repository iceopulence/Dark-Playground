using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameObject weapon;
    
    int weaponState = 0;

    [SerializeField] Transform handT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon();
        }
    }

    void EquipWeapon()
    {
        weaponState = 1;
        Transform weaponT = Instantiate(weapon, handT.position, handT.rotation * weapon.transform.rotation).transform;
        weaponT.parent = handT;
    }
}
