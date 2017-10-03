using System.Linq;
using ChatBotClient.ViewModel;
using Xamarin.Forms;

namespace ChatBotClient
{
    public partial class ChatBotClientPage : ContentPage
    {
        public ChatBotClientPage()
        {
			InitializeComponent();
			Title = "Mi primer bot";
			BindingContext = new MainPageViewModel();
			(BindingContext as MainPageViewModel).Messages.CollectionChanged += Messages_CollectionChanged;
			if ((BindingContext as MainPageViewModel).Messages.Any())
				MessagesListView.ScrollTo((BindingContext as MainPageViewModel)?.Messages?.Last(), ScrollToPosition.MakeVisible, true);
            
		}

		void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			MessagesListView.ScrollTo((BindingContext as MainPageViewModel).Messages.Last(), ScrollToPosition.MakeVisible, true);

		}


		private void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			MessagesListView.SelectedItem = null;
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			MessageEntry.Unfocus();
		}

		private void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			MessagesListView.SelectedItem = null;

		}

		void Handle_Completed(object sender, System.EventArgs e)
		{
			(BindingContext as MainPageViewModel).SendCommand.Execute(null);
			//MessagesListView.ScrollTo((BindingContext as MainPageViewModel).Messages.Last(), ScrollToPosition.MakeVisible, true);

		}



		void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
		{
		}
    }
}
