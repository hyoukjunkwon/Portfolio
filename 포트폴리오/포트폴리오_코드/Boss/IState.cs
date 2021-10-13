public interface Istate<T>
{
    void OnEnter(T sender);
    void OnFixedUpdate(T sender);
    void OnExit(T sender);
    void OnUpdate(T sender);
}