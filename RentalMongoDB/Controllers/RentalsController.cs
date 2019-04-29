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
    public class RentalsController : Controller
    {

        MongoDBContext dBContext;

        public RentalsController()
        {
            dBContext = new MongoDBContext();

        }

        
        // GET: Rentals
        public ActionResult Index()
        {
            var rentalsList = dBContext.db.GetCollection<RentalModel>("Rentals").FindAll().ToList();
            return View(rentalsList);
        }

        // GET: Rentals/Details/5
        [HttpGet]
        public ActionResult Details(string id)
        {
            var rental = Query<RentalModel>.EQ(x => x.Id, new ObjectId(id));
            var rentalDetails = dBContext.db.GetCollection<RentalModel>("Rentals").FindOne(rental);
            return View(rentalDetails);
        }

        // GET: Rentals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        public ActionResult Create(RentalModel rental)
        {
            var rentalList = dBContext.db.GetCollection<BsonDocument>("Rentals");
            var userList = dBContext.db.GetCollection<BsonDocument>("Users");
            var vehicleList = dBContext.db.GetCollection<BsonDocument>("Vehicles");

            var queryUser = Query.EQ("IdNumber", rental.IdNumber);
            var queryVehicle = Query.EQ("Plate", rental.Plate);

            var existsUser = userList.FindAs<UserModel>(queryUser).Count();
            var existsVehicle = vehicleList.FindAs<VehicleModel>(queryVehicle).Count();


            if ( (existsUser > 0) && (existsVehicle > 0) )
            {
                if (isRented(rental))
                {
                    ViewBag.Error = "Car is Rented";
                    return View("Create", rental);

                }
                else
                {
                    if ((rental.IdNumber != 0) && (rental.Plate != 0) && (rental.RentalDays != 0))
                    {
                        int costs = CalculateCosts(rental);

                        ViewBag.Message = "El costo de renta es de " + costs.ToString();
                        var result = userList.Insert(rental);
                        changeState(rental);
                        return View("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Please fill the form completely";
                        return View("Create", rental);
                    }
                }
            }
            else
            {

                ViewBag.Error = "User or Vehicle doesn't exist";
                return View("Create", rental);
            }
        }

        // GET: Rentals/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rentals/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rentals/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rentals/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public int CalculateCosts(RentalModel rental)
        {

            var query = Query<VehicleModel>.EQ(x => x.Plate, rental.Plate);

            var vehicleDetails = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(query);
            int cost = (vehicleDetails.RentalPrice).Value * rental.RentalDays;

            return cost;
        }
        

        public bool isRented(RentalModel rental)
        {
            var query = Query<VehicleModel>.EQ(x => x.Plate, rental.Plate);

            var vehicleDetails = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(query);
            
            if (vehicleDetails.State.Equals("Rented"))
            {
                return true ;
            }
            else
            {
                return false ;
            }
        }

        public void changeState(RentalModel rental)
        {
            var query = Query<VehicleModel>.EQ(x => x.Plate, rental.Plate);
            var vehicleList = dBContext.db.GetCollection<BsonDocument>("Vehicles");
            var vehicleDetails = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(query);

            if (vehicleDetails.State.Equals("Rented"))
            {
                vehicleDetails.State = "Available";
                vehicleList.Update(query, Update.Replace(rental), UpdateFlags.None);
            }
            else
            {
                vehicleDetails.State = "Rented";
                vehicleList.Update(query, Update.Replace(rental), UpdateFlags.None);
            }


        }
    }
}
