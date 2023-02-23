// 点数ステータス
public class MahjongScoreStatus
{
    // 子ロン点数
    private int koRon = 0;
    // 親ロン点数
    private int oyaRon = 0;
    // 子ツモ点数被子
    private int koTumoChild = 0;
    // 子ツモ点数被親
    private int koTumoParent = 0;

    /* コンストラクタ */
    public MahjongScoreStatus() { }
    public MahjongScoreStatus(int koRon, int oyaRon, int koTumoChild, int koTumoParent)
    {
        this.koRon = koRon;
        this.oyaRon = oyaRon;
        this.koTumoChild = koTumoChild;
        this.koTumoParent = koTumoParent;
    }

    /* セッターゲッター */
    public int KoRon
    {
        set { this.koRon = value; }
        get { return this.koRon; }
    }
    public int OyaRon
    {
        set { this.oyaRon = value; }
        get { return this.oyaRon; }
    }
    public int KoTumoChild
    {
        set { this.koTumoChild = value; }
        get { return this.koTumoChild; }
    }
    public int KoTumoParent
    {
        set { this.koTumoParent = value; }
        get { return this.koTumoParent; }
    }
}
