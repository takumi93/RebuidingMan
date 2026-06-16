using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // UIを管理するためのデータ構造
    private Stack<BaseUI> _uiStack = new Stack<BaseUI>();

    private void Awake()
    {
        // なかったら削除
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// UIを表示したときにUIのデータに追加する
    /// </summary>
    /// <param name="ui">追加するUI</param>
    public void Push(BaseUI ui)
    {
        if (CurrentUI() == ui) return;

        // 今のUIを非表示にする
        if (_uiStack.Count > 0)
        {
            _uiStack.Peek().OnUnFocus();
        }

        // UIのデータに追加する
        _uiStack.Push(ui);

        ui.Show();
        ui.OnFocus();
    }

    /// <summary>
    /// UIを非表示する時にUIデータの最初にあるUIを実行する
    /// </summary>
    public void Pop()
    {
        // UIを管理してるデータの中身がないときは無視
        if (_uiStack.Count == 0) return;

        BaseUI current = _uiStack.Pop();

        current.OnUnFocus();
        current.Hide();

        if (_uiStack.Count > 0)
        {
            _uiStack.Peek().OnFocus();
        }
    }

    /// <summary>
    /// 現在のUIを取得する
    /// </summary>
    /// <returns>現在のUI、なければnullを返す</returns>
    public BaseUI CurrentUI()
    {
        return _uiStack.Count > 0 ? _uiStack.Peek() : null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool HasOpenUI()
    {
        return _uiStack.Count > 0;
    }

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (!focus) return;

    //    if(CurrentUI() == null) {
    //        // マウスカーソルを非表示に設定
    //        Cursor.visible = false;
    //        Cursor.lockState = CursorLockMode.Locked;
    //    }
    //}
}
