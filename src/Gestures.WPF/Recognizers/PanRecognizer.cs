﻿// <copyright file="PanRecognizer.cs" company="Velocity Systems">
//     Copyright (c) 2020 Velocity Systems
// </copyright>

using System.Windows;
using System.Windows.Input;
using WPoint = System.Windows.Point;

namespace Velocity.Gestures.WPF
{
    /// <summary>
    /// A multi-touch pan gesture recognizer.
    /// </summary>
    public class PanRecognizer : PlatformPanRecognizer<FrameworkElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanRecognizer"/> class.
        /// </summary>
        /// <param name="view">The native view.</param>
        public PanRecognizer(FrameworkElement view) : base(view)
        {
            // On WPF, touch events are promoted to mouse events.
            // However, touch events like TouchDown and ManipulationDelta etc do not fire for pointer actions like on UWP.
            // So we must listen for mouse events rather than touch events.
            View.MouseDown += OnMouseDown;
            View.MouseMove += OnMouseMove;
            View.MouseUp += OnMouseUp;
            View.MouseLeave += OnMouseLeave;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            View.MouseDown -= OnMouseDown;
            View.MouseMove -= OnMouseMove;
            View.MouseUp -= OnMouseUp;
            View.MouseLeave -= OnMouseLeave;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(View);

            OnTouchesBegan(point.X, point.Y);
            OnPanningBegan(point.X, point.Y);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!PanInProgress)
            {
                return;
            }

            var point = e.GetPosition(View);
            OnPanningPositionChanged(point.X, point.Y);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e) => OnMouseEnded(e.GetPosition(View));

        private void OnMouseLeave(object sender, MouseEventArgs e) => OnMouseEnded(e.GetPosition(View));

        private void OnMouseEnded(WPoint point)
        {
            if (!PanInProgress)
            {
                return;
            }

            OnPanningStateChanged(GestureState.Ended);
            OnTouchesEnded(point.X, point.Y);
        }
    }
}