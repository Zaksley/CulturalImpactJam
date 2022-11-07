using UnityEngine;

public class DebugScriptFeather : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in collision");
        if (other.CompareTag("Player"))
        {
            Debug.Log("done");
            other.GetComponent<GeneralController>().GainFeather();
            Destroy(gameObject);
        }
    }
}
