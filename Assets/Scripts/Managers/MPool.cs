using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;

namespace Managers
{
    public sealed class MPool : BaseManager
    {
	    [SerializeField] private bool _logStatus = false;
	    
	    [Space,SerializeField] private List<PoolItem> _poolItems = new List<PoolItem>();

	    private bool _dirty = false;

	    private Transform _root;

	    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _prefabLookup = 
		    new Dictionary<GameObject, ObjectPool<GameObject>>();
	    
	    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup = 
		    new Dictionary<GameObject, ObjectPool<GameObject>>();

	    protected override void Init()
	    {
		    base.Init();
		    
		    _root = new GameObject().transform;
		    
		    _root.SetParent(transform);
		    
		    _root.name = "Pool";
	    }

	    protected override void Launch()
	    {
		    base.Launch();

		    Observable
			    .EveryUpdate()
			    .Where(_ => _logStatus && _dirty)
			    .Subscribe(_ =>
			    {
				    PrintStatus();
			    
				    _dirty = false;
			    })
			    .AddTo(ManagerDisposable);

		    FirstWarmPool();
	    }

	    protected override void Clear()
	    {
		    base.Clear();
		    
		    _prefabLookup.Clear();
		    _instanceLookup.Clear();
	    }

	    private void Warm(GameObject prefab, int size)
	    {
		    prefab.gameObject.SetActive(false);

		    if (_prefabLookup.ContainsKey(prefab))
		    {
			    throw new Exception($"Pool for prefab {prefab.name} has already been created");
		    }

		    ObjectPool<GameObject> pool = 
			    new ObjectPool<GameObject>(() => InstantiatePrefab(prefab), size);
		    
		    _prefabLookup[prefab] = pool;
		    _dirty = true;
	    }

	    private GameObject Spawn(GameObject prefab)
	    {
		    return Spawn(prefab, Vector3.zero, Quaternion.identity);
	    }

	    private GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	    {
		    if (!_prefabLookup.ContainsKey(prefab))
		    {
			    WarmPool(prefab, 1);
		    }

		    ObjectPool<GameObject> pool = _prefabLookup[prefab];

		    GameObject clone = pool.GetItem();
		    
		    clone.transform.position = position;
		    clone.transform.rotation = rotation;
		    clone.SetActive(true);

		    _instanceLookup.Add(clone, pool);
		    _dirty = true;
		    
		    return clone;
	    }

	    private void Release(GameObject clone)
	    {
		    clone.SetActive(false);

		    if (_instanceLookup.ContainsKey(clone))
		    {
			    _instanceLookup[clone].ReleaseItem(clone);
			    _instanceLookup.Remove(clone);
			    _dirty = true;
		    }
		    else
		    {
			    CustomDebug.LogWarning($"No pool contains the object: {clone.name}");
		    }
	    }

	    private GameObject InstantiatePrefab(GameObject prefab)
	    {
		    GameObject go = Instantiate(prefab);
		    
		    if (_root != null)
		    {
			    go.transform.parent = _root;
		    }

		    return go;
	    }

	    private void PrintStatus()
	    {
		    foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in _prefabLookup)
		    {
			    CustomDebug.Log($"Object Pool for Prefab: {keyVal.Key.name} In Use: {keyVal.Value.CountUsedItems} Total: {keyVal.Value.Count}");
		    }
	    }

	    private void FirstWarmPool()
	    {
		    foreach (PoolItem pool in _poolItems)
		    {
			    WarmPool(pool.Prefab, pool.Count);
		    }
	    }

	    private void WarmPool(GameObject prefab, int size)
	    {
		    Warm(prefab, size);
	    }

	    public GameObject SpawnObject(GameObject prefab)
	    {
		    return Spawn(prefab);
	    }

	    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
	    {
		    return Spawn(prefab, position, rotation);
	    }

	    public void ReleaseObject(GameObject clone)
	    {
		    Release(clone);
	    }
    }

    public sealed class ObjectPool<T>
    {
	    private readonly List<ObjectPoolContainer<T>> _list;
	    private readonly Dictionary<T, ObjectPoolContainer<T>> _lookup;
	    private readonly Func<T> _factoryFunc;
	    
	    private int _lastIndex = 0;

	    public int Count => _list.Count;
	    public int CountUsedItems => _lookup.Count;

	    public ObjectPool(Func<T> factoryFunc, int initialSize)
	    {
		    _factoryFunc = factoryFunc;

		    _list = new List<ObjectPoolContainer<T>>(initialSize);
		    _lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

		    Warm(initialSize);
	    }

	    private void Warm(int capacity)
	    {
		    for (int i = 0; i < capacity; i++)
		    {
			    CreateContainer();
		    }
	    }

	    private ObjectPoolContainer<T> CreateContainer()
	    {
		    ObjectPoolContainer<T> container = new ObjectPoolContainer<T>
		    {
			    Item = _factoryFunc()
		    };
		    
		    _list.Add(container);
		    
		    return container;
	    }

	    public T GetItem()
	    {
		    ObjectPoolContainer<T> container = null;
		    
		    for (int i = 0; i < _list.Count; i++)
		    {
			    _lastIndex++;

			    if (_lastIndex > _list.Count - 1)
			    {
				    _lastIndex = 0;
			    }

			    if (_list[_lastIndex].Used)
			    {
				    continue;
			    }
			    else
			    {
				    container = _list[_lastIndex];
				    break;
			    }
		    }

		    if (container == null)
		    {
			    container = CreateContainer();
		    }

		    container.Consume();
		    
		    _lookup.Add(container.Item, container);
		    
		    return container.Item;
	    }

	    public void ReleaseItem(T item)
	    {
		    if (_lookup.ContainsKey(item))
		    {
			    ObjectPoolContainer<T> container = _lookup[item];
			    container.Release();
			    _lookup.Remove(item);
		    }
		    else
		    {
			    CustomDebug.LogWarning($"This object pool does not contain the item provided: {item}");
		    }
	    }
    }
    
    public sealed class ObjectPoolContainer<T>
    {
	    public bool Used { get; private set; }
	    public T Item { get; set; }

	    public void Consume()
	    {
		    Used = true;
	    }

	    public void Release()
	    {
		    Used = false;
	    }
    }

    [System.Serializable]
    public struct PoolItem
    {
	    public GameObject Prefab;
	    public int Count;
    }
}