using System;
using ChatBotClient.View.Cells;
using ChatBotClient.ViewModel;
using Xamarin.Forms;

namespace ChatBotClient.View
{
	public class MyDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
	{
		public MyDataTemplateSelector()
		{
			// Retain instances!
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingCell));
			this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingCell));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var messageVm = item as MessageViewModel;
			if (messageVm == null)
				return null;
			return messageVm.IsIncoming ? this.incomingDataTemplate : this.outgoingDataTemplate;
		}

		private readonly DataTemplate incomingDataTemplate;
		private readonly DataTemplate outgoingDataTemplate;
	}
}
