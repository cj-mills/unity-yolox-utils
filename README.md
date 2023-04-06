## YOLOX Unity Utilities

This Unity package provides utility functions to work with YOLOX object detection models. It helps in generating grid coordinates with strides, and extracting bounding box proposals from the model output.



## Features

1. `GridCoordinateAndStride` struct: Represents grid coordinates and stride information.

2. `YOLOXUtility`class: Provides utility methods for working with YOLOX object detection models.
   - `GenerateGridCoordinatesWithStrides`: Generates a list of `GridCoordinateAndStride` objects based on input strides, height, and width.
   - `GenerateBoundingBoxProposals`: Generates a list of `BBox2D` objects representing bounding box proposals based on the model output, grid strides, and other parameters.



## Getting Started

### Prerequisites

- Unity game engine

### Installation

You can install the YOLOX Unity Utilities package using the Unity Package Manager:

1. Open your Unity project.
2. Go to Window > Package Manager.
3. Click the "+" button in the top left corner, and choose "Add package from git URL..."
4. Enter the GitHub repository URL: `https://github.com/cj-mills/unity-yolox-utils.git`
5. Click "Add". The package will be added to your project.

For Unity versions older than 2021.1, add the Git URL to the `manifest.json` file in your project's `Packages` folder as a dependency:

```json
{
  "dependencies": {
    "com.cj-mills.unity-yolox-utils": "https://github.com/cj-mills/unity-yolox-utils.git",
    // other dependencies...
  }
}

```



## License

This project is licensed under the MIT License. See the [LICENSE](Documentation~/LICENSE) file for details.