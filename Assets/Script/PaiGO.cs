using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌ゲームオブジェクト(Paiはnewができなくて不都合なのでゲームオブジェクト用として作成)
public class PaiGO : MonoBehaviour
{
    // 牌の種類(萬子、筒子、索子、字牌)
    [SerializeField]
    private PaiStatus.PAIKIND paiKind;

    // 字牌
    [SerializeField]
    private PaiStatus.PAICHARACTERS paiCharacters = PaiStatus.PAICHARACTERS.無;

    // 数字
    [SerializeField]
    private int paiNumber = 0;

    // 赤か
    [SerializeField]
    private bool isRed = false;

    // 牌オブジェクト
    private Pai pai;

    // 牌オブジェクト初期座標
    private Vector3 initVec;

    void Awake()
    {
        /* 牌オブジェクトを生成 */
        this.pai = new Pai(this.paiKind, this.paiCharacters,
            this.paiNumber, this.isRed);

        // 初期座標設定
        Vector3 vec = this.gameObject.transform.position;
        this.initVec = new Vector3(vec.x, vec.y + 1, vec.z);

        /* エラーチェック */
        try
        {
            // 字牌で0以外の数値が設定されているか
            if (this.pai.PaiKind.ToString().Equals("字牌"))
            {
                if (this.pai.PaiNumber != 0)
                {
                    // エラー
                    throw new Exception("字牌に数字が設定されています。");
                }
            }
            else
            {
                // 字牌以外で1-9以外の数値が設定されているか
                if (this.pai.PaiNumber < 1 && this.pai.PaiNumber > 9)
                {
                    // エラー
                    throw new Exception("数牌の数字が不正に設定されています。");
                }
                // 牌の種類に無以外が設定されているか
                else if (this.pai.PaiCharacters != PaiStatus.PAICHARACTERS.無)
                {
                    // エラー
                    throw new Exception("数牌の種類が不正に設定されています。");
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /* セッターゲッター */
    public Pai Pai
    {
        set { this.pai = value; }
        get { return this.pai; }
    }
    public Vector3 InitVec
    {
        set { this.initVec = value; }
        get { return this.initVec; }
    }
}
