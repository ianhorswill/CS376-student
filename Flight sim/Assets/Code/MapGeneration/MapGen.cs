using UnityEngine;

public class MapGen : MonoBehaviour {
    [Header("Remake Map")]
    public bool GenerateNewMap = true;
    public int Rows = 64;
    public int Cols = 64;
    public float[,] Heights;

    [Header("Cragginess")]
    [Range(0f, 1f)]
    public float Smoothness = 0.5f;
    [Header("Island Height")]
    public float Amplitude = 0.1f;

    public Terrain Topography;

    private Island[] Islands;
    
    void Start () {
        if (GenerateNewMap) {
            Islands = FindObjectsOfType<Island>();
            InitializeMap();
            InitializeTerrain();
        }
    }

    private void InitializeMap() {
        Heights = new float[Rows, Cols];
        float _rowInc = 1f / Rows;
        float _colInc = 1f / Cols;
        float x = 0f;
        float y = 0f;

        float[,] localAmplitudes = InitializeAmps();

        for (int row = 0; row < Rows; row++) {
            for (int col = 0; col < Cols; col++) {

                var localAmplitude = localAmplitudes[row, col] * Amplitude;
                var scale = 1 / Smoothness;
                Heights[row, col] = localAmplitude*Mathf.PerlinNoise(x*scale, y*scale);
                scale *= 2f;
                Heights[row, col] += localAmplitude*Mathf.PerlinNoise(x*scale, y*scale) * 0.5f;
                scale *= 2f;
                Heights[row, col] += localAmplitude*Mathf.PerlinNoise(x*scale, y*scale) * 0.25f;
                scale *= 2f;
                Heights[row, col] += localAmplitude*Mathf.PerlinNoise(x*scale, y*scale) * 0.125f;
                y += _colInc;
            }
            x += _rowInc;
            y = 0f;
        }
    }

    private float[,] InitializeAmps() {
        float[,] result = new float[Rows, Cols];
        int length = Islands.Length;
        for (float row = 0f; row < Rows; row++) {
            for (float col = 0f; col < Cols; col++) {
                float amplitude = 0f;
                for (int i = 0; i < length; i++) {
                    amplitude += Islands[i].Height *
                    MathUtil.MathUtil.Gaussian2D(row, col,
                        Islands[i].x, Islands[i].y,
                        Islands[i].LengthX, Islands[i].LengthY);
                }
                result[(int)row, (int)col] = amplitude;
            }
        }
        return result;
    }

    private void InitializeTerrain() {
        Topography.terrainData.SetHeights(0, 0, Heights);
    }
}
