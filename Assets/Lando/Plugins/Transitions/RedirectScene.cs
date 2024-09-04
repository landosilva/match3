using UnityEngine;

namespace Lando.Plugins.Transitions
{
    public class RedirectScene : MonoBehaviour
    {
        [SerializeField, SceneReference] private int _scene;
        [SerializeField] private float _transitionInDuration;
        [SerializeField] private float _delay;
        [SerializeField] private float _transitionOutDuration;

        private async void Start() => await Transition.To(_scene, _transitionInDuration, _delay, _transitionOutDuration);
    }
}
