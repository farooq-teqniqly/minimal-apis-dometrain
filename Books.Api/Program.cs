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

			app.MapPost("books", async (Book book, IBookRepository bookRepository) =>
			{
				var added = await bookRepository.AddBookAsync(book);
				return Results.Created($"books/{book.Isbn}", book);
			});

			app.MapGet("books/{isbn}", async (string isbn, IBookRepository bookRepository) =>
			{
				var book = await bookRepository.GetBookAsync(isbn);
				return Results.Ok(book);
			});

			app.Run();
		}
	}
}