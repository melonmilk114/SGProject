namespace Melon
{
    public interface ITargetObjectReceiver<T> 
    {
        public void SetTargetObject(T inObject);
        public T GetTargetObject { get; set; }
    }
}