using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AttackCollider : MonoBehaviour , IAttackCollider
{
    private List<IDamageable> _damageableHitted;
    
    // Probably used in an Unity Event
    public void ResetListDamageableHitted()
    {
        _damageableHitted = new List<IDamageable>();
    }
    
    public event EventHandler OnCollideWithIDamageable;
    public UnityEvent OnHitEvent;
    AnimationEvent _damageActiveAnimatorEvent;
    
    [SerializeField] private bool sendEventOnEnter = true;
    [SerializeField] private bool sendEventOnStay = false; 
    [SerializeField] private bool sendEventOnExit = false;

    [SerializeField] private LayerMask interactWithLayers;
    private void OnEnable()
    {
        ResetListDamageableHitted();
    }

    private void OnDisable()
    {
        ResetListDamageableHitted();
    }

    private void OnHit(IDamageable damageable)
    {
        if (_damageableHitted.Contains(damageable)) { return; }
        
        _damageableHitted.Add(damageable);
        OnHitEvent?.Invoke();

        OnCollideWithIDamageable?.Invoke(this, new AttackDamageableEventArgs()
        {
            idamageable = damageable

        });  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        
        if (!enabled) return;
        if (!sendEventOnEnter) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnHit(damageable);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        if (!enabled) return;
        if (!sendEventOnStay) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnHit(damageable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        
        if (!enabled) return;
        if (!sendEventOnExit) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnHit(damageable);
        }
    }
    
    private bool IsInteractable(GameObject gameObject)
    {
        if (interactWithLayers == 0) return true;
        return interactWithLayers == (interactWithLayers | (1 << gameObject.layer));
    }
}