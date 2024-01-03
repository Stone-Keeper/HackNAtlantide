using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    #region Unity Events

    public UnityEvent OnInteract;
    public UnityEvent OnResetInteract;
    public UnityEvent OnBecameClosestObject;
    public UnityEvent OnBecameNotClosestObject;

    #endregion

    #region Variables

    private bool _isClosetstInteractable;

    #endregion

    #region Properties

    public bool IsClosestInteractable 
    { 
        get { return _isClosetstInteractable; } 
        set 
        { 
            _isClosetstInteractable = value; 
            
            if (_isClosetstInteractable) 
            { 
                OnBecameClosestObject?.Invoke();
            } 
            else
            {
                OnBecameNotClosestObject?.Invoke();
            }
        } 
    }

    #endregion

    #region Logic Methods

    public void LaunchOnInteract()
    {
        OnInteract?.Invoke();
    }

    public void LaunchOnResetInteract()
    {
        OnResetInteract?.Invoke();
    }
    
    public abstract void CancelInteract();

    public abstract bool Interact();

    public abstract void ResetInteract();

    public abstract void ResetTransform();

    #endregion
}
