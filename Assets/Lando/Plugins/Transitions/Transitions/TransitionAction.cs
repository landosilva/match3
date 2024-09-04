using System.Threading.Tasks;
using UnityEngine;

namespace Lando.Plugins.Transitions.Transitions
{
    public abstract class TransitionAction : MonoBehaviour
    {
        [field: SerializeField] public  float Delay { get; private set; }
        
        public abstract void PrepareIn();
        public virtual async Task In(float duration)
        {
            await Task.CompletedTask;
        }
        public abstract void PrepareOut();
        public virtual async Task Out(float duration)
        {
            await Task.CompletedTask;
        }
    }
}