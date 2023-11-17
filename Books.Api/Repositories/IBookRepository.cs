// <copyright file="IBookRepository.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api.Repositories
{
	using Models;

	public interface IBookRepository
	{
		Task<Book?> GetBookAsync(string isbn);
	}
}
