using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ScrollingDirection
{
    up,
    Down,
    Left,
    Right,
}

public class ObjectScroller : MonoBehaviour
{
    public ScrollingDirection direction = ScrollingDirection.up;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _scrollingSpeed;
    [Space]

    private bool _startScrolling;
    internal Vector3 dir;
    private List<GameObject> _spawnedObjects;

    private void Awake()
    {
        _spawnedObjects = new List<GameObject>();
        _startScrolling = false;
        dir = ChooseScrollingDirection(direction);
    }
    private void Update()
    {
        if (_startScrolling == false) return;
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            _spawnedObjects[i].transform.position += dir * _scrollingSpeed * Time.deltaTime;
        }
    }

    public void AddScrollingObject(GameObject scrollingObject)
    {
        _spawnedObjects.Add(scrollingObject);

        _startScrolling = true;
    }

    public void RemoveScrollingObject(GameObject scrollingObject)
    {
        _spawnedObjects.Remove(scrollingObject);
    }

    public Vector3 GetRandomRadius()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle * _spawnRadius;
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        return randomPosition;
    }


    private Vector3 ChooseScrollingDirection(ScrollingDirection direction)
    {
        Vector3 dir;

        switch (direction)
        {
            case ScrollingDirection.up:
                dir = Vector3.forward;
                break;
            case ScrollingDirection.Down:
                dir = Vector3.back;
                break;
            case ScrollingDirection.Left:
                dir = Vector3.left;
                break;
            case ScrollingDirection.Right:
                dir = Vector3.right;
                break;
            default:
                return dir = Vector3.one;
        }

        return dir;
    }


    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _spawnRadius);
#endif
    }
}
