using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 入力値から速度を返す
    /// </summary>
    /// <param name="speed">速さ</param>
    /// <returns>速度</returns>
    public Vector3 InputMove(float speed)
    {
        //移動ベクトル
        Vector3 moveVec = Vector3.zero;

        //入力値を取得する
        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveVec -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVec -= Vector3.right;
        }

        //真っ直ぐと斜めの移動でベクトルの大きさが変わらないようにする
        if (moveVec.magnitude > 1f)
        {
            moveVec = moveVec.normalized;
        }

        return moveVec * speed * Time.deltaTime;
    }

    /// <summary>
    /// マウスカーソル方向に回転座標を更新する
    /// </summary>
    public void UpdateRotationForMouse(Camera camera, Transform myTransform)
    {
        //プレイヤーのワールド空間座標をスクリーン座標に変換
        Vector3 screenPos = camera.WorldToScreenPoint(myTransform.position);

        //プレイヤーからマウスカーソルの方向
        Vector3 direction = Input.mousePosition - screenPos;

        //2つのベクトルのなす角を取得
        //float angle = Utilities.GetAngle(Vector3.zero, direction);
        var dx = direction.x;
        var dy = direction.y;
        var rad = Mathf.Atan2(dx, dy);
        float angle = rad * Mathf.Rad2Deg;

        //プレイヤーの回転座標の更新
        Vector3 angles = myTransform.localEulerAngles;
        angles.y = angle;
        myTransform.localEulerAngles = angles;
    }
}
