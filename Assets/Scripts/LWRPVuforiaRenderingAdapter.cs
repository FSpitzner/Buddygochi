using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Vuforia;


/// <summary>
/// Adapter to enable video background rendering with  Vuforia Engine 8.0 and the Unity Light Weight Rendering Pipeline (LWRP).
/// Uses LWRP callbacks to replace Unity's OnPreRender and OnPostRender callback which are not available with LWRP 
///
/// Attach to ARCamera GameObject which holds the VuforiaBehaviour component
///
/// Note: This script may become incompatible with future Vuforia Engine Versions.
/// Do not use in non-LWRP projects
/// </summary>
[RequireComponent(typeof(VuforiaBehaviour))]
public class LWRPVuforiaRenderingAdapter : MonoBehaviour
{
    Action mVbBehaviourOnPreRender = () => { };
    Action mVbBehaviourOnPostRender = () => { };

    void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void OnVuforiaStarted()
    {
        ConnectToVideoBackgroundBehaviour();
    }

    void ConnectToVideoBackgroundBehaviour()
    {
        var vbBehaviour = GetComponent<VideoBackgroundBehaviour>();

        if (vbBehaviour == null)
            return;

        var onPreRenderMethod = vbBehaviour.GetType()
            .GetMethod("OnPreRender", BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var onPostRenderMethod = vbBehaviour.GetType()
            .GetMethod("OnPostRender", BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        mVbBehaviourOnPreRender = () => onPreRenderMethod.Invoke(vbBehaviour, null);
        mVbBehaviourOnPostRender = () => onPostRenderMethod.Invoke(vbBehaviour, null);
    }

    void OnEnable()
    {
        RenderPipeline.beginFrameRendering += OnBeginFrameRendering; //replaces OnPreRender()
        RenderPipeline.beginCameraRendering += OnBeginCameraRendering; //replaces OnPostRender()
    }
 
    void OnDisable()
    {
        RenderPipeline.beginFrameRendering -= OnBeginFrameRendering;
        RenderPipeline.beginCameraRendering -= OnBeginCameraRendering;
    }
 
    void OnBeginFrameRendering(Camera[] cams)
    {
        mVbBehaviourOnPreRender();
    }

    void OnBeginCameraRendering(Camera cam)
    {
        mVbBehaviourOnPostRender();
    }
}
