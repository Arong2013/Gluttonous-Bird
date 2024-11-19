public enum BehaviorState
{
    SUCCESS,   
    RUNNING,   
    FAILURE    
}
public interface ICombatable
{
    void TakeDamage(float dmg,CharacterAnimeBool characterAnimeBoo = 0l);
}
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void UnregisterObserver(IObserver observer);
    void NotifyObservers();
}
public interface IObserver
{
    void UpdateObserver();
}