using UnityEngine;

public class EndlessTilingBackground : MonoBehaviour
{
    public GameObject backgroundTilePrefab;
    public int gridSize = 3; // 3x3 grid
    public Transform cameraTransform;

    private GameObject[,] tiles;
    private Vector2 previousCameraPosition;
    private Vector2 tileSize;
    private float cameraViewWidth;
    private float cameraViewHeight;

    
    public float thresholdMultiplier = 0.1f;

    void Start()
    {
        
        tileSize = CalculateTileSize(backgroundTilePrefab);
        
        CalculateCameraViewSize();
        
        tiles = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * tileSize.x, y * tileSize.y, 0);
                tiles[x, y] = Instantiate(backgroundTilePrefab, position, Quaternion.identity, transform);
            }
        }

        // Set the initial camera position
        previousCameraPosition = new Vector2(cameraTransform.position.x, cameraTransform.position.y);
    }

    void Update()
    {
        Vector2 cameraPosition = new Vector2(cameraTransform.position.x, cameraTransform.position.y);

        // Calculate a reduced threshold based on the multiplier (shift earlier)
        float shiftThresholdX = tileSize.x * thresholdMultiplier;
        float shiftThresholdY = tileSize.y * thresholdMultiplier;

        // Has the camera has moved enough to warrant repositioning tiles??
        if (Mathf.Abs(cameraPosition.x - previousCameraPosition.x) >= shiftThresholdX ||
            Mathf.Abs(cameraPosition.y - previousCameraPosition.y) >= shiftThresholdY)
        {
            RepositionTiles(cameraPosition);
            previousCameraPosition = cameraPosition;
        }
    }

    void CalculateCameraViewSize()
    {
        Camera camera = Camera.main;
        cameraViewHeight = 2f * camera.orthographicSize;
        cameraViewWidth = cameraViewHeight * camera.aspect;
    }

    void RepositionTiles(Vector2 cameraPosition)
    {
     
        int xOffset = Mathf.FloorToInt(cameraPosition.x / tileSize.x);
        int yOffset = Mathf.FloorToInt(cameraPosition.y / tileSize.y);

        // Move tiles horizontally if needed
        if (xOffset != Mathf.FloorToInt(previousCameraPosition.x / tileSize.x))
        {
            ShiftTilesHorizontally(xOffset);
        }

        // Move tiles vertically if needed
        if (yOffset != Mathf.FloorToInt(previousCameraPosition.y / tileSize.y))
        {
            ShiftTilesVertically(yOffset);
        }
    }

    void ShiftTilesHorizontally(int xOffset)
    {
        for (int y = 0; y < gridSize; y++)
        {
            if (cameraTransform.position.x > tiles[gridSize - 1, y].transform.position.x)
            {
                tiles[0, y].transform.position = new Vector3(tiles[gridSize - 1, y].transform.position.x + tileSize.x, tiles[0, y].transform.position.y, 0);
                ShiftRow(0, gridSize, y);
            }
            else if (cameraTransform.position.x < tiles[0, y].transform.position.x)
            {
                tiles[gridSize - 1, y].transform.position = new Vector3(tiles[0, y].transform.position.x - tileSize.x, tiles[gridSize - 1, y].transform.position.y, 0);
                ShiftRow(gridSize - 1, -1, y);
            }
        }
    }

    void ShiftTilesVertically(int yOffset)
    {
        for (int x = 0; x < gridSize; x++)
        {
            if (cameraTransform.position.y > tiles[x, gridSize - 1].transform.position.y)
            {
                tiles[x, 0].transform.position = new Vector3(tiles[x, gridSize - 1].transform.position.x, tiles[x, gridSize - 1].transform.position.y + tileSize.y, 0);
                ShiftColumn(0, gridSize, x);
            }
            else if (cameraTransform.position.y < tiles[x, 0].transform.position.y)
            {
                tiles[x, gridSize - 1].transform.position = new Vector3(tiles[x, 0].transform.position.x, tiles[x, 0].transform.position.y - tileSize.y, 0);
                ShiftColumn(gridSize - 1, -1, x);
            }
        }
    }

    void ShiftRow(int startIndex, int endIndex, int rowIndex)
    {
        GameObject temp = tiles[startIndex, rowIndex];
        for (int i = startIndex; (endIndex > startIndex ? i < endIndex - 1 : i > endIndex + 1); i += (endIndex > startIndex ? 1 : -1))
        {
            tiles[i, rowIndex] = tiles[i + (endIndex > startIndex ? 1 : -1), rowIndex];
        }
        tiles[endIndex > startIndex ? endIndex - 1 : 0, rowIndex] = temp;
    }

    void ShiftColumn(int startIndex, int endIndex, int colIndex)
    {
        GameObject temp = tiles[colIndex, startIndex];
        for (int i = startIndex; (endIndex > startIndex ? i < endIndex - 1 : i > endIndex + 1); i += (endIndex > startIndex ? 1 : -1))
        {
            tiles[colIndex, i] = tiles[colIndex, i + (endIndex > startIndex ? 1 : -1)];
        }
        tiles[colIndex, endIndex > startIndex ? endIndex - 1 : 0] = temp;
    }

    Vector2 CalculateTileSize(GameObject tile)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            return new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
        }
        else
        {
            Debug.LogWarning("Tile prefab does not have a Renderer. Using default size.");
            return new Vector2(10f, 10f); // Fallback size
        }
    }
}
