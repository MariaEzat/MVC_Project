﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using MVC_Project.Models;

namespace MVC_Project.Hubs
{
	public class ReviewsHub:Hub
	{
        private readonly BookStoreContext bookStore;
        private readonly UserManager<ApplicationUser> userManager;
        public ReviewsHub(BookStoreContext bookStore, UserManager<ApplicationUser> userManager)
		{
            this.bookStore = bookStore;
            this.userManager = userManager;
        }
		public async Task SendReview(string message, int bookId)
		{
			var CurrentCustomer = await userManager.GetUserAsync(Context.User);
            var loginId = await userManager.GetUserIdAsync(CurrentCustomer);
            string name = CurrentCustomer.UserName;
           


            Review review = new Review
			{
               
				UserId= loginId,
                Text = message,
				BookID = bookId
			};
			bookStore.Reviews.Add(review);
			await bookStore.SaveChangesAsync();
			await Clients.All.SendAsync("RecieveNewComment", name, message, bookId);
			
		}
	}
}
