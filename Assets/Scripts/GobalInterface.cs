﻿public enum TurnState
{
    SUCCESS,    // 턴이 성공적으로 완료됨
    RUNNING,    // 턴이 진행 중임
    FAILURE     // 턴이 실패함
}
public interface ITurnStateable
{
    TurnState CurrentTurnState { get; set; }  // 현재 턴 상태
    TurnState ExecuteTurn();                  // 턴을 실행하고 그 결과를 반환
}