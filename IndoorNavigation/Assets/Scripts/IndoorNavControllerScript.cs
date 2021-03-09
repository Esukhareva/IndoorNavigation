using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//#if UNITY_EDITOR
//    // Set up touch input propagation while using Instant Preview in the editor.
//    using Input = InstantPreviewInput;
//#endif

/// <summary>
/// Controls the HelloAR example.
/// </summary>
public class IndoorNavControllerScript : MonoBehaviour
{
    public Text camPoseText;

    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary>
    public Camera FirstPersonCamera;

    public GameObject cameraTarget;

    private Vector3 m_prevARPosePosition;
    private bool trackingStarted = false;
    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error,
    /// otherwise false.
    /// </summary>
    private bool _isQuitting = false;
    private bool Tracking = false;


    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public void Start()
    {
        m_prevARPosePosition = Vector3.zero;
    }

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    public void Update()
    {
        UpdateApplicationLifecycle();

        //move the person indicator according to position
        Vector3 currentArPosition = Frame.Pose.position;
        if (!Tracking) 
        {
            Tracking = true;
            m_prevARPosePosition = Frame.Pose.position;
        }
        //Remember the previous position so we can apply deltas
        Vector3 deltaPosition = currentArPosition - m_prevARPosePosition;
        m_prevARPosePosition = currentArPosition;

        if (cameraTarget != null) 
        {
            //The initial forward vector of the sphere must be aligned with the initial camera
            //   direction in the XZ plane.
            //We apply translation only in the XZ plane.
            cameraTarget.transform.Translate(deltaPosition.x, 0.0f, deltaPosition.z);
            // Set the pose rotation to be used in the CameraFollow script
            FirstPersonCamera.GetComponent<FollowTarget>().targetRot = Frame.Pose.rotation;
        }

    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (_isQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            ShowAndroidToastMessage("Camera permission is needed to run this application.");
            _isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            _isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}

