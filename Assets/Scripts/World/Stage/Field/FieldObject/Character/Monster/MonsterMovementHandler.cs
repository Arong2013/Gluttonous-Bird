using UnityEngine;
using UnityEngine.TextCore.Text;
public class MonsterMovementHandler : CharacterMovementHandler
{
    private MonsterMarcine character;
    private Rigidbody rigidbody;
    public MonsterMovementHandler(CharacterMarcine character, Rigidbody rigidbody)
    {
        this.character = character as MonsterMarcine;
        this.rigidbody = rigidbody;
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
        character.characterData.UpdateBaseStat(CharacterStatName.SP, -character.characterData.GetStat(CharacterStatName.RollSP));
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        Vector3 backwardForce = character.transform.forward * 5f;
        rigidbody.AddForce(backwardForce, ForceMode.Impulse);
    }
    public override void Jump()
    {
        rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    public override void RollAnimeEvent(CapsuleCollider capsuleCollider, bool force)
    {
        if (force)
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
