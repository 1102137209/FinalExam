﻿using FinalExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FinalExam.Controllers
{
    public class FinalExamController : Controller
    {
        DB db = new DB();
        // GET: FinalExam
        public ActionResult Index()
        {
            List<String> position = db.Employees.Select(x => x.Title).ToList();
            ViewBag.position = position;

            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection search)
        {
            String EmpID = search["EmpID"];
            String EmpName = search["EmpName"];
            String Position = search["Position"];
            String AppliDate = search["AppliDate"];
            String Gender = search["Gender"];
            String Age = search["Age"];


            return RedirectToAction("search", new { EmpID, EmpName, Position, AppliDate, Gender ,Age});
        }
        public ActionResult search(String EmpID, String EmpName, String Position, String AppliDate,String Gender,String Age)
        {
           
            DateTime appliDate = new DateTime();
            appliDate = Convert.ToDateTime(AppliDate);
           





            List<Employees> ID = db.Employees.Where(x =>
                (String.IsNullOrEmpty(EmpID) ? true : x.EmployeeID.ToString() == EmpID) &&
                (String.IsNullOrEmpty(EmpName) ? true : x.FirstName == EmpName) &&
                (String.IsNullOrEmpty(Position) ? true : x.Title == Position) &&
                (String.IsNullOrEmpty(AppliDate) ? true : x.HireDate == appliDate) &&
                (String.IsNullOrEmpty(Gender) ? true : x.Gender == Gender)
                ).ToList();
            ViewBag.ID = ID;

            return View();
        }
        public ActionResult add()
        {
            int EmpID = db.Employees.Select(x => x.EmployeeID).Max() + 1;
            ViewBag.EmpID = EmpID;

            List<Employees> Position = db.Employees.ToList();//之後去HTML塞選資料
            ViewBag.Position = Position;

            List<Employees> EmpName = db.Employees.ToList();//跟上面一模一樣不用打
            ViewBag.EmpName = EmpName;

            //List<Employees> EmpLastName = db.Employees.ToList();//跟上面一模一樣不用打
            //ViewBag.EmpLastName = EmpLastName;


            return View();
        }

        [HttpPost]

        public ActionResult add(FormCollection input)//FormCollection->從View裡面拿資料
        {
            String EmpID = input["EmpID"];
            String Option = input["Option"];
            String EmpName = input["EmpName"];
            String EmpDate = input["EmpDate"];
            String AppliDate = input["AppliDate"];
            String Phone = input["Phone"];
            String Address = input["Address"];
            String City = input["City"];
            String Region = input["Region"];
           
            String Country = input["Country"];
         
            String EmpLastName = input["EmpLastName"];
            String Title = input["Title"];
            String Gender = input["Gender"];
            String Supervesion = input["Supervesion"];
            String MonthSalary = input["MonthSalary"];
            String YearSalary = input["YearSalary"];


            Employees data = new Employees();//新增一個Employee的資料

            //轉換型態
            data.EmployeeID = Convert.ToInt32(EmpID);
            data.Title = Option;
            data.FirstName = EmpName;
            data.BirthDate = Convert.ToDateTime(EmpDate);
            data.HireDate = Convert.ToDateTime(AppliDate);
            data.Phone = Phone;
            data.Address = Address;
            data.City = City;
            data.Region = Region;
            data.Gender = Gender;
            data.Country = Country;
            data.YearlyPayment = Convert.ToInt32(YearSalary);
            data.MonthlyPayment = Convert.ToInt32(MonthSalary);
            data.TitleOfCourtesy = Title;
            data.LastName = EmpLastName;

            db.Employees.Add(data);//增加到DB裡
            db.SaveChanges();

            return RedirectToAction("Index");//導向Index
        }


        public ActionResult update(String EmpID)//這裡的EmpID跟下面==EmpID一樣
        {

            String BirthMonth = "";
            String BirthDay = "";

            String AppliMonth = "";
            String AppliDay = "";
            String AppliDate = "";
            String BirthDate = "";



            List<Employees> Request = db.Employees.Where(x => x.EmployeeID.ToString() == EmpID).ToList();

            Employees data = new Employees();//開一個空的資料表寫一次ViewBag

            //抓到EmpID資料存在Foreach
            foreach (var tmp in Request)
            {
                data.EmployeeID = tmp.EmployeeID;
                data.FirstName = tmp.FirstName;
                data.LastName = tmp.LastName;
                data.Phone = tmp.Phone;
                
                data.Region = tmp.Region;
                data.Title = tmp.Title;
                data.TitleOfCourtesy = tmp.TitleOfCourtesy;
                data.Address = tmp.Address;
                data.Country = tmp.Country;
                data.City = tmp.City;
                data.YearlyPayment = tmp.YearlyPayment;
                data.MonthlyPayment = tmp.MonthlyPayment;
                

                BirthMonth = DateAddZero(tmp.BirthDate.Month.ToString());
                BirthDay = DateAddZero(tmp.BirthDate.Day.ToString());
                BirthDate = String.Format("{0}-{1}-{2}", tmp.BirthDate.Year, BirthMonth, BirthDay);

                AppliMonth = DateAddZero(tmp.HireDate.Month.ToString());
                AppliDay = DateAddZero(tmp.HireDate.Day.ToString());
                AppliDate = String.Format("{0}-{1}-{2}", tmp.HireDate.Year, AppliMonth, AppliDay);



            }
            ViewBag.EmpID = data;
            ViewBag.BirthDate = BirthDate;
            ViewBag.AppliDate = AppliDate;


            List<Employees> Name = db.Employees.Where(x => x.FirstName != data.FirstName).ToList();
            ViewBag.EmpName = Name;

            List<Employees> EmpLastName = db.Employees.Where(x => x.LastName != data.LastName).ToList();
            ViewBag.EmpLastName = EmpLastName;

            List<Employees> Position = db.Employees.Where(x => x.Title != data.Title).ToList();
            ViewBag.Position = Position;

            List<Employees> Country = db.Employees.Where(x => x.Title != data.Title).ToList();
            ViewBag.Country = Country;

            List<Employees> City = db.Employees.Where(x => x.Title != data.Title).ToList();
            ViewBag.City = City;

            List<Employees> Gender = db.Employees.Where(x => x.Title != data.Title).ToList();
            ViewBag.Gender = Gender;

            return View();
        }

        public String DateAddZero(String BirthMonth)
        {
            StringBuilder a = new StringBuilder();

            if (BirthMonth.Length < 2)
            {
                a.Append("0");
            }
            a.Append(BirthMonth);

            return a.ToString();
        }

        [HttpPost]
        public ActionResult update(FormCollection input)
        {
            String EmpID = input["EmpID"];
            String EmpName = input["EmpName"];
            String EmpLastName = input["EmpLastName"];
            String EmpDate = input["EmpDate"];
            String AppliDate = input["AppliDate"];
            String Phone = input["Phone"];
            String Address = input["Address"];
            String City = input["City"];
            String Region = input["Region"];
          
            String Country = input["Country"];
            String Option = input["Option"];
            String TitleCountry = input["TitleCountry"];
          
        

            Employees data = db.Employees.Find(Convert.ToInt32(EmpID));//用Find(一定要是Table的ID)找到EmpID

            //給值順便轉成資料庫裡面的型態
            data.EmployeeID = Convert.ToInt32(EmpID);
            data.Title = Option;
            data.FirstName = EmpName;
            data.BirthDate = Convert.ToDateTime(EmpDate);
            data.HireDate = Convert.ToDateTime(AppliDate);
            data.Phone = Phone;
            data.Address = Address;
            data.City = City;
            data.Region = Region;
       
            data.Country = Country;
            data.TitleOfCourtesy = TitleCountry;
            data.LastName = EmpLastName;



            db.SaveChanges();
            return RedirectToAction("Index"); ;
        }

        public void Delete(int EmpID)
        {
            Employees employee = db.Employees.Find(EmpID);

            db.Employees.Remove(employee);
            db.SaveChanges();
        }
    }
}