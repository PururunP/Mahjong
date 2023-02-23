using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メイン操作
public class Main : MonoBehaviour
{
    void Start()
    {
        // 牌を牌の量分生成
        PaiAmountManager paiAmountManager = this.gameObject.GetComponent<PaiAmountManager>();
        paiAmountManager.setPaiAmount();

        // 牌問題生成
        PaiQ paiQ = this.gameObject.GetComponent<PaiQ>();
        paiQ.setPai();
    }
}
