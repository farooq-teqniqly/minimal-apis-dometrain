// <copyright file="GetBookSpec.cs" company="Teqniqly">
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
	public class GetBookSpec : IClassFixture<CustomWebApplicationFactory>, IDisposable
	{
		private readonly ApiTestClient client;
		private readonly IBookRepository bookRepository;
		private HttpResponseMessage getBookResponse = default!;

		private readonly Book addedBook = new()
		{
			Isbn = "123",
			Title = "A Clockwork Orange",
			Author = "Anthony Burgess",
			PageCount = 240,
			ReleaseDate = new DateOnly(2019, 5, 21)
		};

		public GetBookSpec(CustomWebApplicationFactory webApplicationFactory)
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

		[Given(@"a book with the given isbn has been added")]
		public void GivenABookWithTheGivenIsbnHasBeenAdded() =>
			A.CallTo(() =>
				bookRepository.GetBookAsync(addedBook.Isbn)).Returns(Task.FromResult<Book?>(addedBook));

		[When(@"a get request is made")]
		public async Task WhenAGetRequestIsMade()
		{
			getBookResponse = await client.GetAsync($"books/{addedBook.Isbn}");
			getBookResponse.Should().NotBeNull();
		}

		[Then(@"a (.*) ok status is returned")]
		public void ThenAOkStatusIsReturned(int p0) => getBookResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		private Book retrievedBook = default!;

		[Then(@"the response body contains the retrieved book")]
		public async Task ThenTheResponseBodyContainsTheRetrievedBook()
		{
			retrievedBook = await ApiTestClient.ReadFromJsonAsync<Book>(getBookResponse);
			retrievedBook.Should().NotBeNull();
		}

		[Then(@"the retrieved book has an isbn")]
		public void ThenTheRetrievedBookHasAnIsbn() => retrievedBook.Isbn.Should().Be(addedBook.Isbn);

		[Then(@"the retrieved book has an author")]
		public void ThenTheRetrievedBookHasAnAuthor() => retrievedBook.Author.Should().Be(addedBook.Author);

		[Then(@"the retrieved book has an title")]
		public void ThenTheRetrievedBookHasAnTitle() => retrievedBook.Title.Should().Be(addedBook.Title);

		[Then(@"the retrieved book has an page count")]
		public void ThenTheRetrievedBookHasAnPageCount() => retrievedBook.PageCount.Should().Be(addedBook.PageCount);

		[Then(@"the retrieved book has an release date")]
		public void ThenTheRetrievedBookHasAnReleaseDate() => retrievedBook.ReleaseDate.Should().Be(addedBook.ReleaseDate);

		public void Dispose() => client.Dispose();
	}
}