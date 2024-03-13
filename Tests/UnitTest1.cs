public class NuGetWayTests // this test works
{
    [Fact]
    public void GetLongLatNuGetWay_ReturnsCorrectLocation()
    {
        // Arrange
        var mockLocationService = new Mock<IMapLocationService>();
        mockLocationService.Setup(service => service.GetLatLongFromAddress(It.IsAny<string>()))
            .Returns(new CustomMapPoint { Latitude = 26.6406, Longitude = -81.8723 }); // Sample latitude and longitude for Fort Myers, Florida

        var nuGetWay = new NuGetWay(mockLocationService.Object);
        var address = "Fort Myers, Florida";
        var expected = $"Address (Fort Myers, Florida) is at 26.6406,-81.8723";

        // Act
        var result = nuGetWay.GetLongLatNuGetWay(address);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetLongLatNuGetWay_HandlesExceptions()
    {
        // Arrange
        var mockLocationService = new Mock<IMapLocationService>();
        mockLocationService.Setup(service => service.GetLatLongFromAddress(It.IsAny<string>()))
            .Throws(new Exception("Service unavailable"));

        var nuGetWay = new NuGetWay(mockLocationService.Object);
        var address = "Fort Myers, Florida";
        var expected = "Error retrieving coordinates: Service unavailable";

        // Act
        var result = nuGetWay.GetLongLatNuGetWay(address);

        // Assert
        Assert.Equal(expected, result);
    }
}

// test ideas? 
// what if you pass in an address thats not real
// pass different address (Theory test)
// invalid inputs


public class LongVersionTests // this test works
{
    [Fact]
    public async Task GetGeolocationAsync_ReturnsGeolocationForValidAddress()
    {
        // Arrange
        var mockResponseContent = @"
        {
            ""status"": ""OK"",
            ""results"": [
                {
                    ""geometry"": {
                        ""location"": {
                            ""lat"": 37.4224764,
                            ""lng"": -122.0842499
                        }
                    }
                }
            ]
        }";

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(mockResponseContent)
            });

        var client = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://example.com")
        };

        var longVersion = new LongVersion(client, "YOUR_API_KEY");
        var address = "1600 Amphitheatre Parkway, Mountain View, CA";

        // Act
        var geolocationResult = await longVersion.GetGeolocationAsync(address);

        // Assert
        var expectedGeolocation = "Latitude: 37.4224764, Longitude: -122.0842499";
        Assert.Equal(expectedGeolocation, geolocationResult);
    }
}


