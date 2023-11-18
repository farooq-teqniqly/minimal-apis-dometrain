// <copyright file="Book.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api.Models
{
	public class Book
	{
		public required string Isbn { get; init; } = default!;
		public string Title { get; init; } = default!;
		public string Author { get; init; } = default!;
		public int PageCount { get; init; }
		public DateOnly ReleaseDate { get; init; }
	}
}