using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

public class Overseer<TKey, TValue> : Dictionary<TKey, TValue>
{

    public Overseer([NotNull] IDictionary<TKey, TValue> dictionary) : base(dictionary)
    {
        if (dictionary.Count > 1) throw new System.Exception("SingleEntryMap cannot have more than one entry");
    }

    public Overseer([NotNull] IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(
        dictionary, comparer)
    {
        if (dictionary.Count > 1) throw new System.Exception("SingleEntryMap cannot have more than one entry");
    }

    public Overseer(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
    {
        if (collection.Count() > 1) throw new System.Exception("SingleEntryMap cannot have more than one entry");
    }

    public Overseer(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) :
        base(collection, comparer)
    {
        throw new NotImplementedException("SingleEntryMap cannot have more than one entry");
    }

    public Overseer(IEqualityComparer<TKey> comparer) : base(comparer)
    {
        throw new NotImplementedException("Constructor not implemented");
    }

    public Overseer(int capacity) : base(capacity)
    {
        if (capacity > 1) throw new System.Exception("SingleEntryMap cannot have more than one entry");
    }

    public Overseer(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
    {
        if (capacity > 1) throw new System.Exception("SingleEntryMap cannot have more than one entry");
    }

    protected Overseer(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public void Add(TKey k, TValue v)
    {
        if (this.Count == 1)
        {
            throw new System.Exception("SingleEntryMap already has an entry");
        }

        base.Add(k, v);
    }

    private TValue this[TKey key]
    {
        get { throw null; }
        set { }
    }
}