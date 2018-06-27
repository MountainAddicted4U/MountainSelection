using System;
using Newtonsoft.Json;

namespace MountainAddicted.Library.Elevation
{

	public class JsonEnumTypeConverter : JsonConverter
	{
		public static ServiceResponseStatus AsResponseStatus(string s)
		{
			var result = ServiceResponseStatus.Unknown;

			switch(s)
			{
				case "OK":
					result = ServiceResponseStatus.Ok;
					break;
				case "ZERO_RESULTS":
					result = ServiceResponseStatus.ZeroResults;
					break;
				case "OVER_QUERY_LIMIT":
					result = ServiceResponseStatus.OverQueryLimit;
					break;
				case "REQUEST_DENIED":
					result = ServiceResponseStatus.RequestDenied;
					break;
				case "INVALID_REQUEST":
					result = ServiceResponseStatus.InvalidRequest;
					break;
				case "MAX_WAYPOINTS_EXCEEDED":
					result = ServiceResponseStatus.MaxWaypointsExceeded;
					break;
				case "NOT_FOUND":
					result = ServiceResponseStatus.NotFound;
					break;
			}

			return result;
		}
        
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ServiceResponseStatus);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object result = null;

			if(objectType == typeof(ServiceResponseStatus))
				result = AsResponseStatus(reader.Value.ToString());

			return result;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new System.NotImplementedException();
		}
	}
}
