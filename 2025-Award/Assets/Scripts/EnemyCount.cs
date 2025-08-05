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

    private void FixedUpdate()
    {
        UpdateEnemyCountSprite();
        if (EnemyCountCurrent <= 0)
        {
            StageScene.Instance.StageClear();
        }
    }

    private void UpdateEnemyCountSprite()
    {
        int ones = EnemyCountCurrent % 10;
        int tens = EnemyCountTotal / 10;

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

    public void EnemyIncrease()
    {
        EnemyCountTotal += 1;
        EnemyCountCurrent += 1;
    }

    public void EnemyDecrease()
    {
        EnemyCountCurrent -= 1;
    }
}
