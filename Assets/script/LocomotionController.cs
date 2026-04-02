using UnityEngine;

public class LocomotionController : MonoBehaviour
{
    public Behaviour moveProvider;
    public Behaviour turnProvider;
    public Behaviour teleportProvider;

    public void DisableLocomotion()
    {
        if (moveProvider) moveProvider.enabled = false;
        if (turnProvider) turnProvider.enabled = false;
        if (teleportProvider) teleportProvider.enabled = false;

        Debug.Log("[Locomotion] 已关闭");
    }

    public void EnableLocomotion()
    {
        if (moveProvider) moveProvider.enabled = true;
        if (turnProvider) turnProvider.enabled = true;
        if (teleportProvider) teleportProvider.enabled = true;

        Debug.Log("[Locomotion] 已开启");
    }
}