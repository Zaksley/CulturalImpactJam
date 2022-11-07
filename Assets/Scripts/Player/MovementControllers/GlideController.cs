using UnityEngine;
using UnityEngine.UIElements;

public class GlideController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TopDownCharacterMover _movementController;
    [SerializeField] private GeneralController _generalController; 
    
    [Header("Fall speed")]
    [SerializeField] private float _counterSpeedFall = 0.1f;
    [SerializeField] private float _counterSpeedFallFlight = 0.1f;
    
    [Header("Fly speed")]
    [SerializeField] private float _flyUpSpeed = 5f;
    
    private void Update()
    {
        if (_movementController.IsOnGround || !_generalController.CanGlide)
            return;

        if (Input.GetKey(KeyCode.Space) || _generalController.CanFly)
        {
            if (!_movementController.IsRising)
            {
                Glide();
            }
        }

        if (_generalController.CanFly && Input.GetKeyDown(KeyCode.Space))
        {
            Fly(); 
        }
    }

    /// <summary>
    /// Counter force against fall speed
    /// </summary>
    private void Glide()
    {
        _rb.velocity += new Vector3(0, _counterSpeedFall, 0);
    }

    private void Fly()
    {
        _rb.velocity += new Vector3(0, _flyUpSpeed, 0);
    }

    public void UpdateCounterFall()
    {
        _counterSpeedFall = _counterSpeedFallFlight; 
    }
}
