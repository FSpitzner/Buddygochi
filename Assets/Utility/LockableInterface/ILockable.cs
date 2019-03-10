using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface ILockable 
{
    void Lock(bool locking, Component lockRaiser);
}
