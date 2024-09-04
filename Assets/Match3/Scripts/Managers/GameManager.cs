using Lando.Core.Extensions;
using Match3.Entities;
using Match3.Factories;
using UnityEngine;
using Event = Lando.Plugins.Events.Event;
using Grid = Match3.Entities.Grid;

namespace Match3.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CameraManager _cameraManager;
        
        private void Awake()
        {
            Event.Subscribe<Grid.Events.Initialized>(OnGridInitialized);
        }

        private void OnDestroy()
        {
            Event.Unsubscribe<Grid.Events.Initialized>(OnGridInitialized);
        }

        private void OnGridInitialized(Grid.Events.Initialized e)
        {
            Event.Unsubscribe<Grid.Events.Initialized>(OnGridInitialized);
            _cameraManager.Camera.transform.position = e.Grid.Center - Vector3.one.With(z: 0) * 0.5f;
            _cameraManager.Camera.orthographicSize = e.Grid.Size.y + 1.5f;

            e.Grid.Fill();
        }
    }
}
