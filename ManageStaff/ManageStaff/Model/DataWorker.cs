using System;
using System.Collections.Generic;
using System.Linq;
using ManageStaff.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageStaff.Model
{
    public static class DataWorker
    {
        private static readonly List<string> CitiesList = new() {
            "Волгоград", "Москва", "Санкт-Петербург", "Казань", "Екатеринбург"
        };
        private static readonly List<string> EmployeesList = new() {
            "Печорин", "Гуков", "Попов", "Вершина", "Носков", "Руденко", "Шпалина",
            "Алексеев", "Соловьев", "Федотова", "Углов", "Мезенцев", "Трунова", "Ермолаев"
        };
        private static readonly List<string> WorkshopsList = new() {
            "В1Строй", "В2Строй", "М1Строй", "М2Строй", "СП1Строй", "К1Строй", "Е1Строй"
        };

        private static List<Employee> _allEmployeesList = GetAllEmployees();
        private static List<Workshop> _allWorkshopsList = GetAllWorkshops();
        private static List<City> _allCitiesList = GetAllCities();

        public static byte TimeBrigade()
        {
            //оперделение номера бригады по текущему времени 
            TimeSpan time1 = new TimeSpan(8,0,0);
            TimeSpan time2 = new TimeSpan(20, 0, 0);
            TimeSpan timeNow = DateTime.Now.TimeOfDay;
            if (timeNow >= time1 && timeNow <= time2)
                return 0;
            return 1;
        }

        #region PROPERTIES_TO_GET_LISTS_DATA
        public static List<City> GetAllCities()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Cities.Include("Workshops").ToList();
                return result;
            }
        }
        public static List<Workshop> GetAllWorkshops()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Workshops.Include("Employees").Include("City").ToList();
                return result;
            }
        }
        public static List<Workshop> GetAllWorkshops(City city)
        {
            return _allWorkshopsList.Where(w => w.CityId == city.Id).ToList();
        }

        public static List<Employee> GetAllEmployees(Workshop workshop)
        {
            return _allEmployeesList.Where(empl => empl.WorkshopId == workshop.Id).ToList();
        }
        public static List<Employee> GetAllEmployees()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Employees.Include("Workshop").ToList();
                return result;
            }
        }

        #endregion

        #region METHODS_CREATE

        private static string CreateCity(string name)
        {
            string result = "Такой город уже существует!";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Cities.Any(el => el.Name == name);
                if (!checkIsExist)
                {
                    City newCity = new City { Name = name};
                    db.Cities.Add(newCity);
                    db.SaveChanges();
                    result = "Готово!";
                }
            }
            return result;
        }
        private static string CreateWorkshop(string name, City city)
        {
            string result = "Такой цех уже существует!";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Workshops.Any(el => el.Name == name && el.City == city);
                if (!checkIsExist)
                {
                    Workshop newWorkshop = new Workshop
                    {
                        Name = name,
                        CityId = city.Id
                    };
                    db.Workshops.Add(newWorkshop);
                    db.SaveChanges();
                    result = "Готово!";
                }
            }
            return result;
        }
        private static string CreateEmployee(string name, Workshop workshop)
        {
            string result = "Такой сотрудник уже существует!";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Employees.Any(el => el.Name == name && el.Workshop == workshop);
                if (!checkIsExist)
                {
                    Employee newEmployee = new Employee
                    {
                        Name = name,
                        WorkshopId = workshop.Id
                    };
                    db.Employees.Add(newEmployee);
                    db.SaveChanges();
                    result = "Готово!";
                }
            }
            return result;
        }

        public static bool CreateDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //проверка на существование заполненной БД
                if (db.Cities.Count() != 0 )
                    if(db.Workshops.Count() != 0)
                        if (db.Employees.Count() != 0)
                            return true;
            }

            int countEmployeesOnWorkshop = EmployeesList.Count / WorkshopsList.Count;
            foreach (var city in CitiesList)
                Console.WriteLine(CreateCity(city));

            List<City> allCities = GetAllCities();
            foreach (var city in allCities)
                foreach (var workshop in WorkshopsList)
                    if(workshop[0] == city.Name[0])
                        Console.WriteLine(CreateWorkshop(workshop, city));

            List<Workshop> allWorkshops = GetAllWorkshops();
            int i = 0;
            foreach (var workshop in allWorkshops)
            {
                for (int j = 0; j < countEmployeesOnWorkshop; j++)
                    Console.WriteLine(CreateEmployee(EmployeesList[i+j], workshop));
                i += countEmployeesOnWorkshop;
            }
            while (i < EmployeesList.Count)
            {
                Console.WriteLine(CreateEmployee(EmployeesList[i], allWorkshops[^1]));
                i++;
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                db.SaveChanges();
            }
            _allEmployeesList = GetAllEmployees();
            _allWorkshopsList = GetAllWorkshops();
            _allCitiesList = GetAllCities();
            return true;
        }

        #endregion

    }
}
