using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�����̒Ǐ]
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField, Header("�v���C���[���i�[")]
    private GameObject player;
    
    private Vector3 offset;      //���΋����擾�p

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera(�������g)��player�Ƃ̑��΋��������߂�
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�V�����g�����X�t�H�[���̒l��������
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + offset.z);
    }
}
