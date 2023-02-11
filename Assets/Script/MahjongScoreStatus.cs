using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 点数ステータス
public class MahjongScoreStatus : MonoBehaviour
{
    // ロン子点数
    private int koRon;
    // ロン親点数
    private int oyaRon;
    // ツモ子点数
    private int koTumo;
    // ツモ親点数
    private int oyaTumo;

    /* コンストラクタ */
    public MahjongScoreStatus(int koRon, int oyaRon, int koTumo, int oyaTumo)
    {
        this.koRon = koRon;
        this.oyaRon = oyaRon;
        this.koTumo = koTumo;
        this.oyaTumo = oyaTumo;
    }
}
