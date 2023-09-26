using System;
using System.Collections.Generic;
using UnityEngine;

namespace CJM.YOLOXUtils
{
    // Serializable classes to store color map information from JSON
    [System.Serializable]
    class Colormap
    {
        public string label;
        public List<float> color;
    }

    [System.Serializable]
    class ColormapList
    {
        public List<Colormap> items;
    }

    /// <summary>
    /// Utility class for YOLOX-related operations.
    /// </summary>
    public static class ColormapUtility
    {
        /// <summary>
        /// Load the color map list from the JSON file
        /// <summary>
        public static List<(string, Color)> LoadColorMapList(TextAsset colormapFile)
        {
            if (IsColorMapListJsonNullOrEmpty(colormapFile))
            {
                Debug.LogError("Class labels JSON is null or empty.");
                return new List<(string, Color)>();
            }

            ColormapList colormapObj = DeserializeColorMapList(colormapFile.text);
            return UpdateColorMap(colormapObj);
        }

        /// <summary>
        /// Check if the color map JSON file is null or empty
        /// <summary>
        private static bool IsColorMapListJsonNullOrEmpty(TextAsset colormapFile)
        {
            return colormapFile == null || string.IsNullOrWhiteSpace(colormapFile.text);
        }

        /// <summary>
        /// Deserialize the color map list from the JSON string
        /// <summary>
        private static ColormapList DeserializeColorMapList(string json)
        {
            try
            {
                return JsonUtility.FromJson<ColormapList>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to deserialize class labels JSON: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update the color map list with deserialized data
        /// <summary>
        private static List<(string, Color)> UpdateColorMap(ColormapList colormapObj)
        {
            List<(string, Color)> colormapList = new List<(string, Color)>();

            if (colormapObj == null)
            {
                return colormapList;
            }

            // Add label and color pairs to the colormap list
            foreach (Colormap colormap in colormapObj.items)
            {
                Color color = new Color(colormap.color[0], colormap.color[1], colormap.color[2]);
                colormapList.Add((colormap.label, color));
            }

            return colormapList;
        }
    }
    
}
