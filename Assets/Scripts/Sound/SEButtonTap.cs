using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEサウンドの1つの音を出すときに使うスクリプト
/// </summary>
public class SEButtonTap : MonoBehaviour
{

    [Header("サウンドエフェクト")]
    [Tooltip("ボタンタップのSE")]
    [SerializeField] private AudioClip buttonTapSE;         // ボタンタップのSE

    // SEを参照するやつ
    private SESound SEManager;

    // Start is called before the first frame update
    void Start()
    {
        // BGM・SEのスクリプト参照
        SEManager = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }


    // ボタンclickSE
    public void ButtonTapSE()
    {
        SEManager.SetSEOneShot(buttonTapSE);
    }

}
