using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("SE")]
    // UŒ‚‰¹i•Ší‚ğU‚Á‚½‰¹j
    [SerializeField] private AudioClip _swingSound = null;

    // UŒ‚‰¹iUŒ‚‚ª“–‚½‚Á‚½‰¹j
    [SerializeField] private AudioClip _attackSound = null;

    // E‚Á‚½‰¹
    [SerializeField] private AudioClip _grabSound = null;

    // ƒƒ{ƒbƒg‚ğì¬‚µ‚½‰¹
    [SerializeField] private AudioClip _createSound = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// •Ší‚ğU‚Á‚½‰¹‚ğÄ¶
    /// </summary>
    public void PlaySwing()
    {
        _audioSource.PlayOneShot(_swingSound);
    }

    /// <summary>
    /// UŒ‚‚ª“–‚½‚Á‚½‰¹‚ğÄ¶
    /// </summary>
    public void PlayAttack()
    {
        _audioSource.PlayOneShot(_attackSound);
    }

    /// <summary>
    /// E‚Á‚½‰¹‚ğÄ¶
    /// </summary>
    public void PlayGrab()
    {
        _audioSource?.PlayOneShot(_grabSound);
    }

    /// <summary>
    /// ì¬‚µ‚½‰¹‚ğÄ¶
    /// </summary>
    public void PlayCreate()
    {
        _audioSource.PlayOneShot(_createSound);
    }
}
