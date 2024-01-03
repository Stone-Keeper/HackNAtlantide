using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricArcController : MonoBehaviour
{
    [SerializeField] private ElectricArc[] electricArcs;

    public void ActivateFX(IInteractable parent)
    {
        foreach (ElectricArc electricArc in electricArcs)
        {
            electricArc.gameObject.SetActive(true);
            electricArc.InitializeArc();
            electricArc.SetLastPosParent(parent.transform);
        }
    }
    public void ActivateFX(Transform parent1, Transform parent2)
    {
        foreach (ElectricArc electricArc in electricArcs)
        {
            electricArc.gameObject.SetActive(true);
            electricArc.InitializeArc();
            electricArc.SetBothParent(parent1.transform, parent2.transform);
        }
    }
    public void DeactivateFX()
    {
        foreach (ElectricArc electricArc in electricArcs)
        {
            electricArc.Deactivate();
            electricArc.transform.parent = transform.parent;
        }
    }
}