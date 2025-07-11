using System;
using UnityEngine;

namespace TileMatch.Utility
{
    public static class JsonExtension
    {
        [Serializable]
        private class ItemList<T>
        {
            public T[] Items;
        }

        public static T[] GetFromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return Array.Empty<T>();
            }

            try
            {
                var itemList = JsonUtility.FromJson<ItemList<T>>(json);
                return itemList?.Items ?? Array.Empty<T>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}");
                return Array.Empty<T>();
            }
        }

        public static string SetToJson<T>(T[] array)
        {
            if (array == null)
            {
                array = Array.Empty<T>();
            }

            try
            {
                return JsonUtility.ToJson(new ItemList<T> { Items = array });
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to serialize JSON: {e.Message}");
                return JsonUtility.ToJson(new ItemList<T> { Items = Array.Empty<T>() });
            }
        }
    }
}