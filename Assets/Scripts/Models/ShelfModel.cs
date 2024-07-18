using System.Collections.Generic;
using System.Linq;
using BimiBooTest.Views;

namespace BimiBooTest.Models
{
    public class ShelfModel
    {
        public ShelfView ShelfView { get; }

        public List<ToyModel> Toys { get; } = new List<ToyModel>();

        public ShelfModel(ShelfView shelfView)
        {
            ShelfView = shelfView;
        }

        public bool CheckCorrect() => Toys.All(x => x.CheckCorrect());
    }
}