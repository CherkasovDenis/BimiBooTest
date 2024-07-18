using UnityEngine;

namespace BimiBooTest.Views
{
    public class ShelfView : MonoBehaviour
    {
        public ToyType ToyType => _toyType;

        public Transform[] ToyPositions => _toyPositions;

        [SerializeField]
        private ToyType _toyType;

        [SerializeField]
        private Transform[] _toyPositions;
    }
}