using UnityEngine;
using UnityEngine.InputSystem;

namespace BoatAttack
{
    public class HumanController : MonoBehaviour
    {
        public Engine engine;

        private InputControls _controls;

        public float _throttle;
        public float _steering;

        private bool _paused;

        [SerializeField] private Transform skybox;

        private void Awake()
        {
            _controls = new InputControls();
            
            _controls.BoatControls.Trottle.performed += context => _throttle = context.ReadValue<float>();
            _controls.BoatControls.Trottle.canceled += context => _throttle = 0f;
            
            _controls.BoatControls.Steering.performed += context => _steering = context.ReadValue<float>();
            _controls.BoatControls.Steering.canceled += context => _steering = 0f;

            _controls.BoatControls.Reset.performed += ResetBoat;
            _controls.BoatControls.Pause.performed += FreezeBoat;
        }

        private void SkyboxPosSet()
        {
            skybox.position = transform.position;
        }

        private void OnEnable()
        {
            _controls.BoatControls.Enable();
        }

        private void OnDisable()
        {
            _controls.BoatControls.Disable();
        }

        private void ResetBoat(InputAction.CallbackContext context)
        {

        }

        private void FreezeBoat(InputAction.CallbackContext context)
        {
            _paused = !_paused;
            if(_paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        void FixedUpdate()
        {
            SkyboxPosSet();

            engine.Accelerate(_throttle);
            engine.Turn(_steering);
        }
    }
}

