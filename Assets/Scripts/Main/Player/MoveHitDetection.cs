using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動した時の当たり判定
/// </summary>
public class MoveHitDetection : MonoBehaviour
{

    [SerializeField, Header("プレイヤーのオブジェクト参照")]
    private PlayerManager player;

    private bool hitDetection = false;  // 当たっているか

    // 同士が接触している（重なっている）間、持続的に実行
    private void OnTriggerStay(Collider other)
    {
        // キャラクターの状況が止まっている時以外は返す
        if (!player.PlayerMoveStopCheck()) return;

        // 障害物
        if (other.CompareTag("Obstacle"))
        {
            hitDetection = true;    // 障害物に当たった
        }
    }

    /// <summary>
    /// 移動した時の当たり判定をリセット
    /// </summary>
    public void MoveHitDetectionReset()
    {
        hitDetection = false;
    }

    /// <summary>
    /// 移動した時の当たり判定
    /// </summary>
    public bool GetMoveHitDetection()
    {
        return hitDetection;
    }

}
