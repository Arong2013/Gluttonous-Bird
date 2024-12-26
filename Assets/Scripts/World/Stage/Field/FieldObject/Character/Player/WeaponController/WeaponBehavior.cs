using UnityEngine;
using System.Collections.Generic;
public abstract class WeaponBehavior : MonoBehaviour
{
    protected PlayerMarcine player;
    protected CapsuleCollider capsuleCollider;
    public void Initialize(PlayerMarcine player)
    {
        this.player = player;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    public abstract void BtnDown();
    public abstract void BtnUp();
    public void ColliderSet(bool isStart) => capsuleCollider.enabled = isStart;
}
