using System.Collections.Generic;
using Managers;
using Objects.Scriptable;
using UnityEngine;
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
        
        private Rigidbody2D _rb;
        private Camera _mainCamera;
        private Vector2 _screenResolution;
        private float _lastPieThrownTime = -1.0f;
        private List<GameObject> _indicators = new List<GameObject>();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
            _screenResolution = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            if (GameManager.IsPaused) return;
            
            // Clear the trajectory if the pie is not ready.
            ClearTrajectory();
            
            // Draw the trajectory if the pie is ready.
            if (IsPieReady())
                DrawTrajectory();
        }

        private void DrawTrajectory()
        {
            // Get the mouse position in world space.
            var mousePosition = GetAimInput();
            // Calculate the direction and force of the throw.
            var direction = (mousePosition - transform.position).normalized;
            // Add the current velocity to the throw force.
            var force = (direction.normalized * throwForce).Add(_rb.velocity / 2);
            // Calculate the distance to the mouse position from the player.
            var distanceToMouse = Vector2.Distance(mousePosition, transform.position);
            // Calculate the rounded total arrows to draw based on the distance to the mouse.
            var totalArrows = 10; //Mathf.FloorToInt(distanceToMouse / blockSize);
            
            for (var i = 0; i < totalArrows; i++)
            {
                // Calculate the time it takes for the pie to reach the next arrow position.
                var time = (i + 1) * blockSize / force.magnitude;
                // Calculate the trajectory point at the given time.
                var position = CalculateTrajectoryPoint(transform.position, force, time);
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
               Physics2D.gravity * (time * time) / 2; // - g*t²/2 
        
        
        private void ClearTrajectory()
        {
            _indicators.ForEach(Destroy);
            _indicators.Clear();
        }

        public void HandlePieThrow()
        {
            if (!IsPieReady()) return;
            
            // Get the mouse position in world space.
            var mousePosition = GetAimInput();
            // Calculate the direction and force of the throw.
            var direction = (mousePosition - transform.position).normalized;
            // Add the current velocity to the throw force.
            var force = (direction.normalized * throwForce).Add(_rb.velocity / 2);
            
            // Create a new pie and throw it.
            var pie = PrefabManager.Create<Pie>(Prefabs.Pie, transform);
            pie.throwForce = force;
            pie.direction = direction;
            pie.ThrowPie();
            
            // Update the last time a pie was thrown.
            _lastPieThrownTime = Time.time;
        }
        
        private Vector3 GetAimInput()
        {
            // Check if controller input is detected.
            var aimInput = InputManager.Aim.ReadValue<Vector2>();
            if (aimInput != Vector2.zero)
                return aimInput * _screenResolution;

            // If no controller input is detected, use the mouse position.
            var mouseInput = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return mouseInput;
        }
        private bool IsPieReady() => !GameManager.IsPaused && Time.time - _lastPieThrownTime >= pieCooldownDuration;
    }
}