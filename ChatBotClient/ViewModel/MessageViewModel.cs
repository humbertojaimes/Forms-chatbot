using System;
namespace ChatBotClient.ViewModel
{
	public class MessageViewModel : BaseViewModel
	{
		private string text;

		public string Text
		{
			get { return text; }
			set { text = value; RaisePropertyChanged(); }
		}

		private DateTime messageDateTime;

		public DateTime MessagDateTime
		{
			get { return messageDateTime; }
			set { messageDateTime = value; RaisePropertyChanged(); }
		}

		private bool isIncoming;

		public bool IsIncoming
		{
			get { return isIncoming; }
			set { isIncoming = value; RaisePropertyChanged(); }
		}

		public bool HasAttachement => !string.IsNullOrEmpty(attachementUrl);

		private string attachementUrl;

		public string AttachementUrl
		{
			get { return attachementUrl; }
			set { attachementUrl = value; RaisePropertyChanged(); }
		}


	}
}
