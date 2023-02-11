using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌計算
public class PaiCal : MonoBehaviour
{
    // 基本形を符計算
    public static int allPaiSetCalPoint(List<PaiSet> paiSetList, PaiPair paiPair,
        PaiStatus.PAICHARACTERS ownWindPaiCharacters,
        PaiStatus.PAICHARACTERS roundWindPaiCharacters)
    {
        // 返却する符を宣言(副底込)
        int points = 20;

        // 全面子を符計算して加算
        foreach (PaiSet paiSet in paiSetList)
        {
            points += calPointOfPaiSet(paiSet);
        }
        // 頭が役牌かどうか
        bool isValueTiles = false;
        // 頭のPaiスクリプトを取得
        Pai pai = paiPair.Pai1.GetComponent<Pai>();
        if (pai.PaiCharacters == ownWindPaiCharacters ||
            pai.PaiCharacters == roundWindPaiCharacters)
        {
            isValueTiles = true;
        }

        // 頭を符計算して加算
        points += calPointOfPaiPair(paiPair, isValueTiles);

        return points;
    }

    // 面子の符計算
    public static int calPointOfPaiSet(PaiSet paiSet)
    {
        // 符宣言
        int points = 0;

        // 牌を1つ取得
        Pai pai = paiSet.Pai1.GetComponent<Pai>();

        /* 么九牌かどうかを判定 */
        // フラグ宣言
        bool isTerminalsAndHonors = false;
        bool isTerminals = false;

        // 么九牌チェック
        if (pai.PaiKind == PaiStatus.PAIKIND.字牌)
        {
            isTerminalsAndHonors = true;
        }
        else if (pai.PaiNumber == 1 || pai.PaiNumber == 9)
        {
            isTerminalsAndHonors = true;
            isTerminals = true;
        }

        // 明刻かどうか
        if (paiSet.IsConcealedPung)
        {
            points = 2;
            // 么九牌なら符を2倍に
            if (isTerminalsAndHonors)
            {
                points += points * 2;
            }
        }

        // 暗刻かどうか 
        else if (paiSet.IsConcealedPung)
        {
            points = 4;
            // 么九牌なら符を2倍に
            if (isTerminalsAndHonors)
            {
                points += points * 2;
            }
        }

        // 明槓かどうか
        else if (paiSet.IsConcealedPung)
        {
            points = 8;
            // 么九牌なら符を2倍に
            if (isTerminalsAndHonors)
            {
                points += points * 2;
            }
        }

        // 暗槓かどうか
        else if (paiSet.IsConcealedPung)
        {
            points = 16;
            // 么九牌なら符を2倍に
            if (isTerminalsAndHonors)
            {
                points += points * 2;
            }
        }

        // 和了牌があるかを確認
        if (paiSet.WinningPai != null)
        {
            // 数牌なら処理続行
            if (isTerminals)
            {
                // 面子の牌3つの数字を抽出
                Pai pai1 = paiSet.Pai1.GetComponent<Pai>();
                Pai pai2 = paiSet.Pai2.GetComponent<Pai>();
                Pai pai3 = paiSet.Pai3.GetComponent<Pai>();
                int[] paiNumberArray = { pai1.PaiNumber, pai2.PaiNumber, pai3.PaiNumber };

                // 配列を昇順にソート
                Array.Sort(paiNumberArray);

                // 和了牌の数字取得
                int winningPaiNumber = paiSet.WinningPai.GetComponent<Pai>().PaiNumber;

                // 和了牌が配列の何番目かを検査
                for (int i = 0; i < paiNumberArray.Length; i++)
                {
                    if (paiNumberArray[i] == winningPaiNumber)
                    {
                        // 一致した要素数が0,2の場合、辺張待ちかどうか検査
                        if (i == 0)
                        {
                            // 7,8,9なら辺張待ちなので符+2
                            if (paiNumberArray[0] == 7 &&
                                paiNumberArray[1] == 8 &&
                                paiNumberArray[2] == 9)
                            {
                                points += 2;
                                break;
                            }
                        }
                        else if (i == 2)
                        {
                            // 1,2,3なら辺張待ちなので符+2
                            if (paiNumberArray[0] == 1 &&
                                paiNumberArray[1] == 2 &&
                                paiNumberArray[2] == 3)
                            {
                                points += 2;
                                break;
                            }
                        }
                        else if (i == 1)
                        {
                            // 1の場合、嵌張待ちなので符+2
                            points += 2;
                            break;
                        }
                    }
                }
            }
        }
        return points;
    }

    // 対子の符計算
    public static int calPointOfPaiPair(PaiPair paiPair, bool isValueTiles)
    {
        // 符宣言
        int points = 0;

        // 牌を1つ取得
        Pai pai = paiPair.Pai1.GetComponent<Pai>();

        // 牌が役牌かどうかをチェック
        if (isValueTiles)
        {
            // 頭が役牌なので符+2
            points += 2;
        }

        // 和了牌があるかを確認
        if (paiPair.WinningPai != null)
        {
            // 単騎待ちしかないので符+2
            points += 2;
        }
        return points;
    }

    // 点数計算
    public static MahjongScoreStatus calScore(int points, int doubles)
    {
        // 翻数で分岐処理 // TODO 点数計算を追加
        if (doubles == 1)
        {
            switch (points)
            {
                case 30:
                    return new MahjongScoreStatus(1000, 1500, 300, 500);
                case 40:
                    return new MahjongScoreStatus(1300, 2000, 400, 700);
                case 50:
                    return new MahjongScoreStatus(1600, 2400, 400, 800);
                case 60:
                    return new MahjongScoreStatus(2000, 2900, 500, 1000);
                case 70:
                    return new MahjongScoreStatus(2300, 3400, 600, 1200);
                case 80:
                    return new MahjongScoreStatus(2600, 0, 700, 1300);
                case 90:
                    return new MahjongScoreStatus(2900, 0, 800, 1500);
                case 100:
                    return new MahjongScoreStatus(3200, 0, 800, 1600);
                case 110:
                    return new MahjongScoreStatus(2600, 0, 900, 1800);
                case 120:
                    return new MahjongScoreStatus(4000, 5800, 1000, 2000);
                default:
                    break;
            }
        }
        else if (doubles == 2)
        { }
        else if (doubles == 3)
        { }
        else if (doubles == 4)
        { }
        else if (doubles == 5)
        {
            // 満貫確定
            return new MahjongScoreStatus(8000, 12000, 2000, 4000);
        }
        else if (doubles == 6 || doubles == 7)
        {
            // 跳満確定
            return new MahjongScoreStatus(12000, 18000, 3000, 6000);
        }
        else if (doubles == 8 || doubles == 9 || doubles == 10)
        {
            // 倍満確定
            return new MahjongScoreStatus(16000, 24000, 4000, 8000);
        }
        else if (doubles == 11 || doubles == 12)
        {
            // 三倍満確定
            return new MahjongScoreStatus(24000, 36000, 6000, 12000);
        }
        else if (doubles >= 13)
        {
            // 役満確定
            return new MahjongScoreStatus(32000, 48000, 8000, 16000);
        }
        // 該当の点数がなかった場合はnullを返却
        return null;
    }
}