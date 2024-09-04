using System.Threading.Tasks;
using Lando.Plugins.Singletons.MonoBehaviour;
using Lando.Plugins.Transitions.Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lando.Plugins.Transitions
{
    public class Transition : PersistentSingleton<Transition>
    {
        [SerializeField] private TransitionAction _transitionActionPrefab;
        
        private TransitionAction _transitionAction;
        
        protected override void Awake()
        {
            base.Awake();
            _transitionAction = Instantiate(_transitionActionPrefab, transform);
            _transitionAction.PrepareIn();
            _transitionAction.gameObject.SetActive(false);
        }

        private static async Task To(string sceneName, float inTransitionDuration = DEFAULT_DURATION, float delay = DEFAULT_DELAY, float outTransitionDuration = DEFAULT_DURATION)
        {
            AsyncOperation loadingAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (loadingAsync == null)
                return;
            
            loadingAsync.allowSceneActivation = false;
            await Instance._transitionAction.In(inTransitionDuration);
            loadingAsync.allowSceneActivation = true;
            int milliseconds = Mathf.RoundToInt(delay * 1000);
            await Task.Delay(milliseconds);
            
            await Instance._transitionAction.Out(outTransitionDuration);
        }
        
        public static async Task To(int sceneIndex, float inTransitionDuration = DEFAULT_DURATION, float delay = DEFAULT_DELAY, float outTransitionDuration = DEFAULT_DURATION)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            await To(sceneName, inTransitionDuration, delay, outTransitionDuration);
        }
        
        private const float DEFAULT_DURATION = 0.3f;
        private const float DEFAULT_DELAY = 0.1f;
    }
}