using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

namespace Player {
    [RequireComponent(typeof(NetworkRigidbody2D), typeof(PlayerData), typeof(Health))]
    public class PlayerController : NetworkBehaviour, IPlayerActions {
        private PlayerInput _playerInput;
        private Vector2 _moveInput = new();
        private Vector2 _cursorLocation;
        [SerializeField] public Rigidbody2D _bodyRb;

        private Transform _shipTransform;
        private Rigidbody2D _rb;

        [SerializeField] private Transform turretPivotTransform;

        public UnityAction<bool> onFireEvent;

        [Header("Settings")] [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float shipRotationSpeed = 100f;
        [SerializeField] private float turretRotationSpeed = 4f;
        private Task _task;
        private PlayerData _data;

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                if (_playerInput == null) {
                    _playerInput = new();
                    _playerInput.Player.SetCallbacks(this);
                }

                _playerInput.Player.Enable();

                _rb = GetComponent<Rigidbody2D>();
                _shipTransform = transform;

                if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);

                _task = SetPlayerName();
                _data = GetComponent<PlayerData>();
                StartCoroutine(LateInitialization());
            }
            else {
                Debug.Log($"name is: {GetComponent<PlayerData>().AccountName.Value}");
                
            }
            
        }

        private void Start() {
            StartCoroutine(LateInitialization());
        }

        #region Gustav

        private IEnumerator LateInitialization() {
            // super shitty solution.. not my proudest moment
            yield return new WaitForEndOfFrame();
            yield return _task;
        }

        private async Task SetPlayerName() {
            var result = await AuthenticationService.Instance.GetPlayerNameAsync();
            _data.SetName(result);
        }

        #endregion

        public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context) {
            if (context.performed) onFireEvent.Invoke(true);
            else if (context.canceled) onFireEvent.Invoke(false);
        }

        public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context) =>
            _moveInput = context.ReadValue<Vector2>();


        private void FixedUpdate() {
            if (!IsOwner) return;
            var tf = _bodyRb.transform;
            _rb.velocity = tf.up * (_moveInput.y * movementSpeed);
            tf.localPosition = Vector2.zero; // hate these
            _bodyRb.MoveRotation(_bodyRb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            if (!IsOwner) return;

            var screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
            var position = turretPivotTransform.position;
            var targetDirection = new Vector2(screenToWorldPosition.x - position.x,
                screenToWorldPosition.y - position.y).normalized;
            var currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection,
                Time.deltaTime * turretRotationSpeed);
            turretPivotTransform.up = currentDirection;
        }

        public void OnAim(InputAction.CallbackContext context) => _cursorLocation = context.ReadValue<Vector2>();
    }
}