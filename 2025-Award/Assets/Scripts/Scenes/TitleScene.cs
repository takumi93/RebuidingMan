using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    //スクリプトの指定
    // タイトルUIを指定
    [SerializeField] private TitleUI _titleUI = null;
    // ステージセレクトUIを指定
    [SerializeField] private StageSelectUI _stageSelectUI = null;

    [SerializeField] private StageFadeUI _stageFadeUI = null;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    private void Start()
    {
        // UnityEvent
        //playIntroUI.onPlayGame.AddListener(PlayGame);
        // タイトル画面でのボタン処理
        _titleUI.onExitButtonClick.AddListener(ExitGame);

        // ステージ選択画面でのボタン処理
        _stageSelectUI.FirstStageRequested.AddListener(() => LoadNextScene("Stage1"));
        _stageSelectUI.SecondStageRequested.AddListener(() => LoadNextScene("Stage2"));
        _stageSelectUI.ThirdStageRequested.AddListener(() => LoadNextScene("Stage3"));
        _stageSelectUI.ForceStageRequested.AddListener(() => LoadNextScene("Stage4"));

        Time.timeScale = 1;

        UIManager.Instance.Push(_titleUI);
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

    /// <summary>
    /// OnLoadNextSceneで指定したシーンを呼び出す
    /// </summary>
    /// <param name="Scene">移動先のシーン名</param>
    public void LoadNextScene(string Scene)
    {
        _stageFadeUI.FadeOut();
        StartCoroutine(OnLoadNextScene(Scene));
    }

    /// <summary>
    /// 指定したシーンに遷移する
    /// </summary>
    /// <param name="sceneName">移動先のシーン名</param>
    /// <returns></returns>
    IEnumerator OnLoadNextScene(string sceneName)
    {
        //seAudio.PlayOneShot(changeSceneAudio);
        //animator.SetTrigger(outroId);

        yield return new WaitForSeconds(2);
        //bgmAudio.Stop();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine(OnExitGame());
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    /// <returns></returns>
    IEnumerator OnExitGame()
    {
        //seAudio.PlayOneShot(changeSceneAudio);
        //animator.SetTrigger(outroId);

        yield return new WaitForSeconds(1);
        //bgmAudio.Stop();
        Application.Quit();
    }
}
