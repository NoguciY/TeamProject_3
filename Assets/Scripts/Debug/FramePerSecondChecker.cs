using UnityEngine;
using UnityEngine.UI;

public class FramePerSecondChecker : MonoBehaviour
{
    Text fpsText;
    float countTime;
    float countFrame;

    private void Start()
    {
        Canvas canvas = GetCanvas();
        fpsText = CreateFPSText(canvas.transform);
        countTime = 0f;
        countFrame = 0f;
    }

    /// <summary>
    /// FPS表示用のテキストを配置するキャンバスを取得
    /// </summary>
    private Canvas GetCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            return canvas;
        }

        //ゲーム内にキャンバスがない場合は生成する
        GameObject canvasObj = new GameObject("Canvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        return canvas;
    }

    /// <summary>
    /// FPS表示用のテキストを生成
    /// </summary>
    /// <returns></returns>
    private Text CreateFPSText(Transform parent)
    {
        Text fpsText = new GameObject("FPSText").AddComponent<Text>();
        fpsText.transform.SetParent(parent);
        fpsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        fpsText.alignment = TextAnchor.UpperLeft;
        fpsText.fontSize = 24;
        fpsText.transform.SetParent(parent);
        fpsText.rectTransform.anchorMin = Vector2.up;
        fpsText.rectTransform.anchorMax = Vector2.up;
        fpsText.rectTransform.pivot = Vector2.up;
        fpsText.rectTransform.anchoredPosition = new Vector2(10f, -10f);
        fpsText.rectTransform.sizeDelta = new Vector2(120f, 30f);
        return fpsText;
    }

    private void Update()
    {
        if (fpsText == null) return;

        countTime += Time.deltaTime;
        countFrame++;

        //0.5秒経過するまではフレーム数をカウント
        if (countTime < 0.5f) return;

        //0.5秒後、その期間にカウントされた
        //フレーム数からFPSを計算して表示
        fpsText.text = "FPS：" + (countFrame / countTime).ToString("F2");

        countTime = 0f;
        countFrame = 0f;
    }
}
