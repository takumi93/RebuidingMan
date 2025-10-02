using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StageScene;

public class TitleScene : MonoBehaviour
{
    //スクリプトの指定
    // タイトルUIを指定
    [SerializeField] private TitleUI titleUI = null;
    // サウンドUIを指定
    [SerializeField] private AudioMixerController soundUI = null;
    // オプションUIを指定
    [SerializeField] private OptionUI optionUI = null;
    // アイコンリストUIを指定
    [SerializeField] private IconListUI iconListUI = null;
    // 操作説明UIを指定
    [SerializeField] private OperationUI operationUI = null;
    // ステージセレクトUIを指定
    [SerializeField] private StageSelectUI stageSelectUI = null;

    // アニメーター
    private Animator animator;

    // StartボタンまたはEscapeボタンを押した時に指定のUIを表示するためUIごとに状態を指定
    public enum OptionState
    {
        // タイトル画面
        Title,
        // ステージ選択画面
        StageSelect,
        // オプション画面
        Option,
        // 音量設定画面
        Sound,
        // 操作説明画面
        Operation,
        // アイコンリスト画面
        IconList,
    }

    public OptionState optionState = OptionState.Title;

    // パラメーターID
    static readonly int OutroId = Animator.StringToHash("Outro");

    private void Start()
    {
        // 参照
        animator = GetComponent<Animator>();
        // UnityEvent
        //playIntroUI.onPlayGame.AddListener(PlayGame);
        // タイトル画面でのボタン処理
        titleUI.onStageSelectButtonClick.AddListener(OpenStageSelect);
        titleUI.onOptionButtonClick.AddListener(Option);
        titleUI.onExitButtonClick.AddListener(ExitGame);
        // オプション画面でのボタン処理
        optionUI.onOperatingButtonClick.AddListener(Operation);
        optionUI.onResumeButtonClick.AddListener(OptionExit);
        optionUI.onSoundButtonClick.AddListener(Sound);
        optionUI.onIconListButtonClick.AddListener(IconList);
        // 音量画面でのボタン処理

        // ステージ選択画面でのボタン処理
        stageSelectUI.OnFirstStageButtonClick.AddListener(() => LoadNextScene("Stage1"));
        stageSelectUI.OnSecondStageButtonClick.AddListener(() => LoadNextScene("Stage2"));
        stageSelectUI.OnThirdStageButtonClick.AddListener(() => LoadNextScene("Stage3"));
        stageSelectUI.OnForceStageButtonClick.AddListener(() => LoadNextScene("Stage4"));

        Time.timeScale = 1;
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // StartボタンまたはEscapeボタンを押した時の処理
    public void OnCancel(InputAction.CallbackContext context)
    {
        // 指定されたボタンを押した時
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
        switch (optionState)
        {
            // タイトル画面
            case OptionState.Title:
                break;
            // ステージ選択画面
            case OptionState.StageSelect:
                CloseStageSelect();
                break;
            // オプション画面
            case OptionState.Option:
                OptionExit();
                break;
            // 音量設定画面
            case OptionState.Sound:
                SoundExit();
                break;
            // 操作説明画面
            case OptionState.Operation:
                OperationExit();
                break;
            // アイコンリスト画面
            case OptionState.IconList:
                IconListExit();
                break;
        }
    }

    // ================================================================================
    // 関数名:     Option
    // 処理内容:   オプション画面を表示し、タイトル画面を非表示
    // ================================================================================
    public void Option()
    {
        optionUI.Show();
        titleUI.Hide();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:     OptionExit
    // 処理内容:   オプション画面を非表示にし、タイトル画面を表示
    // ================================================================================
    public void OptionExit()
    {
        titleUI.Show();
        optionUI.Hide();
        optionState = OptionState.Title;
    }

    // ================================================================================
    // 関数名:     Sound
    // 処理内容:   音量設定画面を表示し、オプション画面を非表示
    // ================================================================================
    public void Sound()
    {
        soundUI.Show();
        optionUI.Hide();
        optionState = OptionState.Sound;
    }

    // ================================================================================
    // 関数名:     SoundExit
    // 処理内容:   音量設定画面を非表示にし、オプション画面を表示
    // ================================================================================
    public void SoundExit()
    {
        soundUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:     Operation
    // 処理内容:   操作説明画面を表示し、オプション画面を非表示
    // ================================================================================
    public void Operation()
    {
        operationUI.Show();
        optionUI.Hide();
        optionState = OptionState.Operation;
    }

    // ================================================================================
    // 関数名:     OperationExit
    // 処理内容:   操作説明画面を非表示にし、オプション画面を表示
    // ================================================================================
    public void OperationExit()
    {
        operationUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:     IconList
    // 処理内容:   アイコンリスト画面を表示し、オプション画面を非表示
    // ================================================================================
    //IconListを表示
    public void IconList()
    {
        iconListUI.Show();
        optionUI.Hide();
        optionState = OptionState.IconList;
    }

    // ================================================================================
    // 関数名:     IconListExit
    // 処理内容:   アイコンリスト画面を非表示にし、オプション画面を表示
    // ================================================================================
    public void IconListExit()
    {
        iconListUI.Hide();
        optionUI.Show();
        optionState = OptionState.Option;
    }

    // ================================================================================
    // 関数名:     OpenStageSelect
    // 処理内容:   ステージ選択画面を表示し、タイトル画面を非表示
    // ================================================================================
    public void OpenStageSelect()
    {
        stageSelectUI.Show();
        titleUI.Hide();
        optionState = OptionState.StageSelect;
    }

    // ================================================================================
    // 関数名:     CloseStageSelect
    // 処理内容:   ステージ選択画面を非表示にし、タイトル画面を表示
    // ================================================================================
    // ステージセレクトUIを非表示
    public void CloseStageSelect()
    {
        titleUI.Show();
        stageSelectUI.Hide();
        optionState = OptionState.Title;
    }

    // ================================================================================
    // 関数名:     LoadNextScene
    // 処理内容:   OnLoadNextSceneを呼び出す
    // Scene:      シーン名
    // ================================================================================
    public void LoadNextScene(string Scene)
    {
        StartCoroutine(OnLoadNextScene(Scene));
    }

    // ================================================================================
    // 関数名:     OnLoadNextScene
    // 処理内容:   指定するシーンに遷移する
    // Scene:       シーン名
    // ================================================================================
    IEnumerator OnLoadNextScene(string sceneName)
    {
        //seAudio.PlayOneShot(changeSceneAudio);
        //animator.SetTrigger(outroId);

        yield return new WaitForSeconds(1);
        //bgmAudio.Stop();
        SceneManager.LoadScene(sceneName);
    }

    // ================================================================================
    // 関数名:     ExitGame
    // 処理内容:   OnExitGameを呼び出す
    // ================================================================================
    //ゲームの終了
    public void ExitGame()
    {
        StartCoroutine(OnExitGame());
    }

    // ================================================================================
    // 関数名:     OnExitGame
    // 処理内容:   ゲームを終了する
    // ================================================================================
    IEnumerator OnExitGame()
    {
        //seAudio.PlayOneShot(changeSceneAudio);
        //animator.SetTrigger(outroId);

        yield return new WaitForSeconds(1);
        //bgmAudio.Stop();
        Application.Quit();
    }
}
