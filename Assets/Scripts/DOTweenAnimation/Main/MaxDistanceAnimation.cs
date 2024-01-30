using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メイン画面の最大距離テキストのアニメーション
/// </summary>
public class MaxDistanceAnimation : MonoBehaviour
{

    [Header("拡縮アニメーション(DOScale)")]
    [SerializeField, Tooltip("目標の大きさ")]
    private Vector3 targetScale = Vector3.one;
    [SerializeField, Tooltip("サイズの拡縮演出時間")]
    private float scaleProductionTime = 1.0f;
    [SerializeField, Tooltip("サイズの拡縮の繰り返しタイプ")]
    private LoopType scaleLoopType = LoopType.Restart;
    [SerializeField, Tooltip("サイズの拡縮の変化の緩急（＝イージング）を指定")]
    private Ease scaleEase = Ease.Unset;

    /// <summary>
    /// 最大距離テキストのサイズ拡縮アニメーション
    /// </summary>
    public void ScaleAnimation()
    {
        
        this.transform.localScale = Vector3.one;

        // 指定した値のサイズに
        transform.DOScale( //返り値を保存    
            targetScale,                // スケール値
            scaleProductionTime         // 演出時間
        )
        .SetLoops(2, scaleLoopType)     // 繰り返し（2回）、移動後、最初の位置から再び移動を開始
        .SetEase(scaleEase)             // 変化の緩急（＝イージング）を指定、一定の速度で値が変化   
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

}
