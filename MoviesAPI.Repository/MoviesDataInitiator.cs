using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesAPI.Domain;
using MoviesAPI.Repository.Entities;

namespace MoviesAPI.Repository
{
    public static class MoviesDataInitiator
    {
        public static void Initialize(IServiceProvider serviceProvider,
                                        string blobHostUrl)
        {
            using (var context = new MoviesDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<MoviesDbContext>>()))
            {
                // Look for any categories to initialize
                if (context.Categories.Any())
                {
                    return;   // Data was already seeded
                }
                else
                {
                    context.Categories.AddRange(
                        new CategoryEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Horror"
                        },
                        new CategoryEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Historical"
                        },
                        new CategoryEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Social"
                        }
                        );

                    context.SaveChanges();
                }


                // Look for any movies to initialize
                if (context.Movies.Any())
                {
                    return;   // Data was already seeded
                }
                else
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        context.Movies.AddRange(
                            new MovieEntity
                            {
                                Id = Guid.NewGuid(),
                                Title = $"Movie {i}",
                                Category = context.Categories.Where(c => c.Title == "Horror").FirstOrDefault(),
                                ImageUrl = $"{blobHostUrl}video{i}.jpg"
                            }
                            );
                    }

                    for (int i = 11; i <= 20; i++)
                    {
                        context.Movies.AddRange(
                            new MovieEntity
                            {
                                Id = Guid.NewGuid(),
                                Title = $"Movie {i}",
                                Category = context.Categories.Where(c => c.Title == "Historical").FirstOrDefault(),
                                ImageUrl = $"{blobHostUrl}video{i}.jpg"
                            }
                            );
                    }

                    for (int i = 21; i <= 30; i++)
                    {
                        context.Movies.AddRange(
                            new MovieEntity
                            {
                                Id = Guid.NewGuid(),
                                Title = $"Movie {i}",
                                Category = context.Categories.Where(c => c.Title == "Social").FirstOrDefault(),
                                ImageUrl = $"{blobHostUrl}video{i}.jpg"
                            }
                            );
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
