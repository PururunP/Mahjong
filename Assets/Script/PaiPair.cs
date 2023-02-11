using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 対子オブジェクト
public class PaiPair : MonoBehaviour
{
    // 牌1
    private GameObject pai1;
    // 牌2
    private GameObject pai2;
    // 和了牌
    private GameObject winningPai = null;

    /* コンストラクタ */
    public PaiPair(GameObject pai1, GameObject pai2)
    {
        this.pai1 = pai1;
        this.pai2 = pai2;
    }

    /* セッターゲッター */
    public GameObject Pai1 { set; get; }
    public GameObject Pai2 { set; get; }
    public GameObject WinningPai { set; get; }
}
