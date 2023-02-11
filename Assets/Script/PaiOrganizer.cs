using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌整理
public class PaiOrganizer : MonoBehaviour
{
    // 山ゲームオブジェクト
    [SerializeField]
    private GameObject yamaGameObject = null;

    // 乱数
    private System.Random random = new System.Random();

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
            Pai paiSctipt = randPai.GetComponent<Pai>();

            /* 牌の種類により分岐処理 */
            // 字牌かどうか
            if (paiSctipt.PaiKind == PaiStatus.PAIKIND.字牌)
            {
                // 同じ牌をツモ
                Pai pai = new Pai(paiSctipt.PaiKind, paiSctipt.PaiCharacters);

                GameObject paiGameObj1 = yamaScript.drawPai(pai);
                GameObject paiGameObj2 = yamaScript.drawPai(pai);
                GameObject paiGameObj3 = yamaScript.drawPai(pai);

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
                    if (paiSctipt.PaiNumber == 1)
                    {
                        pai1 = new Pai(paiSctipt.PaiKind, 2, false);
                        pai2 = new Pai(paiSctipt.PaiKind, 3, false);
                    }
                    else if (paiSctipt.PaiNumber == 9)
                    {
                        pai1 = new Pai(paiSctipt.PaiKind, 8, false);
                        pai2 = new Pai(paiSctipt.PaiKind, 7, false);
                    }
                    else
                    {
                        // 数値が5の場合、赤くするかを分岐処理で決定
                        // 1/4の確率で赤くする
                        bool isRed1 = false;
                        bool isRed2 = false;
                        if ((paiSctipt.PaiNumber - 1) == 5 && this.random.Next(0, 4) == 9)
                        {
                            isRed1 = true;
                        }
                        if ((paiSctipt.PaiNumber + 1) == 5 && this.random.Next(0, 4) == 0)
                        {
                            isRed2 = true;
                        }
                        pai1 = new Pai(paiSctipt.PaiKind, paiSctipt.PaiNumber - 1, isRed1);
                        pai2 = new Pai(paiSctipt.PaiKind, paiSctipt.PaiNumber + 1, isRed2);
                    }

                    GameObject paiGameObj1 = yamaScript.drawPai(pai1);
                    GameObject paiGameObj2 = yamaScript.drawPai(pai2);

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
                    Pai pai = new Pai(paiSctipt.PaiKind, paiSctipt.PaiNumber, false);

                    GameObject paiGameObj1 = yamaScript.drawPai(pai);
                    GameObject paiGameObj2 = yamaScript.drawPai(pai);
                    GameObject paiGameObj3 = yamaScript.drawPai(pai);

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
            // 面子の基準となる牌をランダムツモ
            GameObject randPai = yamaScript.drawPai();

            // Paiスクリプト抽出
            Pai paiScript = randPai.GetComponent<Pai>();

            // 同牌をツモ
            Pai pai = null;
            if (paiScript.PaiKind == PaiStatus.PAIKIND.字牌)
            {
                pai = new Pai(paiScript.PaiKind, paiScript.PaiCharacters);
            }
            else
            {
                pai = new Pai(paiScript.PaiKind, paiScript.PaiNumber, false);
            }

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
                // 対子オブジェクト返却
                return new PaiPair(pai1: randPai, pai2: paiGameObj);
            }
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
                    Pai paiSctipt1 = paiSetList[i].Pai1.GetComponent<Pai>();
                    Pai paiSctipt2 = paiSetList[j].Pai1.GetComponent<Pai>();

                    if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.萬子)
                    {
                        // 同種牌の場合
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.筒子)
                    {
                        // 萬子を探索
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // iとjを入れ替え
                            (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                        }
                        // 同種牌の場合
                        else
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.索子)
                    {
                        // 萬子筒子を探索
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子 ||
                            paiSctipt2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // iとjを入れ替え
                            (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                        }
                        // 同種牌の場合
                        else
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.字牌)
                    {
                        // 字牌かどうか
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.字牌)
                        {
                            // 字牌の順番に入れ替え
                            switch (paiSctipt1.PaiCharacters)
                            {
                                case PaiStatus.PAICHARACTERS.東:
                                    // 何もしない
                                    break;
                                case PaiStatus.PAICHARACTERS.南:
                                    // 東なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東)
                                    {
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.西:
                                    // 東南なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南)
                                    {
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.北:
                                    // 東南西なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西)
                                    {
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.白:
                                    // 東南西北なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.北)
                                    {
                                        (paiSetList[i], paiSetList[j]) = (paiSetList[j], paiSetList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.發:
                                    // 東南西北白なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.北 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.白)
                                    {
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
                    Pai paiSctipt1 = paiPairList[i].Pai1.GetComponent<Pai>();
                    Pai paiSctipt2 = paiPairList[j].Pai1.GetComponent<Pai>();

                    if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.萬子)
                    {
                        // 同種牌の場合
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.筒子)
                    {
                        // 萬子を探索
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子)
                        {
                            // iとjを入れ替え
                            (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                        }
                        // 同種牌の場合
                        else
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.索子)
                    {
                        // 萬子筒子を探索
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.萬子 ||
                            paiSctipt2.PaiKind == PaiStatus.PAIKIND.筒子)
                        {
                            // iとjを入れ替え
                            (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                        }
                        // 同種牌の場合
                        else
                        {
                            // 数字を比較
                            if (paiSctipt1.PaiNumber > paiSctipt2.PaiNumber)
                            {
                                // iとjを入れ替え
                                (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                            }
                        }
                    }
                    else if (paiSctipt1.PaiKind == PaiStatus.PAIKIND.字牌)
                    {
                        // 字牌かどうか
                        if (paiSctipt2.PaiKind == PaiStatus.PAIKIND.字牌)
                        {
                            // 字牌の順番に入れ替え
                            switch (paiSctipt1.PaiCharacters)
                            {
                                case PaiStatus.PAICHARACTERS.東:
                                    // 何もしない
                                    break;
                                case PaiStatus.PAICHARACTERS.南:
                                    // 東なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東)
                                    {
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.西:
                                    // 東南なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南)
                                    {
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.北:
                                    // 東南西なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西)
                                    {
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.白:
                                    // 東南西北なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.北)
                                    {
                                        (paiPairList[i], paiPairList[j]) = (paiPairList[j], paiPairList[i]);
                                    }
                                    break;
                                case PaiStatus.PAICHARACTERS.發:
                                    // 東南西北白なら入れ替え
                                    if (paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.東 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.南 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.西 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.北 ||
                                        paiSctipt2.PaiCharacters == PaiStatus.PAICHARACTERS.白)
                                    {
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

    // 全牌を萬子→筒子→索子→字牌の順に並び替える(未実装)
    public List<GameObject> sortPai(List<GameObject> paiGameObjList)
    {
        return paiGameObjList;
    }
}