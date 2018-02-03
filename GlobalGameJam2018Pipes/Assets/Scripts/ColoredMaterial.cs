using System;
using UnityEngine;

[Serializable]
public class ColoredMaterial : IEquatable<ColoredMaterial>
{
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

    public override string ToString()
    {
        return $"ColoredMaterial: {col} {mat}";
    }

    
    public bool Equals(ColoredMaterial other)
    {
        if (other == null) return false;
        return mat == other.mat && col == other.col;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(obj as ColoredMaterial);
    }

    public override int GetHashCode()
    {
        return 100 * mat.GetHashCode() + col.GetHashCode();
    }

}
