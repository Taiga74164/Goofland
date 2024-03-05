using System.Collections.Generic;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Controllers
{
    public class PieController : MonoBehaviour
    {
        [Header("Pie Settings")]
        [SerializeField] private float throwForce = 15.0f;
        [SerializeField] private float pieCooldownDuration = 1.0f;

        [Header("Trajectory Settings")]
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private float blockSize = 1.0f;
        [SerializeField] private AnimationCurve indicatorCurve;
        [SerializeField] private bool drawMaxDistance;

        [SerializeField] private Transform _squeakBody;

        //private PlayerInput _playerInput;
        private InputController _inputController;
        private Rigidbody2D _rb;
        private Camera _mainCamera;
        private Vector2 _screenResolution;
        private Vector2 _aimInput;
        private float _pieThrowCooldown;
        private List<GameObject> _indicators = new List<GameObject>();

        private void Awake()
        {
            _inputController = GetComponent<InputController>();
            _rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
            _screenResolution = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            if (GameManager.IsPaused) return;
            
            // Update the pie throw cooldown.
            if (_pieThrowCooldown > 0.0f)
                _pieThrowCooldown -= Time.deltaTime;
            
            // Clear the trajectory if the pie is not ready.
            if(!IsPieReady())
                ClearTrajectory();
            
            //Draw the trajectory if the pie is ready.
            if (IsPieReady())
                DrawTrajectory();
        }

        
        private void DrawTrajectory()
        {
            // Get the mouse position in world space.
            
            // Calculate the direction and force of the throw.
            var direction = (GetAimInput());
            // Add the current velocity to the throw force.
            var velocity = (direction * throwForce);
            // Calculate maximum distance of the pie throw
            var maxDistance = CalculateMaxDistance(velocity);
            // Calculate the rounded total arrows to draw based on the angle and throw force
            var totalArrows = Mathf.Abs((Mathf.FloorToInt(maxDistance / blockSize)));
            for (var i = 0; i < totalArrows; i++)
            {
                // Calculate the time it takes for the pie to reach the next arrow position.
                var time = (i + 1) * blockSize / velocity.magnitude;
                // Calculate the trajectory point at the given time.
                var position = CalculateTrajectoryPoint(transform.position, velocity, time);
                // Calculate the rotation of the arrow based on the trajectory point.
                // var rotation = Quaternion.LookRotation(Vector3.forward, 
                //     CalculateTrajectoryPoint(transform.forward, force, time) - position);
                
                var indicator = Instantiate(indicatorPrefab, position, Quaternion.identity, transform);
                _indicators.Add(indicator);
                
                // Calculate the curve value based on the current arrow and the total arrows.
                var curveValue = indicatorCurve.Evaluate(i / (float)totalArrows);
                
                // Scale the arrow based on the curve value.
                indicator.transform.localScale *= curveValue;
                // Set the alpha of the arrow based on the curve value.
                var spriteRenderer = indicator.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(1, 1, 1, curveValue);
            }
        }
        
        /// <summary>
        /// Calculate the trajectory point based on the position, force, and time.
        /// Equation: f(t) = (x0 + x*t, y0 + y*t - g*t²/2)
        /// Where:
        /// - f(t) is the trajectory point at time t.
        /// - x0 and y0 are the initial position.
        /// - x and y are the velocity.
        /// - g is the gravity.
        /// - t is the time.
        /// </summary>
        /// <param name="position">The position of the object. Represents x0 and y0.</param>
        /// <param name="velocity">The velocity of the object. Represents x and y.</param>
        /// <param name="time">The time to calculate the trajectory point at. Represents t.</param>
        /// <returns>The calculated trajectory point at the given time.</returns>
        private static Vector2 CalculateTrajectoryPoint(Vector2 position, Vector2 velocity, float time) 
            => position + // x0, y0
               (velocity * time) + // x*t, y*t 
               Physics2D.gravity * Mathf.Pow(time, 2) / 2; // g*t²/2 
        
        /// <summary>
        /// Calculate the max distance of the trajectory.
        /// Equation: R = v^2 sin(2θ) / g
        /// Where:
        /// - R is the range.
        /// - v is the initial velocity.
        /// - theta is the angle of projection.
        /// - g is the gravity.
        /// </summary>
        /// <param name="initialVelocity">The initial velocity of the object.</param>
        /// <returns>The calculated max distance of the trajectory.</returns>
        private static float CalculateMaxDistance(Vector2 initialVelocity)
        {
            var xValue = initialVelocity.x;
            if (xValue == 0)
                xValue = initialVelocity.y;
                    
            var g = Mathf.Abs(Physics2D.gravity.y);
            var angle = Mathf.Atan2(initialVelocity.y, xValue); // 45 * Mathf.Deg2Rad; 
            var distance = Mathf.Pow(initialVelocity.magnitude, 2) * Mathf.Sin(2 * angle) / g;
            return distance;
        }
        
        private void ClearTrajectory()
        {
            _indicators.ForEach(Destroy);
            _indicators.Clear();
        }

        public void HandlePieThrow()
        {
            if (!IsPieReady()) return;
            
            // Calculate the direction and force of the throw.
            var direction = GetAimInput();
            // Add the current velocity to the throw force.
            var force = (direction.normalized * throwForce).Add(_rb.velocity / 2);
            
            // Create a new pie and throw it.
            var pie = PrefabManager.Create<Pie>(Prefabs.Pie, transform);
            pie.throwForce = force;
            pie.direction = direction;
            pie.ThrowPie();
            
            // Update the last time a pie was thrown.
            _pieThrowCooldown = pieCooldownDuration;
        }

        //gets the input of the player and returns the appropriate angle
        private Vector2 GetAimInput()
        {
            ClearTrajectory();
            //might want to change this to a state machine similar to how the player works
            Vector2 forwardDirection = _squeakBody.right;
            float x = forwardDirection.x;

            if (_inputController.IsAimingUp)
                return Vector2.up;

            else if (_inputController.IsAimingDown)
                return Vector2.down;

            else if (_inputController.IsAngleUp)
                return new Vector2(x, 1);

            else if (_inputController.IsAngleDown)
                return new Vector2(x, -1);

            else
                return forwardDirection;
        }

        private void OnAim(InputValue value) => _aimInput =  value.Get<Vector2>();
        
        private bool IsPieReady() => !GameManager.IsPaused && _pieThrowCooldown <= 0.0f;
    }
}