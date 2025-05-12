using UnityEngine;
using Props;
using Extensions;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Config", order = 1)]
public class ConfigPropsLibrary : ScriptableObject
{
    public float FigureScale;
    public List<PropBaseFigure> Figures;
    public List<PropColor> Colors;
    public List<PropIcon> Icons;

    private void OnValidate()
    {
        IndexSetter();
    }

    private void IndexSetter()
    {
        Figures.SetIndexes();
        Colors.SetIndexes();
        Icons.SetIndexes();
    }
}