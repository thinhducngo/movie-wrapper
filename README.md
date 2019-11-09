# movie-wrapper

### Getting Started

1. Down loading library from https://github.com/thinhducngo/movie-wrapper
2. Build MovieWrapper.dll file
3. In your console app or web apps add reference MovieWrapper.dll
4. MovieWrapper.dll depends on Json.NET version 12.0.2 -> please pull from Nuget to your project.

### Usage

#### 1. Methods:

``` note
Task<MovieResult> GetMovieDetails(VendorType type, string movieId)
Task<MovieResult> GetMovieDetails(VendorType type, string domain, string movieId)

Task<MovieSessionListResult> GetMovieSessions(VendorType type, string movieId)
Task<MovieSessionListResult> GetMovieSessions(VendorType type, string domain, string movieId)

Task<MovieListResult> GetShowingMovies(VendorType type)
Task<MovieListResult> GetShowingMovies(VendorType type, string domain)
```

#### 2. Enums

``` note
enum VendorType:
- GalaxyCinema,
- Lotteria
```

#### 3. Response Format

``` note
class ServiceResult<T>
- Success: string;   // request success or failed
- Message: string;  // give info if request failed.
- Data: T           // return data with type T
```

#### 3. How to use (Example):

- Initialize:

``` code c#
var service = new MovieService();   // init wrapper service
```

- Get showing movies (default domain Galaxy: https://www.galaxycine.vn)

``` code c#
var response = await service.GetShowingMovies(VendorType.GalaxyCinema);
```

- Get showing movies (in case galaxy change domain)

``` code c#
var response = await service.GetShowingMovies(VendorType.GalaxyCinema, "https://www.galaxycine-custom.vn");
```

- Get movie details (default domain Galaxy: https://www.galaxycine-custom.vn)

``` code c#
var response = await service.GetMovieDetails(VendorType.GalaxyCinema, movieId);
```

- Get movie details (in case galaxy change domain)

``` code c#
var response = await service.GetMovieDetails(VendorType.GalaxyCinema, "https://www.galaxycine-custom.vn", movieId);
```

- Get movie sessions (default domain Galaxy: https://www.galaxycine.vn)

``` code #
var response = await service.GetMovieSessions(VendorType.GalaxyCinema, movieId);
```

- Get movie sessions (in case galaxy change domain)

``` code #
var response = await service.GetMovieSessions(VendorType.GalaxyCinema, "https://www.galaxycine-custom.vn", movieId);
```
