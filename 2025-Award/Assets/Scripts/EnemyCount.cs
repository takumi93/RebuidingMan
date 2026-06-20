using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyCount : MonoBehaviour
{
    // 数字スプライト
    [SerializeField] List<Sprite> numberSprites = new List<Sprite>();

    // 表示する Image
    [SerializeField] Image digit0 = null; // 一の位
    [SerializeField] Image digit1 = null;

    int EnemyCountCurrent = 0;
    int EnemyCountTotal = 0;

    void Start()
    {
        EnemyCountTotal = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyCountCurrent = EnemyCountTotal;
        UpdateEnemyCountSprite();
    }

    /// <summary>
    /// 敵の数を変更する
    /// </summary>
    private void UpdateEnemyCountSprite()
    {
        int ones = EnemyCountCurrent % 10;
        int tens = EnemyCountCurrent / 10;

        // 画像が正しく設定されているか確認して設定
        if (digit0 != null && numberSprites.Count > ones)
        {
            digit0.sprite = numberSprites[ones];
        }
        if(digit1 != null && numberSprites.Count > tens)
        {
            digit1.sprite = numberSprites[tens];
        }
    }

    /// <summary>
    /// 敵の追加
    /// </summary>
    public void EnemyIncrease()
    {
        EnemyCountTotal++;
        EnemyCountCurrent++;

        UpdateEnemyCountSprite();
    }

    /// <summary>
    /// 敵の減少
    /// </summary>
    public void EnemyDecrease()
    {
        EnemyCountCurrent--;

        UpdateEnemyCountSprite();

        if (EnemyCountCurrent <= 0)
        {
            StageScene.Instance.StageClear();
        }
    }
}
