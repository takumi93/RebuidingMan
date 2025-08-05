using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OperationUI : MonoBehaviour
{
    void Awake()
    {
        Hide();
    }

    //UI•\Ž¦
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //UI”ñ•\Ž¦
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
