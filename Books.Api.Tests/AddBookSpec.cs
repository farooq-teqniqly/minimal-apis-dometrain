// <copyright file="AddBookSpec.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Books.Api.Tests
{
	using Books.Api.Models;
	using Books.Api.Repositories;
	using FakeItEasy;
	using FluentAssertions;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using System.Net;
	using TechTalk.SpecFlow;

	[Binding]
	public class AddBookSpec : IClassFixture<CustomWebApplicationFactory>, IDisposable
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private HttpResponseMessage addBookResponse = default!;

		private readonly Book bookToAdd = new()
		{
			Isbn = "0393341763",
			Title = "A Clockwork Orange",
			Author = "Anthony Burgess",
			PageCount = 240,
			ReleaseDate = new DateOnly(2019, 5, 21)
		};

		private Book addedBook = default!;

		public AddBookSpec(CustomWebApplicationFactory webApplicationFactory)
		{
			var customWebApplicationFactory = webApplicationFactory.WithWebHostBuilder(b =>
			{
				b.ConfigureTestServices(s =>
				{
					s.RemoveAll<IBookRepository>();
					s.AddSingleton(_ => A.Fake<IBookRepository>());
				});
			});

			client = new ApiTestClient(customWebApplicationFactory.CreateClient());
			bookRepository = customWebApplicationFactory.Services.GetRequiredService<IBookRepository>();
		}

		[Given(@"a book with the given isbn does not exist")]
		public void GivenABookWithTheGivenIsbnDoesNotExist()
		{
			A.CallTo(() => bookRepository.AddBookAsync(bookToAdd)).Returns(true);
		}

		[When(@"a valid post request is made")]
		public async Task WhenAValidPostRequestIsMade()
		{
			addBookResponse = await client.PostAsync("books", bookToAdd);
			addBookResponse.Should().NotBeNull();

		}

		[Then(@"a (.*) created status is returned")]
		public void ThenACreatedStatusIsReturned(int p0) => addBookResponse.StatusCode.Should().Be(HttpStatusCode.Created);

		[Then(@"the location header is set")]
		public void ThenTheLocationHeaderIsSet()
		{
			var locationHeader = addBookResponse.Headers.Location;
			locationHeader.Should().NotBeNull();

			locationHeader!.ToString().Should().EndWith($"books/{bookToAdd.Isbn}");
		}

		[Then(@"the response body contains the added book")]
		public async Task ThenTheResponseBodyContainsTheAddedBook()
		{
			addedBook = await ApiTestClient.ReadFromJsonAsync<Book>(addBookResponse);
			addedBook.Should().NotBeNull();
		}

		[Then(@"the book in the response has an isbn")]
		public void ThenTheBookInTheResponseHasAnIsbn() => addedBook.Isbn.Should().Be(bookToAdd.Isbn);

		[Then(@"the book in the response has an author")]
		public void ThenTheBookInTheResponseHasAnAuthor() => addedBook.Author.Should().Be(bookToAdd.Author);

		[Then(@"the book in the response has a title")]
		public void ThenTheBookInTheResponseHasATitle() => addedBook.Title.Should().Be(bookToAdd.Title);

		[Then(@"the book in the response has a page count")]
		public void ThenTheBookInTheResponseHasAPageCount() => addedBook.PageCount.Should().Be(bookToAdd.PageCount);

		[Then(@"the book in the response has a release date")]
		public void ThenTheBookInTheResponseHasAReleaseDate() => addedBook.ReleaseDate.Should().Be(bookToAdd.ReleaseDate);

		public void Dispose() => client.Dispose();
	}
}