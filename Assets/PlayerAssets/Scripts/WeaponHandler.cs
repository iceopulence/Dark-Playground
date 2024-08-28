using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private Animator animator;
    public GameObject weapon;
    public ItemSO heldWeapon;

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

    [Header("Weapon Flags")]
    public bool hasPipe = false;
    public bool hasGun = false;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        camT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPipe && Input.GetKeyDown(KeyCode.F))
        {
            if (!holdingWeapon)
            {
                EquipWeapon();
            }
            else
            {
                UnEquipWeapon();
            }
            
            //AnimState
            animator.SetInteger("AnimState", holdingWeapon ? 1 : 0);
        }

        if (holdingWeapon && Input.GetKeyDown(KeyCode.Mouse0))
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
