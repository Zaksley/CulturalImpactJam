using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maarti {
    [ExecuteInEditMode]
    public class WallBuilder : MonoBehaviour {

        [Header("Wall")]
        [Tooltip("Whether you want to generate a wall between the first and last point.")] public bool closeTheLoop = false;
        [Tooltip("Points through which the wall must pass. Name must start with \"WallPoint \".")] public Transform[] crossingPoints = new Transform[2];

        [Header("Individual Block")]
        [Tooltip("Individual wall block prefabs. At least one is required but you can add several to add diversity.")] public GameObject[] blockPrefabs;
        [Tooltip("Lenght size of one individual wall block.")] public float blockSize = 1f;
        [Tooltip("Wheter the length of the wall prefab is along the Z axis (true) or X axis (false).")] public bool useZAsLengthAxis = false;
        [Tooltip("Whether each block should be stretched to fit exactly the space between 2 points.")] public bool shouldStretch = true;
        [Tooltip("Reverse X (or Z) axis on some blocks to add visual diversity.")] public bool randomlyReverseLenghtAxis = false;

        [Header("Collider")]
        [Tooltip("Generate one box collider for each wall.")] public bool generateOptimizedCollider = true;
        [Tooltip("Thickness and height of the generated collider.")] public Vector2 colliderDimensions = new Vector2(0.2f, 1f);
        [Tooltip("If the block units have collider components, it will remove it.")] public bool removeEachBlockColliders = true;

        [Header("Editor")]
        [Tooltip("Size of the handles in the scene view.")] [Range(0f, 10f)] public float handleSize = 1f;
        [Tooltip("Whether it should lock the Y axis when moving the handles in the scene view.")] public bool lockYAxis = true;


        [Header("Safety")]
        [Tooltip("Limit the number of blocks for each wall.")] public int maxBlocksPerWall = 1000;
        [Tooltip("Maximum time the generation can take. This can avoid infinite loop.")] public float generationMaxTime = 10f;

        private Transform _start;
        private Transform _end;
        private float _stretchedBlockSize;
        private Transform _wallBlocksContainer;
        private Vector3 _direction;
        private float _wallDistance;
        private Quaternion _rotation;
        private Vector3 _centerOfWall;
        // Change detection
        private List<Vector3> _lastPointsPosition = new List<Vector3>();
        private GameObject[] _lastBlockPrefabs;
        private float _lastObjectSize;
        private bool _lastCloseTheLoop;
        private bool _lastShouldStretch;
        private bool _lastGenerateCollider;
        private Vector2 _lastColliderDimensions;
        private bool _lastRemoveEachBlockCollider;
        private bool _lastRandomlyReverseZAxis;
        private bool _lastUseZAsLengthAxis;

        private void Awake() {
            if (crossingPoints != null && crossingPoints.Length >= 1) {
                Vector3 direction = Vector3.forward + Vector3.right * blockSize * 2;
                for (int i = 0; i < crossingPoints.Length; i++) {
                    Transform point = crossingPoints[i];
                    if (point == null) {
                        Transform wallPointTransform = transform.Find("WallPoint " + i);
                        if (wallPointTransform == null) {
                            GameObject wallPoint = new GameObject("WallPoint " + i);
                            wallPointTransform = wallPoint.transform;
                            wallPointTransform.position = transform.position + direction * i;
                        }
                        wallPointTransform.transform.parent = transform;
                        crossingPoints[i] = wallPointTransform;
                    }
                }
            }
        }

        public void Update() {
            // Checks
            if (crossingPoints == null || crossingPoints.Length < 2 || crossingPoints[0] == null || blockPrefabs == null || blockPrefabs.Length == 0) {
                return;
            }
            foreach (GameObject block in blockPrefabs) {
                if (block == null) {
                    Debug.LogWarning(GetType().Name + ": at least one of the blockPrefabs is null, assign it or delete it.");
                    return;
                }
            }
            if (!ChangeDetection()) {
                return;
            }
            _start = _end = crossingPoints[0];
            for (int i = 0; i < crossingPoints.Length; i++) {
                if (crossingPoints[i] == null) {
                    continue;
                }
                // Closing the loop
                if (i == 0 && closeTheLoop) {
                    _start = crossingPoints[crossingPoints.Length - 1];
                }
                // If not closing the loop, destroy the "Wall 0" if exists (that previously closed the loop)
                else if (i == 0) {
                    _wallBlocksContainer = FindBlocksContainer(0);
                    DestroyWall();
                    DestroyImmediate(_wallBlocksContainer.gameObject);
                    continue;
                }
                else {
                    _start = _end;
                }
                _end = crossingPoints[i];
                InitVariables(i);
                DestroyWall();
                GenerateWall();
                DestroyCollider();
                GenerateCollider();
            }
        }

        private void InitVariables(int iterationNumber) {
            _direction = (_end.position - _start.position).normalized;
            _wallDistance = Vector3.Distance(_start.position, _end.position);
            _rotation = Quaternion.LookRotation(_direction);
            _centerOfWall = (_end.position - _start.position) / 2 + _start.position;
            _wallBlocksContainer = FindBlocksContainer(iterationNumber);
            _wallBlocksContainer.position = _centerOfWall;
            _wallBlocksContainer.rotation = _rotation;
        }

        private void DestroyWall() {
            if (_wallBlocksContainer != null) {
                for (int i = _wallBlocksContainer.childCount - 1; i >= 0; i--) {
                    DestroyImmediate(_wallBlocksContainer.GetChild(i).gameObject);
                }
            }
        }

        private void GenerateWall() {
            if (blockSize <= 0f) {
                Debug.LogWarning(GetType().Name + ": blockSize must be greater than zero.");
                blockSize = 1f;
            }
            float startTime = Time.realtimeSinceStartup;
            float dotProduct = 1f;
            float scaleRatio = 1f;
            _stretchedBlockSize = blockSize;
            if (shouldStretch) {
                float remainingSpaceToFit = _wallDistance % blockSize;
                int numberOfBlocks = Mathf.FloorToInt(_wallDistance / blockSize);
                numberOfBlocks = Mathf.Max(1, numberOfBlocks);
                float spaceToFitByBlock = remainingSpaceToFit / numberOfBlocks;
                _stretchedBlockSize = blockSize + spaceToFitByBlock;
                scaleRatio = 1 + (spaceToFitByBlock / blockSize);
            }
            Vector3 position = _start.position + _direction * _stretchedBlockSize / 2f;
            Quaternion rotation = Quaternion.LookRotation(_direction);
            bool killSwitch = false;
            // Instantiate first block
            SpawnWall(position, rotation, scaleRatio);
            Vector3 lastPosition = position;
            while (!killSwitch && _wallBlocksContainer.childCount <= maxBlocksPerWall && dotProduct > 0f) {
                if (Time.realtimeSinceStartup - startTime > generationMaxTime) {
                    killSwitch = true;
                    DestroyWall();
                    Debug.LogError(GetType().Name + " took more time than generationMaxTime (" + generationMaxTime + "s). Generation aborted. Script disabled.");
                    enabled = false;
                }
                position = lastPosition + _direction * _stretchedBlockSize;
                dotProduct = Vector3.Dot((_end.position - position).normalized, _direction);
                if (dotProduct > 0f) {
                    SpawnWall(position, rotation, scaleRatio);
                }
                lastPosition = position;
            }
        }

        private void SpawnWall(Vector3 position, Quaternion rotation, float scaleRatio = 1f) {
            GameObject wall = Instantiate(blockPrefabs[UnityEngine.Random.Range(0, blockPrefabs.Length)], position, rotation, _wallBlocksContainer);
            Vector3 scale = wall.transform.localScale;
            if (shouldStretch && useZAsLengthAxis) {
                scale.z *= scaleRatio;
            }
            else if (shouldStretch && !useZAsLengthAxis) {
                scale.x *= scaleRatio;
            }
            if (randomlyReverseLenghtAxis && UnityEngine.Random.value > .5f) {
                if (useZAsLengthAxis) {
                    scale.z *= -1;
                }
                else {
                    scale.x *= -1;
                }
            }
            wall.transform.localScale = scale;
            if (!useZAsLengthAxis) {
                Vector3 localEulerAngles = wall.transform.localEulerAngles;
                localEulerAngles.y = 90;
                wall.transform.localEulerAngles = localEulerAngles;
            }
            if (removeEachBlockColliders) {
                foreach (Collider collider in wall.GetComponentsInChildren<Collider>()) {
                    DestroyImmediate(collider);
                }
            }
        }

        private void DestroyCollider() {
            if (_wallBlocksContainer == null) {
                return;
            }
            foreach (BoxCollider collider in _wallBlocksContainer.GetComponents<BoxCollider>()) {
                DestroyImmediate(collider);
            }
        }

        private void GenerateCollider() {
            if (!generateOptimizedCollider || _wallBlocksContainer == null) {
                return;
            }
            BoxCollider boxCollider = _wallBlocksContainer.gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(colliderDimensions.x, colliderDimensions.y, _wallDistance);
            boxCollider.center = new Vector3(0f, colliderDimensions.y / 2f, 0f);

        }

        private bool ChangeDetection() {
            bool hasChanged = false;
            if (_lastPointsPosition.Count != crossingPoints.Length) {
                hasChanged = true;
                // Init _lastPointsPosition
                _lastPointsPosition.Clear();
                for (int i = 0; i < crossingPoints.Length; i++) {
                    _lastPointsPosition.Add(crossingPoints[i].position);
                }
            }
            // Check change in crossing points position
            for (int i = 0; i < crossingPoints.Length; i++) {
                if (crossingPoints[i] == null) {
                    Debug.LogError(GetType().Name + ": Element " + i + " of crossingPoints (" + name + ") is null. Assign it or delete this element.");
                }
                else if (_lastPointsPosition[i] != crossingPoints[i].position) {
                    hasChanged = true;
                    _lastPointsPosition[i] = crossingPoints[i].position;
                    break;
                }
            }
            // Check change in other properties
            hasChanged = hasChanged ||
                blockSize != _lastObjectSize ||
                blockPrefabs != _lastBlockPrefabs ||
                shouldStretch != _lastShouldStretch ||
                generateOptimizedCollider != _lastGenerateCollider ||
                colliderDimensions != _lastColliderDimensions ||
                removeEachBlockColliders != _lastRemoveEachBlockCollider ||
                closeTheLoop != _lastCloseTheLoop ||
                randomlyReverseLenghtAxis != _lastRandomlyReverseZAxis ||
                useZAsLengthAxis != _lastUseZAsLengthAxis;
            if (hasChanged) {
                _lastObjectSize = blockSize;
                _lastBlockPrefabs = blockPrefabs;
                _lastShouldStretch = shouldStretch;
                _lastGenerateCollider = generateOptimizedCollider;
                _lastColliderDimensions = colliderDimensions;
                _lastRemoveEachBlockCollider = removeEachBlockColliders;
                _lastCloseTheLoop = closeTheLoop;
                _lastRandomlyReverseZAxis = randomlyReverseLenghtAxis;
                _lastUseZAsLengthAxis = useZAsLengthAxis;
            }
            return hasChanged;
        }

        private Transform FindBlocksContainer(int number) {
            Transform containerTransform = transform.Find("Walls");
            if (containerTransform == null) {
                GameObject containerObj = new GameObject("Walls");
                containerObj.transform.parent = transform;
                containerTransform = containerObj.transform;
            }
            Transform wallContainerTransform = containerTransform.Find("Wall " + number);
            if (wallContainerTransform == null) {
                GameObject containerObj = new GameObject("Wall " + number);
                containerObj.transform.parent = containerTransform;
                containerObj.isStatic = true;
                wallContainerTransform = containerObj.transform;
                wallContainerTransform.localPosition = Vector3.zero;
                wallContainerTransform.rotation = _rotation;
            }
            return wallContainerTransform;
        }

        public void AddCrossingPoint() {
            GameObject point = new GameObject("WallPoint " + crossingPoints.Length);
            point.transform.parent = transform;
            point.transform.localPosition = Vector3.zero;
            if (crossingPoints.Length > 0 && crossingPoints[crossingPoints.Length - 1] != null) {
                Vector2 randomVector = UnityEngine.Random.insideUnitCircle;
                point.transform.position = crossingPoints[crossingPoints.Length - 1].position + new Vector3(blockSize + randomVector.x, 0f, blockSize + randomVector.y) * 2;
            }
            Transform[] newCrossingPoints = new Transform[crossingPoints.Length + 1];
            Array.Copy(crossingPoints, newCrossingPoints, crossingPoints.Length);
            newCrossingPoints[newCrossingPoints.Length - 1] = point.transform;
            crossingPoints = newCrossingPoints;
        }

        public void RemoveLastCrossingPoint() {
            if (crossingPoints.Length <= 0) {
                return;
            }
            Transform point = crossingPoints[crossingPoints.Length - 1];
            if (point != null) {
                DestroyImmediate(point.gameObject);
            }
            Transform[] newCrossingPoints = new Transform[crossingPoints.Length - 1];
            Array.Copy(crossingPoints, newCrossingPoints, crossingPoints.Length - 1);
            crossingPoints = newCrossingPoints;
            // Refresh walls
            DestroyAllWalls();
        }

        private void DestroyAllWalls() {
            Transform containerTransform = transform.Find("Walls");
            if (containerTransform != null) {
                DestroyImmediate(containerTransform.gameObject);
            }
        }

        private void DestroyAllWallPoints() {
            for (int i = transform.childCount - 1; i >= 0; i--) {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.name.StartsWith("WallPoint")) {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
        }

        public void Reset() {
            DestroyAllWalls();
            DestroyAllWallPoints();
            Awake();
        }

    }
}
