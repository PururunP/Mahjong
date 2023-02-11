using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌ステータス
public class PaiStatus : MonoBehaviour
{
    // 牌種類
    public enum PAIKIND
    {
        // 萬子
        萬子,
        // 筒子
        筒子,
        // 索子
        索子,
        // 字牌
        字牌
    }

    // 字牌
    public enum PAICHARACTERS
    {
        無,
        東,
        南,
        西,
        北,
        白,
        發,
        中
    }
}
