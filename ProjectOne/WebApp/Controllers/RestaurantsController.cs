using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using BusinessLogic;
using NLog;

namespace WebApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private Library lib = new Library();
        Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: Restaurants
        public ActionResult Index(string searchName, string sortBy)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            ViewBag.DeliverySort = sortBy == "delivery" ? "delivery_desc" : "delivery";

            IEnumerable<Restaurant> restaurants = (lib.PartialNameSearch(false, searchName)).Select(x => ToWeb(x));

            switch (sortBy)
            {
                case "name_desc":
                    restaurants = restaurants.OrderByDescending(r => r.Name);
                    break;
                case "delivery":
                    restaurants = restaurants.OrderBy(r => r.DeliveryOption);
                    break;
                case "delivery_desc":
                    restaurants = restaurants.OrderBy(r => r.DeliveryOption);
                    break;
                default:
                    restaurants = restaurants.OrderBy(r => r.Name);
                    break;
            }
            List<String> topThree = lib.TopThree().ToList();
            ViewBag.First = " Restaurant: " + topThree[0];
            ViewBag.FirstRating = " Rating: " + topThree[1];
            ViewBag.Second = " Restaurant: " + topThree[2];
            ViewBag.SecondRating = " Rating: " + topThree[3];
            ViewBag.Third = " Restaurant: " + topThree[4];
            ViewBag.ThirdRating = " Rating: " + topThree[5];

            return View(restaurants.ToList());
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = ToWeb(lib.GetRestaurantById(id));
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }


        // GET: Restaurants/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Restaurants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RestaurantId,Name,Address,Phone," +
            "Website,DeliveryOption,FoodType")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                List<string> dataList = new List<string>() {restaurant.Name, restaurant.Address, restaurant.Phone, restaurant.Website,
                                                         restaurant.DeliveryOption, restaurant.FoodType };

                lib.Add(dataList);
                logger.Info("Added a new restaurant.");
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }


        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Error("Edit: BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = ToWeb(lib.GetRestaurantById(id));
            if (restaurant == null)
            {
                logger.Error("Edit: Restaurant not found");
                return HttpNotFound();
            }
            return View(restaurant);
        }


        // POST: Restaurants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RestaurantId,Name,Address,Phone," +
            "Website,DeliveryOption,FoodType")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                List<string> dataList = new List<string>() {restaurant.RestaurantId.ToString(), restaurant.Name,
                                                        restaurant.Address, restaurant.Phone, restaurant.Website,
                                                         restaurant.DeliveryOption, restaurant.FoodType
                };

                lib.Modify(dataList);
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }


        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Error("Delete: BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = ToWeb(lib.GetRestaurantById(id));
            if (restaurant == null)
            {
                logger.Error("Delete: Restaurant not found");
                return HttpNotFound();
            }
            return View(restaurant);
        }


        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = ToWeb(lib.GetRestaurantById(id));
            lib.Delete(restaurant.RestaurantId);
            return RedirectToAction("Index");
        }

        public static Restaurant ToWeb(Data.Restaurant dataRestaurant)
        {
            var webRestaurant = new Restaurant()
            {
                RestaurantId = dataRestaurant.RestaurantId,
                Name = dataRestaurant.Name,
                Address = dataRestaurant.Address,
                Phone = dataRestaurant.Phone,
                Website = dataRestaurant.Website,
                DeliveryOption = dataRestaurant.DeliveryOption,
                FoodType = dataRestaurant.FoodType
            };
            return webRestaurant;
        }
    }
}


//var restaurants = rests.Select(x => ToWeb(x));