using UnityEngine;
using UnityEngine.UI;

public class RobotSetUI : MonoBehaviour
{
    // プレイヤーを指定
    [SerializeField] Player player = null;
    // 頭のUIを指定
    [SerializeField] GameObject ImageHead;
    // 胴のUIを指定
    [SerializeField] GameObject ImageBody;
    // 足のUIを指定
    [SerializeField] GameObject ImageLeg;

    // 頭の画像を指定
    [SerializeField] Image IconHeadImage;
    // 胴の画像を指定
    [SerializeField] Image IconBodyImage;
    // 足の画像を指定
    [SerializeField] Image IconLegImage;

    // 頭の役割アイコンを指定
    [SerializeField] Sprite[] IconHead;
    // 胴の役割アイコンを指定
    [SerializeField] Sprite[] IconBody;
    // 足の役割アイコンを指定
    [SerializeField] Sprite[] IconLeg;
    // 役割アイコンを指定
    [SerializeField] Sprite emptyIcon;

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが頭オブジェクトを持っているとき
        if (player.grabHead)
        {
            // 頭アイコンを表示
            ImageHead.SetActive(true);
            // 持っているオブジェクトのタグが該当したときの処理
            if (player.grabHead.CompareTag("Pawn"))
            {
                // アイコンを入れ替え
                IconHeadImage.sprite = IconHead[0];
            }
            else if (player.grabHead.CompareTag("Rook"))
            {
                // アイコンを入れ替え
                IconHeadImage.sprite = IconHead[1];
            }
            else
            {
                // アイコンを入れ替え
                IconHeadImage.sprite = IconHead[2];
            }
        }
        else
        {
            // 頭アイコンを非表示
            ImageHead.SetActive(false);
            // アイコンを入れ替え
            IconHeadImage.sprite = emptyIcon;
        }

        // プレイヤーが胴オブジェクトを持っているとき
        if (player.grabBody)
        {
            // 胴アイコンを表示
            ImageBody.SetActive(true);
            // 持っているオブジェクトのタグが該当したときの処理
            if (player.grabBody.CompareTag("Normal"))
            {
                // アイコンを入れ替え
                IconBodyImage.sprite = IconBody[0];
            }
            else if (player.grabBody.CompareTag("Gun"))
            {
                // アイコンを入れ替え
                IconBodyImage.sprite = IconBody[1];
            }
            else
            {
                // アイコンを入れ替え
                IconBodyImage.sprite = IconBody[2];
            }
        }
        else
        {
            // 胴アイコンを非表示
            ImageBody.SetActive(false);
            // アイコンを入れ替え
            IconBodyImage.sprite = emptyIcon;
        }

        // プレイヤーが足オブジェクトを持っているとき
        if (player.grabLeg)
        {
            // 足アイコンを表示
            ImageLeg.SetActive(true);
            // 持っているオブジェクトのタグが該当したときの処理
            if (player.grabLeg.CompareTag("Walk"))
            {
                // アイコンを入れ替え
                IconLegImage.sprite = IconLeg[0];
            }
            else
            {
                // アイコンを入れ替え
                IconLegImage.sprite = IconLeg[1];
            }
        }
        else
        {
            // 足アイコンを非表示
            ImageLeg.SetActive(false);
            // アイコンを入れ替え
            IconLegImage.sprite = emptyIcon;
        }
    }
}
