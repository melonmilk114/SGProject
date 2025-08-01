using UnityEditor;
using UnityEngine;

namespace Melon
{
    public class UIGameImageEditorMenu
    {
        [MenuItem("GameObject/Melon/UIGameImage")]
        static void CreateButton()
        {
            GameObject selectedObject = Selection.activeGameObject;
            Transform parent = selectedObject != null ? selectedObject.transform : null;

            if (selectedObject == null)
            {
                Debug.LogError("SelectObject == null");
            }
            else
            {
                GameObject panelObject = new GameObject("GameImage");
                panelObject.transform.SetParent(parent, false);

                panelObject.AddComponent<RectTransform>();
                var gameImage = panelObject.AddComponent<UIGameImage>();

                RectTransform panelRectTransform = panelObject.GetComponent<RectTransform>();
                if (parent != null)
                {
                    panelRectTransform.anchoredPosition = Vector2.zero;
                    panelObject.transform.localPosition = Vector3.zero;
                    panelRectTransform.localScale = Vector3.one;
                    panelRectTransform.sizeDelta = new Vector2(100, 100); // 부모에 맞춰 크기 조정 (선택 사항)
                }

                Selection.activeGameObject = panelObject;

                // 기본 설정
                gameImage.Init_Editor();
            }
        }
    }
}