using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuoteApp.Annotations;
using SQLite;

namespace QuoteApp.Backend.Model
{
    public class Autor: INotifyPropertyChanged
    {
        private int _numberOfQuotes;
        private int _numberOfReadQuotes;

        [PrimaryKey]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        public int NumberOfQuotes
        {
            get => _numberOfQuotes;
            set
            {
                if (value == _numberOfQuotes) return;
                _numberOfQuotes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasBeenFullyRead));
            }
        }

        public int NumberOfReadQuotes
        {
            get => _numberOfReadQuotes;
            set
            {
                if (value == _numberOfReadQuotes) return;
                _numberOfReadQuotes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasBeenFullyRead));
            }
        }

        public bool HasBeenFullyRead => NumberOfQuotes <= NumberOfReadQuotes;

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}