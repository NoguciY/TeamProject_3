using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    private Transform _playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();
        if (gettableItemObject != null)
        {
            _playerTransform = other.gameObject.transform;
            if(GameManager.Instance.items.Count != 0)
            { 
                foreach (var item in GameManager.Instance.items)
                {
                    item.Collect(_playerTransform);
                    Debug.Log($"�o���l��{GameManager.Instance.items.Count}�������");
                }
                // �A�C�e���擾����
                Destroy(gameObject);
            }
        }
    }
}
