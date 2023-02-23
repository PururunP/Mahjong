using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 山ゲームオブジェクト操作
public class Yama : MonoBehaviour
{
    // 牌全種
    private List<GameObject> pais;

    // 乱数クラス
    private System.Random random;

    void Awake()
    {
        /* 初期化 */
        this.pais = new List<GameObject>();
        this.random = new System.Random();
    }

    // 牌ゲームオブジェクトをメンバ変数に加える
    public void setPaiGameObjectToMemberVariable()
    {
        // 山ゲームオブジェクト直下のオブジェクト参照
        Transform yamaChild = this.gameObject.GetComponentInChildren<Transform>();

        // メンバにセット
        foreach (Transform t in yamaChild)
        {
            this.pais.Add(t.gameObject);
        }
    }

    // 山からツモる
    public GameObject drawPai()
    {
        // 乱数番目の牌ゲームオブジェクト取得
        GameObject paiGameObj = this.pais[this.random.Next(0, this.pais.Count)];

        // 山からツモった牌ゲームオブジェクトを排除
        this.pais.Remove(paiGameObj);
        return paiGameObj;
    }

    // 特定の牌を指定してツモる
    public GameObject drawPai(Pai pai)
    {
        // 条件にあう牌を探索
        foreach (GameObject paiGameObj in this.pais)
        {
            // 山ゲームオブジェクトのPaiゲームオブジェクトのPaiスクリプト取得
            Pai paiScipt = paiGameObj.GetComponent<PaiGO>().Pai;

            // 各値と一致するかチェック
            if (pai.PaiKind == paiScipt.PaiKind)
            {
                if (pai.PaiCharacters == paiScipt.PaiCharacters)
                {
                    if (pai.PaiNumber == paiScipt.PaiNumber)
                    {
                        if (pai.IsRed == paiScipt.IsRed)
                        {
                            // すべて一致するなら山から除外してゲームオブジェクトを返却
                            this.pais.Remove(paiGameObj);
                            //Debug.Log("残り牌：" + this.pais.Count);
                            return paiGameObj;
                        }
                    }
                }
            }
        }

        // 探索結果がなかったらnull返却
        return null;
    }

    // 引数の牌ゲームオブジェクトを山に追加
    public void addPai(GameObject paiGameObj)
    {
        // nullチェック
        if (paiGameObj != null)
        {
            // nullじゃないなら山に牌ゲームオブジェクトを追加
            this.pais.Add(paiGameObj);
        }
    }
}
