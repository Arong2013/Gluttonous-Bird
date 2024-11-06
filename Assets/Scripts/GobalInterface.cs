public enum BehaviorState
{
    SUCCESS,   
    RUNNING,   
    FAILURE    
}
public enum CharacterState
{
    Idle = 0,          // 캐릭터가 아무 행동도 하지 않을 때
    Walk = 1,          // 캐릭터가 걷고 있을 때
    Roll = 2,          // 캐릭터가 구르기 중일 때
    Attack = 2,        // 캐릭터가 공격 중일 때
    Interacting = 1,   // 캐릭터가 상호작용하고 있을 때
    Block = 1,         // 캐릭터가 방어하고 있을 때
    TakingDamage = 6,  // 캐릭터가 데미지를 받고 있을 때
    Healing = 7,       // 캐릭터가 회복 중일 때
    Stunned = 8,       // 캐릭터가 기절 상태일 때
    Paralyzed = 8,     // 캐릭터가 기절과 동일한 우선순위를 가짐
    Dead = 10          // 캐릭터가 사망 상태일 때
}
