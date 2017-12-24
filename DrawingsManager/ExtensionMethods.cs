using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DrawingsManager
{
	public static class ExtensionMethods
	{
		public static List<T> ToAList<T>(this IEnumerator enumerator)
		{
			var list = new List<T>();
			while (enumerator.MoveNext())
			{
				var loop = (T)enumerator.Current;
				if (loop != null)
					list.Add(loop);
			}
			return list;
		}

		public static string ToJson(this object obj, bool formatting = true)
		{
			return
				formatting
					? JsonConvert.SerializeObject(obj, Formatting.Indented)
					: JsonConvert.SerializeObject(obj, Formatting.None);
		}

		public static T FromJson<T>(this string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}