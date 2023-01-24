using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Parameters")]
    [SerializeField] protected GameObject prefab = default;
    [SerializeField] protected Vector3 itemPosition = default;
    [SerializeField] protected Quaternion itemRotation = default;

    public static readonly int INTERACTABLE_LAYER = 6;

    public virtual void Awake()
    {
        gameObject.layer = INTERACTABLE_LAYER;
    }
    
    public abstract void OnInteract(GameObject itemsContainer, ref List<HandItem> allPlayerObjects);
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
}
