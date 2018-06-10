using Agenda.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Agenda
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsPage : ContentPage
	{
        private ObservableCollection<Contact> _contacts;
        private SQLiteAsyncConnection _connection;
        private bool _IsDataLoaded;

		public ContactsPage ()
		{
			InitializeComponent ();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            if (_IsDataLoaded) return;
            _IsDataLoaded = true;

            await LoadData();

            base.OnAppearing();
        }

        private async Task LoadData()
        {
            await _connection.CreateTableAsync<Contact>();
            var model = await _connection.Table<Contact>().ToListAsync();
            _contacts = new ObservableCollection<Contact>(model);
            contactsListView.ItemsSource = _contacts;
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var contact = (sender as MenuItem).CommandParameter as Contact;

            if (await DisplayAlert("Warning", $"Are you sure you want to delete {contact.FullName}?", "Yes", "No"))
            {
                await _connection.DeleteAsync(contact);
                _contacts.Remove(contact);
            }
        }

        private async void OnContactSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (contactsListView.SelectedItem == null) return;

            var selectedContact = e.SelectedItem as Contact;

            contactsListView.SelectedItem = null;

            var page = new ContactDetail(selectedContact);
            page.ContactUpdated += (source, contact) =>
            {
                selectedContact.Id = contact.Id;
                selectedContact.FirstName = contact.FirstName;
                selectedContact.LastName = contact.LastName;
                selectedContact.Phone = contact.Phone;
                selectedContact.Email = contact.Email;
                selectedContact.IsBlocked = contact.IsBlocked;
            };

            await Navigation.PushAsync(page);
            
        }

        private async void OnAddContact(object sender, EventArgs e)
        {
            var page = new ContactDetail(new Contact());
            page.ContactAdded += (source, contact) =>
            {
                _contacts.Add(contact);
            };

            await Navigation.PushAsync(page);
        }
    }
}