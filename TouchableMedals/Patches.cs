using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using XD;

namespace TouchableMedals.Patches
{
    internal class Touchable : MonoBehaviour
    {
        private bool _hovered;
        private bool _queuedClick;
        private MedalDisplay _display;
        private GameObject _medal;
        private SphereCollider _collider;
        private Vector3 _basePosition;
        private Quaternion _targetRotation;

        protected void Awake()
        {
            _medal = transform.Find("Container").gameObject;
            _display = GetComponent<MedalDisplay>();
            _collider = GetComponent<SphereCollider>();
            _basePosition = _medal.transform.localPosition;

            if (_collider == null)
            {
                _collider = _medal.AddComponent<SphereCollider>();
                _collider.center = Vector3.zero;
                _collider.radius = 0.5f;
                _collider.isTrigger = false;
            }
        }

        protected void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _queuedClick = true;
            }

            _medal.transform.localRotation = Quaternion.Slerp(
                _medal.transform.localRotation,
                _targetRotation,
                Time.deltaTime * 20f
            );
        }

        protected void FixedUpdate()
        {
            if (!Application.isFocused) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider == _collider;

            if (hit)
            {
                if (!_hovered)
                {
                    _display._animatorCache.animator.Play("MedalDisplay_Unawarded", 0, 0.0f);
                    _display._animatorCache.animator.speed = 0f;
                    _basePosition = _medal.transform.localPosition;
                    _hovered = true;
                }

                if (_queuedClick)
                {
                    _display.PlayRankSound();
                    _display._animatorCache.animator.speed = 1f;
                    _display._animatorCache.animator.Play("MedalDisplay_Achieved", 0, 0.0f);
                }

                Vector3 position = hitInfo.transform.InverseTransformPoint(hitInfo.point) / _collider.radius;
                _targetRotation =
                Quaternion.AngleAxis(position.x * -30f, Vector3.up) *
                Quaternion.AngleAxis(position.y * 30f, Vector3.right) *
                Quaternion.Euler(0f, -30f, 0f);
            }
            else if (_hovered)
            {
                _targetRotation = Quaternion.identity;
                _medal.transform.localPosition = _basePosition;
                _display._animatorCache.animator.speed = 1f;
                _hovered = false;
            }

            _queuedClick = false;
        }
    }

    [HarmonyPatch]
    internal static class MedalHandler
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MedalDisplay), nameof(MedalDisplay.OnEnable))]
        private static void AddMedal(MedalDisplay __instance)
        {
            var go = __instance.gameObject;
            if (go.GetComponent<Touchable>() == null)
            {
                go.AddComponent<Touchable>();
            }
        }
    }
}
