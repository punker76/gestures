﻿// <copyright file="RecognizerPlatformEffect.cs" company="Velocity Systems">
//     Copyright (c) 2020 Velocity Systems
// </copyright>

[assembly: Xamarin.Forms.ResolutionGroupName("Velocity")]
[assembly: Xamarin.Forms.ExportEffect(
    typeof(Velocity.Gestures.Forms.WPF.RecognizerPlatformEffect),
    nameof(Velocity.Gestures.Forms.RecognizerEffect))]

namespace Velocity.Gestures.Forms.WPF
{
    using System.Reactive.Disposables;
    using Velocity.Gestures.WPF;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.WPF;
    using FormsHoverGestureRecognizer = Velocity.Gestures.Forms.HoverGestureRecognizer;
    using FormsLongPressGestureRecognizer = Velocity.Gestures.Forms.LongPressGestureRecognizer;
    using FormsPanGestureRecognizer = Velocity.Gestures.Forms.PanGestureRecognizer;
    using FormsSwipeGestureRecognizer = Velocity.Gestures.Forms.SwipeGestureRecognizer;
    using FormsTapGestureRecognizer = Velocity.Gestures.Forms.TapGestureRecognizer;

    /// <summary>
    /// Platform implementation for <see cref="RecognizerEffect"/>.
    /// </summary>
    public class RecognizerPlatformEffect : PlatformEffect
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        /// <summary>
        /// Initialize the platform effect.
        /// </summary>
        internal static void Init()
        {
            // Force .NET Native linker to preserve the effect.
            // https://bugzilla.xamarin.com/show_bug.cgi?id=31076
            _ = typeof(RecognizerPlatformEffect);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            if (!(Element is View view))
            {
                return;
            }

            foreach (var recognizer in view.GestureRecognizers)
            {
                switch (recognizer)
                {
                    case FormsTapGestureRecognizer tap:
                        _disposable.Add(new TapRecognizer(Control, tap.NumberOfTapsRequired, tap.NumberOfTouchesRequired).Bind(tap, view, _disposable));
                        break;

                    case FormsLongPressGestureRecognizer longPress:
                        _disposable.Add(new LongPressRecognizer(Control, longPress.NumberOfTouchesRequired).Bind(longPress, view, _disposable));
                        break;

                    case FormsSwipeGestureRecognizer swipe:
                        _disposable.Add(new SwipeRecognizer(Control, swipe.DirectionMask, swipe.NumberOfTouchesRequired).Bind(swipe, view, _disposable));
                        break;

                    case FormsPanGestureRecognizer pan:
                        _disposable.Add(new PanRecognizer(Control).Bind(pan, view, _disposable));
                        break;

                    case FormsHoverGestureRecognizer hover:
                        _disposable.Add(new HoverRecognizer(Control).Bind(hover, view, _disposable));
                        break;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetached() => _disposable?.Dispose();
    }
}