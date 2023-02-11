using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌オブジェクト
public class Pai : MonoBehaviour
{
    // 牌の種類(萬子、筒子、索子、字牌)
    [SerializeField]
    private PaiStatus.PAIKIND paiKind;

    [SerializeField]
    // 字牌
    private PaiStatus.PAICHARACTERS paiCharacters = PaiStatus.PAICHARACTERS.無;

    [SerializeField]
    // 数字
    private int paiNumber = 0;

    // 赤か
    [SerializeField]
    private bool isRed = false;

    /* コンストラクタ */
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

    void Start()
    {
        try
        {
            /* エラーチェック */
            // 字牌で0以外の数値が設定されているか
            if (this.paiKind.ToString().Equals("字牌"))
            {
                if (this.paiNumber == 0)
                {
                    // エラー
                    throw new Exception("字牌に数字が設定されています。");
                }

            }
            else
            {
                // 字牌以外で1-9以外の数値が設定されているか
                if (this.paiNumber >= 1 && this.paiNumber <= 9)
                {
                    // エラー
                    throw new Exception("字牌に数字が設定されています。");
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /* セッターゲッター */
    public PaiStatus.PAIKIND PaiKind { set; get; }
    public PaiStatus.PAICHARACTERS PaiCharacters { set; get; }
    public int PaiNumber { set; get; }
    public bool IsRed { set; get; }
}
