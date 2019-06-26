namespace Action
{
	[System.Serializable]
	public class ParserException : System.Exception
	{
		/// <summary>
		/// パーサーエラーの発生した行を取得します。
		/// </summary>
		/// <value></value>
		public int LineNumber { get; }

		public ParserException(int number = 0) 
		{ 
			LineNumber = number;
		}

		public ParserException(string message, int number = 0) : base(message)
		{
			LineNumber = number;
		}

		public ParserException(string message, System.Exception inner, int number = 0) : base(message, inner)
		{ 
			LineNumber = number;
		}

		protected ParserException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}