using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    //スクリプトの指定
    // タイトルUIを指定
    [SerializeField] private TitleUI titleUI = null;
    // ステージセレクトUIを指定
    [SerializeField] private StageSelectUI stageSelectUI = null;

    // アニメーター
    private Animator animator;

    // パラメーターID
    static readonly int OutroId = Animator.StringToHash("Outro");

    private void Start()
    {
        // 参照
        animator = GetComponent<Animator>();
        // UnityEvent
        //playIntroUI.onPlayGame.AddListener(PlayGame);
        // タイトル画面でのボタン処理
        //titleUI.onStageSelectButtonClick.AddListener(OpenStageSelect);
        //titleUI.onOptionButtonClick.AddListener(Option);
        titleUI.onExitButtonClick.AddListener(ExitGame);

        // ステージ選択画面でのボタン処理
        stageSelectUI.FirstStageRequested.AddListener(() => LoadNextScene("Stage1"));
        stageSelectUI.SecondStageRequested.AddListener(() => LoadNextScene("Stage2"));
        stageSelectUI.ThirdStageRequested.AddListener(() => LoadNextScene("Stage3"));
        stageSelectUI.ForceStageRequested.AddListener(() => LoadNextScene("Stage4"));

        Time.timeScale = 1;
    }

    private void Update()
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
