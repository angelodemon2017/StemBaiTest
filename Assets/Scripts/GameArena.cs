using System.Collections.Generic;
using UnityEngine;

public class GameArena : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<Transform> _slots;

    public Transform GetSpawnPoint => _spawnPoint;
    public List<Transform> Slots => _slots;
}