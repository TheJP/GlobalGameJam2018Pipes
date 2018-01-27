public class ColoredMaterial {

    [SerializeField] private Material mat;

    [SerializeField] private MaterialColor col;

    public Material material
    {
        get { return mat; }
        set { mat = value; }
    }

    public MaterialColor color
    {
        get { return col; }
        set { col = value; }
    }
}
