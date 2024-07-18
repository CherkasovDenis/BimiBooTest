using BimiBooTest.Configs;
using BimiBooTest.Controllers;
using BimiBooTest.Factories;
using BimiBooTest.Models;
using BimiBooTest.Services;
using BimiBooTest.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BimiBooTest
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private VisualConfig _visualConfig;

        [SerializeField]
        private AudioConfig _audioConfig;

        [SerializeField]
        private ToyView[] _toyViewPrefabs;

        [SerializeField]
        private ShelfView[] _shelfViews;

        [SerializeField]
        private AudioSource _audioSource;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_visualConfig);
            builder.RegisterInstance(_audioConfig);
            builder.RegisterInstance(_toyViewPrefabs);

            builder.RegisterInstance(_shelfViews);
            builder.RegisterInstance(_audioSource);

            builder.RegisterEntryPoint<GameEntryPoint>();
            builder.RegisterEntryPoint<RackController>();

            builder.Register<RackModel>(Lifetime.Singleton);

            builder.Register<ToyController>(Lifetime.Transient);
            builder.Register<ToyFactory>(Lifetime.Singleton);

            builder.Register<AudioService>(Lifetime.Singleton);
        }
    }
}