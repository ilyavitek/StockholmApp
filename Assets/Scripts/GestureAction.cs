using Academy.HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on 
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour {
    [Tooltip ("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;

    private float rotationFactor;

    void Update () {
        PerformRotation ();
    }

    private void PerformRotation () {
        if (GestureManager.Instance.IsNavigating) {
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            transform.Rotate (new Vector3 (0, -1 * rotationFactor, 0));
        }
    }
}