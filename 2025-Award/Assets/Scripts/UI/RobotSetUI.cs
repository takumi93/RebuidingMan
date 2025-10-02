using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotSetUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

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
        if (playerInventory.HasHead)
        {
            // 頭アイコンを表示
            ImageHead.SetActive(true);

            PartsData headPart = playerInventory.GetPartOfType(PartsType.Head);

            if (headPart is HeadData headData)
            {
                if (headData.headType == HeadType.Pawn)
                {
                    // アイコンを入れ替え
                    IconHeadImage.sprite = IconHead[0];
                } else if (headData.headType == HeadType.Rook)
                {
                    // アイコンを入れ替え
                    IconHeadImage.sprite = IconHead[1];
                }
                else if (headData.headType == HeadType.Knight)
                {
                    // アイコンを入れ替え
                    IconHeadImage.sprite = IconHead[2];
                }
            }
        }
        else
        {
            // 頭アイコンを非表示
            ImageHead.SetActive(false);
            // アイコンを入れ替え
            IconHeadImage.sprite = emptyIcon;
        }

        // プレイヤーが頭オブジェクトを持っているとき
        if (playerInventory.HasBody)
        {
            // 胴アイコンを表示
            ImageBody.SetActive(true);

            PartsData bodyPart = playerInventory.GetPartOfType(PartsType.Body);

            if (bodyPart is BodyData bodyData)
            {
                if (bodyData.bodyType == BodyType.Normal)
                {
                    // アイコンを入れ替え
                    IconBodyImage.sprite = IconBody[0];
                }
                else if (bodyData.bodyType == BodyType.Gun)
                {
                    // アイコンを入れ替え
                    IconBodyImage.sprite = IconBody[1];
                }
                else if (bodyData.bodyType == BodyType.Axe)
                {
                    // アイコンを入れ替え
                    IconBodyImage.sprite = IconBody[2];
                }
            }
        }
        else
        {
            // 胴アイコンを非表示
            ImageBody.SetActive(false);
            // アイコンを入れ替え
            IconBodyImage.sprite = emptyIcon;
        }

        // プレイヤーが頭オブジェクトを持っているとき
        if (playerInventory.HasLeg)
        {
            // 足アイコンを表示
            ImageLeg.SetActive(true);

            PartsData legPart = playerInventory.GetPartOfType(PartsType.Leg);

            if (legPart is LegData legData)
            {
                if (legData.legType == LegType.Normal)
                {
                    // アイコンを入れ替え
                    IconLegImage.sprite = IconLeg[0];
                }
                else if (legData.legType == LegType.Caterpillar)
                {
                    // アイコンを入れ替え
                    IconLegImage.sprite = IconLeg[1];
                }
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
