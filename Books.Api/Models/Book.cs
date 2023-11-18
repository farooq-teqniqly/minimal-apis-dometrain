// <copyright file="Book.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Book
	{
		[RegularExpression(@"\d{10}", ErrorMessage = "Invalid ISBN format.")]
		public required string Isbn { get; set; } = default!;
		public required string Author { get; set; } = default!;
		public required string Title { get; set; } = default!;
		public required int PageCount { get; set; }
		public required DateOnly ReleaseDate { get; set; }
	}
}