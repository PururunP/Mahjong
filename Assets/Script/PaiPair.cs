using UnityEngine;

// 対子オブジェクト
public class PaiPair
{
    // 牌1
    private GameObject pai1;
    // 牌2
    private GameObject pai2;
    // 和了牌
    private GameObject winningPai = null;

    /* コンストラクタ */
    public PaiPair() { }
    public PaiPair(GameObject pai1, GameObject pai2)
    {
        this.pai1 = pai1;
        this.pai2 = pai2;
    }

    /* セッターゲッター */
    public GameObject Pai1
    {
        set { this.pai1 = value; }
        get { return this.pai1; }
    }
    public GameObject Pai2
    {
        set { this.pai2 = value; }
        get { return this.pai2; }
    }
    public GameObject WinningPai
    {
        set { this.winningPai = value; }
        get { return this.winningPai; }
    }
}
