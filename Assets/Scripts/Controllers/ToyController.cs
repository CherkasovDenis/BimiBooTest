using System;
using System.Linq;
using BimiBooTest.Models;
using BimiBooTest.Services;
using BimiBooTest.Views;
using UnityEngine;

namespace BimiBooTest.Controllers
{
    public class ToyController : IDisposable
    {
        private readonly RackModel _rackModel;
        private readonly AudioService _audioService;
        private readonly Collider2D[] _droppedColliders;

        private ToyView _toyView;
        private ToyModel _toyModel;

        public ToyController(RackModel rackModel, AudioService audioService)
        {
            _rackModel = rackModel;
            _audioService = audioService;
            _droppedColliders = new Collider2D[3];
        }

        public void Initialize(ToyView toyView, ToyModel toyModel)
        {
            _toyView = toyView;
            _toyModel = toyModel;

            _toyView.PointerDown += _audioService.PlayGetSound;
            _toyView.BeginDrag += DisableOtherToysIntractable;
            _toyView.Dropped += TrySwap;
            _toyView.AttractedToPosition += UpdateInteractableStatus;

            _toyModel.EnabledInteractable += EnableInteractable;
            _toyModel.DisabledInteractable += DisableInteractable;
            _toyModel.ChangedPositionData += ChangePosition;

            DisableInteractable();
            _toyView.SetColliderStatus(false);
        }

        public void Dispose()
        {
            if (_toyView != null)
            {
                _toyView.PointerDown -= _audioService.PlayGetSound;
                _toyView.BeginDrag -= DisableOtherToysIntractable;
                _toyView.Dropped -= TrySwap;
                _toyView.AttractedToPosition -= UpdateInteractableStatus;
            }

            _toyModel.EnabledInteractable -= EnableInteractable;
            _toyModel.DisabledInteractable -= DisableInteractable;
            _toyModel.ChangedPositionData -= ChangePosition;
        }

        private void DisableOtherToysIntractable()
        {
            _rackModel.OnDisableOtherToysInteractable(_toyView);
        }

        private void TrySwap()
        {
            for (var i = 0; i < Physics2D.OverlapPointNonAlloc(_toyView.transform.position, _droppedColliders); i++)
            {
                if (!_droppedColliders[i].TryGetComponent(out ToyView otherToy))
                {
                    continue;
                }

                if (otherToy.ToyType == _toyView.ToyType)
                {
                    if (otherToy.ID == _toyView.ID)
                    {
                        continue;
                    }

                    WrongDrop();
                    return;
                }

                if (!_rackModel.TryGetToyModel(otherToy.ToyType, otherToy.ID, out var otherToyModel))
                {
                    continue;
                }

                if (otherToyModel.ShelfToyType != _toyView.ToyType)
                {
                    WrongDrop();
                    return;
                }

                Swap(otherToyModel);
                return;
            }

            WrongDrop();
        }

        private void Swap(ToyModel otherToyModel)
        {
            var otherShelfToyType = otherToyModel.ShelfToyType;
            var otherToyCurrentIndex = otherToyModel.CurrentIndex;

            otherToyModel.ChangePositionData(_toyModel.ShelfToyType, _toyModel.CurrentIndex);
            _toyModel.ChangePositionData(otherShelfToyType, otherToyCurrentIndex);

            _audioService.PlayRightSound();
        }

        private void WrongDrop()
        {
            _audioService.PlayWrongSound();
            _toyView.AttractToPosition();
            _toyView.StartRotateAnimation();
        }

        private void ChangePosition()
        {
            var shelfModel = _rackModel.Shelves.FirstOrDefault(x => x.ShelfView.ToyType == _toyModel.ShelfToyType);

            if (shelfModel == null)
            {
                return;
            }

            _toyView.SetParent(shelfModel.ShelfView.ToyPositions[_toyModel.CurrentIndex]);
            _toyView.AttractToPosition();
        }

        private void UpdateInteractableStatus()
        {
            _toyView.SetColliderStatus(!_toyModel.CheckCorrect());

            _rackModel.OnEnableToysInteractable();
        }

        private void EnableInteractable() => _toyView.Interactable = true;

        private void DisableInteractable() => _toyView.Interactable = false;
    }
}