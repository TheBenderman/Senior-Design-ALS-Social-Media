// The MIT License (MIT)
//
// CoreTweet - A .NET Twitter Library supporting Twitter API 1.1
// Copyright (c) 2013-2016 CoreTweet Development Team
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreTweet.Core;
using Newtonsoft.Json;

namespace CoreTweet
{
    /// <summary>
    /// Represents a place, which are specific, named locations with corresponding geo coordinates.
    /// </summary>
    /// <remarks>
    /// <para>They can be attached to Tweets by specifying a place_id when tweeting.</para>
    /// <para>Tweets associated with places are not necessarily issued from that location but could also potentially be about that location.</para>
    /// <para>Places can be searched for.</para>
    /// <para>Tweets can also be found by place_id.</para>
    /// </remarks>
    public class Place : CoreBase
    {
        /// <summary>
        /// <para>Gets or sets a hash of variant information about the place.</para>
        /// </summary>
        [JsonProperty("attributes")]
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets or sets a bounding box of coordinates which encloses this place.
        /// </summary>
        [JsonProperty("bounding_box")]
        public BoundingBox BoundingBox { get; set; }

        [JsonProperty("centroid")]
        public double[] Centroid { get; set; }

        /// <summary>
        /// Gets or sets the array of Places contained within this Place.
        /// </summary>
        [JsonProperty("contained_within")]
        public Place[] ContainedWithin { get; set; }

        /// <summary>
        /// Gets or sets the name of the country containing this place.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the shortened country code representing the country containing this place.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the full human-readable representation of the name of the place.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("geometry")]
        public Coordinates Geometry { get; set; }

        /// <summary>
        /// <para>Gets or sets the ID representing this place.</para>
        /// <para>Note that this is represented as a string, not an integer.</para>
        /// <para>In trends/available or trends/closest, ID is a Yahoo! Where On Earth ID.</para>
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the short human-readable representation of the name of the place.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of location represented by this place.
        /// </summary>
        [JsonProperty("place_type")]
        public string PlaceType { get; set; }

        [JsonProperty("polylines")]
        public string[] Polylines { get; set; }

        /// <summary>
        /// Gets or sets the URL representing the location of additional place metadata for this place.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("vendor_info")]
        public GeoVendorInfo VendorInfo { get; set; }

        /// <summary>
        /// Returns the ID of this instance.
        /// </summary>
        /// <returns>The ID of this instance.</returns>
        public override string ToString()
        {
            return this.Id;
        }
    }

    /// <summary>
    /// Represents a place with rate limit.
    /// </summary>
    /// <remarks> 
    /// <para>Places are specific, named locations with corresponding geo coordinates.</para>
    /// <para>They can be attached to Tweets by specifying a place_id when tweeting.</para>
    /// <para>Tweets associated with places are not necessarily issued from that location but could also potentially be about that location.</para>
    /// <para>Places can be searched for.</para>
    /// <para>Tweets can also be found by place_id.</para>
    /// </remarks>
    public class PlaceResponse : Place, ITwitterResponse
    {
        /// <summary>
        /// Gets or sets the rate limit of the response.
        /// </summary>
        public RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the JSON of the response.
        /// </summary>
        public string Json { get; set; }
    }

    /// <summary>
    /// <para>Represents a bounding box.</para>
    /// <para>This class can be converted to a JSON with <see cref="JsonConvert.SerializeObject(object)"/>.</para>
    /// </summary>
    [JsonObject]
    public class BoundingBox : CoreBase, IEnumerable<Coordinates>
    {
        /// <summary>
        /// <para>Gets or sets a series of longitude and latitude points, defining a box which will contain the Place entity this bounding box is related to.</para>
        /// <para>Each point is an array in the form of [longitude, latitude].</para>
        /// <para>Points are grouped into an array per bounding box.</para>
        /// <para>Bounding box arrays are wrapped in one additional array to be compatible with the polygon notation.</para>
        /// </summary>
        [JsonProperty("coordinates")]
        public double[][][] Coordinates { get; set; }

        IEnumerable<Coordinates> GetCoordinates()
        {
            return Coordinates[0].Select(x => new Coordinates(x[0], x[1]));
        }

        /// <summary>
        /// <para>Gets or sets the type of data encoded in the coordinates property.</para>
        /// <para>This will be "Polygon" for bounding boxes.</para>
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetCoordinates().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinates> GetEnumerator()
        {
            return GetCoordinates().GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public Coordinates this[int index]
        {
            get
            {
                return GetCoordinates().ElementAt(index);
            }
            set
            {
                Coordinates[0][index][0] = value.Longitude;
                Coordinates[0][index][1] = value.Latitude;
            }
        }
    }


    /// <summary>
    /// Represents the places near the given coordinates which are similar in name.
    /// </summary>
    [JsonObject]
    public class GeoResult : CoreBase, IEnumerable<Place>, ITwitterResponse
    {
        /// <summary>
        /// Gets or set the names of the places.
        /// </summary>
        [JsonProperty("places")]
        public Place[] Places { get; set; }

        /// <summary>
        /// Gets or sets the token needed to be able to create a new place.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the rate limit of the response.
        /// </summary>
        public RateLimit RateLimit { get; set; }

        /// <summary>
        /// Gets or sets the JSON of the response.
        /// </summary>
        public string Json { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => this.Places.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Place> GetEnumerator() => (this.Places as IEnumerable<Place>).GetEnumerator();
    }

    /// <summary>
    /// Represents a trending topics for a specific WOEID.
    /// </summary>
    [JsonObject]
    public class TrendsResult : CoreBase, IEnumerable<Trend>
    {
        /// <summary>
        /// Gets or sets the UTC datetime that the trends are valid as of.
        /// </summary>
        [JsonProperty("as_of")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset AsOf { get; set; }

        /// <summary>
        /// Gets or sets the UTC datetime that this result was created at.
        /// </summary>
        [JsonProperty("created_at")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Locations of trending topics.
        /// </summary>
        [JsonProperty("locations")]
        public Location[] Locations { get; set; }

        /// <summary>
        /// Gets or sets the queried trends.
        /// </summary>
        [JsonProperty("trends")]
        public Trend[] Trends { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => Trends.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Trend> GetEnumerator()
        {
            return (Trends as IEnumerable<Trend>).GetEnumerator();
        }
    }

    /// <summary>
    /// Represents a location.
    /// </summary>
    public class Location : CoreBase
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the WOEID of the location.
        /// </summary>
        [JsonProperty("woeid")]
        public long WoeId { get; set; }

        /// <summary>
        /// Returns the ID of this instance.
        /// </summary>
        /// <returns>The ID of this instance.</returns>
        public override string ToString()
        {
            return this.WoeId.ToString("D");
        }
    }

    /// <summary>
    /// Represents the trend.
    /// </summary>
    public class Trend : CoreBase
    {
        /// <summary>
        /// Gets or sets the name of this trend.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL to search this trend.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the query string for search.
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Get or sets the volume of tweets per trend for the last 24 hours.
        /// </summary>
        [JsonProperty("tweet_volume")]
        public int? TweetVolume { get; set; }

        //TODO: promoted_content, events
    }

    /// <summary>
    /// Represents a location.
    /// </summary>
    public class TrendLocation : Location
    {
        /// <summary>
        /// Gets or sets the country name of the location.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country code of the location.
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the WOEID of the parent location.
        /// </summary>
        [JsonProperty("parentid")]
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or sets the WOEID type of the location.
        /// </summary>
        [JsonProperty("placeType")]
        public PlaceType PlaceType { get; set; }

        /// <summary>
        /// Gets or sets the URL of Yahoo! GeoPlanet API.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    /// <summary>
    /// Represents a WOEID type of a location.
    /// </summary>
    public class PlaceType : CoreBase
    {
        /// <summary>
        /// Gets or sets the WOEID of the location.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the WOEID name of the location.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class GeoVendorInfo : CoreBase
    {
        [JsonProperty("yelp")]
        public VendorYelp Yelp { get; set; }
    }

    public class VendorYelp : CoreBase
    {
        [JsonProperty("business_id")]
        public string BusinessId { get; set; }

        [JsonProperty("mobile_url")]
        public string MobileUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}