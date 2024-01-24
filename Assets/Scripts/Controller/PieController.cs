using Managers;
using Objects;
using UnityEngine;

namespace Controller
{
    public class PieController : MonoBehaviour
    {
        public float maxChargeTime = 3.0f;
        public float minForce = 2.0f;
        public float maxForce = 6.0f;

        private Pie _pie;
        private float _chargeTime;
        private float _lastPieThrownTime = -1.0f;
        private readonly float _pieCooldownDuration = 1.0f;
        private Vector2 _throwDirection;
        private float _throwForce;

        public void Charge() => _chargeTime = 0.0f;

        public void Charging()
        {
            _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, maxChargeTime);
            Debug.Log($"charging: {_chargeTime}");
        }

        public void HandlePieThrow() //spawns and sets physics of pie
        {
            if (!IsPieReady()) return;
            _throwForce = Mathf.Lerp(minForce, maxForce, Mathf.Clamp01(_chargeTime / maxChargeTime));

            if (Camera.main != null)
                _throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            var pie = PrefabManager.Create<Pie>(Prefabs.Pie);
            if (pie == null) return;
            pie.throwForce = _throwForce;
            pie.direction = _throwDirection;
            _lastPieThrownTime = Time.time;

            pie.ThrowPie();
        }

        private bool IsPieReady() => Time.time - _lastPieThrownTime >= _pieCooldownDuration;
    }
}