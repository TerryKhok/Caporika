using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/**
 *  @brief 	プレイヤーの管理
 *  
 *  @memo   ・残機の管理
 *          ・生成するマトリョーシカ
 *          ・死ぬときの処理
*/
public class MatryoshkaManager : MonoBehaviour
{
    public int maxLife;                     // 残機
    public GameObject[] matryoshkaPrefabes; // 生成するマトリョーシカのプレハブ

    private int currentLife = 0;            // 現在の残機

    // Start is called before the first frame update
    void Start()
    {
        // 残機をセット
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        // 残機が0の時
        if(currentLife<=0)
        {
            // ゲームオーバー
            GameOver();
        }
    }


    /**
     *  @brief 	残機を増やす
    */
    public void AddLife()
    {
        if(currentLife<= maxLife)
        {
            currentLife++;
        }
    }

    /**
     *  @brief  残機を減らす
     *  @return int 現在の残機
    */
    public int LoseLife()
    {
        if (currentLife >0)
        {
            currentLife--;
        }
        return currentLife;
    }

    /**
     *  @brief  指定のマトリョーシカを生成
     *  @param  int         _index          生成するサイズ(要素番号)
     *  @return GameObject  createPrefab    生成したオブジェクト
    */
    public GameObject InstanceMatryoshka(int _index)
    {
        GameObject createPrefab = null;
        if(_index >0)
        {
            createPrefab = Instantiate(matryoshkaPrefabes[_index - 1]);
        }
        return createPrefab;
    }

    /**
     *  @brief  死んだときの処理
    */
    private void GameOver()
    {
        Debug.Log("GameOver");
    }
}
