using UnityEngine;

namespace BoatAttack
{
    public class Engine : MonoBehaviour
    {
        public Rigidbody RB;

        [Header("Engin Stats")]
        public float steeringTorque = 5f;
        public float horsePower = 18f;
        private float _yHeight;

        private float _turnVel;
        private float _currentAngle;

        public void Accelerate(float modifier)
        {
            if (_yHeight > -0.1f)
            {
                modifier = Mathf.Clamp(modifier, 0f, 1f);
                var forward = RB.transform.forward;
                forward.y = 0f;
                forward.Normalize();
                RB.AddForce(horsePower * modifier * forward, ForceMode.Acceleration);
                RB.AddRelativeTorque(-Vector3.right * modifier, ForceMode.Acceleration);
            }
        }

        public void Turn(float modifier)
        {
            if (_yHeight > -0.1f)
            {
                modifier = Mathf.Clamp(modifier, -1f, 1f);
                RB.AddRelativeTorque(new Vector3(0f, steeringTorque, -steeringTorque * 0.5f) * modifier, ForceMode.Acceleration);
            }

            _currentAngle = Mathf.SmoothDampAngle(_currentAngle, 
                60f * -modifier, 
                ref _turnVel, 
                0.5f, 
                10f,
                Time.fixedTime);
            transform.localEulerAngles = new Vector3(0f, _currentAngle, 0f);
        }
	}
}
