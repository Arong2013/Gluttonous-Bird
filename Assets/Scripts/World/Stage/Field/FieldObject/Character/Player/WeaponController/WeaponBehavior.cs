using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    protected PlayerMarcine player;
    int weaponDMG;
    float disdmg = 100;
    public void Initialize(PlayerMarcine player)
    {
        this.player = player;
        weaponDMG = 100;
    }
    public abstract void BtnDown();
    public abstract void BtnUp();

    public void SetDisDMG(float dmg) => disdmg = dmg;   

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterPart>(out MonsterPart combatable) && !other.GetComponent<PlayerMarcine>())
        {
            combatable.TakeDamage(weaponDMG * disdmg * 0.01f);
        }
    }
}
