﻿// Copyright (c) 2013-2016 CoreTweet Development Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;

namespace CoreTweet.Core
{
    /// <summary>
    /// Represents a response that has rate limit.
    /// </summary>
    public interface ITwitterResponse
    {
        /// <summary>
        /// Gets or sets the rate limit of the response.
        /// </summary>
        RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the JSON of the response.
        /// </summary>
        string Json { get; set; }
    }

    /// <summary>
    /// The collection of response.
    /// </summary>
    public class ListedResponse<T> : ITwitterResponse, IEnumerable<T>
#if !(NET35 || NET40)
    , IReadOnlyList<T>
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTweet.Core.ListedResponse&lt;T&gt;"/> class with a specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="ListedResponse&lt;T&gt;"/>.</param>
        public ListedResponse(List<T> collection)
        {
            this.innerList = collection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTweet.Core.ListedResponse&lt;T&gt;"/> class with a specified collection and rate limit.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="ListedResponse&lt;T&gt;"/>.</param>
        /// <param name="rateLimit">The rate limit.</param>
        /// <param name="json">The JSON.</param>
        public ListedResponse(List<T> collection, RateLimit rateLimit, string json)
            : this(collection)
        {
            this.RateLimit = rateLimit;
            this.Json = json;
        }

        private readonly List<T> innerList;

        /// <summary>
        /// Gets or sets the rate limit of the response.
        /// </summary>
        public RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the JSON of the response.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="ListedResponse&lt;T&gt;"/>.
        /// </summary>
        public int Count => this.innerList.Count;

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] => this.innerList[index];

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => this.innerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    /// <summary>
    /// The collection of response.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class DictionaryResponse<TKey, TValue> : ITwitterResponse, IEnumerable<KeyValuePair<TKey, TValue>>
#if !(NET35 || NET40)
    , IReadOnlyDictionary<TKey, TValue>
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTweet.Core.DictionaryResponse&lt;TKey, TValue&gt;"/> class with a specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary whose elements are copied to the new <see cref="CoreTweet.Core.DictionaryResponse&lt;TKey, TValue&gt;"/>.</param>
        public DictionaryResponse(Dictionary<TKey, TValue> dictionary)
        {
            this.innerDictionary = dictionary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTweet.Core.DictionaryResponse&lt;TKey, TValue&gt;"/> class with a specified dictionary and rate limit.
        /// </summary>
        /// <param name="dictionary">The dictionary whose elements are copied to the new <see cref="CoreTweet.Core.DictionaryResponse&lt;TKey, TValue&gt;"/>.</param>
        /// <param name="rateLimit">The rate limit.</param>
        /// <param name="json">The JSON.</param>
        public DictionaryResponse(Dictionary<TKey, TValue> dictionary, RateLimit rateLimit, string json)
            : this(dictionary)
        {
            this.RateLimit = rateLimit;
            this.Json = json;
        }

        private readonly Dictionary<TKey, TValue> innerDictionary;

        /// <summary>
        /// Gets or sets the rate limit of the response.
        /// </summary>
        public RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the JSON of the response.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if the read-only dictionary contains an element that has the specified key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(TKey key) => this.innerDictionary.ContainsKey(key);

        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary.
        /// </summary>
        /// <value>An enumerable collection that contains the keys in the read-only dictionary.</value>
        public IEnumerable<TKey> Keys => this.innerDictionary.Keys;

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns><c>true</c> if the <see cref="CoreTweet.Core.DictionaryResponse&lt;TKey, TValue&gt;"/> contains an element that has the specified key; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(TKey key, out TValue value) => this.innerDictionary.TryGetValue(key, out value);

        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <value>An enumerable collection that contains the values in the read-only dictionary.</value>
        public IEnumerable<TValue> Values => this.innerDictionary.Values;

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        public TValue this[TKey key] => this.innerDictionary[key];

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>The number of elements in the collection.</value>
        public int Count => this.innerDictionary.Count;

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.innerDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
