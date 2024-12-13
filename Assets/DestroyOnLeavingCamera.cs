using UnityEngine;

public class DestroyOnLeavingCamera : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
