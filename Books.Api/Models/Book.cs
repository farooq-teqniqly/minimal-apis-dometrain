// <copyright file="Book.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api.Models
{
	public class Book
	{
		public required string Isbn { get; set; } = default!;
		public required string Author { get; set; } = default!;
		public required string Title { get; set; } = default!;
		public required int PageCount { get; set; }
		public required DateOnly ReleaseDate { get; set; }
	}
}