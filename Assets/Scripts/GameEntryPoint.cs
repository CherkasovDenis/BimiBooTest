using System.Linq;
using System.Threading;
using BimiBooTest.Configs;
using BimiBooTest.Extensions;
using BimiBooTest.Factories;
using BimiBooTest.Models;
using BimiBooTest.Views;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace BimiBooTest
{
    public class GameEntryPoint : IAsyncStartable
    {
        private readonly VisualConfig _visualConfig;
        private readonly ShelfView[] _shelfViews;
        private readonly ToyFactory _toyFactory;
        private readonly RackModel _rackModel;

        public GameEntryPoint(VisualConfig visualConfig, ShelfView[] shelfViews, ToyFactory toyFactory,
            RackModel rackModel)
        {
            _visualConfig = visualConfig;
            _shelfViews = shelfViews;
            _toyFactory = toyFactory;
            _rackModel = rackModel;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            var mixArray = new ToyView[_shelfViews.Length, _shelfViews.First().ToyPositions.Length];

            for (var i = 0; i < _shelfViews.Length; i++)
            {
                var shelfView = _shelfViews[i];
                var shelfModel = new ShelfModel(shelfView);

                for (var j = 0; j < shelfView.ToyPositions.Length; j++)
                {
                    mixArray[i, j] = _toyFactory.Create(shelfView.ToyType, shelfModel, j);
                }

                _rackModel.Shelves.Add(shelfModel);
            }

            mixArray.Shuffle();

            await UniTask.WaitForSeconds(_visualConfig.BeforeMixDelay, cancellationToken: cancellation);

            for (var i = 0; i < mixArray.GetLength(0); i++)
            {
                var shelfView = _shelfViews[i];

                for (var j = 0; j < mixArray.GetLength(1); j++)
                {
                    var toyView = mixArray[i, j];

                    if (_rackModel.TryGetToyModel(toyView.ToyType, toyView.ID, out var toyModel))
                    {
                        toyModel.ChangePositionData(shelfView.ToyType, j);
                    }
                }
            }
        }
    }
}