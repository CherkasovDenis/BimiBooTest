using System;
using System.Collections.Generic;
using System.Linq;
using BimiBooTest.Views;

namespace BimiBooTest.Models
{
    public class RackModel
    {
        public event Action EnableToysInteractable;
        public event Action<ToyView> DisableOtherToysInteractable;

        public List<ShelfModel> Shelves { get; } = new List<ShelfModel>();

        public bool TryGetToyModel(ToyType toyType, int id, out ToyModel toyModel)
        {
            foreach (var shelf in Shelves)
            {
                foreach (var toy in shelf.Toys)
                {
                    if (toy.ToyType == toyType && toy.ID == id)
                    {
                        toyModel = toy;
                        return true;
                    }
                }
            }

            toyModel = null;
            return false;
        }

        public void OnEnableToysInteractable()
        {
            EnableToysInteractable?.Invoke();
        }

        public void OnDisableOtherToysInteractable(ToyView toyView)
        {
            DisableOtherToysInteractable?.Invoke(toyView);
        }

        public bool CheckCorrect() => Shelves.All(x => x.CheckCorrect());
    }
}