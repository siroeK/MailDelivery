using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの当たり判定の追従
/// </summary>
public class HitDecisionTracking : MonoBehaviour
{

    [SerializeField, Header("プレイヤー情報格納")]
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        // 追従
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
