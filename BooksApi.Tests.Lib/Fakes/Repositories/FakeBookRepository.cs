// <copyright file="FakeBookRepository.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace BooksApi.Tests.Lib.Fakes.Repositories
{
	using Books.Api.Models;
	using Books.Api.Repositories;
	using System.Threading.Tasks;

	public class FakeBookRepository : IBookRepository
	{
		public Task<Book?> GetBookAsync(string isbn) => Task.FromResult(default(Book));
	}
}