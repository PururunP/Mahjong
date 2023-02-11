using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 牌の数量のinputTextの処理
public class InputTextPaiAmount : MonoBehaviour
{
    // 牌の数量InputField
    [SerializeField]
    private InputField inputPaiAmount;

    // 牌の種類
    [SerializeField]
    private Pai pai;

    // 牌の数量取得
    public string getInputPaiAmount()
    {
        return this.inputPaiAmount.GetComponent<InputField>().text;
    }

    // 牌の種類取得
    public Pai getPai()
    {
        return this.pai;
    }
}
