using System;

namespace BimiBooTest.Models
{
    public class ToyModel
    {
        public event Action EnabledInteractable;
        public event Action DisabledInteractable;
        public event Action ChangedPositionData;

        public ToyType ToyType { get; }
        public int ID { get; }
        public ToyType ShelfToyType { get; private set; }
        public int CurrentIndex { get; private set; }

        public ToyModel(ToyType type, int id)
        {
            ToyType = type;
            ID = id;
            ShelfToyType = type;
            CurrentIndex = id;
        }

        public void OnEnabledInteractable()
        {
            EnabledInteractable?.Invoke();
        }

        public void OnDisabledInteractable()
        {
            DisabledInteractable?.Invoke();
        }

        public void ChangePositionData(ToyType shelfToyType, int currentIndex)
        {
            ShelfToyType = shelfToyType;
            CurrentIndex = currentIndex;

            ChangedPositionData?.Invoke();
        }

        public bool CheckCorrect() => ToyType == ShelfToyType;
    }
}