using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedOak.Collections;
using TwistedOak.Util;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

[TestClass]
public class PerishableUtilitiesTest {
    [TestMethod]
    public void ToPerishableCollection() {
        var source = new LifetimeSource();
        var p1 = new Perishable<int>(1, source.Lifetime);
        var p2 = new Perishable<int>(1, Lifetime.Immortal);
        var p = new PerishableCollection<int>();
        var q = p.CurrentAndFutureItems().ToPerishableCollection();
        q.CurrentItems().AssertSequenceEquals();

        p.Add(p1);
        q.CurrentItems().AssertSequenceEquals(p1);

        p.Add(p2.Value, p2.Lifetime);
        q.CurrentItems().AssertSequenceEquals(p1, p2);

        source.EndLifetime();
        q.CurrentItems().AssertSequenceEquals(p2);
    }
    [TestMethod]
    public void ToPerishableCollectionWithLifetime() {
        var source = new LifetimeSource();
        var collectionSource = new LifetimeSource();
        var p1 = new Perishable<int>(1, source.Lifetime);
        var p2 = new Perishable<int>(1, Lifetime.Immortal);
        var p = new PerishableCollection<int>();
        var q = p.CurrentAndFutureItems().ToPerishableCollection(collectionSource.Lifetime);
        q.CurrentItems().AssertSequenceEquals();

        p.Add(p1);
        q.CurrentItems().AssertSequenceEquals(p1);

        collectionSource.EndLifetime();

        p.Add(p2.Value, p2.Lifetime);
        q.CurrentItems().AssertSequenceEquals(p1);

        source.EndLifetime();

        source.EndLifetime();
        q.CurrentItems().AssertSequenceEquals();
    }
    [TestMethod]
    public void PerishableObservableSelect() {
        new[] { new Perishable<int>(1, Lifetime.Immortal) }
            .ToObservable()
            .LiftSelect(e => e + 1)
            .ToList()
            .ToTask()
            .AssertRanToCompletion()
            .AssertSequenceEquals(new Perishable<int>(2, Lifetime.Immortal));
    }
    [TestMethod]
    public void PerishableObservableWhere() {
        new[] { new Perishable<int>(1, Lifetime.Immortal), new Perishable<int>(2, Lifetime.Immortal) }
            .ToObservable()
            .LiftWhere(e => e != 1)
            .ToList()
            .ToTask()
            .AssertRanToCompletion()
            .AssertSequenceEquals(new Perishable<int>(2, Lifetime.Immortal));
    }
    [TestMethod]
    public void PerishableEnumerableSelect() {
        new[] { new Perishable<int>(1, Lifetime.Immortal) }.LiftSelect(e => e + 1).AssertSequenceEquals(new Perishable<int>(2, Lifetime.Immortal));
    }
    [TestMethod]
    public void PerishableEnumerableWhere() {
        new[] {
            new Perishable<int>(1, Lifetime.Immortal),
            new Perishable<int>(2, Lifetime.Immortal)
        }.LiftWhere(e => e != 1).AssertSequenceEquals(new Perishable<int>(2, Lifetime.Immortal));
    }
    [TestMethod]
    public void ObserveNonPerishedCount() {
        var li1 = new List<int>();
        new[] { new Perishable<int>(1, Lifetime.Immortal), new Perishable<int>(2, Lifetime.Immortal) }
            .ToObservable()
            .ObserveNonPerishedCount(completeWhenSourceCompletes: true)
            .Subscribe(li1.Add, () => li1.Add(-1));
        li1.AssertSequenceEquals(0, 1, 2, -1);

        var source = new LifetimeSource();
        var li2 = new List<int>();
        new[] { new Perishable<int>(1, source.Lifetime), new Perishable<int>(2, source.Lifetime) }
            .ToObservable()
            .ObserveNonPerishedCount(completeWhenSourceCompletes: false)
            .Subscribe(li2.Add, () => li2.Add(-1));
        li2.AssertSequenceEquals(0, 1, 2);
        source.EndLifetime();
        li2.AssertSequenceEquals(0, 1, 2, 1, 0, -1);
    }
}
