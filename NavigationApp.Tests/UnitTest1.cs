using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NavigationApp.Infra.Navigation;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace NavigationApp.Tests
{
	internal class MockRegionBehaviorCollection : Dictionary<string, IRegionBehavior>, IRegionBehaviorCollection
	{
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	class MockViewsCollection : IViewsCollection
	{
		public ObservableCollection<object> Items = new ObservableCollection<object>();

		public void Add(object view)
		{
			this.Items.Add(view);
		}

		public bool Contains(object value)
		{
			return Items.Contains(value);
		}

		public IEnumerator<object> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add { Items.CollectionChanged += value; }
			remove { Items.CollectionChanged -= value; }
		}
	}
	class MockNavigationService : IRegionNavigationService
	{

		public IRegionNavigationJournal Journal
		{
			get { throw new NotImplementedException(); }
		}

		public event EventHandler<RegionNavigationEventArgs> Navigated;

		public event EventHandler<RegionNavigationEventArgs> Navigating;

		public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed;

		public IRegion Region
		{
			get;
			set;
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			throw new NotImplementedException();
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
		{
			throw new NotImplementedException();
		}
	}
	class MockPresentationRegion : IRegion
	{
		public MockViewsCollection MockViews = new MockViewsCollection();
		public MockViewsCollection MockActiveViews = new MockViewsCollection();

		public MockPresentationRegion()
		{
			Behaviors = new MockRegionBehaviorCollection();
		}
		public IRegionManager Add(object view)
		{
			MockViews.Items.Add(view);

			return null;
		}

		public void Remove(object view)
		{
			MockViews.Items.Remove(view);
			MockActiveViews.Items.Remove(view);
		}

		public void Activate(object view)
		{
			MockActiveViews.Items.Add(view);
		}

		public IRegionManager Add(object view, string viewName)
		{
			throw new NotImplementedException();
		}

		public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
		{
			throw new NotImplementedException();
		}

		public object GetView(string viewName)
		{
			throw new NotImplementedException();
		}

		public IRegionManager RegionManager { get; set; }

		public IRegionBehaviorCollection Behaviors { get; set; }

		public IViewsCollection Views
		{
			get { return MockViews; }
		}

		public IViewsCollection ActiveViews
		{
			get { return MockActiveViews; }
		}

		public void Deactivate(object view)
		{
			MockActiveViews.Items.Remove(view);
		}

		private object context;
		public object Context
		{
			get { return context; }
			set
			{
				context = value;
				OnPropertyChange("Context");
			}
		}

		public NavigationParameters NavigationParameters
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		private string name;
		public string Name
		{
			get { return this.name; }
			set
			{
				this.name = value;
				this.OnPropertyChange("Name");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChange(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool Navigate(Uri source)
		{
			throw new NotImplementedException();
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
		{
			var navigationResult = new NavigationResult(new NavigationContext(null, target), true);
		}

		public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
		{
			throw new NotImplementedException();
		}

		public IRegionNavigationService NavigationService
		{
			get;
			set;
		}


		public Comparison<object> SortComparison
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}

	[TestClass]
	public class UnitTest1
	{
		private class TestableRegionBehavior : WorkspaceRegionBehavior
		{
			public TestableRegionBehavior()
			{
				
			}
			public bool onAttachCalled;

			protected override void OnAttach()
			{
				onAttachCalled = true;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CannotChangeRegionAfterAttach()
		{
			TestableRegionBehavior regionBehavior = new TestableRegionBehavior();

			regionBehavior.Region = new MockPresentationRegion();

			regionBehavior.Attach();
			regionBehavior.Region = new MockPresentationRegion();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldFailWhenAttachedWithoutRegion()
		{
			TestableRegionBehavior regionBehavior = new TestableRegionBehavior();
			regionBehavior.Attach();
		}

		[TestMethod]
		public void ShouldCallOnAttachWhenAttachMethodIsInvoked()
		{
			TestableRegionBehavior regionBehavior = new TestableRegionBehavior();

			regionBehavior.Region = new MockPresentationRegion();

			regionBehavior.Attach();

			Assert.IsTrue(regionBehavior.onAttachCalled);
		}

		[TestMethod]
		public void Test1()
		{
			TestableRegionBehavior regionBehavior = new TestableRegionBehavior();

			regionBehavior.Region = new MockPresentationRegion();
			regionBehavior.Region.NavigationService = new MockNavigationService();

			regionBehavior.Attach();

			object view = new object();

			regionBehavior.Region.RequestNavigate(view.ToString());


			
		}

	}
}
