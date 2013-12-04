using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RavenMvcApp.Indexes;
using RavenMvcApp.Models;

namespace RavenMvcApp.Controllers
{
    public class PersonController : RavenBaseController
    {
        public ActionResult Index()
        {
            var people = RavenSession.Query<Person>().ToList();

            //add number of people in each state
            ViewBag.peoplePerState = RavenSession.Query<People_CountByState.ReduceResults, People_CountByState>();

            return View("Index", people);
        }

        public ActionResult Details(string id)
        {
            var person = RavenSession.Load<Person>(id);
            return View(person);
        }

        public ActionResult Create()
        {
            var person = new Person();
            return View(person);
        }

        [HttpPost]
        public ActionResult Create(Person person)
        {
            try
            {
                RavenSession.Store(person);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            var person = RavenSession.Load<Person>(id);
            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(string id, Person person)
        {
            try
            {
                RavenSession.Store(person);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(person);
            }
        }

        public ActionResult Delete(string id)
        {
            var person = RavenSession.Load<Person>(id);
            return View(person);
        }

        [HttpPost]
        public ActionResult Delete(string id, Person person)
        {
            try
            {
                RavenSession.Advanced.DocumentStore.DatabaseCommands.Delete(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(person);
            }
        }
    }
}
