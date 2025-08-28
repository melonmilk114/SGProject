using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Melon
{
    public class ScriptCombiner
    {
        [MenuItem("Tools/Combine Scripts")]
        public static void CombineAllScripts()
        {
            // string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
            // string outputPath = Path.Combine(Application.dataPath, "CombinedScripts.txt");
            //
            // if (!Directory.Exists(scriptsPath))
            // {
            //     Debug.LogError("Scripts 폴더가 존재하지 않습니다!");
            //     return;
            // }
            
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                Debug.LogError("폴더를 선택해주세요!");
                return;
            }

            string selectedPath = AssetDatabase.GetAssetPath(selectedObject);
            if (!AssetDatabase.IsValidFolder(selectedPath))
            {
                Debug.LogError("선택한 항목은 폴더가 아닙니다!");
                return;
            }

            string fullSelectedPath = Path.Combine(Application.dataPath, selectedPath.Replace("Assets/", ""));
            string outputPath = Path.Combine(Application.dataPath, "CombinedScripts.txt");
            
            StringBuilder combinedContent = new StringBuilder();
            combinedContent.AppendLine("=== 모든 스크립트 파일 합본 ===");
            combinedContent.AppendLine($"생성 날짜: {System.DateTime.Now}");
            combinedContent.AppendLine("================================================\n");
            
            // .cs 파일들을 재귀적으로 찾기
            //string[] scriptFiles = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);
            string[] scriptFiles = Directory.GetFiles(fullSelectedPath, "*.cs", SearchOption.AllDirectories);
            
            if (scriptFiles.Length == 0)
            {
                Debug.LogWarning("Scripts 폴더에 .cs 파일이 없습니다!");
                return;
            }
            
            foreach (string filePath in scriptFiles)
            {
                try
                {
                    string relativePath = filePath.Replace(Application.dataPath, "Assets");
                    string fileName = Path.GetFileName(filePath);
                    string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
                    
                    combinedContent.AppendLine($"// ================================================");
                    combinedContent.AppendLine($"// 파일: {fileName}");
                    combinedContent.AppendLine($"// 경로: {relativePath}");
                    combinedContent.AppendLine($"// ================================================");
                    combinedContent.AppendLine();
                    combinedContent.AppendLine(fileContent);
                    combinedContent.AppendLine();
                    combinedContent.AppendLine();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"파일 읽기 실패: {filePath}, 오류: {e.Message}");
                }
            }
            
            try
            {
                File.WriteAllText(outputPath, combinedContent.ToString(), Encoding.UTF8);
                AssetDatabase.Refresh();
                
                Debug.Log($"스크립트 합본 완료! 총 {scriptFiles.Length}개 파일이 합쳐졌습니다.");
                Debug.Log($"결과 파일: Assets/CombinedScripts.txt");
                
                // 결과 파일을 Project 창에서 선택하기
                string assetPath = "Assets/CombinedScripts.txt";
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"파일 쓰기 실패: {e.Message}");
            }
        }
        
        [MenuItem("Assets/Combine Scripts", false, 1000)]
        public static void CombineScriptsFromAssets()
        {
            CombineAllScripts();
        }
    }
}