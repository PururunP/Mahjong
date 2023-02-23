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
    // ロン親テキスト
    [SerializeField]
    private Text textTumoOrRon;

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

        string log = "";

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

            Debug.Log(this.paiOrganizer.log);
            this.paiOrganizer.log = "";

            /* 和了牌を決定 */
            // 対子からランダムに決定
            int randNumber = this.random.Next(0, this.paiPair7List.Count);
            this.paiPair7List[randNumber].WinningPai = this.paiPair7List[randNumber].Pai1;

            // ツモかロンかを1/2で決定 
            if (this.random.Next(0, 2) == 0)
            {
                this.isSelfDrawWin = true;
                this.isConcealedRon = false;
            }
            else
            {
                this.isSelfDrawWin = false;
                this.isConcealedRon = true;
            }

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

            // 座標にオブジェクトを設定
            // 手牌の初期座標
            float initXPai = -4.0f, initYPai = 0.0f, initZPai = 0.0f;

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
            // 1/3で分岐処理
            if (this.random.Next(0, 3) != 0)
            {
                // 面子から決定
                // 乱数で0-3を取得し、どの面子かを決める
                int randPaiSet = this.random.Next(0, 4);
                // 乱数で0-2を取得し、どの牌を和了牌にするか決める
                int randPai = this.random.Next(0, 3);

                // 和了牌を取得(槓子は全部同じ牌なので考慮しない)
                if (randPai == 0)
                {
                    this.paiSetList[randPaiSet].WinningPai =
                        this.paiSetList[randPaiSet].Pai1;
                }
                else if (randPai == 1)
                {
                    this.paiSetList[randPaiSet].WinningPai =
                        this.paiSetList[randPaiSet].Pai2;
                }
                else
                {
                    this.paiSetList[randPaiSet].WinningPai =
                        this.paiSetList[randPaiSet].Pai3;
                }
            }
            else
            {
                // 頭から対子の片方の牌に決定
                this.paiPair.WinningPai = this.paiPair.Pai1;
            }

            // ツモかロンかを1/2で決定 
            if (this.random.Next(0, 2) == 0)
            {
                this.isSelfDrawWin = true;
                this.isConcealedRon = false;
            }
            else
            {
                this.isSelfDrawWin = false;
                this.isConcealedRon = true;

                // 門前かどうかをチェック
                foreach (PaiSet paiSet in this.paiSetList)
                {
                    if (paiSet.IsConcealedPung ||
                        paiSet.IsConcealedKong ||
                        paiSet.IsCalledChow)
                    {
                        // 鳴いていたら門前ロンフラグをfalse
                        this.isConcealedRon = false;
                    }
                }
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
                    // 暗槓
                    if (paiSet.IsMeldedPung || paiSet.IsConcealedPung ||
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

            // 座標にオブジェクトを設定
            // 手牌の初期座標
            float initXPai = -4.0f, initYPai = 0.0f, initZPai = 0.0f;

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

                log += paiSetList[i].Pai1.name + " : " + paiSetList[i].Pai1.transform.position + "\n";
                log += paiSetList[i].Pai2.name + " : " + paiSetList[i].Pai2.transform.position + "\n";
                log += paiSetList[i].Pai3.name + " : " + paiSetList[i].Pai3.transform.position + "\n";
            }
            // 頭牌の座標を決定
            // 牌2つの座標を設定し、X軸を増加
            // 和了牌セットで和了牌の場合はスルー
            if (paiPair.WinningPai != paiPair.Pai1)
            {
                paiPair.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;
                log += paiPair.Pai1.name + " : " + paiPair.Pai1.transform.position + "\n";
            }
            if (paiPair.WinningPai != paiPair.Pai2)
            {
                paiPair.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initXPai += 0.6f;
                log += paiPair.Pai2.name + " : " + paiPair.Pai2.transform.position + "\n";
            }

            // 和了牌との間を増やす
            initXPai += 1.0f;

            // 和了牌の座標を決定 // TODO こっちは分岐処理した方がいい
            if (winPaiSet != null)
            {
                winPaiSet.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);
            }
            else if (winPaiPair != null)
            {
                winPaiPair.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);
            }

            // 鳴き牌の初期座標
            initXPai = 8.0f;
            initYPai = 0.0f;
            initZPai = 0.0f;

            // 鳴き牌を決定 // TODO 暗槓はひっくり返す // TODO 鳴き牌を横にする
            foreach (PaiSet paiSet in paiSetConcealedList)
            {
                // 牌4つの座標を設定し、Z軸を増加
                // 鳴き牌なら横にする
                if (paiSet.Pai1 == paiSet.PaiConcealed)
                {
                    // 横にする
                    paiSet.Pai1.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);

                    paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 1.0f;
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

                    paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 1.0f;
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

                    paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initXPai -= 1.0f;
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

                    paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                }
                else if (paiSet.Pai4 != null)
                {
                    paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                }
                log += paiSet.Pai1.name + " : " + paiSet.Pai1.transform.position + "\n";
                log += paiSet.Pai2.name + " : " + paiSet.Pai2.transform.position + "\n";
                log += paiSet.Pai3.name + " : " + paiSet.Pai3.transform.position + "\n";
                // X軸を鳴き牌の初期座標にリセット
                initXPai = 8.0f;
                // Z軸を上に変更
                initZPai -= 1.0f;
            }
        }
        //Debug.Log(log);
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
            this.textOyaTumo.text = "親ツモ：" + mahjongScoreStatus.KoTumoParent;
        }
        this.textKoRon.text = "子ロン：" + koRon;
        this.textOyaRon.text = "親ロン：" + oyaRon;
        this.textKoTumo.text = "子ツモ：" + koTumoChild + "-" + koTumoParent;

        // ツモ和了か
        if (this.isSelfDrawWin)
        {
            this.textTumoOrRon.text = "和了：ツモ";
        }
        else
        {
            this.textTumoOrRon.text = "和了：ロン";
        }
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
