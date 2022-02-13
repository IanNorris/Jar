using System.Threading.Tasks;

namespace Jar
{
	public enum MessageIcon
	{
		Warning,
		Error,
		Success,
		Info
	}

	public class MessageBox
	{
		public delegate Task ExecuteJavascriptDelegate(string Code);
		public delegate void ShowMessageDelegate(string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode);

		const string ErrorMessageFormat = @"swal({{
				text: ""{0}"",
				title: ""{1}"",
				icon: ""{2}"",
				closeOnEsc: true,
				dangerMode: {3},
				buttons: {{
					cancel: {4},
					confirm: true
				}}
			}});";

		public void BindBrowser(ExecuteJavascriptDelegate callback)
		{
			_callback = callback;
		}

		private static string BuildMessageCommand(string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode)
		{
			return string.Format(ErrorMessageFormat, Text, Title, Icon.ToString().ToLower(), DangerMode.ToString().ToLower(), ShowCancel.ToString().ToLower());
		}

		public async Task ShowMessage(string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode)
		{
			string JS = BuildMessageCommand(Text, Title, Icon, ShowCancel, DangerMode);
			await _callback(JS);
		}

		private ExecuteJavascriptDelegate _callback;
	}
}
