using UnityEngine;

public class ColoredMaterial {

    [SerializeField] private Material mat;

    [SerializeField] private MaterialColor col;

    public ColoredMaterial(Material mat, MaterialColor col)
    {
        this.mat = mat;
        this.col = col;
    }

    public Material Material
    {
        get { return mat; }
        set { mat = value; }
    }

    public MaterialColor Color
    {
        get { return col; }
        set { col = value; }
    }
}
