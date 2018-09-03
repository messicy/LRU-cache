using System;
using System.Collections.Generic;

public class LRUCache<T>
{
	public LRUCache(int size, Action<T> func = null)
	{
		cache = new List<DataBlock>(size);
		cacheSize = size;
        destoryFunc = func;
	}

	public class DataBlock
	{
		public string name;
		public T obj;
		public DataBlock(string name, T obj)
		{
			this.name = name;
			this.obj = obj;
		}
	}

	List<DataBlock> cache;
	int cacheSize;
    Action<T> destoryFunc;

	public bool TryGetObject(string name, out T obj)
	{
		if (null == name)
		{
			throw new ArgumentNullException ("name");
		}

		for(int i = 0; i < cache.Count; i++)
		{
			if(name == cache[i].name)
			{
				obj = cache[i].obj;
				DataBlock temp = cache[i];
				cache.RemoveAt(i);
				cache.Add(temp);
				return true;
			}
		}
		obj = default(T);
		return false;
	}

	public void AddObject(string name, T obj)
	{
		if (null == name)
		{
			throw new ArgumentNullException ("name");
		}

        if (null != cache.Find(b => b.name == name))
            return;

		if(cacheSize == cache.Count)
		{
            if (null != destoryFunc)
            {
                destoryFunc(cache[0].obj);
            }
			cache.RemoveAt(0);
		}

		DataBlock db = new DataBlock(name, obj);
		cache.Add(db);
	}

	public void Clear()
	{
        if (null != destoryFunc)
        {
            for (int i = 0; i < cache.Count; i++)
            {
                destoryFunc(cache[i].obj);
            }
        }

		cache.Clear();
	}
}
