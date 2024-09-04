using UnityEngine;

namespace Match3.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        public Camera Camera => _camera;
    }
}