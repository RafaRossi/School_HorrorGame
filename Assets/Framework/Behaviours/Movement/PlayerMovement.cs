using System.Collections;
using Framework.Entities;
using Framework.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.Behaviours.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CharacterFlags characterFlags;
        [SerializeField] private PlayerStatsComponent playerStatsComponent;
        
        [SerializeField] private StatModifier characterDashSpeed;
        
        private Camera _camera;

        private Vector3 _movementDirection = Vector3.zero;

        private void Awake()
        {
            _camera ??= Camera.main;
        }

        public void Move(Vector3 movementDirection)
        {
            if(characterFlags.TryGetFlag(Flag.IsDashing)) return;
        
            _movementDirection = movementDirection;
        
            _movementDirection = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * _movementDirection;
        
            characterController.Move(_movementDirection.normalized * (playerStatsComponent.MoveSpeed.Value * Time.deltaTime));
        }

        private static float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public void Rotate(Vector3 facingDirection)
        {
            var positionOnScreen = _camera.WorldToViewportPoint (transform.position);
            var mouseOnScreen = (Vector2)_camera.ScreenToViewportPoint(Input.mousePosition);
        
            var angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen) + _camera.transform.eulerAngles.y;
        
            transform.rotation =  Quaternion.Euler(new Vector3(0f,180 - angle,0f));
        }
    
        public void Dash()
        {
            StartCoroutine(PerformDash());
            return;

            IEnumerator PerformDash()
            {
                if (characterFlags.TryGetFlag(Flag.IsDashing)) yield break;

                characterFlags.AddFlag(Flag.IsDashing);
        
                var startTime = Time.time;
                
                playerStatsComponent.MoveSpeed.AddModifier(characterDashSpeed);

                while (Time.time < startTime + playerStatsComponent.DashDuration.Value)
                {
                    characterController.Move(_movementDirection.normalized * (playerStatsComponent.MoveSpeed.Value * Time.deltaTime));
                
                    yield return null;
                }

                playerStatsComponent.MoveSpeed.RemoveModifier(characterDashSpeed);
                characterFlags.RemoveFlag(Flag.IsDashing);
            }
        }
        
        public Stat GetCharacterMoveSpeed() => playerStatsComponent.MoveSpeed;
    
    }
}
