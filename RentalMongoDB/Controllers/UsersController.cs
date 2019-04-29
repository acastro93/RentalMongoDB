using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RentalMongoDB.App_Start;
using RentalMongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalMongoDB.Controllers
{
    public class UsersController : Controller
    {

        MongoDBContext dBContext;

        public UsersController()
        {
            dBContext = new MongoDBContext();

        }

        
        // GET: Users
        [HttpGet]
        public ActionResult Index()
        {
            var userList = dBContext.db.GetCollection<UserModel>("Users").FindAll().ToList();
            return View(userList);
        }

        // GET: Users/Details/5
        [HttpGet]
        public ActionResult Details(string id)
        {
            var user = Query<UserModel>.EQ(x => x.Id, new ObjectId(id));
            var userDetails = dBContext.db.GetCollection<UserModel>("Users").FindOne(user);
            return View(userDetails);
        }


        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(UserModel user)
        {
            var userList = dBContext.db.GetCollection<BsonDocument>("Users");


            var query = Query.EQ("IdNumber", user.IdNumber);
            var existsOne = userList.FindAs<UserModel>(query).Count();

            if (existsOne == 0)
            {
                if ( (user.IdNumber != 0) && (user.FirstName != null) && (user.LastName != null) && (user.PhoneNumber != null) )
                {
                    var result = userList.Insert(user);
                }
                else
                {
                    ViewBag.Error = "Please fill the form completely";
                    return View("Create", user);
                }
                
            }
            else
            {
                
                ViewBag.Error = "User already registered";
                return View("Create", user);
            }

            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            var userList = dBContext.db.GetCollection<UserModel>("Users");

            var userCount = userList.FindAs<UserModel>(Query.EQ("_id", new ObjectId(id))).Count();

            if (userCount > 0)
            {
                var userId = Query<UserModel>.EQ(x => x.Id, new ObjectId(id));
                var userDetail = dBContext.db.GetCollection<UserModel>("Users").FindOne(userId);

                return View(userDetail);
            }

            return RedirectToAction("Index");
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, UserModel user)
        {
            try
            {
                user.Id = new ObjectId(id);

                var userId = Query<UserModel>.EQ(x => x.Id, new ObjectId(id));

                var userList = dBContext.db.GetCollection<UserModel>("Users");

                var query = Query.EQ("IdNumber", user.IdNumber);
                var existsOne = userList.FindAs<UserModel>(query).Count();

                if (existsOne == 0)
                {
                    var result = userList.Update(userId, Update.Replace(user), UpdateFlags.None);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "User already registered";
                    return View("Edit", user);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var userId = Query<UserModel>.EQ(x => x.Id, new ObjectId(id));

            var userDetails = dBContext.db.GetCollection<UserModel>("Users").FindOne(userId);

            return View(userDetails);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, UserModel user)
        {
            try
            {
                var userId = Query<UserModel>.EQ(x => x.Id, new ObjectId(id));

                var userList = dBContext.db.GetCollection<UserModel>("Users");

                var deletion = userList.Remove(userId, RemoveFlags.Single);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
