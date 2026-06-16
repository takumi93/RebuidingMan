using UnityEngine;

[CreateAssetMenu(fileName = "InputIconData", menuName = "Scriptable Objects/InputIconData")]
// 操作方法が切り替わった時にUIのボタンを変更する際に使うデータ
public class InputIconData : ScriptableObject
{
    public Sprite keyboard;     // キーボード
    public Sprite controller;   // コントローラー
}
