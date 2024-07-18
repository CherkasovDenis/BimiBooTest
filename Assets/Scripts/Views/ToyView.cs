using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace BimiBooTest.Views
{
    public class ToyView : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action PointerDown;
        public event Action BeginDrag;
        public event Action Dropped;
        public event Action AttractedToPosition;

        public bool Interactable { get; set; }
        public ToyType ToyType => _toyType;
        public int ID { get; private set; }

        [SerializeField]
        private ToyType _toyType;

        [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField]
        private Transform _toyTransform;

        [SerializeField]
        private SortingGroup _sortingGroup;

        private Transform _transform;
        private Camera _camera;
        private Vector3 _dragOffset;
        private bool _isDragging;

        private Tween _attractTween;
        private Sequence _scaleSequence;
        private Sequence _wrongRotateSequence;

        private Vector3 _startScale;
        private Vector3 _tapScale;
        private float _scaleUpTime;
        private float _scaleDownTime;
        private Vector3[] _wrongRotateAngles;
        private float _wrongRotateSegmentTime;
        private float _attractTime;
        private int _defaultSortingOrder;
        private int _attractSortingOrder;
        private int _dragSortingOrder;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;
        }

        private void OnDestroy()
        {
            _attractTween?.Kill();
            _scaleSequence?.Kill();
            _wrongRotateSequence?.Kill();
        }

        public void Initialize(int index, Vector3 startScale, Vector3 tapScale, float scaleUpTime, float scaleDownTime,
            Vector3[] wrongRotateAngles, float wrongRotateTime, float attractTime, int defaultSortingOrder,
            int attractSortingOrder, int dragSortingOrder)
        {
            ID = index;
            _startScale = startScale;
            _tapScale = tapScale;
            _scaleUpTime = scaleUpTime;
            _scaleDownTime = scaleDownTime;
            _wrongRotateAngles = wrongRotateAngles;
            _wrongRotateSegmentTime = wrongRotateTime / _wrongRotateAngles.Length;
            _attractTime = attractTime;
            _defaultSortingOrder = defaultSortingOrder;
            _attractSortingOrder = attractSortingOrder;
            _dragSortingOrder = dragSortingOrder;

            _toyTransform.localScale = _startScale;
            _sortingGroup.sortingOrder = _defaultSortingOrder;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable || _isDragging) return;

            StartScaleAnimation();
            PointerDown?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Interactable) return;

            _isDragging = true;
            _dragOffset = _transform.position - _camera.ScreenToWorldPoint(eventData.position);
            _sortingGroup.sortingOrder = _dragSortingOrder;

            BeginDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Interactable || !_isDragging) return;

            var newPosition = _camera.ScreenToWorldPoint(eventData.position) + _dragOffset;
            newPosition.z = _transform.position.z;
            _transform.position = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Interactable || !_isDragging) return;

            ResetDrag();

            Dropped?.Invoke();
        }

        public void SetParent(Transform parent)
        {
            _transform.SetParent(parent);
        }

        public void AttractToPosition(float time = -1f)
        {
            Interactable = false;
            ResetDrag();

            if (_sortingGroup.sortingOrder == _defaultSortingOrder)
            {
                _sortingGroup.sortingOrder = _attractSortingOrder;
            }

            _attractTween?.Kill();

            _attractTween = _transform.DOLocalMove(Vector3.zero, time < 0f ? _attractTime : time)
                .OnComplete(() =>
                {
                    _sortingGroup.sortingOrder = _defaultSortingOrder;
                    AttractedToPosition?.Invoke();
                });
        }

        public void StartRotateAnimation()
        {
            _wrongRotateSequence = DOTween.Sequence();

            foreach (var angle in _wrongRotateAngles)
            {
                _wrongRotateSequence.Append(_toyTransform.DOLocalRotate(angle, _wrongRotateSegmentTime));
            }
        }

        public void SetColliderStatus(bool status)
        {
            _collider.enabled = status;
        }

        private void ResetDrag()
        {
            _isDragging = false;
            _dragOffset = Vector3.zero;
        }

        private void StartScaleAnimation()
        {
            _scaleSequence?.Kill();
            _scaleSequence = DOTween.Sequence();

            _toyTransform.localScale = _startScale;

            _scaleSequence
                .Append(_toyTransform.DOScale(_tapScale, _scaleUpTime))
                .Append(_toyTransform.DOScale(_startScale, _scaleDownTime));
        }
    }
}