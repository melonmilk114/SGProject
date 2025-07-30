using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGameButton), true)]
    public class UIGameButtonEditor : ButtonEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            // var button = target as UIGameButton;
            // if (button != null)
            // {
            //     // 기본 설정
            //     if (button.image == null)
            //     {
            //         button.image = button.gameObject.AddComponent<Image>();
            //         var sprite = Resources.Load<Sprite>("Sprite/square");
            //         if (sprite != null)
            //             button.image.sprite = sprite;
            //         else
            //             button.image.sprite = null;
            //     }
            //
            //     button.transition = Selectable.Transition.ColorTint;
            //     button.colors = new ColorBlock
            //     {
            //         normalColor = new Color(1f, 1f, 1f, 1f),
            //         highlightedColor = new Color(1f, 1f, 1f, 1f),
            //         pressedColor = new Color(1f, 1f, 1f, 1f),
            //         selectedColor = new Color(1f, 1f, 1f, 1f),
            //         disabledColor = new Color(0.8f, 0.8f, 0.8f, 0.5f),
            //         colorMultiplier = 1,
            //         fadeDuration = 0.1f
            //     };
            //     
            //     if (button.isClickScaleAni)
            //     {
            //         if (button.downScaleAni == null)
            //         {
            //             var downScaleAni = button.gameObject.AddComponent<UIAnimationScaleDown>();
            //             downScaleAni.startScale = new Vector3(1f, 1f, 1f);
            //             downScaleAni.endScale = new Vector3(0.9f, 0.9f, 1f);
            //             downScaleAni.duration = 0.1f;
            //             downScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
            //         }
            //         
            //         if (button.upScaleAni == null)
            //         {
            //             var downScaleAni = button.gameObject.AddComponent<UIAnimationScaleUp>();
            //             downScaleAni.startScale = new Vector3(0.9f, 0.9f, 1f);
            //             downScaleAni.endScale = new Vector3(1f, 1f, 1f);
            //             downScaleAni.duration = 0.1f;
            //             downScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
            //         }
            //     }
            //     else
            //     {
            //         var compList = button.gameObject.GetComponents<UIAnimationScale>();
            //         foreach (var comp in compList)
            //         {
            //             DestroyImmediate(comp);
            //         }
            //     }
            //     
            //     if (button.isClickSound)
            //     {
            //         if (button.clickSound == null)
            //             button.gameObject.AddComponent<UIGameButtonSound>();
            //     }
            //     else
            //         DestroyImmediate(button.gameObject.GetComponent<UIGameButtonSound>());
            // }
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIGameButton button = (UIGameButton)target;
            button.isClickScaleAni = EditorGUILayout.Toggle("UseClickScaleAni", button.isClickScaleAni);
            button.isClickSound = EditorGUILayout.Toggle("UseClickSound", button.isClickSound);

            // 변경 사항 적용
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}