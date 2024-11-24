using UnityEngine;
using UnityEngine.TextCore.Text;
public class PlayerMovementHandler : CharacterMovementHandler
{
    private PlayerMarcine character;
    private Rigidbody rigidbody;
    public PlayerMovementHandler(CharacterMarcine character)
    {
        this.character = character as PlayerMarcine;
        this.rigidbody = character.GetComponent<Rigidbody>();
    }
    public override void Move()
    {
        float speed = character.characterData.GetStat(CharacterStatName.SPD) * character.currentDir.magnitude;
        Vector3 moveDirection = new Vector3(character.currentDir.x, 0, 0).normalized * speed * Time.deltaTime; 
        rigidbody.MovePosition(character.transform.position + moveDirection);
        if (character.currentDir.x != 0)
        {
            character.transform.rotation = Quaternion.Euler(0, character.currentDir.x > 0 ? 90 : -90, 0);
        }
    }
    public override void Roll()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        Vector3 backwardForce = character.transform.forward * 5f;
        rigidbody.AddForce(backwardForce, ForceMode.Impulse);
    }
    public override void Jump()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    public override void Climb()
    {
        var dirY = character.currentDir.y;
        Vector3 dir;
        if (dirY > 0)
            dir = Vector3.up;
        else if (dirY == 0)
            dir = Vector3.zero;
        else
            dir = Vector3.down;
        Vector3 climbMovement = character.transform.position + dir * 5f * Time.deltaTime;
        rigidbody.MovePosition(climbMovement);
    }

    public override void RollAnimeEvent(CapsuleCollider capsuleCollider, bool force)
    {
        if(force)
        {
            rigidbody.useGravity = false;
            capsuleCollider.enabled = false;    
        }
        else
        {
            rigidbody.useGravity = true;
            capsuleCollider.enabled = true;
        }
    }
}
