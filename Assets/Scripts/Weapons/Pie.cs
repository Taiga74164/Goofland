using System;
using System.Collections.Generic;
using Enemies;
using Levels;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public struct PieSkin
    {
        public string name;
        public Sprite sprite;
        public ParticleSystem splatEffect;
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pie : MonoBehaviour, IWeapon
    {
        [Header("Pie Settings")]
        public List<PieSkin> pieSkins = new List<PieSkin>();
        public Vector2 throwForce = new Vector2(5.0f, 5.0f);
        [Tooltip("How long before the pie destroys itself.")]
        public float destructionTime = 2.0f;
        

        [Header("Audio Settings")]
        [SerializeField] private List<AudioData> splatAudioDatas;
        
        private const float SpawnOffSet = 0.1f;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private ParticleSystem _effect;
        private float timer = 0;
        
        private void Awake()
        {
            // Set the initial position of the pie.
            transform.Translate(Vector3.up * SpawnOffSet);
            
            // Get the rigidbody and sprite renderer components.
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;
            
            // Get the sprite renderer.
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Set Pie skin and effect based on the current level name.
            var levelName = LevelUtil.CurrentLevelName;
            foreach (var skin in pieSkins)
            {
                if (levelName.Contains(skin.name))
                {
                    _spriteRenderer.sprite = skin.sprite;
                    _effect = skin.splatEffect;
                    break;
                }
                else
                {
                    // Set the default pie skin.
                    _spriteRenderer.sprite = pieSkins[0].sprite;
                    _effect = pieSkins[0].splatEffect;
                }
            }
        }

        private void Update()
        {
            // Rotate the pie to face the direction it's moving.
            var moveDirection = _rigidbody2D.velocity;
            var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            // Flip the pie sprite based on the direction it's moving.
            var shouldFlip = moveDirection.x < 0;
            _spriteRenderer.flipY = shouldFlip;
            transform.Rotate(0, 0, shouldFlip ? 90 : -90);

            timer += Time.deltaTime;
            if (timer >= destructionTime)
                Destroy(gameObject);
            if (!_spriteRenderer.isVisible)
                Destroy(gameObject);
        }
        
        private void LateUpdate()
        {
            if (GameManager.IsPaused) return;
        
            // Destroy the pie if it falls off the map.
            if (transform.position.y < -100.0f) Destroy(gameObject);
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<Enemy>().GotHit(this);
            else if (other.gameObject.CompareTag("Coin"))
                other.gameObject.GetComponent<Coin>().CollectCoin();
            
            // Play a random audio clip from the list.
            var randomIndex = Random.Range(0, splatAudioDatas.Count - 1);
            var position = transform.position;
            AudioManager.Instance.PlayOneShotAudio(splatAudioDatas[randomIndex], position);
            
            // Stop the pie from moving on impact.
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _spriteRenderer.enabled = false;
            
            // Create the splat effect.
            Instantiate(_effect, position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    
        /// <summary>
        /// Throws the pie in the specified direction.
        /// </summary>
        public void ThrowPie()
        {
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);
        }
    }
}
