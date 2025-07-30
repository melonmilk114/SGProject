using UnityEngine;

namespace Melon
{
    public static class JsonHelper
    {
        public static T[] LoadJsonArrayFromFile<T>(string inFilePath) where T : class
        {
            var textAsset = CommonUtils.LoadTextAsset(inFilePath);
            if (textAsset == null)
            {
                Debug.LogError($"Failed to load json file: {inFilePath}");
                return null;
            }
            
            string newJson = "{ \"datas\": " + textAsset + "}";
            JsonWrapper<T> jsonWrapper = JsonUtility.FromJson<JsonWrapper<T>>(newJson);
            return jsonWrapper.datas;
        }
        
        [System.Serializable]
        private class JsonWrapper<T>
        {
            public T[] datas;
        }
    }
    }