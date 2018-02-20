// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    /// <summary>
    /// GestureManager creates a gesture recognizer and signs up for a tap gesture.
    /// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
    /// GestureManager then sends a message to that game object.
    /// </summary>
    [RequireComponent(typeof(GazeManager))]
    public partial class GestureManager : Singleton<GestureManager>
    {
        /// <summary>
        /// To select even when a hologram is not being gazed at,
        /// set the override focused object.
        /// If its null, then the gazed at object will be selected.
        /// </summary>
        public GameObject OverrideFocusedObject
        {
            get; set;
        }

        /// <summary>
        /// Gets the currently focused object, or null if none.
        /// </summary>
        public GameObject FocusedObject
        {
            get { return focusedObject; }
        }

        private GestureRecognizer gestureRecognizer;
        private GameObject focusedObject;

        public bool IsNavigating { get; private set; }

        public Vector3 NavigationPosition { get; private set; }

        void Start()
        {
            // Create a new GestureRecognizer. Sign up for tapped events.
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap |
                GestureSettings.NavigationX);

            gestureRecognizer.Tapped += GestureRecognizer_Tapped;

            gestureRecognizer.NavigationStarted += NavigationRecognizer_NavigationStarted;
            gestureRecognizer.NavigationUpdated += NavigationRecognizer_NavigationUpdated;
            gestureRecognizer.NavigationCompleted += NavigationRecognizer_NavigationCompleted;
            gestureRecognizer.NavigationCanceled += NavigationRecognizer_NavigationCanceled;


            // Start looking for gestures.
            gestureRecognizer.StartCapturingGestures();
        }

        private void GestureRecognizer_Tapped(TappedEventArgs args)
        {
            if (focusedObject != null) {
                focusedObject.SendMessage ("OnSelect");
            }
        }

        private void NavigationRecognizer_NavigationStarted (NavigationStartedEventArgs obj) {
            //Debug.Log ("NavigationRecognizer_NavigationStarted");

            IsNavigating = true;

            NavigationPosition = Vector3.zero;
        }

        private void NavigationRecognizer_NavigationUpdated (NavigationUpdatedEventArgs obj) {
            //Debug.Log ("NavigationRecognizer_NavigationUpdated");

            IsNavigating = true;

            NavigationPosition = obj.normalizedOffset;
        }

        private void NavigationRecognizer_NavigationCompleted (NavigationCompletedEventArgs obj) {
            //Debug.Log ("NavigationRecognizer_NavigationCompleted");

            IsNavigating = false;
        }

        private void NavigationRecognizer_NavigationCanceled (NavigationCanceledEventArgs obj) {
            //Debug.Log ("NavigationRecognizer_NavigationCanceled");

            IsNavigating = false;
        }


        void LateUpdate ()
        {
            GameObject oldFocusedObject = focusedObject;

            if (GazeManager.Instance.Hit &&
                OverrideFocusedObject == null &&
                GazeManager.Instance.HitInfo.collider != null)
            {
                // If gaze hits a hologram, set the focused object to that game object.
                // Also if the caller has not decided to override the focused object.
                focusedObject = GazeManager.Instance.HitInfo.collider.gameObject;
            }
            else
            {
                // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
                focusedObject = OverrideFocusedObject;
            }

            if (focusedObject != oldFocusedObject) {
                // If the currently focused object doesn't match the old focused object, cancel the current gesture.
                // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
                gestureRecognizer.CancelGestures ();
                gestureRecognizer.StartCapturingGestures ();
            }
        }

        void OnDestroy()
        {
            gestureRecognizer.StopCapturingGestures();
            gestureRecognizer.Tapped -= GestureRecognizer_Tapped;
        }
    }
}