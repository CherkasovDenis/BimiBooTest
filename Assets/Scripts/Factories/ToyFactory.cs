using System.Collections.Generic;
using BimiBooTest.Configs;
using BimiBooTest.Controllers;
using BimiBooTest.Models;
using BimiBooTest.Views;
using UnityEngine;
using VContainer;

namespace BimiBooTest.Factories
{
    public class ToyFactory
    {
        private readonly IObjectResolver _container;
        private readonly VisualConfig _visualConfig;
        private readonly Dictionary<ToyType, ToyView> _toyPrefabs = new Dictionary<ToyType, ToyView>();

        public ToyFactory(IObjectResolver container, ToyView[] toyViewPrefabs, VisualConfig visualConfig)
        {
            _container = container;
            _visualConfig = visualConfig;

            foreach (var toyPrefab in toyViewPrefabs)
            {
                _toyPrefabs.Add(toyPrefab.ToyType, toyPrefab);
            }
        }

        public ToyView Create(ToyType toyType, ShelfModel shelfModel, int index)
        {
            var parent = shelfModel.ShelfView.ToyPositions[index];

            var toyView = Object.Instantiate(_toyPrefabs[toyType], parent, false);

            var startScale = _visualConfig.StartScale;
            var tapScale = _visualConfig.TapScaleMultiplier * startScale;

            toyView.Initialize(index, startScale, tapScale, _visualConfig.ScaleUpTime, _visualConfig.ScaleDownTime,
                _visualConfig.WrongRotateAngles, _visualConfig.WrongRotateTime, _visualConfig.AttractTime,
                _visualConfig.DefaultSortingOrder, _visualConfig.AttractSortingOrder, _visualConfig.DragSortingOrder);

            var toyModel = new ToyModel(toyType, index);

            var toyController = _container.Resolve<ToyController>();
            toyController.Initialize(toyView, toyModel);

            shelfModel.Toys.Add(toyModel);

            return toyView;
        }
    }
}