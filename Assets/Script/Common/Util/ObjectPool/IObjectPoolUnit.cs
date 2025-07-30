using JetBrains.Annotations;

namespace Melon
{
    public interface IObjectPoolUnit
    {
        public void OnPoolDequeue();   // 풀에서 꺼내질 때 호출됨
        public void OnPoolEnqueue();   // 풀에 다시 반납될 때 호출됨
    }
    
    
}