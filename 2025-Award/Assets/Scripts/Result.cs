using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Result : MonoBehaviour
{
    // 数字の画像を指定
    [SerializeField] private Sprite[] numbers = new Sprite[10];

    // 数値を表示するImageを指定
    [SerializeField] private Image[] values = null;

    // ボタンを指定
    [SerializeField] private Button titleButton = null;
    [SerializeField] private Button retryButton = null;
    [SerializeField] private Button nextStageButton = null;
    // 最終ステージクリア後のボタンの配置
    [SerializeField] private Vector3 titleAnchoredPosition = new(-325f, 150f, 0f);
    [SerializeField] private Vector3 retryAnchoredPosition = new(325f, 150f, 0f);

    // それぞれのステージ名を指定
    [SerializeField] private string firstStageName;
    [SerializeField] private string secondStageName;
    [SerializeField] private string thirdStageName;

    // オーディオ
    [SerializeField] private AudioSource bgmAudio = null;
    [SerializeField] private AudioSource seAudio = null;
    [SerializeField] private AudioClip changeSceneAudio = null;

    // ランク画像
    [SerializeField] private Sprite[] lanks = new Sprite[3];
    // ランクを表示するイメージを指定
    [SerializeField] private Image lankImage = null;

    // クリアしたステージ画像
    [SerializeField] private Sprite[] clearStage = new Sprite[3];
    // ステージ名を表示するイメージを指定
    [SerializeField] private Image clearStageImage = null;

    // 各ステージごとのランクタイム
    [SerializeField] private float stage1TimeS;
    [SerializeField] private float stage1TimeA;
    [SerializeField] private float stage2TimeS;
    [SerializeField] private float stage2TimeA;
    [SerializeField] private float stage3TimeS;
    [SerializeField] private float stage3TimeA;

    // 位置を変更するための変数
    RectTransform titleRectTransform;
    RectTransform retryRectTransform;

    // 次のステージを保存するための変数
    private string nextStage;

    // アニメーター
    private Animator animator;

    // パラメータID
    static readonly int outroId = Animator.StringToHash("Outro");

    // 前のシーンの名前を保存する変数
    public string BeforeSceneName { get; private set; }

    private void Awake()
    {
        // アニメーターを参照
        animator = GetComponent<Animator>();

        // 直前のステージの名前を読み込む
        //BeforeSceneName = StageScene.GetSceneName();

        // コンポーネントを参照
        titleRectTransform = titleButton.GetComponent<RectTransform>();
        retryRectTransform = retryButton.GetComponent<RectTransform>();
    }

    private void Start()
    {
        // 保存しているクリアタイムを取得
        var ClearTime = PlayerPrefs.GetFloat("ClearTimeKey", 0);

        // 最終ステージの場合次のステージのボタンを消す
        if (BeforeSceneName == thirdStageName)
        {
            nextStageButton.gameObject.SetActive(false);
        }

        // 直前のシーンによって変数に入れるものを変える、引数を変えてランクの関数を呼び出す、クリアしたステージ数を表示する
        if (BeforeSceneName == firstStageName)
        {
            nextStage = secondStageName;
            Lank(stage1TimeS, stage1TimeA, ClearTime);
            clearStageImage.sprite = clearStage[0];
        }
        else if (BeforeSceneName == secondStageName)
        {
            nextStage = thirdStageName;
            Lank(stage2TimeS, stage2TimeA, ClearTime);
            clearStageImage.sprite = clearStage[1];
        }
        else if (BeforeSceneName == thirdStageName)
        {
            Lank(stage3TimeS, stage3TimeA, ClearTime);
            clearStageImage.sprite = clearStage[2];

            // 最終ステージだけボタンの配置を変える
            SetButtonAnchorAndPosition();
        }

        // 分単位の計算
        var minutes = Mathf.FloorToInt(ClearTime / 60);
        for (int i = 0; i < 2; i++)
        {
            values[i].sprite = numbers[minutes % 10];
            minutes /= 10;
        }

        // 秒単位の計算
        var playTime = Mathf.FloorToInt(ClearTime % 60 * 100);
        for (int index = 2; index < values.Length; index++)
        {
            values[index].sprite = numbers[playTime % 10];
            playTime /= 10;
        }

        retryButton.Select();
    }

    // 最終ステージの実ボタンの配置を変える
    public void SetButtonAnchorAndPosition()
    {
        titleRectTransform.anchorMin = new Vector3(0.5f, 0f);
        titleRectTransform.anchorMax = new Vector3(0.5f, 0f);

        titleRectTransform.anchoredPosition3D = titleAnchoredPosition;
        retryRectTransform.anchoredPosition3D = retryAnchoredPosition;
    }

    // ランクを表示
    public void Lank(float sLankTime, float aLankTime, float clearTime)
    {
        if (clearTime <= sLankTime)
        {
            lankImage.sprite = lanks[0];
        }
        else if (clearTime <= aLankTime)
        {
            lankImage.sprite = lanks[1];
        }
        else if (clearTime > aLankTime)
        {
            lankImage.sprite = lanks[2];
        }
    }

    // ボタンが押されたときにタイトルか次のステージを読み込む
    public void LoadTitleScene()
    {
        StartCoroutine(OnLoadScene("TitleScene"));
    }
    public void LoadNextScene()
    {
        StartCoroutine(OnLoadScene(nextStage));
    }
    IEnumerator OnLoadScene(string sceneName)
    {
        seAudio.PlayOneShot(changeSceneAudio);

        animator.SetTrigger(outroId);

        yield return new WaitForSeconds(1f);

        bgmAudio.Stop();

        SceneManager.LoadScene(sceneName);
    }

    // リトライボタンが押されたときに直前のシーンを読み込む
    public void LoadRetryScene()
    {
        StartCoroutine(OnLoadBeforeScene());
    }
    IEnumerator OnLoadBeforeScene()
    {
        // 直前のシーンを保存
        //BeforeSceneName = StageScene.GetSceneName();

        seAudio.PlayOneShot(changeSceneAudio);

        animator.SetTrigger(outroId);

        yield return new WaitForSeconds(1f);

        bgmAudio.Stop();

        SceneManager.LoadScene(BeforeSceneName);
    }
}


