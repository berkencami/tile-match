#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TileMatch.LevelSystem;
using TileMatch.Utility;

namespace TileMatch.LevelEditor
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private SerializedProperty itemPalette;
        private SerializedProperty layerCount;
        private SerializedProperty layers;
        private int selectedItem = 0;
        private int selectedLayer = 0;

        private void OnEnable()
        {
            itemPalette = serializedObject.FindProperty("_itemPalette");
            layerCount = serializedObject.FindProperty("_layerCount");
            layers = serializedObject.FindProperty("_layers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            itemPalette.objectReferenceValue = (ItemPalette)EditorGUILayout.ObjectField("Item Palette",
                itemPalette.objectReferenceValue, typeof(ItemPalette), false);
            var newPalette = (ItemPalette)itemPalette.objectReferenceValue;

            EditorGUILayout.Space(20);

            if (newPalette == null)
            {
                EditorGUILayout.LabelField("Please assign a palette");
                return;
            }

            int newLayerCount = EditorGUILayout.IntSlider("Layer Count", layerCount.intValue, 1, 10);
            if (newLayerCount != layerCount.intValue)
            {
                layerCount.intValue = newLayerCount;
                UpdateLayersList();
            }

            selectedLayer = EditorGUILayout.Popup("Selected Layer", selectedLayer, 
                Enumerable.Range(0, layerCount.intValue).Select(i => $"Layer {i + 1}").ToArray());

            EditorGUILayout.Space(10);

            if (selectedLayer < layers.arraySize)
            {
                var currentLayer = layers.GetArrayElementAtIndex(selectedLayer);
                var row = currentLayer.FindPropertyRelative("row");
                var column = currentLayer.FindPropertyRelative("column");

                row.intValue = EditorGUILayout.IntSlider("Row", row.intValue, 1, 100);
                column.intValue = EditorGUILayout.IntSlider("Column", column.intValue, 1, 100);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Items");
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            for (var i = 0; i < newPalette.Items.Count; i++)
            {
                if (GUILayout.Button(newPalette.Items[i].Icon, GUILayout.Width(35), GUILayout.Height(35)))
                {
                    selectedItem = i;
                }
            }

            if (GUILayout.Button("Erase", GUILayout.Width(50), GUILayout.Height(35)))
            {
                selectedItem = newPalette.Items.Count + 1;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(20);

            if (selectedLayer < layers.arraySize)
            {
                var currentLayer = layers.GetArrayElementAtIndex(selectedLayer);
                var row = currentLayer.FindPropertyRelative("row").intValue;
                var column = currentLayer.FindPropertyRelative("column").intValue;
                var data = currentLayer.FindPropertyRelative("data");

                var levelData = GetLevelData(data.stringValue, row, column);
                levelData = GenerateGrid(levelData, row, column);
                data.stringValue = JsonExtension.SetToJson(levelData.ToArray());
            }

            if (GUILayout.Button("Save Level", GUILayout.Width(100), GUILayout.Height(35)))
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateLayersList()
        {
            while (layers.arraySize < layerCount.intValue)
            {
                layers.arraySize++;
                var newLayer = layers.GetArrayElementAtIndex(layers.arraySize - 1);
                newLayer.FindPropertyRelative("row").intValue = 1;
                newLayer.FindPropertyRelative("column").intValue = 1;
                newLayer.FindPropertyRelative("data").stringValue = string.Empty;
            }

            while (layers.arraySize > layerCount.intValue)
            {
                layers.arraySize--;
            }
        }

        private static LevelData[] GetLevelData(string json, int sizeX, int sizeY)
        {
            var data = new List<LevelData>();
            if (!string.IsNullOrEmpty(json))
            {
                data = JsonExtension.GetFromJson<LevelData>(json).ToList();
            }

            if (sizeX * sizeY > 0 && data.Count == sizeX * sizeY)
            {
                return data.ToArray();
            }

            data.Clear();
            for (var x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data.Add(new LevelData
                    {
                        _row = x,
                        _column = y,
                        _itemID = string.Empty
                    });
                }
            }

            return data.ToArray();
        }

        private LevelData[] GenerateGrid(LevelData[] data, int sizeX, int sizeY)
        {
            if (sizeX <= 0 || sizeY <= 0)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return data;
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear"))
            {
                data = GetLevelData(string.Empty, sizeX, sizeY);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            var backgroundColor = GUI.backgroundColor;
            var palette = this.itemPalette.objectReferenceValue as ItemPalette;

            for (var y = 0; y < sizeY; y++)
            {
                EditorGUILayout.BeginHorizontal();

                for (var x = 0; x < sizeX; x++)
                {
                    var item = data.FirstOrDefault(fd => fd._row == x && fd._column == y);
                    var id = item._itemID ?? string.Empty;
                    var icon = string.IsNullOrEmpty(id)
                        ? default
                        : (palette.Items.FirstOrDefault(fd => fd.Id == id)?.Icon ?? default);

                    var button = GUILayout.Button(icon, GUILayout.Width(35), GUILayout.Height(35));
                    if (button)
                    {
                        item._itemID = selectedItem < 0 || selectedItem >= palette.Items.Count
                            ? string.Empty
                            : palette.Items[selectedItem].Id;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            GUI.backgroundColor = backgroundColor;
            EditorGUILayout.Space(30);
            return data;
        }
    }
}
#endif
