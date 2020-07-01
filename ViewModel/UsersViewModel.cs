using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LibraryApp.Model; 

namespace LibraryApp.ViewModel
{
    class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Users> _usersInfo;
        public ObservableCollection<Users> UsersInfo
        {
            get => _usersInfo;
            set
            {
                _usersInfo = value;
                OnPropertyChanged("UsersInfo");
            }
        }

        private void OnPropertyChanged(string v)
        {
            UsersInfo.Clear();
            UsersInfo.Add(new Users
            {
                Email = "",
                Name = "",
                Phone = "",
                Surname = "",
                IdentityNo = 0,
                UserId = 0
            });
            DatabaseHelpers.Select<Users>("Select * from Users").ToList().OrderByDescending(x=> x.UserId).ToList().
                ForEach(x => UsersInfo.Add(new Users
                {
                    Email = x.Email,
                    Name = x.Name,
                    Phone = x.Phone,
                    IdentityNo = x.IdentityNo,
                    Surname = x.Surname,
                    UserId = x.UserId
                }));
          
        }

        private ICommand _saveCommand { get; set; }
        private ICommand _deleteCommand { get; set; }
        private bool _canExecute = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute
        {
            get => _canExecute;
            set
            {
                if (_canExecute.Equals(value))
                    return;
                _canExecute = value;
            }
        }
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => _saveCommand = value;
        }
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => _deleteCommand = value;
        }
        public UsersViewModel()
        {
            UsersInfo = new ObservableCollection<Model.Users>();
            //DatabaseHelpers.Select<Users>("Select * from Users").ToList().
            //    ForEach(x => UsersInfo.Add(new Users
            //    {
            //        Email = x.Email,
            //        Name = x.Name,
            //        Phone = x.Phone,
            //        Surname = x.Surname,
            //        UserId = x.UserId
            //    }));
            SaveCommand = new RelayCommand(SaveUpdate, param => this.CanExecute);
            DeleteCommand = new RelayCommand(Remove, param => this.CanExecute);
        }  
        private void SaveUpdate(object parameter)
        {
            if (!(parameter is Users usr)) return;
            if (usr.UserId == 0)
            {
                DatabaseHelpers.Insert(usr);
            }
            else
            {
                DatabaseHelpers.Update(usr);
            }

            OnPropertyChanged("UsersInfo");
            

        }
        private void Remove(object parameter)
        {
            if (!(parameter is Users usr)) return;
            if (usr.UserId == 0) return;

            DatabaseHelpers.Delete(usr);
            OnPropertyChanged("UsersInfo");
            
        } 
    }
}