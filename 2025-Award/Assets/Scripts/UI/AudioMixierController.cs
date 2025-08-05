using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    // Exit Button が押されたときに発生する UnityEvent
    public UnityEvent onExitButtonClick;
    // BGMSliderの値が変更されたときに発生する　UnityEvent
    public UnityEvent<float> onBGMSliderValueChanged;
    // SESliderの値が変更されたときに発生する　UnityEvent
    public UnityEvent<float> onSESliderValueChanged;

    //Audioミキサーを格納
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider BGMSlider = null;
    [SerializeField] private Slider SESlider = null;
    // SEのテスト再生時のサンプルサウンドを指定します。
    [SerializeField] private AudioClip soundOnTestSE;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        var bgmValue = (float)PlayerPrefs.GetInt("BGM_Value", 3);
        BGMSlider.value = bgmValue;
        var seValue = (float)PlayerPrefs.GetInt("SE_Value", 3);
        SESlider.value = seValue;

        // UnityEvent を追加
        BGMSlider.onValueChanged.AddListener((float value) => { onBGMSliderValueChanged.Invoke(value); });
        SESlider.onValueChanged.AddListener((float value) => { onSESliderValueChanged.Invoke(value); });
    }

    private void Start()
    {
        // スライダーUIの現在値を10段階に補正してから[-80, 0]dBに変換
        audioMixer.SetFloat("BGM", Mathf.Clamp(Mathf.Log10(BGMSlider.value / 10) * 20f, -80f, 0f));
        audioMixer.SetFloat("SE", Mathf.Clamp(Mathf.Log10(SESlider.value / 10) * 20f, -80f, 0f));

        Hide();
    }

    // シーン切り替え時に音量情報を保存します。
    public void OnDestroy()
    {
        PlayerPrefs.SetInt("BGM_Value", (int)BGMSlider.value);
        PlayerPrefs.SetInt("SE_Value", (int)SESlider.value);
        PlayerPrefs.Save();
    }

    // BGMスライダーの値変更によって呼び出されます。
    public void SetBGM(float value)
    {
        //スライダーBGMの現在値を10段階に補正してから[-80, 0]に変換
        var volume = Mathf.Clamp(Mathf.Log10(value / 10) * 20f, -80f, 0f);
        audioMixer.SetFloat("BGM", volume);
    }

    // SEスライダーの値変更によって呼び出されます。
    public void SetSE(float value)
    {
        //スライダーSEの現在値を10段階に補正してから[-80, 0]に変換
        var volume = Mathf.Clamp(Mathf.Log10(value / 10) * 20f, -80f, 0f);
        audioMixer.SetFloat("SE", volume);
        // テスト音声を再生
        audioSource.PlayOneShot(soundOnTestSE);
    }

    // SoundUIを表示します。
    public void Show()
    {
        // 子オブジェクトをすべてアクティブ化
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        BGMSlider.Select();
    }

    // SoundUIUIを非表示します。
    public void Hide()
    {
        // 子オブジェクトをすべて非アクティブ化
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
