using RentalMongoDB.App_Start;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RentalMongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalMongoDB.Controllers
{
    public class VehiclesController : Controller
    {

        MongoDBContext dBContext;

        public VehiclesController()
        {
            dBContext = new MongoDBContext();

        }


        // GET: Vehicles
        [HttpGet]
        public ActionResult Index()
        {
            var vehicleList = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindAll().ToList();
            return View(vehicleList);
        }

        // GET: Vehicles/Details/5
        [HttpGet]
        public ActionResult Details(string id)
        {
            var vehicle = Query<VehicleModel>.EQ(x => x.Id, new ObjectId(id));
            var vehicleDetails = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(vehicle);
            return View(vehicleDetails);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        public ActionResult Create(VehicleModel vehicle)
        {
            var vehicleList = dBContext.db.GetCollection<BsonDocument>("Vehicles");

            var query = Query.EQ("Plate", vehicle.Plate);
            var existsOne = vehicleList.FindAs<VehicleModel>(query).Count();

            if (existsOne == 0)
            {
                if ((vehicle.Plate != 0) && (vehicle.Brand != null) && (vehicle.Model != null) && (vehicle.RentalPrice != 0) && (vehicle.State != null))
                {
                    var result = vehicleList.Insert(vehicle);
                }
                else
                {
                    ViewBag.Error = "Please fill the required spaces (Plate, Brand, Model, Rental Price and State)";
                    return View("Create", vehicle);
                }
            }
            else
            {

                ViewBag.Error = "Vehicle already registered";
                return View("Create", vehicle);
            }

            return RedirectToAction("Index");
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(string id)
        {
            var vehicleList = dBContext.db.GetCollection<VehicleModel>("Vehicles");

            var vehicleCount = vehicleList.FindAs<VehicleModel>(Query.EQ("_id", new ObjectId(id))).Count();

            if (vehicleCount > 0)
            {
                var vehicleId = Query<VehicleModel>.EQ(x => x.Id, new ObjectId(id));
                var vehicleDetail = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(vehicleId);

                return View(vehicleDetail);
            }

            return RedirectToAction("Index");
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, VehicleModel vehicle)
        {
            try
            {
                vehicle.Id = new ObjectId(id);

                var vehicleId = Query<VehicleModel>.EQ(x => x.Id, new ObjectId(id));

                var vehicleList = dBContext.db.GetCollection<VehicleModel>("Vehicles");

                var query = Query.EQ("Plate", vehicle.Plate);
                var existsOne = vehicleList.FindAs<VehicleModel>(query).Count();

                if (existsOne == 0)
                {
                    var result = vehicleList.Update(vehicleId, Update.Replace(vehicle), UpdateFlags.None);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Vehicle already registered";
                    return View("Edit", vehicle);
                }
                

                
            }
            catch
            {
                return View();
            }
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(string id)
        {
            var vehicleId = Query<VehicleModel>.EQ(x => x.Id, new ObjectId(id));

            var vehicleDetails = dBContext.db.GetCollection<VehicleModel>("Vehicles").FindOne(vehicleId);

            return View(vehicleDetails);
        }

        // POST: Vehicles/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, VehicleModel vehicle)
        {
            try
            {
                var vehicleId = Query<VehicleModel>.EQ(x => x.Id, new ObjectId(id));

                var vehicleList = dBContext.db.GetCollection<VehicleModel>("Vehicles");

                var deletion = vehicleList.Remove(vehicleId, RemoveFlags.Single);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
