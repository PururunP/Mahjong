using UnityEngine;

// 面子オブジェクト
public class PaiSet
{
    // 牌1
    private GameObject pai1 = null;
    // 牌2
    private GameObject pai2 = null;
    // 牌3
    private GameObject pai3 = null;
    // 牌4
    private GameObject pai4 = null;
    // 副露牌
    private GameObject paiConcealed = null;
    // 和了牌
    private GameObject winningPai = null;

    // 暗刻か
    private bool isMeldedPung = false;
    // 明刻か
    private bool isConcealedPung = false;
    // 暗槓か
    private bool isMeldedkong = false;
    // 明槓か
    private bool isConcealedKong = false;
    // 鳴き順子か
    private bool isCalledChow = false;

    /* コンストラクタ */
    // 順子Ver
    public PaiSet(GameObject pai1, GameObject pai2,
        GameObject pai3, GameObject paiConcealed,
        bool isCalledChow)
    {
        this.pai1 = pai1;
        this.pai2 = pai2;
        this.pai3 = pai3;
        this.paiConcealed = paiConcealed;
        this.isCalledChow = isCalledChow;
    }

    // 刻子Ver
    public PaiSet(GameObject pai1, GameObject pai2,
        GameObject pai3, GameObject paiConcealed,
        bool isMeldedPung, bool isConcealedPung)
    {
        this.pai1 = pai1;
        this.pai2 = pai2;
        this.pai3 = pai3;
        this.paiConcealed = paiConcealed;
        this.isMeldedPung = isMeldedPung;
        this.isConcealedPung = isConcealedPung;
    }

    // 槓子Ver
    public PaiSet(GameObject pai1, GameObject pai2,
        GameObject pai3, GameObject pai4, GameObject paiConcealed,
        bool isMeldedkong, bool isConcealedKong)
    {
        this.pai1 = pai1;
        this.pai2 = pai2;
        this.pai3 = pai3;
        this.pai4 = pai4;
        this.paiConcealed = paiConcealed;
        this.isMeldedkong = isMeldedkong;
        this.isConcealedKong = isConcealedKong;
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
    public GameObject Pai3
    {
        set { this.pai3 = value; }
        get { return this.pai3; }
    }
    public GameObject Pai4
    {
        set { this.pai4 = value; }
        get { return this.pai4; }
    }
    public GameObject PaiConcealed
    {
        set { this.paiConcealed = value; }
        get { return this.paiConcealed; }
    }
    public GameObject WinningPai
    {
        set { this.winningPai = value; }
        get { return this.winningPai; }
    }
    public bool IsMeldedPung
    {
        set { this.isMeldedPung = value; }
        get { return this.isMeldedPung; }
    }
    public bool IsConcealedPung
    {
        set { this.isConcealedPung = value; }
        get { return this.isConcealedPung; }
    }
    public bool IsMeldedkong
    {
        set { this.isMeldedkong = value; }
        get { return this.isMeldedkong; }
    }
    public bool IsConcealedKong
    {
        set { this.isConcealedKong = value; }
        get { return this.isConcealedKong; }
    }
    public bool IsCalledChow
    {
        set { this.isCalledChow = value; }
        get { return this.isCalledChow; }
    }
}
