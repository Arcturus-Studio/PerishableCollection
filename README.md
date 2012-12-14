PerishableCollection
====================

When an item is added to a PerishableCollection, it is paired with a lifetime. When that lifetime ends, the item is removed from the collection. Both of these operations are constant time. Users can observe items (paired with a lifetime) added to the collection, and know the item has been removed when the lifetime ends.

The main advantage of using lifetimes instead of a remove method is that obsevations of removal are unambiguous. For example, if the collection happens to be over integers modulo 5, a user doesn't need to know to match a 'removed 6' event with an 'added 1' event. The underlying collection worries about it, and the observer gets that information implicitely via the lifetime paired with the 1.

You can filter and project a perishable collection's items, without losing the ability to determine when the item was removed. For example:

    // pair items to a unique id, automatically adding/removing them from a new collection
    var id = 0;
    IObservable<Perishable<T>> items = perishableCollection.AsObservable();
    IObservable<Perishable<Tuple<T, int>>> itemsWithIds = items.Select(e => Tuple.Create(e, Interlocked.Increment(ref id));
    PerishableCollection<Tuple<T, int>> perishableCollectionWithIds = IDed.ToPerishableCollection();
