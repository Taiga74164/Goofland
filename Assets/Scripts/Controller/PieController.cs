using Managers;
using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class PieController : MonoBehaviour
    {
        private float _pieCooldownDuration = 1.0f;
        private float _lastPieThrownTime = -1.0f;


        public float MaxChargeTime = 3.0f;
        public float MinForce = 2.0f;
        public float MaxForce = 6.0f;

        private float _chargeTime;

        private float _throwForce;
        private Vector2 _throwDirection;
        private Pie _lastPieThrown;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Charge()
        {
            if (!gameObject.active) return;
            _chargeTime = 0.0f;
        }
        public void Charging()
        {
            if (!gameObject.active) return;
            _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, MaxChargeTime);
            Debug.Log(_chargeTime);
        }

        public void HandlePieThrow() //spawns and sets physics of pie
        {
            if (!IsPieReady()) return;
            _throwForce = Mathf.Lerp(MinForce, MaxForce, Mathf.Clamp01(_chargeTime / MaxChargeTime));

            _throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            
            GameObject pie = PrefabManager.Instance.Create(Prefabs.Pie);
            Debug.Log("spawn");
            _lastPieThrown = pie.GetComponent<Pie>();
            _lastPieThrown.throwForce = _throwForce;
            _lastPieThrown.direction = _throwDirection;
            _lastPieThrownTime = Time.time;

            _lastPieThrown.ThrowPie();
        }

        private bool IsPieReady() => Time.time - _lastPieThrownTime >= _pieCooldownDuration;
    }
}


