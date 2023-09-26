using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CJM.YOLOXUtils
{
    public class YOLOXConstants
    {
        // Stride values used by the YOLOX model
        public static readonly int[] Strides = { 8, 16, 32 };

        // Number of fields in each bounding box
        public static readonly int NumBBoxFields = 5;
    }
}
