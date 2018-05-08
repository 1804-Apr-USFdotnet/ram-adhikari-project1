using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ReviewsController : Controller
    {
        private Library lib = new Library();

        // GET: Reviews
        public ActionResult Index()
        {
            List<Review> reviews = (lib.GetAllReviews().Select(x => RevToWeb(x))).ToList();
            return View(reviews.ToList());
        }


        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = RevToWeb(lib.GetReviewById(id));           
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }


        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(lib.GetAllRestaurants(), "RestaurantId", "Name");
            return View();
        }


        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewId,RestaurantId,Rating,Username,Comment,ReviewDate")] Review review)
        {            
            if (ModelState.IsValid)
            {
                List<string> dataList = new List<string>() {review.RestaurantId.ToString(), review.Rating.ToString(), review.Username, review.Comment };

                lib.AddReview(dataList);
                return RedirectToAction("Index");
            }
            ViewBag.RestaurantId = new SelectList(lib.GetAllRestaurants(), "RestaurantId", "Name", review.RestaurantId);
            return View(review);
        }


        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = RevToWeb(lib.GetReviewById(id));
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.RestaurantId = new SelectList(lib.GetAllRestaurants(), "RestaurantId", "Name", review.RestaurantId);
            return View(review);
        }


        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewId,RestaurantId,Rating,Username,Comment,ReviewDate")] Review review)
        {
            if (ModelState.IsValid)
            {
                List<string> dataList = new List<string>() { review.ReviewId.ToString(), review.RestaurantId.ToString(),
                                                            review.Rating.ToString(), review.Username, review.Comment };

                lib.ModifyReview(dataList);
                return RedirectToAction("Index");
            }
            ViewBag.RestaurantId = new SelectList(lib.GetAllRestaurants(), "RestaurantId", "Name", review.RestaurantId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = RevToWeb(lib.GetReviewById(id));
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }


        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = RevToWeb(lib.GetReviewById(id));
            lib.DeleteReview(review.ReviewId);
            return RedirectToAction("Index");
        }

        public static Review RevToWeb(Data.Review dataReview)
        {
            var webReview = new Review()
            {
                ReviewId = dataReview.ReviewId,
                RestaurantId = dataReview.RestaurantId,
                Rating = dataReview.Rating,
                Username = dataReview.Username,
                Comment = dataReview.Comment
            };
            return webReview;
        }
    }
}


