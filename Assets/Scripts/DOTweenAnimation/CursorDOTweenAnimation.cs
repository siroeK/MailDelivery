using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// DOTweenを使ったカーソルのアニメーション
/// </summary>
public class CursorDOTweenAnimation : MonoBehaviour
{

    [Header("設定した移動終了地点に移動させるアニメーション(DOLocalMove)")]
    [SerializeField, Tooltip("移動の時の目標地点")]
    private GameObject[] targetPoint = null;
    [SerializeField, Tooltip("移動の演出時間")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("移動の変化の緩急（＝イージング）を指定")]
    private Ease moveEase = Ease.Unset;
    /// <summary>
    /// 移動軸方向の限定
    /// </summary>
    private enum MoveAxisDirectionLimited
    {
        AllAxes,        // 全軸
        VerticalAxis,   // 縦軸
        HorizontalAxis, // 横軸
    }

    [SerializeField, Tooltip("移動軸方向の限定")]
    private MoveAxisDirectionLimited moveAxisDirection = MoveAxisDirectionLimited.AllAxes;

    private bool moveAnimation = false;     // 移動アニメーション中

    [Header("拡縮アニメーション(DOScale)")]
    [SerializeField, Tooltip("目標の大きさ")]
    private Vector3 targetScale = Vector3.one;
    [SerializeField, Tooltip("サイズの拡縮演出時間")]
    private float scaleProductionTime = 1.0f;
    [SerializeField, Tooltip("サイズの拡縮の繰り返しタイプ")]
    private LoopType scaleLoopType = LoopType.Restart;
    [SerializeField, Tooltip("サイズの拡縮の変化の緩急（＝イージング）を指定")]
    private Ease scaleEase = Ease.Unset;

    private Tween focus;                    // 拡縮アニメーションの返り値を用意

    // SEを参照するやつ
    private SESound SEManager;

    private void Awake()
    {
        ScaleAnimation();

        // SEのスクリプト参照
        SEManager = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    /// <summary>
    /// カーソルを移動させる
    /// </summary>
    public void CursorMove(int target)
    {
        if (targetPoint[target] == null) return;

        MoveAnimation(targetPoint[target]);
    }

    /// <summary>
    /// カーソルの移動アニメーション
    /// </summary>
    private void MoveAnimation(GameObject obj)
    {
        if (moveAnimation) return;

        // カーソルフレーム移動のSE
        SEManager.CursorFrameMoveSE();

        switch (moveAxisDirection)
        {
            case MoveAxisDirectionLimited.AllAxes:
                AllAxisMove(obj.transform.localPosition);
                break;
            case MoveAxisDirectionLimited.VerticalAxis:
                VerticalAxisMove(obj.transform.localPosition.y);
                break;
            case MoveAxisDirectionLimited.HorizontalAxis:
                HorizontalAxisMove(obj.transform.localPosition.x);
                break;
            default:
                Debug.LogError("例外の移動アニメション");
                break;
        }
    }

    /// <summary>
    /// 全方向の移動アニメーション
    /// </summary>
    private void AllAxisMove(Vector3 target)
    {

        // ローカル座標で移動
        transform.DOLocalMove(
            target,                     // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            moveAnimation = true;
            this.focus.Kill();          // サイズ拡縮を止める
            this.transform.localScale = Vector3.one;
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            moveAnimation = false;
            ScaleAnimation();
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// 縦軸方向の移動アニメーション
    /// </summary>
    private void VerticalAxisMove(float targetY)
    {
        // ローカル座標で移動
        transform.DOLocalMoveY(
            targetY,                    // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            moveAnimation = true;
            this.focus.Kill();          // サイズ拡縮を止める
            this.transform.localScale = Vector3.one;
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            moveAnimation = false;
            ScaleAnimation();
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// 横軸方向の移動アニメーション
    /// </summary>
    private void HorizontalAxisMove(float targetX)
    {
        // ローカル座標で移動
        transform.DOLocalMoveX(
            targetX,                    // 移動終了地点
            moveProductionTime          // 演出時間
        )
        .SetEase(moveEase)              // 変化の緩急（＝イージング）を指定、一定の速度で値が変化
        .OnStart(() =>                  // 実行開始時のコールバック
        {
            moveAnimation = true;
            this.focus.Kill();          // サイズ拡縮を止める
            this.transform.localScale = Vector3.one;
        })
        .OnComplete(() =>               // 実行完了時のコールバック
        {
            moveAnimation = false;
            ScaleAnimation();
        })
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// カーソルのサイズ拡縮アニメーション
    /// </summary>
    private void ScaleAnimation()
    {
        if (moveAnimation) return;

        // 指定した値のサイズに
        this.focus = transform.DOScale( //返り値を保存    
            targetScale,                // スケール値
            scaleProductionTime         // 演出時間
        )
        .SetLoops(-1, scaleLoopType)    // 繰り返し（ループ）、移動後、最初の位置から再び移動を開始
        .SetEase(scaleEase)             // 変化の緩急（＝イージング）を指定、一定の速度で値が変化   
        .SetLink(gameObject);           // このGameObjectが削除された自動で止めるように紐付ける
    }

    /// <summary>
    /// カーソルの移動中アニメーションなら処理を返す
    /// </summary>
    public bool IsCursorMoveAnimation()
    {
        return moveAnimation;
    }


}
