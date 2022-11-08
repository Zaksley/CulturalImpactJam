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
    [SerializeField] private float _counterPlungeSpeedFall = 1f; 
    
    [Header("Fly speed")]
    [SerializeField] private float _flyUpSpeed = 5f;
    
    public bool IsNormalFlying { get; private set; } = false;
    public bool IsPlunging { get; private set; } = false; 
    
    private void Update()
    {
        if (_movementController.IsOnGround || !_generalController.CanGlide)
        {
            IsNormalFlying = false;
            IsPlunging = false; 
            return;
        }
        
        _movementController.UpdateMovementSpeedPlayer(_generalController.CanGlide, _generalController.CanFly);

        if (Input.GetKey(KeyCode.Space) && _generalController.CanGlide && !_generalController.CanFly)
        {
            if (!_movementController.IsRising)
            {
                Glide();
                IsNormalFlying = true; 
                IsPlunging = false; 
            }
        }

        if (_generalController.CanFly)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift))
            {
                Fly(); 
                IsNormalFlying = true; 
                IsPlunging = false; 
            }
            else
            {
                if (IsNormalFlying)
                    ResetSpeed();
                
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Plunge();
                    IsPlunging = true; 
                    IsNormalFlying = false; 
                }
                // Natural glide 
                else
                {
                    if (IsPlunging)
                        ResetSpeed();
                
                    Glide(); 
                    IsNormalFlying = true; 
                    IsPlunging = false; 
                }
            }
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
        _rb.velocity = new Vector3(0, _flyUpSpeed, 0);
    }

    private void Plunge()
    {
        _rb.velocity -= new Vector3(0, _counterPlungeSpeedFall, 0);
    }

    private void ResetSpeed()
    {
        _rb.velocity -= new Vector3(0, 0, 0);
    }

    public void UpdateCounterFall()
    {
        _counterSpeedFall = _counterSpeedFallFlight; 
    }
}
