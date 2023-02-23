// 牌オブジェクト
public class Pai
{
    // 牌の種類(萬子、筒子、索子、字牌)
    private PaiStatus.PAIKIND paiKind = PaiStatus.PAIKIND.字牌;

    // 字牌
    private PaiStatus.PAICHARACTERS paiCharacters = PaiStatus.PAICHARACTERS.無;

    // 数字
    private int paiNumber = 0;

    // 赤か
    private bool isRed = false;

    /* コンストラクタ */
    // フルVer
    public Pai(PaiStatus.PAIKIND paiKind, PaiStatus.PAICHARACTERS paiCharacters,
        int paiNumber, bool isRed)
    {
        this.paiKind = paiKind;
        this.paiCharacters = paiCharacters;
        this.paiNumber = paiNumber;
        this.isRed = isRed;
    }
    // 字牌Ver
    public Pai(PaiStatus.PAIKIND paiKind, PaiStatus.PAICHARACTERS paiCharacters)
    {
        this.paiKind = paiKind;
        this.paiCharacters = paiCharacters;
    }
    // 数牌Ver
    public Pai(PaiStatus.PAIKIND paiKind, int paiNumber, bool isRed)
    {
        this.paiKind = paiKind;
        this.paiNumber = paiNumber;
        this.isRed = isRed;
    }

    /* セッターゲッター */
    public PaiStatus.PAIKIND PaiKind
    {
        set { this.paiKind = value; }
        get { return this.paiKind; }
    }
    public PaiStatus.PAICHARACTERS PaiCharacters
    {
        set { this.paiCharacters = value; }
        get { return this.paiCharacters; }
    }
    public int PaiNumber
    {
        set { this.paiNumber = value; }
        get { return this.paiNumber; }
    }
    public bool IsRed
    {
        set { this.isRed = value; }
        get { return this.isRed; }
    }
}
