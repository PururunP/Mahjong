using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 牌問題出題セット
public class PaiQ : MonoBehaviour
{
    // 符テキスト
    [SerializeField]
    private Text textPoints;
    // 翻テキスト
    [SerializeField]
    private Text textDoubles;
    // ロン子テキスト
    [SerializeField]
    private Text textKoRon;
    // ロン親テキスト
    [SerializeField]
    private Text textOyaRon;
    // ツモ子テキスト
    [SerializeField]
    private Text textKoTumo;
    // ロン親テキスト
    [SerializeField]
    private Text textOyaTumo;

    // ツモロンドロップダウン
    [SerializeField]
    private Dropdown drdTumoOrRon;
    // 自風ドロップダウン
    [SerializeField]
    private Dropdown drdOwnWind;
    // 場風ドロップダウン
    [SerializeField]
    private Dropdown drdRoundWind;

    // 翻数ドロップダウン
    [SerializeField]
    private Dropdown drdDoubles;

    // 面子リスト
    private List<PaiSet> paiSetList = null;
    // 頭の対子
    private PaiPair paiPair = null;

    // 七対子用の対子リスト
    private List<PaiPair> paiPair7List = null;

    // ツモしたか
    private bool isSelfDrawWin = false;
    // 門前ロン和了か
    private bool isConcealedRon = false;

    // 牌整理スクリプト
    private PaiOrganizer paiOrganizer;
    // 乱数
    private System.Random random;

    void Awake()
    {
        /* 初期化 */
        this.paiSetList = new List<PaiSet>();
        this.paiPair = new PaiPair();
        this.paiPair7List = new List<PaiPair>();
        this.paiOrganizer = this.gameObject.GetComponent<PaiOrganizer>();
        this.random = new System.Random();
    }

    // 点数計算用の牌を揃える
    public void setPai()
    {
        // 手牌があれば山に戻す
        this.paiOrganizer.addPaiSetListToYama(this.paiSetList);
        this.paiOrganizer.addPaiPairListToYama(this.paiPair7List);
        this.paiOrganizer.addPaiPairToYama(this.paiPair);

        // オブジェクトを初期化
        this.paiSetList = new List<PaiSet>();
        this.paiPair = new PaiPair();
        this.paiPair7List = new List<PaiPair>();

        // 手牌の初期座標
        float initXPai = -5.0f, initYPai = 0.0f, initZPai = 0.0f;

        // 1/10で七対子化 // TODO 基本形テスト中
        if (false && this.random.Next(0, 10) == 0)
        {
            // 七対子のカウンター
            int count = 0;

            // 対子を7セット取得してリストに追加
            while (count < 7)
            {
                // 対子取得
                PaiPair pair = this.paiOrganizer.alignPaiPair();

                // 対子の片方の牌のPaiスクリプト取得
                Pai paiSctipt1 = pair.Pai1.GetComponent<PaiGO>().Pai;

                // 対子かぶりフラグ
                bool isPairExist = false;

                // 対子が被ってないかを確認
                foreach (PaiPair p in this.paiPair7List)
                {
                    // リストの対子の片方の牌のPaiスクリプト取得
                    Pai paiScript2 = p.Pai1.GetComponent<PaiGO>().Pai;

                    // オブジェクトのメンバを比較
                    if (paiSctipt1.PaiKind == paiScript2.PaiKind &&
                        paiSctipt1.PaiCharacters == paiScript2.PaiCharacters &&
                        paiSctipt1.PaiNumber == paiScript2.PaiNumber)
                    {
                        // 一致したら山に牌を戻して対子取得し直し
                        this.paiOrganizer.addPaiPairToYama(pair);
                        isPairExist = true;
                        break;
                    }
                }
                // 対子かぶりがあったらやりなおし
                if (isPairExist)
                {
                    continue;
                }
                // 対子をリストに追加
                this.paiPair7List.Add(pair);
                // カウンタをインクリメント
                count++;
            }

            /* 和了牌を決定 */
            // 対子からランダムに決定
            int randNumber = this.random.Next(0, this.paiPair7List.Count);
            this.paiPair7List[randNumber].WinningPai = this.paiPair7List[randNumber].Pai1;

            // 和了牌を含む対子とそれ以外に分ける
            List<PaiPair> paiPairList = new List<PaiPair>();
            PaiPair winPaiPair = null;

            foreach (PaiPair paiPair in this.paiPair7List)
            {
                if (paiPair.WinningPai == null)
                {
                    // 手牌に追加
                    paiPairList.Add(paiPair);
                }
                else
                {
                    // 和了牌を含む対子をセット
                    winPaiPair = paiPair;
                }
            }

            // 牌を整理
            paiPairList = this.paiOrganizer.sortPaiPair(paiPairList);

            /* 座標にオブジェクトを設定 */
            // 手牌の座標を決定
            for (int i = 0; i < paiPairList.Count; i++)
            {
                // 牌2つの座標を設定し、X軸を増加
                paiPairList[i].Pai1.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;

                paiPairList[i].Pai2.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;

            }
            // 待ち牌の座標を決定
            winPaiPair.Pai1.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);

            // 和了牌との間を増やす
            initXPai += 1.0f;

            // 和了牌の座標を決定
            winPaiPair.Pai2.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);
        }
        else
        {
            // 4面子と頭を取得
            for (int i = 0; i < 4; i++)
            {
                this.paiSetList.Add(this.paiOrganizer.alignPaiSet());
            }
            this.paiPair = this.paiOrganizer.alignPaiPair();

            /* 和了牌を決定 */
            // 和了牌が決まったかをフラグで管理
            bool isDecision = false;

            // 全面子を順に副露されているかを確認
            // 副露されていなかったら1/3の確率でその面子を和了牌セットにする
            foreach (PaiSet paiSet in this.paiSetList)
            {
                if (!(paiSet.IsConcealedPung || paiSet.IsMeldedkong ||
                    paiSet.IsConcealedKong || paiSet.IsCalledChow)
                    && this.random.Next(0, 3) == 0)
                {
                    // 乱数で0-2を取得し、どの牌を和了牌にするか決める
                    int randPai = this.random.Next(0, 3);

                    // 和了牌を取得(槓子は全部同じ牌なので考慮しない)
                    if (randPai == 0)
                    {
                        paiSet.WinningPai = paiSet.Pai1;
                    }
                    else if (randPai == 1)
                    {
                        paiSet.WinningPai = paiSet.Pai2;
                    }
                    else
                    {
                        paiSet.WinningPai = paiSet.Pai3;
                    }
                    isDecision = true;
                    break;
                }
            }
            // 面子で和了牌が決まらなかったら対子から和了牌を決める
            if (!isDecision)
            {
                // 頭から対子の片方の牌に決定
                this.paiPair.WinningPai = this.paiPair.Pai1;
            }

            // 鳴き牌と和了牌とそれ以外に分ける
            List<PaiSet> paiSetList = new List<PaiSet>();
            List<PaiSet> paiSetConcealedList = new List<PaiSet>();
            PaiPair paiPair = this.paiPair;
            PaiSet winPaiSet = null;
            PaiPair winPaiPair = null;

            foreach (PaiSet paiSet in this.paiSetList)
            {
                if (paiSet.WinningPai == null)
                {
                    /* 暗槓明槓明刻鳴き順子チェック */
                    if (paiSet.IsConcealedPung || paiSet.IsMeldedkong ||
                        paiSet.IsConcealedKong || paiSet.IsCalledChow)
                    {
                        // 鳴き牌に追加
                        paiSetConcealedList.Add(paiSet);
                    }
                    else
                    {
                        // 手牌に追加
                        paiSetList.Add(paiSet);
                    }
                }
                else
                {
                    // 和了牌を含む面子をセット
                    winPaiSet = paiSet;
                }
            }
            // 面子に和了牌がなかった場合、対子を和了牌として扱う
            if (winPaiSet == null)
            {
                winPaiPair = this.paiPair;
            }

            // 牌を整理
            paiSetList = this.paiOrganizer.sortPaiSet(paiSetList);

            // 手牌の座標を決定
            for (int i = 0; i < paiSetList.Count; i++)
            {
                // 牌3つの座標を設定し、X軸を増加
                paiSetList[i].Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;

                paiSetList[i].Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;

                paiSetList[i].Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;
            }
            // 頭牌の座標を決定
            // 牌2つの座標を設定し、X軸を増加
            // 和了牌セットの場合はスルー
            if (winPaiPair == null)
            {
                paiPair.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;
                paiPair.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;
            }

            // 和了牌の座標を決定
            if (winPaiSet != null)
            {
                // 和了牌とそうでない牌を判別(和了牌に槓子は含まれないので4牌目は考慮しない)
                if (winPaiSet.WinningPai != winPaiSet.Pai1)
                {
                    // 待ち牌の座標を決定
                    winPaiSet.Pai1.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);
                    initXPai += 0.6f;
                }
                if (winPaiSet.WinningPai != winPaiSet.Pai2)
                {
                    // 待ち牌の座標を決定
                    winPaiSet.Pai2.transform.localPosition = new Vector3(initXPai, initYPai, initZPai); ;
                    initXPai += 0.6f;
                }
                if (winPaiSet.WinningPai != winPaiSet.Pai3)
                {
                    // 待ち牌の座標を決定
                    winPaiSet.Pai3.transform.localPosition = new Vector3(initXPai, initYPai, initZPai); ;
                    initXPai += 0.6f;
                }
                // 和了牌との間を増やす
                initXPai += 0.4f;

                // 和了牌の座標を決定
                winPaiSet.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);
            }
            else if (winPaiPair != null)
            {
                // 待ち牌の座標を決定
                winPaiPair.Pai1.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);

                // 和了牌との間を増やす
                initXPai += 1.0f;

                // 和了牌の座標を決定
                winPaiPair.Pai2.transform.localPosition = new Vector3(initXPai, initYPai, initZPai);
            }

            // 鳴き牌の初期座標
            initXPai = 5.0f;
            initYPai = 0.0f;
            initZPai = 0.0f;

            // 鳴き牌を決定
            foreach (PaiSet paiSet in paiSetConcealedList)
            {
                // 牌4つの座標を設定し、Z軸を増加
                // 暗槓かどうか
                if (paiSet.IsMeldedkong)
                {
                    // 端牌なのでひっくり返す
                    paiSet.Pai1.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
                    paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 0.6f;

                    paiSet.Pai2.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                    paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 0.6f;

                    paiSet.Pai3.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                    paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 0.6f;

                    // 端牌なのでひっくり返す
                    paiSet.Pai4.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
                    paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                }
                else
                {
                    // 鳴き牌なら横にする
                    if (paiSet.Pai1 == paiSet.PaiConcealed)
                    {
                        // 横にする
                        paiSet.Pai1.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);

                        paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai - 0.1f);
                        initXPai -= 0.7f;
                    }
                    else
                    {
                        paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                        initXPai -= 0.6f;
                    }

                    if (paiSet.Pai2 == paiSet.PaiConcealed)
                    {
                        // 横にする
                        paiSet.Pai2.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);

                        // 横にする分横にずらす
                        initXPai -= 0.1f;
                        paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai - 0.1f);
                        initXPai -= 0.7f;
                    }
                    else
                    {
                        paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                        initXPai -= 0.6f;
                    }

                    if (paiSet.Pai3 == paiSet.PaiConcealed)
                    {
                        // 横にする
                        paiSet.Pai3.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);

                        // 横にする分横にずらす
                        initXPai -= 0.1f;
                        paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai - 0.1f);
                        initXPai -= 0.7f;
                    }
                    else
                    {
                        paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                        initXPai -= 0.6f;
                    }

                    if (paiSet.Pai4 != null && paiSet.Pai4 == paiSet.PaiConcealed)
                    {
                        // 横にする
                        paiSet.Pai4.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);

                        // 横にする分横にずらす
                        initXPai -= 0.1f;
                        paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai - 0.1f);
                    }
                    else if (paiSet.Pai4 != null)
                    {
                        paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    }
                }
                // X軸を鳴き牌の初期座標にリセット
                initXPai = 5.0f;
                // Z軸を上に変更
                initZPai += 1.0f;
            }
        }
        // 点数計算を実施
        doPaiCal();
    }

    // 点数計算を実施
    public void doPaiCal()
    {
        // 変数用意
        int point = 0;
        int doubles = 0;
        MahjongScoreStatus mahjongScoreStatus = new MahjongScoreStatus();

        // ツモロンを取得
        string tumoOrRon = this.drdTumoOrRon.options[this.drdTumoOrRon.value].text;

        // ツモロンフラグをセット
        // ロンの場合、門前かどうかをチェック
        if (tumoOrRon.Equals("ロン"))
        {
            this.isSelfDrawWin = false;
            this.isConcealedRon = true;

            // 基本型かどうか
            if (this.paiSetList.Count != 0)
            {
                // 門前かどうかをチェック
                foreach (PaiSet paiSet in this.paiSetList)
                {
                    if (paiSet.IsConcealedPung ||
                        paiSet.IsConcealedKong ||
                        paiSet.IsCalledChow)
                    {
                        // 鳴いていたら門前ロンフラグをfalse
                        this.isConcealedRon = false;
                        break;
                    }
                }
            }
        }
        else
        {
            this.isSelfDrawWin = true;
            this.isConcealedRon = false;
        }

        // 七対子か基本形か判別
        if (this.paiPair7List.Count == 7)
        {
            // 符は25符確定
            point = 25;

            // 翻数取得(今はドロップダウンで選択)
            doubles = Int32.Parse(this.drdDoubles.options[this.drdDoubles.value].text);
        }
        else
        {
            // 自風と場風
            PaiStatus.PAICHARACTERS ownWindPaiCharacters = PaiStatus.PAICHARACTERS.無;
            PaiStatus.PAICHARACTERS roundWindPaiCharacters = PaiStatus.PAICHARACTERS.無;
            string ownWind = this.drdOwnWind.options[this.drdOwnWind.value].text;
            string roudWind = this.drdRoundWind.options[this.drdRoundWind.value].text;

            // 自風と場風をenumステータスに変換
            foreach (PaiStatus.PAICHARACTERS paiCharacters in Enum.GetValues(typeof(PaiStatus.PAICHARACTERS)))
            {
                if (paiCharacters.ToString().Equals(ownWind))
                {
                    ownWindPaiCharacters = paiCharacters;
                }
                if (paiCharacters.ToString().Equals(roudWind))
                {
                    roundWindPaiCharacters = paiCharacters;
                }
            }

            // 符計算実施
            point = PaiCal.allPaiSetCalPoint(this.paiSetList, this.paiPair,
                this.isSelfDrawWin, this.isConcealedRon,
                ownWindPaiCharacters, roundWindPaiCharacters);

            // 翻数取得(今はドロップダウンで選択)
            doubles = Int32.Parse(this.drdDoubles.options[this.drdDoubles.value].text);
        }
        // 点数計算
        mahjongScoreStatus = PaiCal.calScore(point, doubles);

        // UI非表示
        this.textPoints.enabled = false;
        this.textDoubles.enabled = false;
        this.textKoRon.enabled = false;
        this.textOyaRon.enabled = false;
        this.textKoTumo.enabled = false;
        this.textOyaTumo.enabled = false;
        // UIを更新
        this.textPoints.text = string.Format("{0,3:d}", point) + " 符";
        this.textDoubles.text = string.Format("{0,3:d}", doubles) + " 翻";

        string koRon = "-";
        string oyaRon = "-";
        string koTumoChild = "-";
        string koTumoParent = "-";
        // 値設定
        if (mahjongScoreStatus.KoRon != 0)
        {
            koRon = "" + mahjongScoreStatus.KoRon;
        }
        if (mahjongScoreStatus.OyaRon != 0)
        {
            oyaRon = "" + mahjongScoreStatus.OyaRon;
        }
        if (mahjongScoreStatus.KoTumoChild == 0 && mahjongScoreStatus.KoTumoParent == 0)
        {
            koTumoChild = "";
            koTumoParent = "";
            this.textOyaTumo.text = "親ツモ：-";
        }
        else
        {
            koTumoChild = "" + mahjongScoreStatus.KoTumoChild;
            koTumoParent = "" + mahjongScoreStatus.KoTumoParent;
            this.textOyaTumo.text = "親ツモ：" + mahjongScoreStatus.KoTumoParent + "オール";
        }
        this.textKoRon.text = "子ロン：" + koRon;
        this.textOyaRon.text = "親ロン：" + oyaRon;
        this.textKoTumo.text = "子ツモ：" + koTumoChild + "-" + koTumoParent;
    }

    // 答え表示
    public void showPaiQAns()
    {
        // UI表示
        this.textPoints.enabled = true;
        this.textDoubles.enabled = true;
        this.textKoRon.enabled = true;
        this.textOyaRon.enabled = true;
        this.textKoTumo.enabled = true;
        this.textOyaTumo.enabled = true;
    }

    public void yamaReset()
    {
        if (this.paiSetList.Count >= 1)
        {
            this.paiOrganizer.addPaiSetListToYama(this.paiSetList);
        }
        if (this.paiPair7List.Count >= 1)
        {
            this.paiOrganizer.addPaiPairListToYama(this.paiPair7List);
        }
        this.paiOrganizer.addPaiPairToYama(this.paiPair);
    }
}
