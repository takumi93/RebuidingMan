using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioMixerController : BaseUI
{
    // Exit Button が押されたときに発生する UnityEvent
    public UnityEvent ExitRequested;
    // BGMSliderの値が変更されたときに発生する　UnityEvent
    public UnityEvent<float> BGMValueChangedRequested;
    // SESliderの値が変更されたときに発生する　UnityEvent
    public UnityEvent<float> SEValueChangedRequested;

    //Audioミキサーを格納
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _BGMSlider = null;
    [SerializeField] private Slider _SESlider = null;

    // SEのテスト再生時のサンプルサウンドを指定します。
    [SerializeField] private AudioClip soundOnTestSE;

    private AudioSource _audioSource;

    protected override void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        var bgmValue = (float)PlayerPrefs.GetInt("BGM_Value", 3);
        _BGMSlider.value = bgmValue;
        var seValue = (float)PlayerPrefs.GetInt("SE_Value", 3);
        _SESlider.value = seValue;

        // UnityEvent を追加
        _BGMSlider.onValueChanged.AddListener((float value) => { BGMValueChangedRequested.Invoke(value); });
        _SESlider.onValueChanged.AddListener((float value) => { SEValueChangedRequested.Invoke(value); });
    }

    protected override void Start()
    {
        base.Start();

        // スライダーUIの現在値を10段階に補正してから[-80, 0]dBに変換
        _audioMixer.SetFloat("BGM", Mathf.Clamp(Mathf.Log10(_BGMSlider.value / 10) * 20f, -80f, 0f));
        _audioMixer.SetFloat("SE", Mathf.Clamp(Mathf.Log10(_SESlider.value / 10) * 20f, -80f, 0f));

        Hide();
    }

    // シーン切り替え時に音量情報を保存します。
    public void OnDestroy()
    {
        PlayerPrefs.SetInt("BGM_Value", (int)_BGMSlider.value);
        PlayerPrefs.SetInt("SE_Value", (int)_SESlider.value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// BGMスライダーの値変更によって呼び出されます。
    /// </summary>
    /// <param name="value"></param>
    public void SetBGM(float value)
    {
        //スライダーBGMの現在値を10段階に補正してから[-80, 0]に変換
        var volume = Mathf.Clamp(Mathf.Log10(value / 10) * 20f, -80f, 0f);
        _audioMixer.SetFloat("BGM", volume);
    }

    /// <summary>
    /// SEスライダーの値変更によって呼び出されます。
    /// </summary>
    /// <param name="value"></param>
    public void SetSE(float value)
    {
        //スライダーSEの現在値を10段階に補正してから[-80, 0]に変換
        var volume = Mathf.Clamp(Mathf.Log10(value / 10) * 20f, -80f, 0f);
        _audioMixer.SetFloat("SE", volume);
        // テスト音声を再生
        _audioSource.PlayOneShot(soundOnTestSE);
    }

    public override void Show()
    {
        base.Show();

        _BGMSlider.Select();
    }
}
