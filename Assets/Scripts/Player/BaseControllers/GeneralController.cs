using UnityEngine;

public class GeneralController : MonoBehaviour
{
    private float[] _valuesMaterial = {-0.24f, -0.137f, 0, 0.085f, 0.138f, 0.24f};
    [SerializeField] private GameObject[] _feathers = new GameObject[6]; 

    public bool CanHighJump { get; private set; }
    public bool CanFly { get; private set; }
    public bool CanGlide { get; private set; }

    [Header("High jump mechanic")] [SerializeField]
    private TopDownCharacterMover _topDownCharacterMover; 
    [SerializeField] private int _neededFeathersToHighJump = 1;
    
    [Header("Flight mechanic")]
    [SerializeField] private int _neededFeathersToFly = 3;

    [Header("Glide mechanic")] [SerializeField]
    private GlideController _glideController; 
    [SerializeField] private int _neededFeathersToGlide = 2;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundCompleteQuest;
    [SerializeField] private float _volumeCompleteQuest; 
    
    // Visual feathers variables
    [SerializeField] private GameObject _crowRenderer; 
    private Material _crowMaterialFeathers;
    private float _valueMaterialFeather; 

    private int _numberFeathers;
    public int NumberFeathers => _numberFeathers;

    private void Start()
    {
        _crowMaterialFeathers = _crowRenderer.GetComponent<SkinnedMeshRenderer>().material;
        for (int i = 0; i < _feathers.Length; i++)
        {
            _feathers[i].SetActive(false);
        }
        UpdateFeatherLooking();
    }

    private void Update()
    {
        UpdateMechanics();
    }

    public void GainFeather(QuestController.QuestFollowing questFollowing)
    {
        _numberFeathers++;
        UpdateFeatherLooking();
        UpdateUIFeatherLooking(questFollowing);
        PlaySoundCompleteQuest(); 
    }

    private void UpdateFeatherLooking()
    {
        // Update visual on the crow 
        _valueMaterialFeather = _valuesMaterial[_numberFeathers]; 
        _crowMaterialFeathers.SetFloat("_WingsAppear", _valueMaterialFeather);
    }

    private void PlaySoundCompleteQuest()
    {
        _audioSource.PlayOneShot(_soundCompleteQuest, _volumeCompleteQuest);
    }

    private void UpdateUIFeatherLooking(QuestController.QuestFollowing questFollowing)
    {
        _feathers[(int) questFollowing].SetActive(true);
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
