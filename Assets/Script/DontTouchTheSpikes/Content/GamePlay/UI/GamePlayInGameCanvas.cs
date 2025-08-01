using System.Collections;
using Melon;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public class GamePlayInGameCanvas : GameElement
    {
        public UIGameImage uiBackImage;
        public UIGameLabel uiScoreLabel;

        public void GameReady()
        {
            uiScoreLabel.gameObject.SetActive(false);
        }

        public void GameStart()
        {
            uiScoreLabel.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            uiScoreLabel.gameObject.SetActive(true);
        }
        
        public void UpdateBackImageColor()
        {
            uiBackImage.color = GetRandomPastelColor();
        }
        
        public void UpdateScore(int score)
        {
            if (uiScoreLabel != null)
            {
                uiScoreLabel.SetText(score.ToString());
                StartCoroutine(CoScoreLabelPop());
            }
        }
        
        private IEnumerator CoScoreLabelPop()
        {
            uiScoreLabel.transform.localScale = Vector3.one;
            float t = 0f;
            float duration = 0.2f;
            float scaleUp = 1.2f;
            while (t < duration)
            {
                float normalized = t / duration;
                float phase = Mathf.PingPong(normalized * 2f, 1f); // 0~1~0

                float scale = Mathf.Lerp(1f, scaleUp, phase);
                uiScoreLabel.transform.localScale = Vector3.one * scale;

                t += Time.deltaTime;
                yield return null;
            }

            uiScoreLabel.transform.localScale = Vector3.one;
        }
        
        public static Color GetRandomPastelColor()
        {
            float hue = Random.Range(0f, 1f);             // 무작위 색상
            float saturation = Random.Range(0.5f, 0.7f);    // 중간 채도 (좀 더 색감 있게)
            float value = Random.Range(0.6f, 0.8f);         // 낮은 밝기 (어둡게)

            return Color.HSVToRGB(hue, saturation, value);
        }
        
        
    }
}