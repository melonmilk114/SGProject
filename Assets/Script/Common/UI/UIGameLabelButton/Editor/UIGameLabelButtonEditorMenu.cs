using UnityEditor;
using UnityEngine;

namespace Melon
{
    public class UIGameLabelButtonEditorMenu : UIGameButtonEditorMenu
    {
        // 프리팹 기반 커스텀 UI 추가 (예시)
        [MenuItem("GameObject/Melon/UIGameLabelButton")]
        static void CreateLabelButton()
        {
            GameObject selectedObject = Selection.activeGameObject;
            Transform parent = selectedObject != null ? selectedObject.transform : null;

            if (selectedObject == null)
            {
                Debug.LogError("SelectObject == null");
            }
            else
            {
                GameObject panelObject = new GameObject("GameLabelButton");
                panelObject.transform.SetParent(parent, false);
                
                panelObject.AddComponent<RectTransform>();
                var gameButton = panelObject.AddComponent<UIGameLabelButton>();

                RectTransform panelRectTransform = panelObject.GetComponent<RectTransform>();
                if (parent != null)
                {
                    panelRectTransform.anchoredPosition = Vector2.zero;
                    panelObject.transform.localPosition = Vector3.zero;
                    panelRectTransform.localScale = Vector3.one;
                    panelRectTransform.sizeDelta = new Vector2(100, 100); // 부모에 맞춰 크기 조정 (선택 사항)
                }
            
                Selection.activeGameObject = panelObject;
                
                gameButton.Init_Editor();
            }
        }
    }
}