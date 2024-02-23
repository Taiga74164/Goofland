using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Controllers
{
    public class PieController : MonoBehaviour
    {
        [SerializeField] private float throwForce = 15.0f;
        [SerializeField] private float pieCooldownDuration = 1.0f;
        
        private Pie _pie;
        private Rigidbody2D _rb;
        private Camera _mainCamera;
        private float _lastPieThrownTime = -1.0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
        }

        public void HandlePieThrow()
        {
            if (!IsPieReady()) return;
            
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePosition - transform.position).normalized;
            
            var pie = PrefabManager.Create<Pie>(Prefabs.Pie, transform);
            pie.throwForce = throwForce;
            pie.direction = direction;

            var velocity = _rb.velocity;
            pie.ThrowPie(velocity);
            
            _lastPieThrownTime = Time.time;
        }

        private bool IsPieReady() => !GameManager.IsPaused && Time.time - _lastPieThrownTime >= pieCooldownDuration;
    }
}