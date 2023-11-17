// <copyright file="Program.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api
{
	using Models;
	using Repositories;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var app = builder.Build();

			app.MapPost("books",
				(Book book, IBookRepository bookRepository) => Results.Created($"books/{book.Isbn}", null));

			app.Run();
		}
	}
}