using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using UnityEngine.InputSystem;
using TMPro;

public class StageScene : MonoBehaviour
{
    //スクリプトの指定
    //　ポーズUIを指定
    [SerializeField] private PauseUI pauseUI = null;
    // サウンドUIを指定
    [SerializeField] private AudioMixerController soundUI = null;
    // ゲームオーバーUIを指定
    [SerializeField] private GameOverUI gameOverUI = null;
    // ステージクリアUIを指定
    [SerializeField] private GameClearUI stageClearUI = null;
    // オプションUIを指定
    [SerializeField] private OptionUI optionUI = null;
    // UIを指定
    [SerializeField] private IconListUI iconListUI = null;
    // UIを指定
    [SerializeField] private OperationUI operationUI = null;

    Animator animator;

    static readonly int outroId = Animator.StringToHash("Outro");
    public enum SceneState
    {
        // ステージ開始演出中
        Intro,
        // ステージプレイ中
        Play,
        // ゲームオーバーが確定していて演出中
        GameOver,
        // ステージクリアーが確定していて演出中
        StageClear,
    }

    //ステージ開始を初期値に設定
    public SceneState gameState = SceneState.Play;

    public enum OptionState
    {
        // プレイ画面
        Play,
        // ポーズ画面
        Pause,
        // オプション画面
        Option,
        // 音量設定画面
        Sound,
        // 操作説明画面
        Operation,
        // アイコンリスト画面
        IconList,
    }

    public OptionState optionState = OptionState.Play;

    public bool IsPlaying { get; private set; } = false;

    // 現在のシーンを保存するstaticの変数
    public static string nowSceneName;

    

    public static StageScene Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;

        // 現在のシーンを保存
        nowSceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        //playIntroUI.onPlayGame.AddListener(PlayGame);
        // ポーズ画面でのボタン処理
        pauseUI.onResumeButtonClick.AddListener(Resume);
        pauseUI.onRetryButtonClick.AddListener(Retry);
        pauseUI.onTitleButtonClick.AddListener(Exit);
        pauseUI.onOptionButtonClick.AddListener(Option);
        // オプション画面でのボタン処理
        optionUI.onOperatingButtonClick.AddListener(Operation);
        optionUI.onResumeButtonClick.AddListener (OptionExit);
        optionUI.onSoundButtonClick.AddListener(Sound);
        optionUI.onIconListButtonClick.AddListener(IconList);
        // 音量画面でのボタン処理
        soundUI.onExitButtonClick.AddListener(SoundExit);
        // ゲームオーバーでのボタン処理
        gameOverUI.onHomeButtonClick.AddListener(LoadTitleScene);
        gameOverUI.onRetryButtonClick.AddListener(Retry);
        // ゲームクリアでのボタン処理
        stageClearUI.onHomeButtonClick.AddListener(LoadTitleScene);
        stageClearUI.onNextStageButtonClick.AddListener(NextStage);
        // アイコンリストでのボタン処理

        gameState = SceneState.Play;    
        Time.timeScale = 1;
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        // ボタンが押された時
        if (context.started)
        {
            Cancel();
        }
    }

    // ================================================================================
    // 関数名:   Cancel
    // 処理内容: optionStateの状態を参照し処理を変更する
    //          （状態ごとの処理内容は現在のUIを非表示にしてひとつ前のUIを表示）
    // ================================================================================
    public void Cancel()
    {
        // ステージプレイ中の時
        if (gameState == SceneState.Play)
        {
            switch (optionState)
            {
                // プレイ画面の時
                case OptionState.Play:
                    Pause();
                    break;
                // ポーズ画面の時
                case OptionState.Pause:
                    Resume();
                    break;
                // オプション画面の時
                case OptionState.Option:
                    OptionExit();
                    break;
                // 音量設定画面の時
                case OptionState.Sound:
                    SoundExit();
                    break;
                // 操作説明画面の時
                case OptionState.Operation:
                    OperationExit();
                    break;
                // アイコンリスト画面の時
                case OptionState.IconList:
                    IconListExit();
                    break;
            }
        }
    }

    public void PlayGame()
    {
        gameState = SceneState.Play;
        //PlayBGM.Play();
    }

    // シーン遷移
    IEnumerator OnLoadScene(string sceneName)
    {
        // アニメーションが終了するまで1秒待機
        yield return new WaitForSecondsRealtime(1);
        // シーンをロードする
        SceneManager.LoadScene(sceneName);
    }

    // タイトルに戻るボタンを押した際にタイトルへ遷移
    public void LoadTitleScene()
    {
        StartCoroutine(OnLoadScene("Title"));
    }

    public void NextStage()
    {
        var nextStage = int.Parse(nowSceneName) + 1;
        if (int.Parse(nowSceneName) == 4)
        {
            StartCoroutine(OnLoadScene("Title"));
        }
        else
        {
            StartCoroutine(OnLoadScene(nextStage.ToString()));
        }
    }

    // もう一度ボタンを押した際に再度同じシーンを呼び出す
    public void Retry()
    {
        StartCoroutine(OnLoadScene(SceneManager.GetActiveScene().name));
    }

    void LoadNextScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // このステージをゲームオーバーとします。
    public void GameOver()
    {
        // ステージプレイ中のみ
        if (gameState == SceneState.Play)
        {
            gameState = SceneState.GameOver;
            Time.timeScale = 0;
            //PlayBGM.Stop();
            //GameOverBGM.Play();
            // ゲームオーバーUIを表示
            gameOverUI.Show();
        }
    }

    public void StageClear()
    {
        // ステージプレイ中のみ
        if (gameState == SceneState.Play)
        {
            gameState = SceneState.StageClear;
            Time.timeScale = 0;
            //PlayBGM.Stop();
            //GameOverBGM.Play();
            // ゲームクリアUIを表示
            stageClearUI.Show();
        }
    }

    // ================================================================================
    // 関数名:   Pause
    // 処理内容: ゲームの一時停止し、ポーズ画面を表示
    // ================================================================================
    // ゲームの一時停止
    public void Pause()
    {
        pauseUI.Show();
        optionState = OptionState.Pause;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ================================================================================
    // 関数名:   Resume
    // 処理内容: ゲームの一時停止を解除しポーズ画面を非表示
    // ================================================================================
    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.Hide();
        optionState = OptionState.Play;
        Cursor.visible = false;
    }

    // ================================================================================
    // 関数名:   Option
    // 処理内容: オプション画面を表示し、ポーズ画面を非表示
    // ================================================================================
    public void　Option()
    {
        optionUI.Show();
        pauseUI.Hide();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:   OptionExit
    // 処理内容: オプション画面を非表示にしてポーズ画面を表示
    // ================================================================================
    public void OptionExit()
    {
        pauseUI.Show();
        optionUI.Hide();
        optionState = OptionState.Pause;
    }

    // ================================================================================
    // 関数名:   Sound
    // 処理内容: 音量設定画面を表示し、オプション画面を非表示
    // ================================================================================
    // SoundUIを表示
    public void Sound()
    {
        soundUI.Show();
        optionUI.Hide();
        optionState = OptionState.Sound;
    }

    // ================================================================================
    // 関数名:   SoundExit
    // 処理内容: 音量設定画面を非表示にしてオプション画面を表示
    // ================================================================================
    // SoundUIを非表示
    public void SoundExit()
    {
        soundUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:   Operation
    // 処理内容: 操作説明画面を表示し、オプション画面を非表示
    // ================================================================================
    public void Operation()
    {
        operationUI.Show();
        optionUI.Hide();
        optionState = OptionState.Operation;
    }

    // ================================================================================
    // 関数名:   OperationExit
    // 処理内容: 操作説明画面を非表示にしてオプション画面を表示
    // ================================================================================
    public void OperationExit()
    {
        operationUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:   IconList
    // 処理内容: アイコンリスト画面を表示し、オプション画面を非表示
    // ================================================================================
    //IconListを表示
    public void IconList()
    {
        iconListUI.Show();
        optionUI.Hide();
        optionState = OptionState.IconList;
    }

    // ================================================================================
    // 関数名:   IconListExit
    // 処理内容: アイコンリスト画面を非表示にしてオプション画面を表示
    // ================================================================================
    public void IconListExit()
    {
        iconListUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:   Exit
    // 処理内容: タイトルシーンに戻る
    // ================================================================================
    public void Exit()
    {
        StartCoroutine(OnLoadScene("Title"));
    }
}
