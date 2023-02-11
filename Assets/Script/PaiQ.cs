using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌問題出題セット
public class PaiQ : MonoBehaviour
{
    // 面子リスト
    private List<PaiSet> paiSetList = new List<PaiSet>();
    // 頭の対子
    private PaiPair paiPair = null;

    // 七対子用の対子リスト
    private List<PaiPair> paiPair7List = new List<PaiPair>();

    // ツモしたか
    private bool isSelfDrawWin = false;

    // 乱数
    private System.Random random = new System.Random();
    // 牌整理スクリプト
    private PaiOrganizer paiOrganizer = new PaiOrganizer();

    // 点数計算用の牌を揃える
    public void setPai()
    {
        // 1/10で七対子化
        if (this.random.Next(0, 10) == 0)
        {
            // 七対子のカウンター
            int count = 1;

            // 対子を7セット取得してリストに追加
            while (count < 7)
            {
                // 対子取得
                PaiPair pair = this.paiOrganizer.alignPaiPair();

                // 対子の片方の牌のPaiスクリプト取得
                Pai paiSctipt1 = pair.Pai1.GetComponent<Pai>();

                // 対子が被ってないかを確認
                foreach (PaiPair p in this.paiPair7List)
                {
                    // リストの対子の片方の牌のPaiスクリプト取得
                    Pai paiScript2 = p.Pai1.GetComponent<Pai>();

                    // オブジェクトのメンバを比較
                    if (paiSctipt1.PaiKind == paiScript2.PaiKind &&
                        paiSctipt1.PaiCharacters == paiScript2.PaiCharacters &&
                        paiSctipt1.PaiNumber == paiScript2.PaiNumber)
                    {
                        // 一致したら対子取得し直し
                        continue;
                    }
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

            // ツモかロンかを1/2で決定 
            if (this.random.Next(0, 2) == 0)
            {
                this.isSelfDrawWin = true;
            }
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
                GameObject winPaiGameObj = null;
                if (randPai == 0)
                {
                    winPaiGameObj = this.paiSetList[randPaiSet].Pai1;
                }
                else if (randPai == 1)
                {
                    winPaiGameObj = this.paiSetList[randPaiSet].Pai2;
                }
                else
                {
                    winPaiGameObj = this.paiSetList[randPaiSet].Pai3;
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
            }
        }
        /* 取得した牌を特定の座標にセット */

        // 七対子かどうかで処理を分ける
        if (this.paiPair7List.Count == 0)
        {
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
            float initXPai = 0.0f, initYPai = 0.0f, initZPai = 0.0f;

            // 手牌の座標を決定
            for (int i = 0; i < paiPairList.Count; i++)
            {
                // 牌2つの座標を設定し、Y軸を増加
                // 和了牌セットで和了牌の場合はスルー
                if (paiPairList[i].WinningPai != paiPairList[i].Pai1)
                {
                    paiPairList[i].Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiPairList[i].WinningPai != paiPairList[i].Pai2)
                {
                    paiPairList[i].Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }
            }

            // 和了牌との間を増やす
            initYPai += 1.0f;

            // 和了牌の座標を決定
            winPaiPair.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);

        }
        else
        {
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
            float initXPai = 0.0f, initYPai = 0.0f, initZPai = 0.0f;

            // 手牌の座標を決定
            for (int i = 0; i < paiSetList.Count; i++)
            {
                // 牌3つの座標を設定し、Y軸を増加
                // 和了牌セットで和了牌の場合はスルー
                if (paiSetList[i].WinningPai != paiSetList[i].Pai1)
                {
                    paiSetList[i].Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiSetList[i].WinningPai != paiSetList[i].Pai2)
                {
                    paiSetList[i].Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiSetList[i].WinningPai != paiSetList[i].Pai3)
                {
                    paiSetList[i].Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }
            }
            // 頭牌の座標を決定
            // 牌2つの座標を設定し、Y軸を増加
            // 和了牌セットで和了牌の場合はスルー
            if (paiPair.WinningPai != paiPair.Pai1)
            {
                paiPair.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initYPai += 1.0f;
            }
            if (paiPair.WinningPai != paiPair.Pai2)
            {
                paiPair.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                initYPai += 1.0f;
            }

            // 和了牌との間を増やす
            initYPai += 1.0f;

            // 和了牌の座標を決定
            if (winPaiSet != null)
            {
                winPaiSet.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);
            }
            else if (winPaiPair != null)
            {
                winPaiPair.WinningPai.transform.position = new Vector3(initXPai, initYPai, initZPai);
            }

            // 鳴き牌の初期座標
            initXPai = 0.0f;
            initYPai = 15.0f;
            initZPai = 0.0f;

            // 鳴き牌を決定
            foreach (PaiSet paiSet in paiSetConcealedList)
            {
                // 牌4つの座標を設定し、Z軸を増加
                // 鳴き牌なら横にする
                if (paiSet.Pai1 == paiSet.PaiConcealed)
                {
                    // TODO 横にする
                    paiSet.Pai1.transform.rotation = new Quaternion();

                    paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 0.0f;
                }
                else
                {
                    paiSet.Pai1.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiSet.Pai2 == paiSet.PaiConcealed)
                {
                    // TODO 横にする
                    paiSet.Pai2.transform.rotation = new Quaternion();

                    paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 0.0f;
                }
                else
                {
                    paiSet.Pai2.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiSet.Pai3 == paiSet.PaiConcealed)
                {
                    // TODO 横にする
                    paiSet.Pai3.transform.rotation = new Quaternion();

                    paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 0.0f;
                }
                else
                {
                    paiSet.Pai3.transform.position = new Vector3(initXPai, initYPai, initZPai);
                    initYPai += 1.0f;
                }

                if (paiSet.Pai4 == paiSet.PaiConcealed)
                {
                    // TODO 横にする
                    paiSet.Pai4.transform.rotation = new Quaternion();

                    paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                }
                else if (paiSet.Pai4 != null)
                {
                    paiSet.Pai4.transform.position = new Vector3(initXPai, initYPai, initZPai);
                }
                // Y軸を鳴き牌の初期座標にリセット
                initYPai = 15.0f;
                // Z軸を上に変更
                initZPai += 2.0f;
            }
        }
    }
}
