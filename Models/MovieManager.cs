namespace ST10361554_PROG6212_ICE_Task_5.Models
{
    public class MovieManager
    {
        private List<MovieModel> _movies = new List<MovieModel>()
        {
            new MovieModel
            {
                Title = "The Matrix",
                Genre = "Action",
                Duration = 136
            },

            new MovieModel
            {
                Title = "The Shawshank Redemption",
                Genre = "Drama",
                Duration = 142
            },

            new MovieModel
            {
                Title = "The Godfather",
                Genre = "Crime",
                Duration = 175
            },

            new MovieModel
            {
                Title = "The Dark Knight",
                Genre = "Action",
                Duration = 152
            },

            new MovieModel
            {
                Title = "Pulp Fiction",
                Genre = "Crime",
                Duration = 154
            }
        };

        private List<ShowtimeModel> _showTimes = new List<ShowtimeModel>();

        private List<TicketModel> _tickets = new List<TicketModel>();

        public void GenerateShowtimes()
        {
            Random random = new Random();

            // Loop to create 7 showtimes
            for (int i = 0; i < 7; i++)
            {
                // Pick a random movie from the _movies list
                var randomMovie = _movies[random.Next(_movies.Count)];

                // Create a random showtime
                var showtime = new ShowtimeModel
                {
                    MovieId = randomMovie.Id, // Assuming MovieId corresponds to the Movie Title in this case
                    MovieTitle = randomMovie.Title,
                    ShowTime = DateTime.Now.AddDays(random.Next(1, 10)).AddHours(random.Next(9, 23)), // Random time within the next 10 days
                    AvailableSeats = random.Next(1, 201) // Random number of seats available (1-200)
                };

                // Add the generated showtime to the _showTimes list
                _showTimes.Add(showtime);
            }
        }

        #region Create Methods (C)
        // This method adds a movie to the list of movies
        public void AddMovie(MovieModel movie)
        {
            try
            {
                _movies.Add(movie);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method adds a showtime to the list of showtimes
        public void AddShowtime(ShowtimeModel showtime)
        {
            try
            {
                _showTimes.Add(showtime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method adds a ticket to the list of tickets
        public void AddTicket(TicketModel ticket)
        {
            try
            {
                _tickets.Add(ticket);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Read Methods (R)
        // This method returns a list of movies
        public List<MovieModel> GetMovies()
        {
            return _movies;
        }

        // This method returns a list of showtimes
        public List<ShowtimeModel> GetShowtimes()
        {
            return _showTimes;
        }

        // This method returns a list of tickets
        public List<TicketModel> GetTickets()
        {
            return _tickets;
        }

        // This method returns a movie by its ID
        public MovieModel GetMovieById(string id)
        {
            try
            {
                return _movies.FirstOrDefault(m => m.Id == id)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns a showtime by its ID
        public ShowtimeModel GetShowtimeById(string id)
        {
            try
            {
                return _showTimes.FirstOrDefault(s => s.Id == id)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns a ticket by its ID
        public TicketModel GetTicketById(string id)
        {
            try
            {
                return _tickets.FirstOrDefault(t => t.Id == id)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns a list of showtimes for a movie
        public List<ShowtimeModel> GetShowtimesByMovieId(string movieId)
        {
            try
            {
                return _showTimes.Where(s => s.MovieId == movieId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns a list of tickets for a showtime
        public List<TicketModel> GetTicketsByShowtimeId(string showtimeId)
        {
            try
            {
                return _tickets.Where(t => t.ShowtimeId == showtimeId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns the number of available seats for a showtime
        public int GetAvailableSeatsByShowtimeId(string showtimeId)
        {
            try
            {
                var showtime = _showTimes.FirstOrDefault(s => s.Id == showtimeId)!;
                var tickets = _tickets.Where(t => t.ShowtimeId == showtimeId).ToList();
                return showtime.AvailableSeats - tickets.Sum(t => t.NumberOfTickets);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method returns the total number of tickets sold for a showtime
        public int GetTotalTicketsSoldByShowtimeId(string showtimeId)
        {
            try
            {
                var tickets = _tickets.Where(t => t.ShowtimeId == showtimeId).ToList();
                return tickets.Sum(t => t.NumberOfTickets);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Methods (U)
        // This method updates the number of available seats for a showtime
        public void UpdateAvailableSeatsByShowtimeId(string showtimeId, int numberOfTickets)
        {
            try
            {
                var showtime = _showTimes.FirstOrDefault(s => s.Id == showtimeId)!;
                showtime.AvailableSeats -= numberOfTickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method updates the details of a movie
        public void UpdateMovie(MovieModel movie)
        {
            var index = _movies.FindIndex(m => m.Id == movie.Id);
            _movies[index] = movie;
        }

        // This method updates the details of a showtime
        public void UpdateShowtime(ShowtimeModel showtime)
        {
            try
            {
                var index = _showTimes.FindIndex(s => s.Id == showtime.Id);
                _showTimes[index] = showtime;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete Methods (D)
        // This method deletes a ticket
        public void DeleteTicket(string id)
        {
            try
            {
                var ticket = _tickets.FirstOrDefault(t => t.Id == id)!;
                _tickets.Remove(ticket);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method deletes a showtime
        public void DeleteShowtime(string id)
        {
            try
            {
                var showtime = _showTimes.FirstOrDefault(s => s.Id == id)!;
                _showTimes.Remove(showtime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // This method deletes a movie
        public void DeleteMovie(string id)
        {
            try
            {
                var movie = _movies.FirstOrDefault(m => m.Id == id)!;
                _movies.Remove(movie);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
