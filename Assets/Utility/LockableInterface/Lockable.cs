using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Lockable : ILockable
{

    List<Component> locks = new List<Component>();
    public bool IsLocked = false;



	public void Lock(bool locking, Component raiser)
    {
        if (locking == true)
        {
            if (!locks.Contains(raiser))
            {
                locks.Add(raiser);
                IsLocked = true;
            }
        }
        else if (locking == false)
        {
            locks.Remove(raiser);
            if (locks.Count == 0) IsLocked = false;
        }
    }
}
