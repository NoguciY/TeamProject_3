using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    private Transform _playerTransform;

    //�A�C�e���X�|�[���̎Q��
    private ItemBoxSpawner itemBoxSpawner;

    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();
        if (gettableItemObject != null)
        {
            _playerTransform = other.gameObject.transform;
            foreach (var item in GameManager.Instance.items)
            {
                item.Collect(_playerTransform);
            }
            // �A�C�e���擾����
            Destroy(gameObject);
            
        }
    }
}
