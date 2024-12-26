using UnityEngine;
public abstract class CharacterMovementHandler 
{
    public abstract void Move();
    public abstract void Roll();
    public abstract void Jump();
    public abstract void RollAnimeEvent(CapsuleCollider capsuleCollider, bool force);
}
