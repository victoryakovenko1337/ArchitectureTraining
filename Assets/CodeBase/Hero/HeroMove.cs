using Assets.CodeBase.Data;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private int _movementSpeed;

        private CharacterController _characterController;
        private IInputService _inputService;

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (progress.WorldData.PositionOnLevel.Position != null)
                {
                    Warp(to: savedPosition);
                }
            }
        }

        private void Awake() 
        {
            _inputService = AllServices.Container.Single<IInputService>();
            
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() 
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = Camera.main.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(movementVector * _movementSpeed * Time.deltaTime);
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector();
            _characterController.enabled = true;
        }

    }
}
