using UnityEngine;

public class DebugScriptFeather : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GeneralController>().GainFeather();
            Destroy(gameObject);
        }
    }
}
