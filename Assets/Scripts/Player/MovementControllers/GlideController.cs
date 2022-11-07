using UnityEngine;

public class GlideController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TopDownCharacterMover _movementController;
    [SerializeField] private GeneralController _generalController; 
    
    [Header("Fall speed")]
    [SerializeField] private float _counterSpeedFall = 0.1f;

    private void Update()
    {
        if (_movementController.IsOnGround || !_generalController.CanGlide || _movementController.IsRising)
            return; 
        
        if (Input.GetKey(KeyCode.Space))
        {
            Glide(); 
        }
    }

    /// <summary>
    /// Counter force against fall speed
    /// </summary>
    private void Glide()
    {
        _rb.velocity += new Vector3(0, _counterSpeedFall, 0);
    }
}
