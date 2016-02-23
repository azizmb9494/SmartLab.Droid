using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartLab
{
	// Response from Smart Lab Website.
	public class Response
	{
		public List<Request> Requests = new List<Request>();	// List of help requests.
		public int InUse = 0;									// Number of computers in use.
		public int Help = 0;									// Number of computers needing help.
		public int Idle = 0; 									// Number of computers idle.
		public int Offline = 0;									// Number of computers offline.
	}
	public class FormResponse
	{
		[JsonProperty("Success")]
		public int Success { get; set; }
	}

	// Help Request
	public class Request
	{
		[JsonProperty("location")]
		public string Location { get; set; }			// Seat Location
		[JsonProperty("time")]
		public DateTime Created { get; set; }			// Timestamp
		[JsonIgnore()]
		public int Count { get; set; }					// Number of times help has been requested.

		/// <summary>
		/// Initializes a new instance of the <see cref="SmartLab.Request"/> class.
		/// </summary>
		public Request()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmartLab.Request"/> class.
		/// </summary>
		/// <param name="location">Location.</param>
		/// <param name="created">Created.</param>
		public Request(string location, DateTime created) 
		{
			this.Location = location;
			this.Created = created;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmartLab.Request"/> class.
		/// </summary>
		/// <param name="location">Location.</param>
		/// <param name="times">Times.</param>
		public Request(string location, int times)
		{
			this.Location = location;
			this.Count = times;
		}

		/// <summary>
		/// Determines if request is from Business Calculus pod.
		/// </summary>
		/// <returns><c>true</c> if Biz Calc otherwise, <c>false</c>.</returns>
		public bool IsBizCalc()
		{
			// Array of Biz Calc Pods
			int[] BCPods = new int[] { 35, 39, 43, 36, 40, 44, 47, 48 };
			//int[] BCPods = new int[] { 36, 40, 44, 47, 48 };

			int pod = Int32.Parse(this.Location.Substring (0, this.Location.Length - 1));
			foreach (var p in BCPods) {
				if (pod == p) {
					return true;
				}
			}
			return false;
		}
	}

	// Calendar of Events
	public class Calendar {
		[JsonProperty("results")]
		public List<Event> List = new List<Event>();
	}

	// Calendar Event 
	public class Event
	{
		[JsonProperty("date")]
		public DateTime Date { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
	}

	public class RequestComparer : IEqualityComparer<Request>
	{

		public bool Equals(Request r1, Request r2)
		{
			return r1.Location == r2.Location && r1.Created == r2.Created;
		}

		public int GetHashCode(Request r)
		{
			string s = r.Location.Substring (r.Location.Length - 1, 1);
			int table = (Int32.Parse(r.Location.Substring (0, r.Location.Length - 1)))*10;
			if (s == "A") {
				return table + 1;
			}

			if (s == "B") {
				return table + 2;
			}

			if (s == "C") {
				return table + 3;
			}

			if (s == "D") {
				return table + 4;
			}

			if (s == "E") {
				return table + 5;
			}

			if (s == "F") {
				return table + 6;
			}

			return table;
		}
	}
}

