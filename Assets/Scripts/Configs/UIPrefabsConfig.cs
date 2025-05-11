using UnityEngine;
using UI;

[CreateAssetMenu(menuName = "Scriptable Objects/UI Prefabs", order = 1)]
public class UIPrefabsConfig : ScriptableObject
{
    public GameArena GameArena;
    public MainGamePlayWindow MainGamePlayWindow;
    public WinWindow WinWindow;
    public FailWindow FailWindow;
}