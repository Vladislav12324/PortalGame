using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter Other;
    public bool IsTeleporting;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void Teleport(Transform obj)
    {
        // Position
        Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint3x4(obj.position);
        localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
        obj.position = Other.transform.localToWorldMatrix.MultiplyPoint3x4(localPos);

        // Rotation
        Quaternion difference = new Quaternion(0, Other.transform.rotation.y, 0, Other.transform.rotation.w) * Quaternion.Inverse( new Quaternion(0,transform.rotation.y,0,transform.rotation.w) * Quaternion.Euler(0, 180, 0));
        obj.rotation = difference * obj.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        ITeleportable teleportObject = other.GetComponent<ITeleportable>();

        if (teleportObject == null) return;

        if (!teleportObject.IsTeleported())
        {
            Other.IsTeleporting = true;
            teleportObject.OnTeleportStart();
            Teleport(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Other.IsTeleporting)
        {
            ITeleportable teleportObject = other.GetComponent<ITeleportable>();

            if (teleportObject == null) return;

            teleportObject.OnTeleportEnd();
            IsTeleporting = false;
        }
    }
}
