using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;



//Property Changed Command wird erstellt um zu Prüfuen ob sich etwas bei der Ansicht verändert hat (bsp Suchtext muss neu reingeladen werden)
namespace TourPlaner_andreas.ViewModels {

    public abstract class ViewModelBase : INotifyPropertyChanged {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "") {
            ValidatePropertyName(propertyName);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void ValidatePropertyName(string propertyName) {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) {
                log.Error("Invalid propery name: " + propertyName);
                throw new ArgumentException("Invalid propery name: " + propertyName);
               
            }
        }
    }
}
