using UnityEngine;

namespace BimiBooTest.Configs
{
    [CreateAssetMenu(fileName = nameof(VisualConfig), menuName = "BimiBooTest/VisualConfig")]
    public class VisualConfig : ScriptableObject
    {
        public Vector3 StartScale => _startScale;

        public float TapScaleMultiplier => _tapScaleMultiplier;

        public float ScaleUpTime => _scaleUpTime;

        public float ScaleDownTime => _scaleDownTime;

        public Vector3[] WrongRotateAngles => _wrongRotateAngles;

        public float WrongRotateTime => _wrongRotateTime;

        public float AttractTime => _attractTime;

        public float BeforeMixDelay => _beforeMixDelay;

        public float MixAttractTime => _mixAttractTime;

        public int DefaultSortingOrder => _defaultSortingOrder;

        public int AttractSortingOrder => _attractSortingOrder;

        public int DragSortingOrder => _dragSortingOrder;

        [SerializeField]
        private Vector3 _startScale;

        [SerializeField]
        private float _tapScaleMultiplier;

        [SerializeField]
        private float _scaleUpTime;

        [SerializeField]
        private float _scaleDownTime;

        [SerializeField]
        private Vector3[] _wrongRotateAngles;

        [SerializeField]
        private float _wrongRotateTime;

        [SerializeField]
        private float _attractTime;

        [SerializeField]
        private float _beforeMixDelay;

        [SerializeField]
        private float _mixAttractTime;

        [SerializeField]
        private int _defaultSortingOrder;

        [SerializeField]
        private int _attractSortingOrder;

        [SerializeField]
        private int _dragSortingOrder;
    }
}