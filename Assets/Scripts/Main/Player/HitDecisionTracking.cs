using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̓����蔻��̒Ǐ]
/// </summary>
public class HitDecisionTracking : MonoBehaviour
{

    [SerializeField, Header("�v���C���[���i�[")]
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        // �Ǐ]
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
