using UnityEngine;
public abstract class CharacterMovementHandler 
{
    private Rigidbody rigidbody;
    public abstract void Move();
    public abstract void Roll();
    public abstract void Climb();
    public abstract void Jump();
    public abstract void RollAnimeEvent(CapsuleCollider capsuleCollider, bool force);
}
