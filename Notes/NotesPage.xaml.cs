using Notes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var notes = new List<Note>();

            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            foreach (var fileName in files)
            {
                notes.Add(new Note
                {
                    Filename = fileName,
                    Text = File.ReadAllText(fileName),
                    Date = File.GetCreationTime(fileName)
                });
            }

            listView_Notes.ItemsSource = notes
                .OrderBy(c => c.Date)
                .ToList();
        }

        async void OnNotesAdded_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteEntryPage
            {
                BindingContext = new Note()
            });
        }

        async void listView_Notes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NoteEntryPage
                {
                    BindingContext = e.SelectedItem as Note
                });
            }
        }
    }
}