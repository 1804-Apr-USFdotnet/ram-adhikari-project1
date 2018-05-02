using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /*  Manages database connection
     *  Use repository pattern for separation of concerns 
     */
    public class DataLayer
    {
        //Database instance
        private RestaurantDBEntities db = new RestaurantDBEntities();

        //Create
        public void AddRestaurant(List<string> list)
        {
            var res = new Restaurant()
            {
                Name = list.ElementAt(0),
                Address = list.ElementAt(1),
                Phone = list.ElementAt(2),
                Website = list.ElementAt(3),
                DeliveryOption = list.ElementAt(4),
                FoodType = list.ElementAt(5)
            };
            db.Restaurants.Add(res);
            db.SaveChanges();
        }

        //Read
        public List<Restaurant> GetRestaurantsList()
        {
            List<Restaurant> restaurants = db.Restaurants.ToList();
            return restaurants;
        }

        //Update
        public void ModifyRestaurant(List<string> list)
        {
            int restaurantId = int.Parse(list[0]);
            Restaurant res = FindRestaurantById(restaurantId);
            if (res != null)
            {
                res.Name = list[1].Length > 0 ? list[1] : res.Name;
                res.Address = list[2].Length > 0 ? list[2] : res.Address;
                res.Phone = list[3].Length > 0 ? list[3] : res.Phone;
                res.Website = list[4].Length > 0 ? list[4] : res.Website;
                res.DeliveryOption = list[5].Length > 0 ? list[5] : res.DeliveryOption;
                res.FoodType = list[6].Length > 0 ? list[6] : res.FoodType;
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Restaurant with id: " + restaurantId + " " + "is not in the database.");
            }
        }

        //Delete
        public void DeleteRestaurant(int restaurantId)
        {
            Restaurant res = FindRestaurantById(restaurantId);
            if (res != null)
            {
                //Delete reviews associated with this restuarant first 
                DeleteReviewsByRestId(res.RestaurantId);
                db.Restaurants.Remove(res);
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Restaurant with id: " + restaurantId + " " + "is not in the database.");
            }
        }
        
        //Helper method
        public Restaurant FindRestaurantById(int restaurantId)
        {
            List<Restaurant> restaurants = GetRestaurantsList();
            foreach (var res in restaurants)
            {
                if (res.RestaurantId == restaurantId)
                {
                    return res;
                }
            }
            return null;
        }

        //Helper method
        public bool IsValidRestaurant(int restaurantId)
        {
            IEnumerable<Restaurant> restaurants = db.Restaurants.ToList();
            foreach (var res in restaurants)
            {
                if (res.RestaurantId == restaurantId)
                {
                    return true;
                }
            }
            return false;
        }

        //------------------------------------------------------------------

        //Create
        public void AddReview(List<string> list)
        {            
            int resId = int.Parse(list.ElementAt(0));
            if (IsValidRestaurant(resId))
            {
                var rev = new Review()
                {
                    RestaurantId = resId,
                    Rating = int.Parse(list.ElementAt(1)),
                    Username = list.ElementAt(2),
                    Comment = list.ElementAt(3),
                };
                db.Reviews.Add(rev);
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Restaurant id " + resId + " does not exist in the database.");
            }
        }

        //Read
        public List<Review> GetReviewsList()
        {
            List<Review> reviews = db.Reviews.ToList();
            return reviews;
        }

        //Update 
        public void ModifyReview(List<string> list)
        {
            int revId = int.Parse(list.ElementAt(0));
            Review rev = FindReviewById(revId);
            if (rev != null)
            {
                rev.RestaurantId = list[1].Length > 0 ? int.Parse(list[1]) : rev.RestaurantId;
                rev.Rating = list[2].Length > 0 ? int.Parse(list[2]) : rev.Rating;
                rev.Username = list[3].Length > 0 ? list[3] : rev.Username;
                rev.Comment = list[4].Length > 0 ? list[4] : rev.Comment;
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Review with id " + revId + " is not in the database.");
            }
        }

        //Delete
        public void DeleteReview(int reviewId)
        {
            Review rev = FindReviewById(reviewId);
            if (rev != null)
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Review with id: " + reviewId + " " + "is not in the database.");
            }
        }

        //Delete reviews in bulk
        public void DeleteReviewsByRestId(int restaurantId)
        {
            List<Review> reviews = GetReviewsList();
            foreach (var rev in reviews)
            {
                if (rev.RestaurantId == restaurantId)
                {
                    db.Reviews.Remove(rev);
                    db.SaveChanges();
                }
            }
        }

        //Helper method
        public Review FindReviewByRestId(int restaurantId)
        {
            List<Review> reviews = GetReviewsList();
            foreach (var rev in reviews)
            {
                if (rev.RestaurantId == restaurantId)
                {
                    return rev;
                }
            }
            return null;
        }

        public Review FindReviewById(int reviewId)
        {
            List<Review> reviews = GetReviewsList();
            foreach (var rev in reviews)
            {
                if (rev.ReviewId == reviewId)
                {
                    return rev;
                }
            }
            return null;
        }

    }
}
