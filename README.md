PerishableCollection
====================

When an item is added to a PerishableCollection, it is paired with a lifetime. When that lifetime ends, the item is removed from the collection. Both of these operations are constant time. Users can observe items (paired with a lifetime) added to the collection, and know the item has been removed when the lifetime ends.

The main advantage of using lifetimes instead of a remove method is that obsevations of removal are unambiguous. For example, if the collection happens to be over integers modulo 5, a user doesn't need to know to match a 'removed 6' event with an 'added 1' event. The underlying collection worries about it, and the observer gets that information implicitely via the lifetime paired with the 1.

You can filter and project a perishable collection's items, without losing the ability to determine when the item was removed. For example:

    // create a new perishable collection
	var collection = new PerishableCollection<string>();

    // add some items to the collection
    var mortalLife = new LifetimeSource();
    collection.Add("mortal", mortalLife.Lifetime); //"mortal" will be removed when mortalLife is ended
    collection.Add("forever", Lifetime.Immortal); //"forever" will never be removed
            
    // get an observable that can be used to track the collection's items
    var itemsObservable = collection.CurrentAndFutureItems(); // IObservable<Perishable<string>>
    
	// pair observed perishable items to a unique id
	var id = 0;
    var itemsWithIds = itemsObservable.LiftSelect(e => Tuple.Create(e, Interlocked.Increment(ref id))); // IObservable<Perishable<Tuple<string, int>>>
	
	// observe id'd items back into a separate new perishable collection
    var collectionWithIds = itemsWithIds.ToPerishableCollection(); //PerishableCollection<Tuple<string, int>>
    var peek1 = collectionWithIds.CurrentItems().ToArray(); // r1 = {("mortal", 1), ("forever", 2)}

    // modify the original collection, checking that changes propagate to the transformed collection
    collection.Add("late", Lifetime.Immortal); //
    mortalLife.EndLifetime();
    var peek2 = collectionWithIds.CurrentItems().ToArray(); // r2 = {("forever", 2), ("late", 3)}
