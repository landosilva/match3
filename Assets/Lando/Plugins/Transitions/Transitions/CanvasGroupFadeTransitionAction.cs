using System.Threading.Tasks;
using UnityEngine;

namespace Lando.Plugins.Transitions.Transitions
{
    public class CanvasGroupFadeTransitionAction : TransitionAction
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public override void PrepareIn()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(true);
        }

        public override async Task In(float duration)
        {
            PrepareIn();
            
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                _canvasGroup.alpha = time / duration;
                await Task.Yield();
            }
            
            PrepareOut();
        }

        public override void PrepareOut()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public override async Task Out(float duration)
        {
            PrepareOut();
            
            float time = duration;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                _canvasGroup.alpha = time / duration;
                await Task.Yield();
            }
            
            PrepareIn();
        }
    }
}