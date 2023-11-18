// <copyright file="Program.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api
{
	using Models;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var app = builder.Build();

			app.MapPost("books", (Book book) =>
			{
				return Results.Created($"books/{book.Isbn}", book);
			});

			app.Run();
		}
	}
}