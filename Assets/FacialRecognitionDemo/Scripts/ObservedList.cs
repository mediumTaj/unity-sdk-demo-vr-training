

using IBM.Watson.DeveloperCloud.Utilities;
/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/
using System;
using System.Collections.Generic;

#pragma warning disable 0067

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	[Serializable]
	public class ObservedList<T> : List<T>
	{
        /// <summary>
        /// Changed delegate. This event does not pass any data.
        /// </summary>
		public event Action<int> Changed = delegate { };
        /// <summary>
        /// Updated delegate. This event does not pass any data.
        /// </summary>
		public event Action Updated = delegate { };

        /// <summary>
        /// Added callback delegate. This event passes an item when called.
        /// </summary>
        /// <param name="item"></param>
        public delegate void AddedCallback(T item);
        /// <summary>
        /// Added event.
        /// </summary>
        public event AddedCallback OnAdded;

        /// <summary>
        /// Removed callback delegate. This event passes an item when called.
        /// </summary>
        /// <param name="item"></param>
        public delegate void RemovedCallback(T item);
        /// <summary>
        /// Removed event.
        /// </summary>
        public event RemovedCallback OnRemoved;

        /// <summary>
        /// Add override. Fires callbacks when an item is added to the list.
        /// </summary>
        /// <param name="item">Item to add to the observed list.</param>
		public new void Add(T item)
		{
			base.Add(item);
			OnAdded(item);
			Updated();
		}

        /// <summary>
        /// Remove override. Fires callback when an item is removed from the list.
        /// </summary>
        /// <param name="item">Item to remove from the observed list.</param>
		public new void Remove(T item)
		{
			base.Remove(item);
			OnRemoved(item);
			Updated();
		}

        /// <summary>
        /// Add range override.
        /// </summary>
        /// <param name="collection">Collection to add to the observed list.</param>
		public new void AddRange(IEnumerable<T> collection)
		{
			base.AddRange(collection);
			Updated();
		}

        /// <summary>
        /// Remove rage override.
        /// </summary>
        /// <param name="index">Index of where to remove items from.</param>
        /// <param name="count">Count of how many items to remove.</param>
		public new void RemoveRange(int index, int count)
		{
			base.RemoveRange(index, count);
			Updated();
		}

        /// <summary>
        /// Clear override.
        /// </summary>
		public new void Clear()
		{
			base.Clear();
			Updated();
		}

        /// <summary>
        /// Insert override. Inserts an item to a particular index of the observed list.
        /// </summary>
        /// <param name="index">The index where the item should be placed.</param>
        /// <param name="item">The item to be placed.</param>
		public new void Insert(int index, T item)
		{
			base.Insert(index, item);
			Updated();
		}

        /// <summary>
        /// InsertRange override.
        /// </summary>
        /// <param name="index">Index where to insert the collection.</param>
        /// <param name="collection">The collection of items to insert into the observed list.</param>
		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			base.InsertRange(index, collection);
			Updated();
		}

        /// <summary>
        /// Remove all override.
        /// </summary>
        /// <param name="match">The items to remove.</param>
		public new void RemoveAll(Predicate<T> match)
		{
			base.RemoveAll(match);
			Updated();
		}

        /// <summary>
        /// The datatype of the item in the ObservedList.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public new T this[int index]
		{
			get { return base[index]; }
			set
			{
				base[index] = value;
				Changed(index);
			}
		}
	}
}
