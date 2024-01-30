using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ウイルスエネミーの管理プロジェクト
/// </summary>
public class ViralEnemyManager : MonoBehaviour
{

    [Header("敵のゲームオーバー理由文章")]
    [SerializeField, Tooltip("敵のゲームオーバー理由文章"), Multiline(2)]
    private string enemyExplanationText = "ウイルスによってメールの\n中身が漏洩してしまった。";

    [SerializeField, Header("爆発エフェクト")]
    private GameObject explosionEffect;

    [Header("移動させるアニメーション(DOLocalMoveX)")]
    [SerializeField, Tooltip("移動の演出時間")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("移動の変化の緩急（＝イージング）を指定")]
    private Ease moveEase = Ease.Unset;

    /// <summary>
    /// DOTweenを使った横軸方向の移動アニメーション
    /// </summary>
    public void HorizontalAxisMove(float targetX)
    {
        // ローカル座標で移動
        transform.DOLocalMoveX(
            targetX,                    // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {

        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            Destroy(this.gameObject);
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// 敵のゲームオーバー理由文章
    /// </summary>
    public string GetEnemyExplanation()
    {
        return enemyExplanationText;
    }

    /// <summary>
    /// 爆発エフェクト生成
    /// </summary>
    public void ExplosionEffectGeneration()
    {
        Instantiate(explosionEffect, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
    }

}
