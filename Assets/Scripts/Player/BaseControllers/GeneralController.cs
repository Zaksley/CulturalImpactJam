using System;
using TMPro;
using UnityEngine;

public class GeneralController : MonoBehaviour
{
    private float[] _valuesMaterial = {-0.24f, -0.137f, 0, 0.085f, 0.138f, 0.24f};

    public bool CanHighJump { get; private set; }
    public bool CanFly { get; private set; }
    public bool CanGlide { get; private set; }

    [Header("High jump mechanic")] [SerializeField]
    private TopDownCharacterMover _topDownCharacterMover; 
    [SerializeField] private int _neededFeathersToHighJump = 1;
    
    [Header("Flight mechanic")]
    [SerializeField] private int _neededFeathersToFly = 4;

    [Header("Glide mechanic")] [SerializeField]
    private GlideController _glideController; 
    [SerializeField] private int _neededFeathersToGlide = 2;

    // Visual feathers variables
    [SerializeField] private GameObject _crowRenderer; 
    private Material _crowMaterialFeathers;
    private float _valueMaterialFeather; 

    private int _numberFeathers;
    public int NumberFeathers => _numberFeathers;

    private void Start()
    {
        _crowMaterialFeathers = _crowRenderer.GetComponent<SkinnedMeshRenderer>().material; 
        UpdateFeatherLooking();
    }

    private void Update()
    {
        UpdateMechanics();
    }

    public void GainFeather()
    {
        _numberFeathers++;
        UpdateFeatherLooking();
    }

    private void UpdateFeatherLooking()
    {
        _valueMaterialFeather = _valuesMaterial[_numberFeathers]; 
        _crowMaterialFeathers.SetFloat("_WingsAppear", _valueMaterialFeather);
    }

    /// <summary>
    /// Handle changes on number of feathers for mechanisms 
    /// </summary>
    private void UpdateMechanics()
    {
        if (_numberFeathers >= _neededFeathersToHighJump && !CanHighJump)
        {
            CanHighJump = true;
            _topDownCharacterMover.UnlockHighJump();
        }

        if (_numberFeathers >= _neededFeathersToGlide && !CanGlide)
        {
            CanGlide = true;
            _topDownCharacterMover.UpdateMovementSpeedPlayer(CanGlide, CanFly);
        }
        
        if (_numberFeathers >= _neededFeathersToFly && !CanFly)
        {
            CanFly = true;
            _topDownCharacterMover.UpdateMovementSpeedPlayer(CanGlide, CanFly); 
            _glideController.UpdateCounterFall();
        }
            
    }
}
