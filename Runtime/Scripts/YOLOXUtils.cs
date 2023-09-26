using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if CJM_BBOX_2D_TOOLKIT
using CJM.BBox2DToolkit;

namespace CJM.YOLOXUtils
{
    /// <summary>
    /// A struct for grid coordinates and stride information.
    /// </summary>
    public struct GridCoordinateAndStride
    {
        public int xCoordinate;
        public int yCoordinate;
        public int stride;

        /// <summary>
        /// Initializes a new instance of the GridCoordinateAndStride struct.
        /// </summary>
        /// <param name="xCoordinate">The x-coordinate of the grid.</param>
        /// <param name="yCoordinate">The y-coordinate of the grid.</param>
        /// <param name="stride">The stride value for the grid.</param>
        public GridCoordinateAndStride(int xCoordinate, int yCoordinate, int stride)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.stride = stride;
        }
    }


    /// <summary>
    /// Utility class for YOLOX-related operations.
    /// </summary>
    public static class YOLOXUtility
    {

        /// <summary>
        /// Crop input dimensions to be divisible by the maximum stride.
        /// </summary>
        public static Vector2Int CropInputDims(Vector2Int inputDims)
        {
            inputDims[0] -= inputDims[0] % YOLOXConstants.Strides.Max();
            inputDims[1] -= inputDims[1] % YOLOXConstants.Strides.Max();

            return inputDims;
        }


        /// <summary>
        /// Generates a list of GridCoordinateAndStride objects based on input strides, height, and width.
        /// </summary>
        /// <param name="strides">An array of stride values.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="width">The width of the grid.</param>
        /// <returns>A list of GridCoordinateAndStride objects.</returns>
        public static List<GridCoordinateAndStride> GenerateGridCoordinatesWithStrides(int[] strides, int height, int width)
        {
            // Generate a list of GridCoordinateAndStride objects by iterating through possible grid positions and strides
            return strides.SelectMany(stride => Enumerable.Range(0, height / stride)
                                                           .SelectMany(g1 => Enumerable.Range(0, width / stride)
                                                                                        .Select(g0 => new GridCoordinateAndStride(g0, g1, stride)))).ToList();
        }

        /// <summary>
        /// Generates a list of bounding box proposals based on the model output, grid strides, and other parameters.
        /// </summary>
        /// <param name="modelOutput">The output of the YOLOX model.</param>
        /// <param name="gridCoordsAndStrides">A list of GridCoordinateAndStride objects.</param>
        /// <param name="numClasses">The number of object classes.</param>
        /// <param name="numBBoxFields">The number of bounding box fields.</param>
        /// <param name="confidenceThreshold">The confidence threshold for filtering proposals.</param>
        /// <returns>A list of BBox2D objects representing the generated proposals.</returns>
        public static List<BBox2D> GenerateBoundingBoxProposals(float[] modelOutput, List<GridCoordinateAndStride> gridCoordsAndStrides, int numClasses, int numBBoxFields, float confidenceThreshold)
        {
            int proposalLength = numClasses + numBBoxFields;

            // Process the model output to generate a list of BBox2D objects
            return gridCoordsAndStrides.Select((grid, anchorIndex) =>
            {
                int startIndex = anchorIndex * proposalLength;

                // Calculate coordinates and dimensions of the bounding box
                float centerX = (modelOutput[startIndex] + grid.xCoordinate) * grid.stride;
                float centerY = (modelOutput[startIndex + 1] + grid.yCoordinate) * grid.stride;
                float w = Mathf.Exp(modelOutput[startIndex + 2]) * grid.stride;
                float h = Mathf.Exp(modelOutput[startIndex + 3]) * grid.stride;

                // Initialize BBox2D object
                BBox2D obj = new BBox2D(
                    centerX - w * 0.5f,
                    centerY - h * 0.5f,
                    w, h, 0, 0);

                // Compute objectness and class probabilities for each bounding box
                float box_objectness = modelOutput[startIndex + 4];

                for (int classIndex = 0; classIndex < numClasses; classIndex++)
                {
                    float boxClassScore = modelOutput[startIndex + numBBoxFields + classIndex];
                    float boxProb = box_objectness * boxClassScore;

                    // Update the object with the highest probability and class label
                    if (boxProb > obj.prob)
                    {
                        obj.index = classIndex;
                        obj.prob = boxProb;
                    }
                }

                return obj;
            })
            .Where(obj => obj.prob > confidenceThreshold) // Filter by confidence threshold
            .OrderByDescending(x => x.prob) // Sort by probability
            .ToList();
        }


        public static BBox2DInfo[] GetBBox2DInfos(List<BBox2D> proposals, List<int> proposalIndices, List<(string, Color)> colormapList)
        {
            return proposalIndices
                .Select(index => proposals[index])
                .Select(bbox => new BBox2DInfo(bbox, colormapList[bbox.index].Item1, colormapList[bbox.index].Item2))
                .ToArray();
        }
    }
}
#endif
