using UnityEngine;

public abstract class Weapon : Item
{
    public float Damage { get; set; }
    public float AttackSpeed { get; set; }

    protected Weapon(string name, Sprite icon, int damage, float attackSpeed)
        : base(name, icon, false, 1)
        {
            Damage = damage;
            AttackSpeed = attackSpeed;
        }
        public abstract override void Use();
}
