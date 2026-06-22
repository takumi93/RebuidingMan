using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageScene : MonoBehaviour
{
    public static StageScene Instance { get; private set; } = null;
    
    public enum SceneState
    {
        Intro,          // ステージ開始演出中

        Play,           // ステージプレイ中

        GameOver,       // ゲームオーバーが確定していて演出中

        StageClear,     // ステージクリアーが確定していて演出中
    }

    [Header("ゲームの状態を管理")]
    //ステージ開始を初期値に設定
    public SceneState gameState = SceneState.Play;

    [Header("UI")]
    // プレイUIを指定
    [SerializeField] private PlayUI _playUI;
    [SerializeField] private StageFadeUI _stageFadeUI;
    //　ポーズUIを指定
    [SerializeField] private PauseUI _pauseUI;
    // ゲームオーバーUIを指定
    [SerializeField] private GameOverUI _gameOverUI;
    // ステージクリアUIを指定
    [SerializeField] private GameClearUI _stageClearUI;

    [Header("味方の設定")]
    // 味方になった時に必要な値
    [SerializeField] private PatrolRoute _allyRoute;

    public PatrolRoute AllyRoute => _allyRoute;                     // 味方の巡回ルート

    [SerializeField] public GameObject GuardianPoint;               // 護衛ポジション

    [SerializeField] public GameObject GuardianTransform;           // 護衛ポジションの親

    // 現在のシーンを保存するstaticの変数
    public static string nowSceneName;

    private void Awake()
    {
        Instance = this;

        // 現在のシーンを保存
        nowSceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        // Intro状態
        gameState = SceneState.Intro;

        // フェードが終了した通知を受け取る
        _playUI.OnFadeInFinish.AddListener(PlayGame);

        InputManager.Instance.DisablePlayerInput();

        // ポーズ画面でのボタン処理
        _pauseUI.RetryRequested.AddListener(Retry);
        _pauseUI.TitleRequested.AddListener(LoadTitleScene);

        // ゲームオーバーでのボタン処理
        _gameOverUI.HomeRequested.AddListener(LoadTitleScene);
        _gameOverUI.RetryRequested.AddListener(Retry);

        // ゲームクリアでのボタン処理
        _stageClearUI.HomeRequested.AddListener(LoadTitleScene);
        _stageClearUI.NextStageRequested.AddListener(NextStage);
    }

    private void Update()
    {
        UpdateCursor();
    }

    public void PlayGame()
    {
        gameState = SceneState.Play;
        Time.timeScale = 1;

        InputManager.Instance.EnablePlayerInput();

        //PlayBGM.Play();
    }

    /// <summary>
    /// カーソルの表示非表示の管理
    /// </summary>
    private void UpdateCursor()
    {
        if (!UIManager.Instance.HasOpenUI())
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// 指定のシーンに遷移
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator OnLoadScene(string sceneName)
    {
        // アニメーションが終了するまで1秒待機
        yield return new WaitForSecondsRealtime(1);
        // シーンをロードする
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// タイトルに戻るボタンを押した際にタイトルへ遷移
    /// </summary>
    public void LoadTitleScene()
    {
        StartCoroutine(OnLoadScene("Title"));
    }

    /// <summary>
    /// 自分のシーンを参照し次のレベルのシーンに遷移
    /// </summary>
    public void NextStage()
    {
        if(nowSceneName == "Stage1")
        {
            StartCoroutine(OnLoadScene("Stage2"));
        }else if (nowSceneName == "Stage2")
        {
            StartCoroutine(OnLoadScene("Stage3"));
        }
        else if (nowSceneName == "Stage3")
        {
            StartCoroutine(OnLoadScene("Title"));
        }
        else
        {
            StartCoroutine(OnLoadScene("Title"));
        }
    }

    /// <summary>
    /// リトライ
    /// </summary>
    public void Retry()
    {
        Debug.Log(UIManager.Instance.GetInstanceID());

        StartCoroutine(OnLoadScene(SceneManager.GetActiveScene().name));
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        // ステージプレイ中のみ
        if (gameState == SceneState.Play)
        {
            gameState = SceneState.GameOver;
            //PlayBGM.Stop();
            //GameOverBGM.Play();
            // ゲームオーバーUIを表示
            UIManager.Instance.Push(_gameOverUI);
        }
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    public void StageClear()
    {
        // ステージプレイ中のみ
        if (gameState == SceneState.Play)
        {
            gameState = SceneState.StageClear;
            //PlayBGM.Stop();
            //GameOverBGM.Play();
            // ゲームクリアUIを表示
            UIManager.Instance.Push(_stageClearUI);
        }
    }
}
