Installation

To make it work, all you need is the 2 following scripts:
- EasyWallBuilder/Editor/WallBuilderEditor.cs
- EasyWallBuilder/Script/WallBuilder.cs

All other files are used only for the demo.

Note that the WallBuilderEditor.cs script MUST be in a folder called "Editor" due to Unity "Special folder names" ( https://docs.unity3d.com/Manual/SpecialFolders.html ).


Getting Started

1- Create an empty Game Object
2- Add "WallBuilder" script component
3- Place your wall prefab (at least one) in the "Block Prefabs" property
4- If needed, check "Use Z as length axis" depending the orientation of your wall prefab.
5- Set the "Block size" property depending on the size of your prefab and depending on the space you want between each block
6- In the scene view, you can now move the handles to generate walls from the prefab.

In the inspector, in the WallBuilder component, click on "Add crossing point" to add more points.
If all wall blocks are perpendicular to the direction of the wall, check the "Use Z As Length Axis" box to rotate the blocks by 90°.
Check "Should Stretch" if your prefabs can be stretched to perfectly fill the area between 2 points.
Mouse over each property for more information.
All the models in the screenshots/video are included.

## Credits
All the assets are made by Bryan Martinet (Maarti https://twitter.com/MaartiBr) except the textures of the brick wall made by Katsukagi (https://3dtextures.me/author/gendosplace/) on https://3dtextures.me/.