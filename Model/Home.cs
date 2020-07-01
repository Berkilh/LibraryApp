using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Annotations;
using LibraryApp.ViewModel; 
using LibraryApp.View.UserControls;

namespace LibraryApp.Model
{
    class BookDelivery : INotifyPropertyChanged 
    {

        public string Name { get; set; }
        public int UserId { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public DateTime ReturnDate { get; set; } 
        public BookDelivery()
        { 
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
