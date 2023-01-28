using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    public bool IsCarring;
    public Transform ParetnObject;

    private void Update()
    {
        if (IsCarring)
        {
            transform.position = ParetnObject.position;
        }
    }

    public void SetCarry(Transform parent)
    {
        ParetnObject = parent;
        IsCarring = true;
    }

    public void BreakCarry()
    {
        ParetnObject = null;
        IsCarring = false;
    }
}
