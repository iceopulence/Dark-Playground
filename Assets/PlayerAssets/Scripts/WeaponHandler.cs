using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private Animator animator;
    public GameObject weapon;

    int weaponState = 0;

    [SerializeField] Transform handT;

    GameObject weaponSpawned;

    bool holdingWeapon = false;

    [Header("Attacking Stuff")]
    public AnimationClip attackAnimation;
    bool attacking = false;

    public float attackRange = 10f;
    public float attackRadius = 10f;
    [SerializeField] LayerMask attackableLayers;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!holdingWeapon)
            {
                EquipWeapon();
            }
            else
            {
                UnEquipWeapon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && holdingWeapon)
        {
            Attack();
        }
    }

    void Attack()
    {
        attacking = true;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("Attacking");
        
        // Vector3 spherecastOrigin = transform.position + Vector3.up * originOffset;

        //check if we hit anything
        RaycastHit hit;
        // if(Physics.SphereCast(spherecastOrigin, attackSize, transform.forward, out hit, attackRange, attackableLayer))
        // {
        //     Health attackedHealth = hit.transform.GetComponent<attackedHealth>();
        //     attackedHealth.Damage(heldWeapon.damage);
        // }

        yield return new WaitForSeconds(attackAnimation.length);
        attacking = false;
    }

    void EquipWeapon()
    {
        holdingWeapon = true;
    }

    void UnEquipWeapon()
    {
        holdingWeapon = false;
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
