using UnityEngine;

public class TeleportObject : MonoBehaviour, ITeleportable
{
    private bool _isTeleported;

    public bool IsTeleported()
    {
        return _isTeleported;
    }

    public void OnTeleportEnd()
    {
        _isTeleported = false;
    }

    public void OnTeleportStart()
    {
        _isTeleported = true;
    }
}
