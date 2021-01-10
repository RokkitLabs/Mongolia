﻿using System;
using System.Threading.Tasks;

namespace Mongolia.Example
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			string username = "Alex";

			DB db = new DB("mongodb://localhost:27017/?ssl=false", "mongoliaTesting");
			DBRepository<User> userRepo = db.GetRepository<User>();
			
			User user = await userRepo.Create(new { username });
			var id = await user.Save();

			Console.WriteLine($"Inserted doc with id {id}");

			var foundUser = await userRepo.FindOne(new { username });
			Console.WriteLine(foundUser.ID);

			await foundUser.Delete();

			/*
				User user = new User()
				{
					Username = "00"
				};

				User user = db.Create<User>();
				user.Username = "00";
				await user.Save();

				await user.Save(db);

				User user = db.Create<User>(new { Username = "00" });
				await user.Save();
			*/
		}
	}
}