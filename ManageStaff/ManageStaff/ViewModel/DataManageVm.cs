using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ManageStaff.Model;
using ManageStaff.View;

namespace ManageStaff.ViewModel
{
    public class DataManageVm: NotifyPropertyChangedBase
    {
        private ObservableCollection<City> _allCities;
        private ObservableCollection<Workshop> _allWorkshops;
        private ObservableCollection<Employee> _allEmployees;
        private readonly List<string> _allBrigades = new() { "Первая(с 8:00 до 20:00)", "Вторая(с 20:00 до 8:00)" };
        private string _brigade;
        private string _shift;
        public DataManageVm()
        {
            CreateDb();
            _allCities = new ObservableCollection<City> (DataWorker.GetAllCities());
            _allWorkshops = new ObservableCollection<Workshop> (DataWorker.GetAllWorkshops());
            _allEmployees = new ObservableCollection<Employee> (DataWorker.GetAllEmployees());
            SelectedCity = AllCities[0];
            Brigade = _allBrigades[DataWorker.TimeBrigade()];
            Shift = "1";
        }

        #region PROPERTIES_FOR_VIEWS
        public ObservableCollection<City> AllCities
        {
            get => _allCities;
            set => SetField(ref this._allCities, value);
        }
        public ObservableCollection<Workshop> AllWorkshops
        {
            get => _allWorkshops;
            set => SetField(ref this._allWorkshops, value);
        }

        public ObservableCollection<Employee> AllEmployees
        {
            get => _allEmployees;
            set => SetField(ref this._allEmployees, value);
        }
        public string Brigade
        {
            get => _brigade;
            set => SetField(ref this._brigade, value);
        }

        public string Shift
        {
            get => _shift;
            set => SetField(ref this._shift, value);
        }

        private City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                AllWorkshops = new ObservableCollection<Workshop>(DataWorker.GetAllWorkshops(value));
                SelectedWorkshop = AllWorkshops[0];
                SetField(ref _selectedCity, value);
            }
        }
        private Workshop _selectedWorkshop;
        public Workshop SelectedWorkshop
        {
            get => _selectedWorkshop;
            set
            {
                if (value != null)
                {
                    AllEmployees = new ObservableCollection<Employee>(DataWorker.GetAllEmployees(value));
                    SelectedEmployee = AllEmployees[0];
                }
                else SelectedEmployee = null;
                SetField(ref _selectedWorkshop, value);
            }
        }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetField(ref _selectedEmployee, value);
        }


        #endregion

        public void CreateDb()
        {
            DataWorker.CreateDataBase();
        }

        #region COMMANDS_CLICK_BUTTON
        public RelayCommand ClickSaveButton
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    FileJSON infoJson = new FileJSON(SelectedCity.Name, SelectedWorkshop.Name, SelectedEmployee.Name,
                        Brigade,Shift == "" ? "1": Shift);
                    ShowMessageToUser(infoJson.Save(infoJson) ? "Файл сохранен!" : "Ошибка при сохранении файла.");
                });
            }
        }

        #endregion

        #region METHODS_TO_SHOW_MESSAGE
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void ShowMessageToUser(string message)
        {
            MessageView messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }

        #endregion

    }
}
