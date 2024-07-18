using System;
using BimiBooTest.Models;
using BimiBooTest.Services;
using BimiBooTest.Views;
using VContainer.Unity;

namespace BimiBooTest.Controllers
{
    public class RackController : IInitializable, IDisposable
    {
        private readonly RackModel _rackModel;
        private readonly AudioService _audioService;

        public RackController(RackModel rackModel, AudioService audioService)
        {
            _rackModel = rackModel;
            _audioService = audioService;
        }

        public void Initialize()
        {
            _rackModel.EnableToysInteractable += CheckAllToysInCorrectPlace;
            _rackModel.DisableOtherToysInteractable += DisableOtherToysInteractable;
        }

        public void Dispose()
        {
            _rackModel.EnableToysInteractable -= CheckAllToysInCorrectPlace;
            _rackModel.DisableOtherToysInteractable -= DisableOtherToysInteractable;
        }

        private void CheckAllToysInCorrectPlace()
        {
            if (_rackModel.CheckCorrect())
            {
                _audioService.PlayFinalSound();
                return;
            }

            foreach (var shelfModel in _rackModel.Shelves)
            {
                foreach (var toyModel in shelfModel.Toys)
                {
                    toyModel.OnEnabledInteractable();
                }
            }
        }

        private void DisableOtherToysInteractable(ToyView exceptToy)
        {
            foreach (var shelfModel in _rackModel.Shelves)
            {
                foreach (var toyModel in shelfModel.Toys)
                {
                    if (toyModel.ToyType == exceptToy.ToyType && toyModel.ID == exceptToy.ID)
                    {
                        continue;
                    }

                    toyModel.OnDisabledInteractable();
                }
            }
        }
    }
}