﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    #region GameObject
    public static T GetRequiredComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError("Expected to find component of type "
               + typeof(T) + ", but found none", obj);
        }

        return component;
    }
    #endregion



    #region Transform
    /// <summary>
    /// Looks for components of type T with specified Tag. Returns the first component of type T found.
    /// </summary>
    public static T FindComponentInChildrenWithTag<T>(this Transform parent, string tag) where T : Component
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].tag == tag)
            {
                return children[i].GetComponent<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// Looks for components of type T with specified Tag. Returns all components of type T found.
    /// </summary>
    public static T[] FindComponentsInChildrenWithTag<T>(this Transform parent, string tag) where T : Component
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        List<T> list = new List<T>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].tag.Contains(tag))
            {
                list.Add(children[i].GetComponent<T>());
            }
        }
        T[] returnArray = new T[list.Count];
        list.CopyTo(returnArray);
        return returnArray;
    }
    #endregion



    #region AudioSource
    /// <summary>
    /// Plays the clip with a specified Fade-In time.
    /// </summary>
    /// <param name="fadeInTime">Length of the Fade-In in seconds</param>
    public static void Play(this AudioSource source, float fadeInTime)
    {
        float originalVolume = source.volume;
        source.volume = 0f;
        source.Play();
        LeanTween.value(source.gameObject, (float f) => { source.volume = f; }, 0f, originalVolume, fadeInTime)
                 .setEase(LeanTweenType.easeInOutQuad);
    }

    /// <summary>
    /// Stops playing the clip with a specified Fade-Out time.
    /// </summary>
    /// <param name="fadeOutTime">Length of the Fade-Out in seconds</param>
    public static void Stop(this AudioSource source, float fadeOutTime)
    {
        float originalVolume = source.volume;
        LeanTween.value(source.gameObject, (float f) => { source.volume = f; }, originalVolume, 0f, fadeOutTime)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() => {
                     source.Stop();
                     source.volume = originalVolume;
                 });
    }

    /// <summary>
    /// Cross-Fades between two AudioSources over the specified time.
    /// </summary>
    /// <param name="otherSource">Reference to the AudioSource that shall fade in</param>
    /// <param name="fadingTime">Length of the Cross-Fade in seconds</param>
    public static void CrossFade(this AudioSource thisSource, AudioSource otherSource, float fadingTime)
    {
        float originalVolumeThis = thisSource.volume;
        float originalVolumeOther = otherSource.volume;
        LeanTween.value(thisSource.gameObject, (float f) => { thisSource.volume = f; }, originalVolumeThis, 0f, fadingTime)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() => {
                     thisSource.Stop();
                     thisSource.volume = originalVolumeThis;
                 });
        LeanTween.value(otherSource.gameObject, (float f) => { otherSource.volume = f; }, 0f, originalVolumeOther, fadingTime)
                 .setEase(LeanTweenType.easeInOutQuad);
        otherSource.Play();
    }
    #endregion



    #region Collider
    /// <summary>
    /// Begins at the hit.transform and goes up in the hierarchy. 
    /// Returns the fist component of type T found.
    /// Useful for OnTrigger methods. Since they don't understand compound colliders, you can get the parent object by looking for a Rigidbody.
    /// </summary>
    public static T FindComponentInParents<T>(this Collider hit) where T : Component
    {
        Transform transform = hit.transform;
        while (transform.GetComponent<T>() == null)
        {
            if (transform.parent == null)
            {
                return null;
            }

            transform = transform.parent;
        }
        return transform.GetComponent<T>();
    }


    /// <summary>
    /// Begins at the hit.transform and goes up in the hierarchy.
    /// Useful for OnTrigger methods. Since they don't understand compound colliders, you can get the parent object by looking for a Tag.
    /// </summary>
    /// <returns>Returns the fist Transform found with the specified tag. Returns null if none is found.</returns>
    public static Transform FindTagInParents(this Collider hit, string tag)
    {
        Transform transform = hit.transform;
        while (transform.tag != tag)
        {
            if (transform.parent == null)
            {
                return null;
            }

            transform = transform.parent;
        }
        return transform;
    }
    #endregion



    #region General Static Helper Methods
    /// <summary>
    /// Checks if a value is in range of a defined target value.
    /// </summary>
    /// <param name="value">Value to check if it is in range.</param>
    /// <param name="targetValue">The target value.</param>
    /// <param name="range">The range applied to the target value.</param>
    /// <returns></returns>
    public static bool InRange(float value, float targetValue, float range)
    {
        if (value <= targetValue + range && value >= targetValue - range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    #endregion
}
