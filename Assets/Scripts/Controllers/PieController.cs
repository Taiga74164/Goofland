using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Controllers
{
    public class PieController : MonoBehaviour
    {
        public float maxChargeTime = 3.0f;
        public float minForce = 2.0f;
        public float maxForce = 6.0f;
        public float _pieCooldownDuration = 1.0f;
        //refactor after beta
        [HideInInspector] public bool canBeThrown = true;

        private Pie _pie;
        private float _chargeTime;
        private float _lastPieThrownTime = -1.0f;
        private Vector2 _throwDirection;
        private float _throwForce;

        public void Charge() => _chargeTime = 0.0f;

        public void Charging() => _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, maxChargeTime);

        public void HandlePieThrow()
        {
            if (!IsPieReady() || Camera.main == null) return;
            
            _throwForce = Mathf.Lerp(minForce, maxForce, Mathf.Clamp01(_chargeTime / maxChargeTime));
            _throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            var pie = PrefabManager.Create<Pie>(Prefabs.Pie);
            pie.throwForce = _throwForce;
            pie.direction = _throwDirection;

            var velocity = GetComponent<Rigidbody2D>().velocity;
            pie.ThrowPie(velocity);
            
            _lastPieThrownTime = Time.time;
            canBeThrown = false;
        }

        private bool IsPieReady() => !GameManager.IsPaused && Time.time - _lastPieThrownTime >= _pieCooldownDuration;
    }
}