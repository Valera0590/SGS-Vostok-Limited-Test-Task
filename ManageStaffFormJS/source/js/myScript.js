
let CitiesList = ["Волгоград", "Москва", "Санкт-Петербург", "Казань", "Екатеринбург"];
let EmployeesList = ["Печорин", "Гуков", "Попов", "Вершина", "Носков", "Руденко", "Шпалина",
    "Алексеев", "Соловьев", "Федотова", "Углов", "Мезенцев", "Трунова", "Ермолаев"];
let WorkshopsList = ["В1Строй", "В2Строй", "М1Строй", "М2Строй", "СП1Строй", "К1Строй", "Е1Строй"];
let Brigades = ["Первая(с 8:00 до 20:00)", "Вторая(с 20:00 до 8:00)"];
let CountEmployeesOnWorkshop = (EmployeesList.length / WorkshopsList.length).toFixed(0);

class City
{
    _id;
    _name;
    _workshops;
    constructor(id, cityName)
    {
        this._id = id;
        this._name = cityName;
        this._workshops = [];
    }
    get cityId()
    {
        return this._id;
    }
    get name()
    {
        return this._name;
    }
    set workshops(value)
    {
        this._workshops = value;
    }
    get workshops()
    {
        return this._workshops;
    }
}
class Workshop
{
    _id;
    _name;
    _employees;
    _cityId;
    constructor(id, workshopName, cityId)
    {
        this._id = id;
        this._name = workshopName;
        this._employees = [];
        this._cityId = cityId;
    }
    get workshopId()
    {
        return this._id;
    }
    get name()
    {
        return this._name;
    }
    set employees(value)
    {
        this._employees = value;
    }
    get employees()
    {
        return this._employees;
    }
    get cityId()
    {
        return this._cityId;
    }
}
class Employee
{
    _id;
    _name;
    _workshopId;
    constructor(id, emplooyName, workshopId)
    {
        this._id = id;
        this._name = emplooyName;
        this._workshopId = workshopId;
    }
    get name()
    {
        return this._emplooyName;
    }
    get workshopId()
    {
        return this._workshopId;
    }
}
/*-----------------------вспомагательные функции---------------------- */
function getArray(arr){
    return Array.from(arr, el => el._name);
}
function clearForm(form){
    let select = document.getElementById(form); 
    while (select.firstChild) {
        select.removeChild(select.firstChild);
    }
}
function getBrigadeTime(){
    let data = new Date();
    let time1 = 8*3600000;
    let time2 = 20*3600000;
    let timeNow = data.getTime();
    if (timeNow >= time1 && timeNow <= time2)
        return 0;
    return 1;
}
/*--------------------------создание "БД"----------------------------- */
var Cities = [];
function initialization(){
    for(var i = 0, c = 0, w = 0; i < EmployeesList.length; c++)
    {
        Cities[c] = new City(c, CitiesList[c]);
        let Workshops = [];
        while( w < WorkshopsList.length && CitiesList[c][0] == WorkshopsList[w][0] )
        {
            Workshops[w] = new Workshop(w, WorkshopsList[w], Cities[c].cityId);
            let Employees = [];
            for(var j = 0; j < CountEmployeesOnWorkshop; j++, i++)
                Employees[i] = new Employee(i, EmployeesList[i], Workshops[w].workshopId);
            
            let emp = new Array();
            Employees.forEach(element => {
                emp.push(element); 
            });
            Workshops[w].employees = emp;
            w++;
        }
        let city = new Array();
            Workshops.forEach(element => {
                city.push(element); 
            });
        Cities[c].workshops = city;
    }
    let selectBrigade = document.getElementById("info_brigade"); 
    selectBrigade.value = Brigades[getBrigadeTime()];
}
/*------------------------обработчики событий-------------------------*/
function ready() {
    initialization();
    fillLists(-1,0,0);
}
function cityChanged(){
    console.log("City has changed");
    clearForm("selectedNumberWorkshop");
    clearForm("selectedNumberEmployee"); 
    let select = document.getElementById("selectedNumberCity");
    fillLists(select.selectedIndex,0,0);
}
function workshopChanged(){
    console.log("Workshop has changed");
    clearForm("selectedNumberEmployee");
    let selectC = document.getElementById("selectedNumberCity");
    let selectW = document.getElementById("selectedNumberWorkshop");
    fillLists(selectC.selectedIndex,selectW.selectedIndex,0);
}
/*------------------------функция заполнения списков--------------------------*/
function fillLists(indCity, indWshop, indEmpl){
    let selectCity = document.getElementById("selectedNumberCity"); 
    let allCities = getArray(Cities); 
    if (indCity == -1)
    {
        for(let i = 0; i < allCities.length; i++) {
            let opt = allCities[i];
            let el = document.createElement("option");
            el.textContent = opt;
            el.value = opt;
            if(i == 0) el.selected = opt;
            selectCity.appendChild(el);
        }
    }

    let selectWorkshop = document.getElementById("selectedNumberWorkshop");
    let wshops = Cities[allCities.findIndex(i => i == selectCity.options[selectCity.selectedIndex].text)]._workshops;
    let workshopsInCity = getArray(wshops); 
    if(indWshop == 0)
    {
        for(let i = 0; i < workshopsInCity.length; i++) {
            let opt = workshopsInCity[i];
            let el = document.createElement("option");
            el.textContent = opt;
            el.value = opt;
            if(i == indWshop) el.selected = opt;
            selectWorkshop.appendChild(el);
        }
    }
    
    let selectEmployee = document.getElementById("selectedNumberEmployee"); 
    let empls = wshops[workshopsInCity.findIndex(i => i == selectWorkshop.options[selectWorkshop.selectedIndex].text)]._employees;
    let employeesOnWorkshop = getArray(empls); 
    for(let i = 0; i < employeesOnWorkshop.length; i++) {
        let opt = employeesOnWorkshop[i];
        let el = document.createElement("option");
        el.textContent = opt;
        el.value = opt;
        if(i == indEmpl) el.selected = opt;
        selectEmployee.appendChild(el);
    }
    let selectBrigade = document.getElementById("info_brigade"); 
    selectBrigade.value = Brigades[getBrigadeTime()];
}


//связывание событий с их обработчиками
document.addEventListener("DOMContentLoaded", ready);
selectedNumberCity.onchange = () => { cityChanged()};
selectedNumberWorkshop.onchange = () => { workshopChanged()};
