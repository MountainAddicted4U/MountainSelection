namespace MountainAddicted.Library.Elevation
{
	internal class QueryStringBuilder
	{
		System.Text.StringBuilder StringBuilder { get { return this._sb; } }
		System.Text.StringBuilder _sb = new System.Text.StringBuilder();

		public override string ToString()
		{
			return _sb.ToString();
		}

		/// <summary>
		/// Appends a key/value pair when the value isn't null.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public QueryStringBuilder Append(string key, string value)
		{
			if(string.IsNullOrEmpty(value) == false)
			{
				if(_sb.Length > 0) _sb.Append("&");
				_sb.Append(key).Append("=").Append(value);
			}
			return this;
		}
		//public QueryStringBuilder Append(string key, System.Nullable<T> value)
		//{
		//    if (value != null)
		//    {
		//        if (_sb.Length > 0) _sb.Append("&");
		//        _sb.Append(key).Append("=").Append(value);
		//    }
		//    return this;
		//}

		/// <summary>
		/// Appends a value when the string isn't null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public QueryStringBuilder Append(string value)
		{
			if(string.IsNullOrEmpty(value) == false)
			{
				if(_sb.Length > 0) _sb.Append("&");
				_sb.Append(value);
			}
			return this;
		}
	}
}
