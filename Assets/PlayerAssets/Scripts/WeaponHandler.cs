using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private Animator animator;
    public GameObject weapon;
    public Weapon heldWeapon;

    int weaponState = 0;

    [SerializeField] Transform handT;

    GameObject weaponSpawned;

    bool holdingWeapon = false;

    [Header("Attacking Stuff")]
    bool attacking = false;

    public float attackRange = 10f;
    public Vector3 attackSize = Vector3.one;
    [SerializeField] float attackOrigin = 2f;
    [SerializeField] LayerMask attackableLayers;

    Transform camT;

    void Awake()
    {
        animator = GetComponent<Animator>();
        camT = Camera.main.transform;
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
            StartAttack();
        }
    }

    void StartAttack()
    {
        animator.SetTrigger("Attacking");
    }

    public void Attack()
    {
        attacking = true;
        // StartCoroutine(AttackCoroutine());

        Collider[] hitColliders = Physics.OverlapBox(transform.position + transform.forward * (attackRange * 0.5f), attackSize * 0.5f, transform.rotation, attackableLayers);


        foreach (Collider col in hitColliders)
        {
            print(col.transform.gameObject);
            Health attackedHealth = col.transform.GetComponent<Health>();
            if(attackedHealth != null)
            {
                attackedHealth.Damage(10);
            }
        }
    }

    public void EndAttack()
    {
        attacking = false;
    }

    // IEnumerator AttackCoroutine()
    // {
    //     Vector3 spherecastOrigin = transform.position + Vector3.up * attackOrigin;

    //     List<Health> hitHealths = new List<Health>();

    //     //check if we hit anything

    //     while(attacking)
    //     {
    //         print("attacking");
    //         RaycastHit hit;
    //         if (Physics.SphereCast(spherecastOrigin, attackSize, camT.forward, out hit, attackRange, attackableLayers))
    //         {
    //             print(hit.transform.gameObject.name);
    //             Health attackedHealth = hit.transform.GetComponent<Health>();
    //             yield return null;
    //             if (!hitHealths.Contains(attackedHealth))
    //             {
    //                 attackedHealth.Damage(heldWeapon.damage);
    //                 hitHealths.Add(attackedHealth);
    //             }

    //             yield return null;
    //         }
    //     }


    //     // yield return new WaitWhile(() => attacking);
    //     attacking = false;
    // }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + transform.forward * (attackRange / 2) + Vector3.up * attackOrigin;
        Gizmos.DrawWireCube(attackCenter, new Vector3(attackSize.x, attackSize.y, attackRange));
    }
}
