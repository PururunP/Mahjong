using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌整理
public class PaiOrganizer : MonoBehaviour
{
    // 山ゲームオブジェクト
    [SerializeField]
    private GameObject yamaGameObject;

    // 乱数
    private System.Random random;

    // ログ出力用文字列
    public string log = "";

    void Awake()
    {
        /* 初期化 */
        this.random = new System.Random();
    }

    // 面子を揃える
    public PaiSet alignPaiSet()
    {
        // 山スクリプト取得
        Yama yamaScript = this.yamaGameObject.GetComponent<Yama>();

        while (true)
        {
            // 面子の基準となる牌をランダムツモ
            GameObject randPai = yamaScript.drawPai();

            // Paiスクリプト抽出
            Pai paiScript = randPai.GetComponent<PaiGO>().Pai;

            log += "面子ランダムツモ：" + randPai.name + "\n"; ;

            /* 牌の種類により分岐処理 */
            // 字牌かどうか
            if (paiScript.PaiKind == PaiStatus.PAIKIND.字牌)
            {
                // 同じ牌をツモ
                Pai pai = new Pai(paiScript.PaiKind, paiScript.PaiCharacters);

                GameObject paiGameObj1 = yamaScript.drawPai(pai);
                GameObject paiGameObj2 = yamaScript.drawPai(pai);
                GameObject paiGameObj3 = yamaScript.drawPai(pai);
                log += "ツモ1：" + paiGameObj1.name + "\n";
                log += "ツモ2：" + paiGameObj2.name + "\n";
                log += "ツモ3：" + paiGameObj3.name + "\n";

                // nullチェック
                if (paiGameObj1 == null || paiGameObj2 == null)
                {
                    // ツモった牌を山に戻す
                    yamaScript.addPai(randPai);
                    yamaScript.addPai(paiGameObj1);
                    yamaScript.addPai(paiGameObj2);
                    yamaScript.addPai(paiGameObj3);
                    continue;
                }
                else
                {
                    // 槓子ができる(4牌目がある)かつ1/2の確率で分岐処理
                    if (paiGameObj3 != null && this.random.Next(0, 2) == 0)
                    {
                        // 槓子として処理
                        // 1/2で分岐処理
                        if (this.random.Next(0, 2) == 0)
                        {
                            // 暗槓として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, pai4: paiGameObj3, paiConcealed: null,
                                isMeldedkong: true, isConcealedKong: false);
                        }
                        else
                        {
                            // 明槓として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, pai4: paiGameObj3, paiConcealed: randPai,
                                isMeldedkong: false, isConcealedKong: true);
                        }
                    }
                    else
                    {
                        // 刻子として処理
                        // 4牌目を山に戻す
                        yamaScript.addPai(paiGameObj3);

                        // 1/2で分岐処理
                        if (this.random.Next(0, 2) == 0)
                        {
                            // 暗刻として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, paiConcealed: null,
                                isMeldedPung: true, isConcealedPung: false);
                        }
                        else
                        {
                            // 明刻として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, paiConcealed: randPai,
                                isMeldedPung: false, isConcealedPung: true);
                        }
                    }
                }
            }
            else
            {
                // 1/2で分岐処理
                if (this.random.Next(0, 2) == 0)
                {
                    /* 順子を構成 */
                    // 么九牌の場合は2,3もしくは8,7の同種牌をツモ
                    // それ以外はn-1,n+1の同種牌をツモ
                    Pai pai1, pai2;
                    if (paiScript.PaiNumber == 1)
                    {
                        pai1 = new Pai(paiScript.PaiKind, 2, false);
                        pai2 = new Pai(paiScript.PaiKind, 3, false);
                    }
                    else if (paiScript.PaiNumber == 9)
                    {
                        pai1 = new Pai(paiScript.PaiKind, 8, false);
                        pai2 = new Pai(paiScript.PaiKind, 7, false);
                    }
                    else
                    {
                        // 数値が5の場合、赤くするかを分岐処理で決定
                        // 1/4の確率で赤くする
                        bool isRed1 = false;
                        bool isRed2 = false;
                        if ((paiScript.PaiNumber - 1) == 5 && this.random.Next(0, 4) == 9)
                        {
                            isRed1 = true;
                        }
                        if ((paiScript.PaiNumber + 1) == 5 && this.random.Next(0, 4) == 0)
                        {
                            isRed2 = true;
                        }
                        pai1 = new Pai(paiScript.PaiKind, paiScript.PaiNumber - 1, isRed1);
                        pai2 = new Pai(paiScript.PaiKind, paiScript.PaiNumber + 1, isRed2);
                    }

                    GameObject paiGameObj1 = yamaScript.drawPai(pai1);
                    GameObject paiGameObj2 = yamaScript.drawPai(pai2);
                    log += "ツモ1：" + paiGameObj1.name + "\n";
                    log += "ツモ2：" + paiGameObj2.name + "\n";

                    // nullチェック
                    if (paiGameObj1 == null || paiGameObj2 == null)
                    {
                        // ツモった牌を山に戻す
                        yamaScript.addPai(randPai);
                        yamaScript.addPai(paiGameObj1);
                        yamaScript.addPai(paiGameObj2);
                        continue;
                    }
                    else
                    {
                        // 1/2で分岐処理
                        if (this.random.Next(0, 2) == 0)
                        {
                            // チーした順子として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, paiConcealed: randPai,
                                isCalledChow: true);
                        }
                        else
                        {
                            // 鳴きなしの順子として面子オブジェクト返却
                            return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                pai3: paiGameObj2, paiConcealed: null,
                                isCalledChow: false);
                        }
                    }
                }
                else
                {
                    /* 刻子か槓子を構成 */
                    // 同数字の同種牌をツモ
                    Pai pai = new Pai(paiScript.PaiKind, paiScript.PaiNumber, false);

                    GameObject paiGameObj1 = yamaScript.drawPai(pai);
                    GameObject paiGameObj2 = yamaScript.drawPai(pai);
                    GameObject paiGameObj3 = yamaScript.drawPai(pai);
                    log += "ツモ1：" + paiGameObj1.name + "\n";
                    log += "ツモ2：" + paiGameObj2.name + "\n";
                    log += "ツモ3：" + paiGameObj3.name + "\n";

                    // nullチェック
                    if (paiGameObj1 == null || paiGameObj2 == null)
                    {
                        // ツモった牌を山に戻す
                        yamaScript.addPai(randPai);
                        yamaScript.addPai(paiGameObj1);
                        yamaScript.addPai(paiGameObj2);
                        yamaScript.addPai(paiGameObj3);
                        continue;
                    }
                    else
                    {
                        // 槓子ができる(4牌目がある)かつ1/2の確率で分岐処理
                        if (paiGameObj3 != null && this.random.Next(0, 2) == 0)
                        {
                            // 槓子として処理
                            // 1/2で分岐処理
                            if (this.random.Next(0, 2) == 0)
                            {
                                // 暗槓として面子オブジェクト返却
                                return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                    pai3: paiGameObj2, pai4: paiGameObj3, paiConcealed: randPai,
                                    isMeldedkong: true, isConcealedKong: false);
                            }
                            else
                            {
                                // 明槓として面子オブジェクト返却
                                return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                    pai3: paiGameObj2, pai4: paiGameObj3, paiConcealed: randPai,
                                    isMeldedkong: false, isConcealedKong: true);
                            }

                        }
                        else
                        {
                            // 刻子として処理
                            // 4牌目を山に戻す
                            yamaScript.addPai(paiGameObj3);

                            // 1/2で分岐処理
                            if (this.random.Next(0, 2) == 0)
                            {
                                // 暗刻として面子オブジェクト返却
                                return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                    pai3: paiGameObj2, paiConcealed: randPai,
                                    isMeldedPung: true, isConcealedPung: false);
                            }
                            else
                            {
                                // 明刻として面子オブジェクト返却
                                return new PaiSet(pai1: randPai, pai2: paiGameObj1,
                                    pai3: paiGameObj2, paiConcealed: randPai,
                                    isMeldedPung: false, isConcealedPung: true);
                            }
                        }
                    }
                }
            }
        }
    }

    // 対子を揃える
    public PaiPair alignPaiPair()
    {
        // 山スクリプト取得
        Yama yamaScript = this.yamaGameObject.GetComponent<Yama>();

        while (true)
        {
            // 対子の基準となる牌をランダムツモ
            GameObject randPai = yamaScript.drawPai();

            // Paiスクリプト抽出
            Pai paiScript = randPai.GetComponent<PaiGO>().Pai;

            log += "対子ランダムツモ：" + randPai.name + "\n";

            // 同牌をツモ(赤牌は普通の牌に)
            Pai pai = new Pai(paiScript.PaiKind, paiScript.PaiCharacters,
                paiScript.PaiNumber, false);
            GameObject paiGameObj = yamaScript.drawPai(pai);

            // nullチェック
            if (paiGameObj == null)
            {
                // ツモった牌を山に戻す
                yamaScript.addPai(randPai);
                yamaScript.addPai(paiGameObj);
                continue;
            }
            else
            {
                log += "ツモ：" + paiGameObj.name + "\n";
                // 対子オブジェクト返却
                return new PaiPair(pai1: randPai, pai2: paiGameObj);
            }
        }
    }

    // 面子オブジェクトリストを山に戻す
    public void addPaiSetListToYama(List<PaiSet> paiSetList)
    {
        if (paiSetList.Count >= 1)
        {
            foreach (PaiSet paiSet in paiSetList)
            {
                addPaiSetToYama(paiSet);
            }
        }
    }

    // 面子オブジェクトを山に戻す
    public void addPaiSetToYama(PaiSet paiSet)
    {
        // 山スクリプト取得
        Yama yama = this.yamaGameObject.GetComponent<Yama>();
        yama.addPai(paiSet.Pai1);
        yama.addPai(paiSet.Pai2);
        yama.addPai(paiSet.Pai3);
        yama.addPai(paiSet.Pai4);

        // 牌があるなら座標を初期位置に戻す
        if (paiSet.Pai1 != null)
        {
            paiSet.Pai1.transform.position = paiSet.Pai1.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiSet.Pai1.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        if (paiSet.Pai2 != null)
        {
            paiSet.Pai2.transform.position = paiSet.Pai2.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiSet.Pai2.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        if (paiSet.Pai3 != null)
        {
            paiSet.Pai3.transform.position = paiSet.Pai3.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiSet.Pai3.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        if (paiSet.Pai4 != null)
        {
            paiSet.Pai4.transform.position = paiSet.Pai4.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiSet.Pai4.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    // 対子オブジェクトリストを山に戻す
    public void addPaiPairListToYama(List<PaiPair> paiPairList)
    {
        if (paiPairList.Count >= 1)
        {
            // 山スクリプト取得
            Yama yama = this.yamaGameObject.GetComponent<Yama>();
            foreach (PaiPair paiPair in paiPairList)
            {
                addPaiPairToYama(paiPair);
            }
        }
    }

    // 対子オブジェクトを山に戻す
    public void addPaiPairToYama(PaiPair paiPair)
    {
        // 山スクリプト取得
        Yama yama = this.yamaGameObject.GetComponent<Yama>();
        yama.addPai(paiPair.Pai1);
        yama.addPai(paiPair.Pai2);

        // 牌があるなら座標を初期位置に戻す
        if (paiPair.Pai1 != null)
        {
            paiPair.Pai1.transform.position = paiPair.Pai1.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiPair.Pai1.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        if (paiPair.Pai2 != null)
        {
            paiPair.Pai2.transform.position = paiPair.Pai2.gameObject.
                GetComponent<PaiGO>().InitVec;
            paiPair.Pai2.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    // 面子を萬子→筒子→索子→字牌の順に並び替える
    public List<PaiSet> sortPaiSet(List<PaiSet> paiSetList)
    {
        for (int i = 0; i < paiSetList.Count - 1; i++)
        {
            for (int j = i; j < paiSetList.Count; j++)
            {
                if (i != j)
                {
                    /* i番目とj番目を比較し、順番を入れ替える */
                    // Paiスクリプト取得
                    Pai paiScript1 = paiSetList[i].Pai1.GetComponent<PaiGO>().Pai;
                    Pai paiScript2 = paiSetList[j].Pai1.GetComponent<PaiGO>().Pai;// 萬子の場合
                    if (paiScript1.PaiKind == PaiStatus.PAIKIND.萬子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                    }
                    // 筒子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.筒子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // iとjを入れ替え
                            (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                        }
                    }
                    // 索子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.索子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.索子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子・筒子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子 ||
                            paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // iとjを入れ替え
                            (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                        }
                    }
                    // 字牌の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.字牌)
                    {
                        // 字牌かどうか
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.字牌)
                        {
                            // 字牌の順番に入れ替え
                            switch (paiScript1.PaiCharacters)
                            {
                                case PaiStatus.PAICHARACTERS.東:
                                    // 何もしない
                                    break;
                                case PaiStatus.PAICHARACTERS.南:
                                    // 東なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東)
                                    {
                                        // iとjを入れ替え
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.西:
                                    // 東南なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南)
                                    {
                                        // iとjを入れ替え
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.北:
                                    // 東南西なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西)
                                    {
                                        // iとjを入れ替え
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.白:
                                    // 東南西北なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北)
                                    {
                                        // iとjを入れ替え
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.發:
                                    // 東南西北白なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.白)
                                    {
                                        // iとjを入れ替え
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.中:
                                    // iとjを入れ替え
                                    (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            // iとjを入れ替え
                            (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                        }
                    }
                }
            }
        }
        return paiSetList;
    }

    // 対子を萬子→筒子→索子→字牌の順に並び替える
    public List<PaiPair> sortPaiPair(List<PaiPair> paiPairList)
    {
        for (int i = 0; i < paiPairList.Count - 1; i++)
        {
            for (int j = i; j < paiPairList.Count; j++)
            {
                if (i != j)
                {
                    /* i番目とj番目を比較し、順番を入れ替える */
                    // Paiスクリプト取得
                    Pai paiScript1 = paiPairList[i].Pai1.GetComponent<PaiGO>().Pai;
                    Pai paiScript2 = paiPairList[j].Pai1.GetComponent<PaiGO>().Pai;

                    // 萬子の場合
                    if (paiScript1.PaiKind == PaiStatus.PAIKIND.萬子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                    }
                    // 筒子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.筒子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // iとjを入れ替え
                            (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                        }
                    }
                    // 索子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.索子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.索子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子・筒子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子 ||
                            paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // iとjを入れ替え
                            (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                        }
                    }
                    // 字牌の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.字牌)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.字牌)
                        {
                            // 字牌の順番に入れ替え
                            switch (paiScript1.PaiCharacters)
                            {
                                case PaiStatus.PAICHARACTERS.東:
                                    // 何もしない
                                    break;
                                case PaiStatus.PAICHARACTERS.南:
                                    // 東なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東)
                                    {
                                        // iとjを入れ替え
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.西:
                                    // 東南なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南)
                                    {
                                        // iとjを入れ替え
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.北:
                                    // 東南西なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西)
                                    {
                                        // iとjを入れ替え
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.白:
                                    // 東南西北なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北)
                                    {
                                        // iとjを入れ替え
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.發:
                                    // 東南西北白なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.白)
                                    {
                                        // iとjを入れ替え
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.中:
                                    // iとjを入れ替え
                                    (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            // iとjを入れ替え
                            (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                        }
                    }
                }
            }
        }
        return paiPairList;
    }

    // 全牌を萬子→筒子→索子→字牌の順に並び替える
    public List<GameObject> sortPai(List<GameObject> paiGameObjList)
    {
        for (int i = 0; i < paiGameObjList.Count - 1; i++)
        {
            for (int j = i; j < paiGameObjList.Count; j++)
            {
                if (i != j)
                {
                    /* i番目とj番目を比較し、順番を入れ替える */
                    // Paiスクリプト取得
                    Pai paiScript1 = paiGameObjList[i].GetComponent<PaiGO>().Pai;
                    Pai paiScript2 = paiGameObjList[j].GetComponent<PaiGO>().Pai;

                    // 萬子の場合
                    if (paiScript1.PaiKind == PaiStatus.PAIKIND.萬子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                            }
                        }
                    }
                    // 筒子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.筒子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // iとjを入れ替え
                            (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                        }
                    }
                    // 索子の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.索子)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.索子)
                        {
                            // 数字を比較
                            if (paiScript1.PaiNumber > paiScript2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                            }
                        }
                        // 入れ替える対象の牌(萬子・筒子)の場合
                        else if (paiScript2.PaiKind == PaiStatus.PAIKIND.萬子 ||
                            paiScript2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // iとjを入れ替え
                            (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                        }
                    }
                    // 字牌の場合
                    else if (paiScript1.PaiKind == PaiStatus.PAIKIND.字牌)
                    {
                        // 同種牌の場合
                        if (paiScript2.PaiKind == PaiStatus.PAIKIND.字牌)
                        {
                            // 字牌の順番に入れ替え
                            switch (paiScript1.PaiCharacters)
                            {
                                case PaiStatus.PAICHARACTERS.東:
                                    // 何もしない
                                    break;
                                case PaiStatus.PAICHARACTERS.南:
                                    // 東なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東)
                                    {
                                        // iとjを入れ替え
                                        (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.西:
                                    // 東南なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南)
                                    {
                                        // iとjを入れ替え
                                        (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.北:
                                    // 東南西なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西)
                                    {
                                        // iとjを入れ替え
                                        (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.白:
                                    // 東南西北なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北)
                                    {
                                        // iとjを入れ替え
                                        (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.發:
                                    // 東南西北白なら入れ替え
                                    if (paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.北 ||
                                        paiScript2.PaiCharacters == PaiStatus.PAICHARACTERS.白)
                                    {
                                        // iとjを入れ替え
                                        (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.中:
                                    // iとjを入れ替え
                                    (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            // iとjを入れ替え
                            (paiGameObjList[i], paiGameObjList[j]) = (paiGameObjList[j], paiGameObjList[i]);
                        }
                    }
                }
            }
        }
        return paiGameObjList;
    }
}